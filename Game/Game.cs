using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This handles the main logic for the game
/// </summary>
public class Game : MonoBehaviour
{
    // Events to trigger but that cannot be changed in the editor
	[HideInInspector]
	public UnityEvent Correct, Incorrect, TimesUp, EndTimesUp, BestScore, NormalScore;

    // Events to trigger but that cannot be changed in the editor
    [HideInInspector]
	public IntEvent IncreasePoints, IncreaseTime;

	private char[] letters; // The three letters
	private GameState gameState;    // The current state of the game
	private Difficulty difficulty;  // The current difficulty
	private GameMode gameMode;  // The current game mode
	private int points;    // The number of points scored
	private float timeLeft = 60;    // The time left in a round
	private DictData dictData, letterList;  // A dictionary of all words and a list of the three letters
	private Scores scores;  // Handler for best scores
	private List<string> answers;   // List of answers already given
	private bool paused, canFocus = false, adRewarded = false; // bools used for game states
	private AudioManager audioManager;  // The audio manager
	private List<string> bannedWords;   // List of banned words (curse words)
	private Word word;  // Word handler
	private ChallengeGenerator challengeGenerator;  // Challenge generator
	private ChallengeInformation challengeInfo; // Holds challenge info for current challenge
	private Menu menu;  // The menu object, used for animations
	private int rerolledLetter = 0; // Stores the letter that is being rerolled
	private char oldLetter; // Stores old letter for rerolls
	private bool rerollUsed = false, restarting = false, rerolling = false; // Various bools for rerolling/restarting states

    // Properties to set in the inspector
	[SerializeField]
	private TextAsset dict, bannedWordsAsset, challengesFile;

	[SerializeField]
	private InputField input;

	[SerializeField]
	private Text[] lettersText;

	// Use this for initialization
	private void Start()
	{
		// Initialise variables
		GameOptions.LoadOptions();
		challengeGenerator = GetComponent<ChallengeGenerator>();
		challengeGenerator.GenerateChallenges();
		audioManager = GetComponent<AudioManager>();
		menu = FindObjectOfType<Menu>();
		
		dictData = new DictData(dict);
		dictData.Setup();
		letterList = new DictData(challengesFile);
		letterList.CleanLetterCombos();
		letters = new char[3];
		scores = new Scores();
		answers = new List<string>();
		word = new Word();
        input.onEndEdit.AddListener(CheckInput);
        gameState = GameState.Pregame;
        IncreasePoints = new IntEvent();
        IncreaseTime = new IntEvent();

        bannedWords = bannedWordsAsset.ToString().Split('\n').ToList();
        for (int i = 0; i < bannedWords.Count(); ++i)
        {
            bannedWords[i] = bannedWords[i].TrimEnd('\r');
        }

        // Check if this is the first time the game has been played
        bool playedBefore = false;
		for (int mode = 1; mode < 3; ++mode)
		{
            // For each game mode
			for (int diff = 1; diff < 4; ++diff)
			{
                // For each difficulty
				if (scores.GetBestScore((GameMode)mode, (Difficulty)diff) > 0)
				{
                    // If high score > 0 then played before
					playedBefore = true;
					break;
				}
			}

			if (playedBefore)
			{
				break;
			}
		}

        // Finished initialising variables

        // If not played before then start with the tutorial
        if (!playedBefore)
		{
			FindObjectOfType<LogoOpening>().SetNotPlayedBefore();
			GameInfo.SetGameMode(GameMode.Welcome);
		}

#if UNITY_IOS || UNITY_ANDROID
        // Initialise ads on mobile
		Ads.AdFinished.AddListener(AdCompleted);
		Ads.AdSkipped.AddListener(AdCompleted);
		Ads.AdFailed.AddListener(GoToMenu);
#endif
	}

    /// <summary>
    /// Save when quitting game
    /// </summary>
	private void OnApplicationQuit()
	{
		GameOptions.SaveOptions();
	}

    /// <summary>
    /// Setup for new round (called from menu)
    /// </summary>
	public void SetupGame()
	{
		difficulty = GameInfo.selectedDifficulty;
		gameMode = GameInfo.gameMode;

		ChangeState(GameState.Advert);
	}

	// Update is called once per frame
	private void Update()
	{
		if (!paused)
		{
			if (input.isFocused && !canFocus)
			{
				input.DeactivateInputField();
			}

			switch (gameState)
			{
				case GameState.Advert:
					break;
				case GameState.Countdown:
					ChangeState(GameState.Game);
					break;
				case GameState.Game:
					timeLeft -= Time.deltaTime;

					if (timeLeft <= 0)
					{
						// Finish
						//TODO: Game over event
						ChangeState(GameState.GameOver);
					}

					if (gameMode == GameMode.Challenge)
					{
						if (points >= challengeInfo.Target)
						{
							// Maybe Change This To A Congratulations?
							ChangeState(GameState.GameOver);
						}
					}
					else if (gameMode == GameMode.Welcome)
					{
						if (points > 0)
						{
							ChangeState(GameState.GameOver);
						}
					}
					break;
				case GameState.GameOver:
					timeLeft -= Time.deltaTime;

					if (timeLeft < 0)
					{
						EndTimesUp.Invoke();

#if UNITY_IOS || UNITY_ANDROID
						if (adRewarded || gameMode == GameMode.Challenge || gameMode == GameMode.Welcome)
						{
							ChangeState(GameState.Leaderboard);
						}
						else
						{
							if (Application.internetReachability == NetworkReachability.NotReachable)
							{
								ChangeState(GameState.Leaderboard);
							}
							else
							{
								ChangeState(GameState.ExtraTimeAdvert);
							}
						}
#else
						adRewarded = true;
						ChangeState(GameState.Leaderboard);
#endif
					}
					break;
				case GameState.Leaderboard:
					if (gameMode == GameMode.Welcome)
					{
						timeLeft -= Time.deltaTime;
						if (timeLeft < 0)
						{
							menu.CloseWelcomeComplete();
							menu.GoToMenu();
							ChangeState(GameState.Pregame);
							FindObjectOfType<MenuStateManager>().SetState(MenuState.Main);
						}
					}
					break;
			}
		}
	}

    /// <summary>
    /// Setup for new round
    /// </summary>
	private void Setup()
	{
		paused = false;

		switch (gameMode)
		{
			case GameMode.Classic:
#if UNITY_IOS || UNITY_ANDROID
				if (!adRewarded)
				{
					timeLeft = 61;
					GenerateLetters();
				}
				else
				{
					timeLeft = 31;
				}
#else
				timeLeft = 60;
				GenerateLetters();
#endif
				break;
			case GameMode.TimeTrial:
#if UNITY_IOS || UNITY_ANDROID
				if (!adRewarded)
				{
					timeLeft = 61;
					GenerateLetters();
				}
				else
				{
					timeLeft = 31;
				}
#else
		timeLeft = 60;
#endif
				break;
			case GameMode.Challenge:
                // Gather the letters and time from challenge info and set the states accordingly
				letters = challengeInfo.Letters;
				timeLeft = challengeInfo.Time;
				word = new Word(challengeInfo.Letters[0], challengeInfo.Letters[1], challengeInfo.Letters[2]);
				word.MatchingWords = dictData.GetMatchingWords(word);
				lettersText[0].text = word.A.ToString();
				lettersText[1].text = word.B.ToString();
				lettersText[2].text = word.C.ToString();
				break;
			case GameMode.Welcome:
                // Preset tutorial
				letters = new char[] { 'A', 'S', 'E' };
				timeLeft = 600;
				word = new Word('A', 'S', 'E');
				word.MatchingWords = dictData.GetMatchingWords(word);
				lettersText[0].text = word.A.ToString();
				lettersText[1].text = word.B.ToString();
				lettersText[2].text = word.C.ToString();
				break;
		}
	}

    /// <summary>
    /// Change to state
    /// </summary>
    /// <param name="to">State to change to</param>
	private void ChangeState(GameState to)
	{
		Debug.Log("Change State To: " + to);

		switch (to)
		{
			case GameState.Advert:
				gameState = GameState.Advert;

#if UNITY_IOS || UNITY_ANDROID
                // Show ad if need to, else go to game
				if (Ads.NeedAdvert())
				{
					Ads.ShowAd();
				}
				else
				{
					ChangeState(GameState.Countdown);
				}
#else
                // No ads on PC, Go to game
				ChangeState(GameState.Countdown);
#endif

				break;
			case GameState.Countdown:
				
#if UNITY_IOS || UNITY_ANDROID
				if (!adRewarded)
				{
                    // Reset points if not in extra time
					points = 0;
				}
#else
                // Reset points
				points = 0;
#endif
			   
				gameState = GameState.Countdown;

				//TODO: Remove This
				ChangeState(GameState.Game);
				break;
			case GameState.Game:
				Setup();
				if (!adRewarded && !restarting && gameMode != GameMode.Welcome)
				{
					answers = new List<string>();
					menu.GoToGame();
					if (restarting)
					{
						restarting = false;
					}
				}
				canFocus = true;
				if (gameMode != GameMode.Welcome)
				{
					ActivateInputField();
				}
				gameState = GameState.Game;
				break;
			case GameState.GameOver:
				canFocus = false;
				input.DeactivateInputField();
				TimesUp.Invoke();
				timeLeft = 2;
				gameState = GameState.GameOver;
				break;
			case GameState.Leaderboard:
				gameState = GameState.Leaderboard;
				ResetLetters();
				adRewarded = false;

				FindObjectOfType<RerollButtonsBehaviour>().Reset();
				rerollUsed = false;
				restarting = false;

				if (gameMode == GameMode.Classic || gameMode == GameMode.TimeTrial)
				{
					if (points > scores.GetBestScore(gameMode, difficulty))
					{
						menu.BestScore();
						scores.SaveBestScore(gameMode, difficulty, points);
						audioManager.PlaySoundEffect("BestScore");
					}
					else
					{
						menu.ScoreScreenEnter();
					}
				}
				else if (gameMode == GameMode.Challenge)
				{
					if (points >= challengeInfo.Target)
					{
						// Congrats
						menu.BestScore();
						audioManager.PlaySoundEffect("BestScore");
						challengeGenerator.SetChallengeComplete(challengeInfo.LevelName);
						// Set as complete
					}
					else
					{
						// Better Luck Next Time
						menu.OpenChallengeFailed();
					}
				}
				else
				{
					// Show some stuff
					menu.OpenWelcomeComplete();
					timeLeft = 3;
				}

				break;
			case GameState.ExtraTimeAdvert:
				menu.OpenWatchAd();
				gameState = GameState.ExtraTimeAdvert;
				break;
			case GameState.Pregame:
				gameState = GameState.Pregame;
				break;
		}
	}

	private void GenerateLetters()
	{
		bool validWordFound = false;
		float timer = 0;
		int minCount = 0;
		int maxCount = int.MaxValue;

		switch (difficulty)
		{
			case Difficulty.Easy:
				minCount = 2000;
				break;
			case Difficulty.Medium:
				minCount = 500;
				maxCount = 2000;
				break;
			case Difficulty.Hard:
				minCount = 50;
				maxCount = 500;
				break;
		}

		while (!validWordFound)
		{
			timer += Time.deltaTime;

			if (timer > 10)
			{
				validWordFound = true;
			}

			for (int i = 0; i < 3; ++i)
			{
				letters[i] = System.Convert.ToChar(System.Convert.ToInt32('A') + Random.Range(0, 26));
			}

			Word word = new Word(letters[0], letters[1], letters[2]);

			if (!bannedWords.Contains(word.CombinedLetters))
			{
				int matches = letterList.GetMatchingNumberOfWords(word);

				if (matches > minCount && matches < maxCount)
				{
					validWordFound = true;
					word.MatchingWords = dictData.GetMatchingWords(word);
					this.word = word;

					if (gameMode == GameMode.TimeTrial && timeLeft < 59)
					{
						FindObjectOfType<AnimatedUIHandler>().GetAnimator("A Image Bounce").completed.AddListener(SetLettersText);
					}
					else
					{
						SetLettersText();
					}
				}
			}
		}
	}

	private void GenerateSingleLetter()
	{
		List<char> matchedChars = new List<char>();
		int minCount = 0;
		int maxCount = int.MaxValue;

		switch (difficulty)
		{
			case Difficulty.Easy:
				minCount = 2000;
				break;
			case Difficulty.Medium:
				minCount = 500;
				maxCount = 2000;
				break;
			case Difficulty.Hard:
				minCount = 50;
				maxCount = 500;
				break;
		}

		for (char i = 'A'; i <= 'Z'; ++i)
		{
			if (i == oldLetter)
			{
				continue;
			}

			letters[rerolledLetter] = i;
			Word w = new Word(letters[0], letters[1], letters[2]);

			int matches = letterList.GetMatchingNumberOfWords(w);

			if (matches > minCount && matches < maxCount)
			{
				matchedChars.Add(i);
			}
		}

		if (matchedChars.Count > 0)
		{
			letters[rerolledLetter] = matchedChars[Random.Range(0, matchedChars.Count - 1)];
			Word word = new Word(letters[0], letters[1], letters[2]);
			word.MatchingWords = dictData.GetMatchingWords(word);
			this.word = word;
			lettersText[0].text = word.A.ToString();
			lettersText[1].text = word.B.ToString();
			lettersText[2].text = word.C.ToString();
		}
	}

	public void CheckInput(string s)
	{
		if (input.text != string.Empty)
		{
			string uppercase = input.text.ToUpper();

			if (word.MatchingWords.Contains(uppercase) && !answers.Contains(input.text))
			{
				Debug.Log(uppercase + " is valid");
				answers.Add(input.text);

				//TODO: Activate event
				Correct.Invoke();
				audioManager.PlaySoundEffect("Good");

				DoAction();
			}
			else
			{
				Debug.Log(uppercase + " is not valid");

				//TODO: Activate event
				Incorrect.Invoke();
				audioManager.PlaySoundEffect("Bad");
			}

			ActivateInputField();
			input.text = string.Empty;
		}
	}

	private void DoAction()
	{
		//TODO: Create Switch Statement
		int scoreIncrease = CalculateScore(input.text);

		switch (gameMode)
		{
			case GameMode.Classic:
				points += scoreIncrease;
				IncreasePoints.Invoke(scoreIncrease);
				break;
			case GameMode.TimeTrial:
				points += scoreIncrease;
				IncreasePoints.Invoke(scoreIncrease);
				IncreaseTime.Invoke(2);
				timeLeft += 2;
				GenerateLetters();

				////menu.ShowLetters();
				break;
			case GameMode.Challenge:
				points += scoreIncrease;
				IncreasePoints.Invoke(scoreIncrease);
				break;
			case GameMode.Welcome:
				points += scoreIncrease;
				IncreasePoints.Invoke(scoreIncrease);
				break;
			default:
				break;
		}
	}
	
	public void Pause()
	{
		paused = true;
		canFocus = false;
		input.DeactivateInputField();
	}

	public void Resume()
	{
		if (gameState == GameState.Game)
		{
			paused = false;
			canFocus = true;
			ActivateInputField();
		}
	}

	public void GoToMenuFromScore()
	{
		//TODO: Activate event
	}

	public void GoToMenuFromPause()
	{
		//TODO: Activate event
	}

	private int CalculateScore(string s)
	{
		float difficultyMultiplier = 1;
		
		if (difficulty == Difficulty.Medium)
		{
			difficultyMultiplier = 1.5f;
		}
		else if (difficulty == Difficulty.Hard)
		{
			difficultyMultiplier = 2;
		}

		return Mathf.RoundToInt(ScrabbleScore(s.ToUpper()) * difficultyMultiplier);
	}

	public void Reset()
	{
		if (paused)
		{
			paused = false;
		}

		rerollUsed = false;
		FindObjectOfType<RerollButtonsBehaviour>().Reset();
		restarting = true;
		answers = new List<string>();

		ChangeState(GameState.Advert);

		//TODO: Activate event
	}

	public void MenuReset()
	{
		if (gameState == GameState.Game)
		{
			//TODO: Activate event
		}

		Resume();
		Reset();
	}

	public void GoToMenu()
	{
		ChangeState(GameState.Pregame);
		rerollUsed = false;
		FindObjectOfType<RerollButtonsBehaviour>().Reset();
	}

	private int ScrabbleScore(string s)
	{
		int scrabbleScore = 0;

		for (int i = 0; i < s.Length; ++i)
		{
			switch (s[i])
			{
				case 'A':
				case 'E':
				case 'I':
				case 'O':
				case 'L':
				case 'N':
				case 'R':
				case 'S':
				case 'T':
				case 'U':
					scrabbleScore += 1;
					break;
				case 'D':
				case 'G':
					scrabbleScore += 2;
					break;
				case 'B':
				case 'C':
				case 'M':
				case 'P':
					scrabbleScore += 3;
					break;
				case 'F':
				case 'H':
				case 'V':
				case 'W':
				case 'Y':
					scrabbleScore += 4;
					break;
				case 'K':
					scrabbleScore += 5;
					break;
				case 'J':
				case 'X':
					scrabbleScore += 8;
					break;
				case 'Q':
				case 'Z':
					scrabbleScore += 10;
					break;
			}
		}

		return scrabbleScore;
	}

	private void AdCompleted()
	{
#if UNITY_IOS || UNITY_ANDROID
		if (gameState == GameState.Advert)
		{
			ChangeState(GameState.Countdown);
		}
		else if (gameState == GameState.ExtraTimeAdvert)
		{
			if (Ads.Result == UnityEngine.Advertisements.ShowResult.Skipped)
			{
				// Do nowt
				gameState = GameState.Leaderboard;
			}
			else if (Ads.Result == UnityEngine.Advertisements.ShowResult.Finished)
			{
				// Reset with extra time
				restarting = false;
				adRewarded = true;
				menu.GameEnterLeft(false);
				ChangeState(GameState.Countdown);
			}
			else if (Ads.Result == UnityEngine.Advertisements.ShowResult.Failed)
			{
				ChangeState(GameState.Leaderboard);
			}
		}
#endif
	}

	public void DontShowAd()
	{
		ChangeState(GameState.Leaderboard);
	}

	public void ShowAd()
	{
		Debug.Log("Show Ad");
#if UNITY_IOS || UNITY_ANDROID
		if (gameState == GameState.ExtraTimeAdvert)
		{
			menu.CloseWatchedAd();
		}

		Ads.ShowRewardedVideo();
		Ads.AdFinished.AddListener(AdCompleted);
#endif
	}

	private void UnloadScene()
	{
		SceneManager.UnloadSceneAsync("Game");
	}

	public void SetGameMode(GameMode mode)
	{
		gameMode = mode;
	}

	public GameMode GetGameMode()
	{
		return gameMode;
	}

	public float GetTimeLeft()
	{
		return timeLeft;
	}

	public int GetPoints()
	{
		return points;
	}

	public int GetBestScore()
	{
		return scores.GetBestScore(gameMode, difficulty);
	}

	public GameState GetGameState()
	{
		return gameState;
	}

	public void SetChallengeInfo(string name)
	{
		challengeInfo = challengeGenerator.GetChallengeInfo(name);
	}

	public void ActivateInputField()
	{
		if (canFocus && !input.isFocused)
		{
			input.ActivateInputField();
		}
	}

	public void ResetLetters()
	{
		lettersText[0].color = new Color(lettersText[0].color.r, lettersText[0].color.g, lettersText[0].color.b, 0);
		lettersText[1].color = new Color(lettersText[1].color.r, lettersText[1].color.g, lettersText[1].color.b, 0);
		lettersText[2].color = new Color(lettersText[2].color.r, lettersText[2].color.g, lettersText[2].color.b, 0);
	}

	public void RerollLetter(int letter)
	{
		oldLetter = letters[letter];
		rerolledLetter = letter;
		GenerateSingleLetter();
		rerollUsed = true;
	}

	public bool GetRerollUsed()
	{
		return rerollUsed;
	}

	private void SetLettersText()
	{
		lettersText[0].text = word.A.ToString();
		lettersText[1].text = word.B.ToString();
		lettersText[2].text = word.C.ToString();
	}

	public void ResetAdRewarded()
	{
		adRewarded = false;
	}

	public void EnterReroll()
	{
		rerolling = true;
	}

	public void ExitReroll(bool callRerollBehaviour)
	{
		if (rerolling && callRerollBehaviour)
		{
			FindObjectOfType<RerollButtonsBehaviour>().EnterReroll();
		}

		rerolling = false;
	}
}

/// <summary>
/// Dummy class used for events with an int param
/// </summary>
public class IntEvent : UnityEvent<int>
{

}
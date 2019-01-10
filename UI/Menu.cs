using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private AnimatedUIHandler handler;

    private Scaler scaler;

    [HideInInspector]
    public UnityEvent MenuClosed, ScoreMenuOpen, ScoreMenuClose;
    
    private float t = 0;
    private bool openingDifficulty = false;
    private Game game;

    private void Start()
    {
        handler = GetComponent<AnimatedUIHandler>();
        game = FindObjectOfType<Game>();
        game.BestScore.AddListener(BestScore);
        scaler = FindObjectOfType<Scaler>();
    }

    private void Update()
    {
    }

    public void PressPlay()
    {
        handler.PlayAnimation("Logo Enter", 0.2f);

        handler.PlayAnimation("Classic Enter", 0.5f);
        handler.PlayAnimation("Time Trial Enter", 0.55f);
        handler.PlayAnimation("Challenge Enter", 0.6f);
        handler.PlayAnimation("Red Bar Enter", 0.65f);
        handler.PlayAnimation("Options Enter", 0.65f);
    }

    public void MenuExit()
    {
        handler.PlayAnimation("Logo Exit", 0.1f);
        handler.PlayAnimation("Classic Exit", 0.15f);
        handler.PlayAnimation("Time Trial Exit", 0.2f);
        handler.PlayAnimation("Challenge Exit", 0.25f);
        handler.PlayAnimation("Red Bar Exit", 0.3f);
        handler.PlayAnimation("Options Exit", 0.3f);
    }

    public void OpenDifficulty()
    {
        handler.PlayAnimation("Logo Exit", 0.1f);
        handler.PlayAnimation("Classic Exit", 0.15f);
        handler.PlayAnimation("Time Trial Exit", 0.2f);
        handler.PlayAnimation("Challenge Exit", 0.25f);
        handler.PlayAnimation("Red Bar Exit", 0.3f);
        handler.PlayAnimation("Options Exit", 0.3f);

        handler.PlayAnimation("Easy Enter", 0.1f);
        handler.PlayAnimation("Difficulty Close Button Enter", 0.1f);
        handler.PlayAnimation("Medium Enter", 0.2f);
        handler.PlayAnimation("Hard Enter", 0.3f);
    }

    public void CloseDifiiculty()
    {
        handler.PlayAnimation("Easy Exit", 0.1f);
        handler.PlayAnimation("Difficulty Close Button Exit", 0.1f);
        handler.PlayAnimation("Medium Exit", 0.1f);
        handler.PlayAnimation("Hard Exit", 0.1f);
    }

    public void CloseDifficultyToMenu()
    {
        handler.PlayAnimationExit("Logo Exit", 0.1f);
        handler.PlayAnimationExit("Classic Exit", 0.15f);
        handler.PlayAnimationExit("Time Trial Exit", 0.2f);
        handler.PlayAnimationExit("Challenge Exit", 0.25f);
        handler.PlayAnimationExit("Red Bar Exit", 0.3f);
        handler.PlayAnimationExit("Options Exit", 0.3f);
                             
        handler.PlayAnimationExit("Easy Enter", 0.1f);
        handler.PlayAnimationExit("Difficulty Close Button Enter", 0.1f);
        handler.PlayAnimationExit("Medium Enter", 0.2f);
        handler.PlayAnimationExit("Hard Enter", 0.3f);
    }

    public void GoToGame()
    {
        handler.PlayAnimation("Points Enter", 0.1f);
        handler.PlayAnimation("A Image Enter", 0.1f);
        handler.PlayAnimation("B Image Enter", 0.2f);
        handler.PlayAnimation("C Image Enter", 0.3f);
        handler.PlayAnimation("Game Bar Enter", 0.2f);
        handler.PlayAnimation("Burger Enter", 0.2f);

        if (game.GetGameMode() != GameMode.Challenge || !scaler.Landscape)
        {
            if (!scaler.Landscape)
            {
                handler.PlayAnimation("Timer Circle Enter", 0.2f);
                handler.PlayAnimation("Reroll Button Enter", 0.2f);
            }
            else
            {
                handler.PlayAnimation("Timer Circle Enter Landscape", 0.2f);
                handler.PlayAnimation("Reroll Button Enter Landscape", 0.2f);
            }
            
        }
        else
        {
            handler.PlayAnimation("Timer Circle Enter Centered", 0.2f);
        }
        
        MenuClosed.AddListener(ShowLetters);
        StartCoroutine(WaitToInvoke(1.0f, MenuClosed));
    }

    public void CloseGame()
    {
        handler.PlayAnimationExit("Points Enter", 0.1f);
        handler.PlayAnimationExit("A Image Enter", 0.1f);
        handler.PlayAnimationExit("B Image Enter", 0.2f);
        handler.PlayAnimationExit("C Image Enter", 0.3f);
        handler.PlayAnimationExit("Game Bar Enter", 0.2f);
        handler.PlayAnimationExit("Burger Enter", 0.2f);

        if (!game.GetRerollUsed() && game.GetGameMode() != GameMode.Challenge && !scaler.Landscape)
        {
            if (!scaler.Landscape)
            {
                handler.PlayAnimationExit("Timer Circle Enter", 0.2f);
                handler.PlayAnimationExit("Reroll Button Enter", 0.2f);
            }
            else
            {
                handler.PlayAnimationExit("Timer Circle Enter Landscape", 0.2f);
                handler.PlayAnimationExit("Reroll Button Enter Landscape", 0.2f);
            }
        }
        else
        {
            handler.PlayAnimationExit("Timer Circle Enter Centered", 0.2f);
            handler.PlayAnimationExit("Reroll Button Enter", 0.2f);
        }

        handler.GetAnimator("C Image Enter").completed.AddListener(game.ResetAdRewarded);
    }

    public void ShowLetters()
    {
        MenuClosed.RemoveListener(ShowLetters);
        handler.PlayAnimation("A Text Enter", 0.1f);
        handler.PlayAnimation("B Text Enter", 0.1f);
        handler.PlayAnimation("C Text Enter", 0.1f);
    }

    public void OpenPauseMenu()
    {
        handler.PlayAnimation("Pause Enter", 0.1f);
    }

    public void ClosePauseMenu()
    {
        handler.PlayAnimationExit("Pause Enter", 0.1f);
        StartCoroutine(WaitToInvoke(1.0f, MenuClosed));
    }

    public void GoToMenu()
    {
        handler.PlayAnimationExit("Logo Exit", 0.1f);
        handler.PlayAnimationExit("Classic Exit", 0.15f);
        handler.PlayAnimationExit("Time Trial Exit", 0.2f);
        handler.PlayAnimationExit("Challenge Exit", 0.25f);
        handler.PlayAnimationExit("Red Bar Exit", 0.3f);
        handler.PlayAnimationExit("Options Exit", 0.3f);
        handler.GetAnimator("Options Exit").completed.AddListener(game.ResetLetters);
        game.GoToMenu();
    }

    public void GameExitLeft()
    {
        handler.PlayAnimation("Points Exit Left", 0.1f);
        handler.PlayAnimation("A Image Exit Left", 0.1f);
        handler.PlayAnimation("B Image Exit Left", 0.2f);
        handler.PlayAnimation("C Image Exit Left", 0.3f);
        handler.PlayAnimation("Game Bar Exit Left", 0.2f);
        handler.PlayAnimation("Burger Exit Left", 0.2f);

        if (!game.GetRerollUsed() && game.GetGameMode() != GameMode.Challenge && game.GetGameMode() != GameMode.Welcome && !scaler.Landscape)
        {
            if (!scaler.Landscape)
            {
                handler.PlayAnimation("Timer Circle Exit Left", 0.2f);
                handler.PlayAnimation("Reroll Button Exit", 0.2f);
            }
            else
            {
                handler.PlayAnimation("Timer Circle Exit Left Landscape", 0.2f);
                handler.PlayAnimation("Reroll Button Exit Landscape", 0.2f);
            }
        }
        else
        {
            handler.PlayAnimation("Timer Circle Exit Left Centered", 0.2f);
            handler.PlayAnimation("Reroll Button Exit", 0.2f);
        }

        if (game.GetGameMode() == GameMode.Welcome)
        {
            handler.PlayAnimation("Instructions Exit", 0.1f);
            handler.PlayAnimation("Instructions Arrow Exit", 0.1f); 
        }
    }

    public void GameEnterLeft(bool restarting)
    {
        handler.PlayAnimationExit("Points Exit Left", 0.3f);
        handler.PlayAnimationExit("A Image Exit Left", 0.3f);
        handler.PlayAnimationExit("B Image Exit Left", 0.2f);
        handler.PlayAnimationExit("C Image Exit Left", 0.1f);
        handler.PlayAnimationExit("Game Bar Exit Left", 0.2f);
        handler.PlayAnimationExit("Burger Exit Left", 0.2f);

        if (!game.GetRerollUsed() && game.GetGameMode() != GameMode.Challenge && game.GetGameMode() != GameMode.Welcome && !scaler.Landscape)
        {
            if (!scaler.Landscape)
            {
                handler.PlayAnimationExit("Timer Circle Exit Left", 0.2f);
                handler.PlayAnimationExit("Reroll Button Exit", 0.2f);
            }
            else
            {
                handler.PlayAnimationExit("Timer Circle Exit Left Landscape", 0.2f);
                handler.PlayAnimationExit("Reroll Button Exit Landscape", 0.2f);
            }
        }
        else
        {
            handler.PlayAnimationExit("Timer Circle Exit Left Centered", 0.2f);
            handler.PlayAnimationExit("Reroll Button Exit", 0.2f);
        }

        if (restarting)
        {
            MenuClosed.AddListener(ShowLetters);
        }

        StartCoroutine(WaitToInvoke(1.0f, MenuClosed));
    }

    public void GameEnterLeftDelayed(bool restarting)
    {
        handler.PlayAnimationExit("Points Exit Left", 0.8f);
        handler.PlayAnimationExit("A Image Exit Left", 0.8f);
        handler.PlayAnimationExit("B Image Exit Left", 0.7f);
        handler.PlayAnimationExit("C Image Exit Left", 0.6f);
        handler.PlayAnimationExit("Game Bar Exit Left", 0.7f);
        handler.PlayAnimationExit("Burger Exit Left", 0.7f);

        if (!game.GetRerollUsed() && game.GetGameMode() != GameMode.Challenge && game.GetGameMode() != GameMode.Welcome && !scaler.Landscape)
        {
            if (!scaler.Landscape)
            {
                handler.PlayAnimationExit("Timer Circle Exit Left", 0.7f);
                handler.PlayAnimationExit("Reroll Button Exit", 0.7f);
            }
            else
            {
                handler.PlayAnimationExit("Timer Circle Exit Left Lanscape", 0.7f);
                handler.PlayAnimationExit("Reroll Button Exit Landscape", 0.7f);
            }
        }
        else
        {
            handler.PlayAnimationExit("Timer Circle Exit Left Centered", 0.7f);
            handler.PlayAnimationExit("Reroll Button Exit", 0.7f);
        }

        if (restarting)
        {
            MenuClosed.AddListener(ShowLetters);
        }

        StartCoroutine(WaitToInvoke(1.5f, MenuClosed));
    }

    public void TimesUp()
    {
        GameExitLeft();
        handler.PlayAnimation("Times Text Enter", 0.1f);
        handler.PlayAnimation("Up Text Enter", 0.15f);
    }

    public void OpenWatchAd()
    {
        handler.PlayAnimation("Continue Enter", 0.1f);
        handler.PlayAnimation("Watch Ad Text Enter", 0.2f);
        handler.PlayAnimation("Watch Ad Enter", 0.3f);
        handler.PlayAnimation("No Thanks Enter", 0.4f);
    }

    public void BestScore()
    {
        handler.PlayAnimation("Blue Circle 1", 0.5f);
        handler.PlayAnimation("Red Circle", 0.65f);
        handler.PlayAnimation("Yellow Circle", 0.8f);
        handler.PlayAnimation("Green Circle", 0.95f);
        handler.PlayAnimation("Blue Circle 2", 1.1f);

        if (game.GetGameMode() == GameMode.Classic || game.GetGameMode() == GameMode.TimeTrial)
        {
            // Time trial or classic
            handler.PlayAnimation("New High Score Score Enter", 0.95f);
            handler.PlayAnimation("New High Score Text Enter", 0.95f);
            handler.PlayAnimation("Best Score Replay Enter", 0.95f);
            handler.PlayAnimation("Best Score Menu Enter", 0.95f);
        }
        else
        {
            // Is challenge mode
            handler.PlayAnimation("Challenge Complete Enter", 0.95f);
            handler.PlayAnimation("Challenge Complete Menu Enter", 0.95f);
        }
    }

    public void BestScoreExit()
    {
        handler.PlayAnimationExit("Blue Circle 1", 0.5f);
        handler.PlayAnimationExit("Red Circle", 0.4f);
        handler.PlayAnimationExit("Yellow Circle", 0.3f);
        handler.PlayAnimationExit("Green Circle", 0.2f);
        handler.PlayAnimationExit("Blue Circle 2", 0.1f);

        if (game.GetGameMode() == GameMode.Classic || game.GetGameMode() == GameMode.TimeTrial)
        {
            handler.PlayAnimationExit("New High Score Score Enter", 0.1f);
            handler.PlayAnimationExit("New High Score Text Enter", 0.1f);
            handler.PlayAnimationExit("Best Score Replay Enter", 0.1f);
            handler.PlayAnimationExit("Best Score Menu Enter", 0.1f);
        }
        else
        {
            handler.PlayAnimationExit("Challenge Complete Enter", 0.5f);
            handler.PlayAnimationExit("Challenge Complete Menu Enter", 0.5f);
        }
    }

    public void ScoreScreenEnter()
    {
        handler.PlayAnimation("Score Text Enter", 0.1f);
        handler.PlayAnimation("Points Text Enter", 0.15f);
        handler.PlayAnimation("Best Score Text Enter", 0.2f);
        handler.PlayAnimation("Best Points Text Enter", 0.25f);
        handler.PlayAnimation("Score Replay Enter", 0.3f);
        handler.PlayAnimation("Score Menu Enter", 0.3f);
    }

    public void ScoreScreenExit()
    {
        handler.PlayAnimationExit("Score Text Enter", 0.1f);
        handler.PlayAnimationExit("Points Text Enter", 0.15f);
        handler.PlayAnimationExit("Best Score Text Enter", 0.2f);
        handler.PlayAnimationExit("Best Points Text Enter", 0.25f);
        handler.PlayAnimationExit("Score Replay Enter", 0.3f);
        handler.PlayAnimationExit("Score Menu Enter", 0.3f);
    }

    public void GoToMenuDelayed()
    {
        handler.PlayAnimationExit("Logo Exit", 0.5f);
        handler.PlayAnimationExit("Classic Exit", 0.65f);
        handler.PlayAnimationExit("Time Trial Exit", 0.7f);
        handler.PlayAnimationExit("Challenge Exit", 0.75f);
        handler.PlayAnimationExit("Red Bar Exit", 0.8f);
        handler.PlayAnimationExit("Options Exit", 0.8f);
        handler.GetAnimator("Options Exit").completed.AddListener(game.ResetLetters);
        handler.GetAnimator("Options Exit").completed.AddListener(game.GoToMenu);
        handler.GetAnimator("Options Exit").completed.AddListener(handler.GetAnimator("Options Exit").completed.RemoveAllListeners);
    }

    public void CloseWatchedAd()
    {
        handler.PlayAnimationExit("Continue Enter", 0.1f);
        handler.PlayAnimationExit("Watch Ad Text Enter", 0.1f);
        handler.PlayAnimationExit("Watch Ad Enter", 0.1f);
        handler.PlayAnimationExit("No Thanks Enter", 0.1f);
    }

    public void CloseWatchAd()
    {
        handler.PlayAnimation("Continue Exit", 0.1f);
        handler.PlayAnimation("Watch Ad Text Exit", 0.1f);
        handler.PlayAnimation("Watch Ad Exit", 0.1f);
        handler.PlayAnimation("No Thanks Exit", 0.1f);
        StartCoroutine(WaitToInvoke(1.0f, MenuClosed));
    }

    public void OpenChallenges()
    {
        handler.PlayAnimation("Classic Exit", 0.15f);
        handler.PlayAnimation("Time Trial Exit", 0.2f);
        handler.PlayAnimation("Challenge Exit", 0.25f);
        handler.PlayAnimation("Red Bar Exit", 0.3f);
        handler.PlayAnimation("Options Exit", 0.3f);
        handler.PlayAnimation("Challenges Logo Enter", 0.1f);
        handler.PlayAnimation("Challenges Text Enter", 0.5f);

        handler.PlayAnimation("Challenges Enter", 0.7f);
        handler.GetAnimator("Challenges Enter").completed.AddListener(FindObjectOfType<ChallengeInformationText>().SetUpdatingTrue);
        handler.GetAnimator("Challenges Enter").completed.AddListener(handler.GetAnimator("Challenges Enter").completed.RemoveAllListeners);
        FindObjectOfType<ChallengeInformationText>().UpdateInfo();
    }

    public void OpenChallengesLeft()
    {
        handler.PlayAnimationExit("Challenges Logo Exit", 0.95f);
        handler.PlayAnimationExit("Challenges Text Exit", 0.95f);
        handler.PlayAnimationExit("Challenges Exit", 1.05f);

        handler.GetAnimator("Challenges Exit").completed.AddListener(FindObjectOfType<ChallengeInformationText>().SetUpdatingTrue);
        handler.GetAnimator("Challenges Exit").completed.AddListener(handler.GetAnimator("Challenges Exit").completed.RemoveAllListeners);
        FindObjectOfType<ChallengeInformationText>().UpdateInfo();
    }

    public void CloseChallenges()
    {
        handler.PlayAnimation("Challenges Logo Exit", 0.1f);
        handler.PlayAnimation("Challenges Text Exit", 0.1f);
        handler.PlayAnimation("Challenges Exit", 0.2f);
    }

    public void ChallengesGoToMenu()
    {
        FindObjectOfType<ChallengeInformationText>().SetUpdatingFalse();
        handler.PlayAnimationExit("Classic Exit", 0.5f);
        handler.PlayAnimationExit("Time Trial Exit", 0.55f);
        handler.PlayAnimationExit("Challenge Exit", 0.6f);
        handler.PlayAnimationExit("Red Bar Exit", 0.65f);
        handler.PlayAnimationExit("Options Exit", 0.65f);
        handler.PlayAnimationExit("Challenges Logo Enter", 0.5f);
        handler.PlayAnimationExit("Challenges Text Enter", 0.1f);
        handler.GetAnimator("Challenges Text Enter").completed.AddListener(game.ResetLetters);


        handler.PlayAnimationExit("Challenges Enter", 0.1f);
    }

    public void OpenChallengeFailed()
    {
        handler.PlayAnimation("Challenge Failed Enter", 0.1f);
        handler.PlayAnimation("Challenge Failed Menu Enter", 0.1f);
    }

    public void CloseChallengeFailed()
    {
        handler.PlayAnimationExit("Challenge Failed Enter", 0.1f);
        handler.PlayAnimationExit("Challenge Failed Menu Enter", 0.1f);
    }

    public void OpenWelcomeGame()
    {
        handler.PlayAnimation("Logo Welcome Game Exit", 0.1f);
        

        game.SetupGame();
        ////handler.PlayAnimation("Game Enter", 0.2f);
        handler.PlayAnimation("Points Enter", 0.5f);
        handler.PlayAnimation("A Image Enter", 0.5f);
        handler.PlayAnimation("B Image Enter", 0.6f);
        handler.PlayAnimation("C Image Enter", 0.7f);
        handler.PlayAnimation("Game Bar Enter", 0.6f);
        ////handler.PlayAnimation("Burger Enter", 0.6f);

        if (!scaler.Landscape)
        {
            handler.PlayAnimation("Timer Circle Enter Centered", 0.6f);
        }
        else
        {
            handler.PlayAnimation("Timer Circle Enter Landscape", 0.6f);
        }

        handler.PlayAnimation("Instructions Enter", 0.7f);
        handler.PlayAnimation("Instructions Arrow Enter", 1.5f);
        MenuClosed.AddListener(ShowLetters);
        StartCoroutine(WaitToInvoke(1.0f, MenuClosed));
    }

    public void OpenWelcomeComplete()
    {
        handler.PlayAnimation("Welcome Complete Enter", 0.1f);
    }

    public void CloseWelcomeComplete()
    {
        handler.PlayAnimationExit("Welcome Complete Enter", 0.1f);
        handler.PlayAnimationExit("Welcome Complete Button Enter", 0.1f);
    }

    private IEnumerator WaitToInvoke(float time, UnityEvent ev)
    {
        yield return new WaitForSeconds(time);
        ev.Invoke();
    }
}

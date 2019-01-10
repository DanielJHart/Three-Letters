using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterBox : MonoBehaviour
{
    private Image normalImage;

    private Game game;

    [SerializeField]
    private GameObject filledImage;

    [SerializeField]
    private Text letter;

    [SerializeField]
    private Color letterHighlightColor;

    [SerializeField]
    private AnimatedUI bounceAnim;

    [SerializeField]
    private AnimatedUI letterBounceAnim;

    private Color letterColor;

    // Use this for initialization
    void Start()
    {
        normalImage = GetComponent<Image>();
        letterColor = letter.color;
        letterColor = new Color(letterColor.r, letterColor.g, letterColor.b, 1);
        game = FindObjectOfType<Game>();
        game.Correct.AddListener(CorrectAnswer);
        game.Incorrect.AddListener(IncorrectAnswer);
        ////bounceAnim = GetComponent<AnimatedUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LetterTyped()
    {
        filledImage.SetActive(true);
        letter.color = letterHighlightColor;
        Bounce();
    }

    public void CorrectAnswer()
    {
        if (game.GetGameState() == GameState.Game)
        {
            Bounce();
            bounceAnim.completed.AddListener(Reset);
        }
    }

    public void IncorrectAnswer()
    {
        if (game.GetGameState() == GameState.Game)
        {
            Reset();
        }
    }

    public void Reset()
    {
        bounceAnim.completed.RemoveAllListeners();
        filledImage.SetActive(false);
        letter.color = letterColor;
    }

    public void Bounce()
    {
        bounceAnim.Play();
        letterBounceAnim.Play();
    }
}

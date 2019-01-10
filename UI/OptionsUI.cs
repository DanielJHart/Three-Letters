using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    private Game game;

    [SerializeField]
    private Image replayButton, menuButton, optionsButton;

    [SerializeField]
    private GameObject timerCircle;

    private float xPos;
    
    // Use this for initialization
    void Start()
    {
        game = FindObjectOfType<Game>();
        xPos = optionsButton.transform.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (game.GetGameState() != GameState.Game)
        {
            //game.GetGameMode() == GameMode.Challenge

            if (replayButton.color != Colors.OffWhite)
            {
                replayButton.color = Colors.White;
                replayButton.GetComponent<ScoreScreenButton>().SetClickable(false);
                menuButton.color = Colors.White;
                menuButton.GetComponent<ScoreScreenButton>().SetClickable(false);
                Vector2 pos = optionsButton.transform.localPosition;
                optionsButton.transform.localPosition = new Vector3(0, pos.y);
                timerCircle.SetActive(false);
            }
        }
        else if (replayButton.color != Colors.Red)
        {
            replayButton.color = Colors.Red;
            replayButton.GetComponent<ScoreScreenButton>().SetClickable(true);
            menuButton.color = Colors.Red;
            menuButton.GetComponent<ScoreScreenButton>().SetClickable(true);
            Vector2 pos = optionsButton.transform.localPosition;
            optionsButton.transform.localPosition = new Vector3(xPos, pos.y);
            timerCircle.SetActive(true);
        }
    }
}

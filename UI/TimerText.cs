using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerText : MonoBehaviour
{
    private Text text;
    private Game game;

    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
        game = FindObjectOfType<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        if (game.GetGameState() == GameState.Game)
        {
            int timeLeft = Mathf.RoundToInt(game.GetTimeLeft());

            string outputString = string.Empty;
            if (timeLeft >= 60)
            {
                if (text.color != Colors.Turquoise)
                {
                    text.color = Colors.Turquoise;
                }

                outputString = "1:" + (timeLeft % 60).ToString("00");
            }
            else
            {
                outputString = timeLeft.ToString();

                if (timeLeft <= 10)
                {
                    if (text.color != Colors.Red)
                    {
                        text.color = Colors.Red;
                    }
                }
                else
                {
                    if (text.color != Colors.Turquoise)
                    {
                        text.color = Colors.Turquoise;
                    }
                }
            }

            text.text = outputString;
        }
    }
}

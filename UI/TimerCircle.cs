using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerCircle : MonoBehaviour
{
    private Image img;
    float timeLeft = 60;
    private Game game;

    // Use this for initialization
    void Start()
    {
        img = GetComponent<Image>();
        game = FindObjectOfType<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        if (game.GetGameState() == GameState.Game)
        {
            float perc = game.GetTimeLeft() / 60;

            img.fillAmount = perc;
        }
    }
}

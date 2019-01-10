using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreChangeText : MonoBehaviour
{
    private Text text;
    private byte alpha = 255;
    private bool active;
    private AnimatedUI anim;
    private enum ScoreChangeType { Points, Time}

    [SerializeField]
    private ScoreChangeType type;

    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
        Game game = FindObjectOfType<Game>();

        if (type == ScoreChangeType.Points)
        {
            game.IncreasePoints.AddListener(Activate);
        }
        else
        {
            game.IncreaseTime.AddListener(ActivateTime);
        }
        anim = GetComponent<AnimatedUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            alpha -= 5;
            text.color = new Color32(Colors.Turquoise.r, Colors.Turquoise.g, Colors.Turquoise.b, alpha);
            if (alpha <= 0)
            {
                active = false;
            }
        }
    }

    private void Activate(int points)
    {
        text.text = "+" + points.ToString();
        text.color = Colors.Turquoise;
        alpha = 255;
        active = true;
        anim.Play();
    }

    private void ActivateTime(int time)
    {
        text.text = "+" + time.ToString() + "s";
        text.color = Colors.Turquoise;
        alpha = 255;
        active = true;
        anim.Play();
    }
}

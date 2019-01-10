using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Points : MonoBehaviour
{
    private Text points;
    private Game game;

    // Use this for initialization
    void Start()
    {
		game = FindObjectOfType<Game>();
        points = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!game)
        {
            game = FindObjectOfType<Game>();
        }

        int point = game.GetPoints();

        points.text = point.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangingTextManager : MonoBehaviour {

    [SerializeField]
    private Text points, endScorePoints, bestScorePoints, newBestScorePoints;

    private Game game;

    private bool HasSetScores = false;

	// Use this for initialization
	void Start () 
    {
        game = FindObjectOfType<Game>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        string pointsValue = game.GetPoints().ToString();

        if (game.GetGameState() == GameState.Game)
        {
            ////points.text = pointsValue;
        }

        if (game.GetGameState() > GameState.Game)
        {
            if (!HasSetScores)
            {
                endScorePoints.text = pointsValue;
                newBestScorePoints.text = pointsValue;
                bestScorePoints.text = game.GetBestScore().ToString();
                HasSetScores = true;
            }
        }
        else if (HasSetScores)
        {
            HasSetScores = false;
        }
    }
}

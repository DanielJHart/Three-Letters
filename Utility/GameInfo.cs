using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo : MonoBehaviour
{
    public static Difficulty selectedDifficulty;
    public static GameMode gameMode;

    // Use this for initialization
    void Start()
    {
        Application.targetFrameRate = 60;

#if UNITY_IOS || UNITY_ANDROID
        Ads.Initlialize();
#endif
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void SetGameMode(GameMode gm)
    {
        gameMode = gm;
    }

    public static void SetDifficulty(Difficulty diff)
    {
        selectedDifficulty = diff;
    }
}

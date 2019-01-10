#if UNITY_IOS || UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Advertisements;
using UnityEngine;
using UnityEngine.Events;
using System.IO;

public static class Ads
{
    public static UnityEvent AdFinished, AdSkipped, AdFailed;
    public static ShowResult Result;
    private static int GamesSinceAdvert;
    private static bool GameAdvert = false;

    // Use this for initialization
    public static void Initlialize()
    {
        string gameId = string.Empty;
#if UNITY_IOS
    gameId = "2637968";
#elif UNITY_ANDROID
        gameId = "2637969";
#endif

        Debug.Log("Initializing with id: " + gameId);
        Advertisement.Initialize(gameId, true);
        AdFinished = new UnityEvent();
        AdSkipped = new UnityEvent();
        AdFailed = new UnityEvent();

        string path = Application.persistentDataPath + @"/data.dat";
        FileStream fs;

        if (!File.Exists(path))
        {
            fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            fs.Dispose();

            string[] linesToWrite =
            {
                "GamesPlayed\t0"
            };

            File.WriteAllLines(path, linesToWrite);
        }

        GetGameData();
    }

    public static bool NeedAdvert()
    {
        if (!Advertisement.isInitialized)
        {
            return false;
        }

        if (GamesSinceAdvert == 3)
        {
            if (!Advertisement.IsReady())
            {
                return false;
            }

            return true;
        }
        else
        {
            ++GamesSinceAdvert;
            SaveGameData();
            return false;
        }
    }

    private static void GetGameData()
    {
        string path = Application.persistentDataPath + @"/data.dat";
        FileStream fs;
        
        fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        fs.Dispose();
        string[] lines = File.ReadAllLines(path);
        
        foreach (string line in lines)
        {
            string[] l = line.Split('\t');
            if (l[0].GetHashCode() == "GamesPlayed".GetHashCode())
            {
                GamesSinceAdvert = int.Parse(l[1]);
            }
        }
    }

    private static void SaveGameData()
    {
        string path = Application.persistentDataPath + @"/data.dat";
        FileStream fs;

        fs = new FileStream(path, FileMode.Open, FileAccess.Write);
        fs.Dispose();
        string GamesPlayed = "GamesPlayed\t" + GamesSinceAdvert.ToString();
        string[] lines =
        {
            GamesPlayed
        };

        File.WriteAllLines(path, lines);
    }

    public static void ShowAd()
    {
        ShowOptions options = new ShowOptions()
        {
            resultCallback = HandleShowResult
        };

        Advertisement.Show("video", options);
        GameAdvert = true;
    }

    public static void ShowRewardedVideo()
    {
        GamesSinceAdvert = 0;
        ShowOptions options = new ShowOptions()
        {
            resultCallback = HandleShowResult
        };

        Advertisement.Show("rewardedVideo", options);
    }

    private static void HandleShowResult(ShowResult result)
    {
        Result = result;

        if (result == ShowResult.Finished)
        {
            Debug.Log("Video completed - Offer a reward to the player");
            AdFinished.Invoke();

            if (GameAdvert)
            {
                GameAdvert = false;
                GamesSinceAdvert = 0;
                SaveGameData();
            }
        }
        else if (result == ShowResult.Skipped)
        {
            Debug.LogWarning("Video was skipped - Do NOT reward the player");
            AdSkipped.Invoke();

            if (GameAdvert)
            {
                GameAdvert = false;
                GamesSinceAdvert = 0;
                SaveGameData();
            }
        }
        else if (result == ShowResult.Failed)
        {
            Debug.LogError("Video failed to show");
            AdFailed.Invoke();
        }
    }
}
#endif

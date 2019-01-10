using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Events;

public class ChallengeGenerator : MonoBehaviour
{
    FileStream file;
    string path;

    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private Transform parent;

    [SerializeField]
    private int localXPos;

    [SerializeField]
    private TextAsset challengesFile;

    List<ChallengeInformation> challengeInfos;

    // Use this for initialization
    void Start()
    {
        challengeInfos = new List<ChallengeInformation>();
        path = Application.persistentDataPath + @"/challenges.txt";
    }

    public void GenerateChallenges()
    {
        if (!File.Exists(path))
        {
            file = new FileStream(path, FileMode.Create, FileAccess.Write);
            file.Dispose();

            SetupFile(path);
        }

        file = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
        file.Dispose();

        string[] lines = File.ReadAllLines(path);
        parent.GetComponent<RectTransform>().sizeDelta = new Vector2((lines.Length + 1) * (Screen.width / 2), 300);

        for (int i = 0; i < lines.Length; ++i)
        {
            ChallengeInformation info = GetChallengeInformation(lines[i]);
            challengeInfos.Add(info);
            GameObject go = Instantiate(prefab);
            go.transform.SetParent(parent, false);
            go.GetComponent<RectTransform>().localPosition = new Vector3(localXPos, 0);

            Color color = Colors.Blue;
            switch(info.difficulty)
            {
                case Difficulty.Medium:
                    color = Colors.Yellow;
                    break;
                case Difficulty.Hard:
                    color = Colors.Red;
                    break;
            }

            go.GetComponent<Text>().text = info.LevelName;
            go.GetComponent<Text>().color = color;
            go.name = info.LevelName;
            localXPos += Screen.width / 3;
        }

        
        file.Close();
    }

    ChallengeInformation GetChallengeInformation(string line)
    {
        ChallengeInformation info = new ChallengeInformation();
        string[] split = line.Split('\t');

        info.LevelName = split[0];
        info.Letters = new char[3]{ split[1][0], split[2][0], split[3][0] };
        
        switch(split[4])
        {
            case "E":
                info.difficulty = Difficulty.Easy;
                break;
            case "M":
                info.difficulty = Difficulty.Medium;
                break;
            case "H":
                info.difficulty = Difficulty.Hard;
                break;
        }

        info.Target = int.Parse(split[5]);

        if (split[6] == "Y")
        {
            info.Completed = true;
        }
        else
        {
            info.Completed = false;
        }

        info.Time = int.Parse(split[7]);

        return info;
    }

    private void OnApplicationQuit()
    {
        file = new FileStream(path, FileMode.Open, FileAccess.Write);
        file.Dispose();
        List<string> lines = new List<string>();

        foreach (var info in challengeInfos)
        {
            string line = string.Empty;
            line += info.LevelName + "\t";
            line += info.Letters[0].ToString() + "\t";
            line += info.Letters[1].ToString() + "\t";
            line += info.Letters[2].ToString() + "\t";
            
            if (info.difficulty == Difficulty.Easy)
            {
                line += "E" + "\t";
            }
            else if (info.difficulty == Difficulty.Medium)
            {
                line += "M" + "\t";
            }
            else
            {
                line += "H" + "\t";
            }

            line += info.Target.ToString() + "\t";
            

            if (info.Completed)
            {
                line += "Y" + "\t";
            }
            else
            {
                line += "N" + "\t";
            }

            line += info.Time.ToString() + "\t";
            lines.Add(line);
        }

        File.WriteAllLines(path, lines.ToArray());

        file.Close();
    }

    private void SetupFile(string path)
    {
        string[] lines = challengesFile.text.Split('\n');

        for (int i = 0; i < lines.Length; ++i)
        {
            lines[i] = lines[i].TrimEnd('\r');
        }

        File.WriteAllLines(path, lines);
        file.Close();
    }

    public ChallengeInformation GetChallengeInfo(string name)
    {
        ChallengeInformation info = new ChallengeInformation();

        foreach (ChallengeInformation ci in challengeInfos)
        {
            if (ci.LevelName == name)
            {
                info = ci;
            }
        }

        return info;
    }

    public void SetChallengeComplete(string levelName)
    {
        for (int i = 0; i < challengeInfos.Count; ++i)
        {
            if (challengeInfos[i].LevelName == levelName)
            {
                challengeInfos[i].Completed = true;
            }
        }
    }
}

public class ChallengeInformation
{
    public string LevelName;
    public char[] Letters;
    public Difficulty difficulty;
    public int Target;
    public bool Completed;
    public int Time;
}

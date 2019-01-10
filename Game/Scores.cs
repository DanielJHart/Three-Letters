using System.IO;
using UnityEngine;

public class Scores
{
    FileStream file;
    string path;

    public Scores()
    {
        path = Application.persistentDataPath + @"/scores.txt";

        if (!File.Exists(path))
        {
            file = new FileStream(path, FileMode.Create, FileAccess.Write);
            file.Dispose();

            SetupFile(path);
        }

        file = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
        file.Dispose();
    }

    ~Scores()
    {
        GameOptions.Close();
        file.Close();
    }

    public int GetBestScore(GameMode gm, Difficulty diff)
    {
        string[] lines = File.ReadAllLines(path);

        for (int i = 0; i < lines.Length; ++i)
        {
            string[] l = lines[i].Split('\t');  // [0] = GameMode, [1] = Difficulty, [2] = Score
            if (l[0] == gm.ToString() && l[1] == diff.ToString())
            {
                return int.Parse(l[2]);
            }
        }

        return 0;
    }

    public void SaveBestScore(GameMode gm, Difficulty diff, int points)
    {
        string[] lines = File.ReadAllLines(path);
        
        for (int i = 0; i < lines.Length; ++i)
        {
            string[] l = lines[i].Split('\t');  // [0] = GameMode, [1] = Difficulty, [2] = Score
            if (l[0] == gm.ToString() && l[1] == diff.ToString())
            {
                lines[i] = gm.ToString() + '\t' + diff.ToString() + '\t' + points.ToString();

                break;
            }
        }

        File.WriteAllLines(path, lines);
    }

    public static void SetupFile(string path)
    {
        string[] lines = 
        {
            "Classic\tEasy\t0",
            "Classic\tMedium\t0",
            "Classic\tHard\t0",
            "TimeTrial\tEasy\t0",
            "TimeTrial\tMedium\t0",
            "TimeTrial\tHard\t0",
        };

        File.WriteAllLines(path, lines);
    }
}

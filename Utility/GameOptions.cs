using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GameOptions
{
    public static int MusicVolume, EffectsVolume;
    private static int MusicHash = "Music".GetHashCode(), EffectsHash = "Effects".GetHashCode();
    private static FileStream file;

    public static void LoadOptions()
    {
        string path = Application.persistentDataPath + @"/options.txt";

        if (!File.Exists(path))
        {
            FileStream createFile = new FileStream(path, FileMode.Create, FileAccess.Write);
            createFile.Dispose();

            string[] createLines =
            {
                "Music\t100",
                "Effects\t100"
            };

            File.WriteAllLines(path, createLines);
            createFile.Close();
        }
        
        file = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
        file.Dispose();

        string[] lines = File.ReadAllLines(path);

        foreach (string line in lines)
        {
            string[] l = line.Split('\t');

            if (l[0].GetHashCode() == MusicHash)
            {
                MusicVolume = int.Parse(l[1]);
            }
            else if (l[0].GetHashCode() == EffectsHash)
            {
                EffectsVolume = int.Parse(l[1]);
            }
        }
    }

    public static void SaveOptions()
    {
        string path = Application.persistentDataPath + @"/options.txt";

        FileStream file = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
        file.Dispose();

        string[] lines = File.ReadAllLines(path);

        for (int i = 0; i < lines.Length; ++i)
        {
            string[] l = lines[i].Split('\t');

            if (l[0].GetHashCode() == MusicHash)
            {
                l[1] = MusicVolume.ToString();
            }
            else if (l[0].GetHashCode() == EffectsHash)
            {
                l[1] = EffectsVolume.ToString();
            }

            lines[i] = l[0] + '\t' + l[1];
        }

        File.WriteAllLines(path, lines);
    }

    public static void Close()
    {
        file.Close();
    }
}

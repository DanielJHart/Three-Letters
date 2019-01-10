using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WordGenerator : MonoBehaviour
{
    [SerializeField]
    TextAsset dict, c;

    List<KeyValuePair<int, char[]>> Letters;
    WordPairComparer comparer;
    DictData dictData;
    private string path;
    string[] lines;
    FileStream file;
    List<Info> infos;

    struct Info
    {
       public int matchingWords;
       public int scrabbleScore;
       public string letters;
    }

    List<ChallengeInformation> cis;

    // Use this for initialization
    void Start()
    {
        ////path = Application.persistentDataPath + @"/challengesBase.txt";
        ////////Letters = new List<KeyValuePair<int, char[]>>();
        ////dictData = new DictData(dict);
        ////////comparer = new WordPairComparer();

        ////////path = Application.persistentDataPath + @"/challengeLettersCombos.txt";

        ////////if (!File.Exists(path))
        ////////{
        ////////    file = new FileStream(path, FileMode.Create, FileAccess.Write);
        ////////    file.Dispose();

        ////////    GenerateLetters();
        ////////    file.Close();
        ////////}
        ////infos = new List<Info>();
        ////cis = new List<ChallengeInformation>();
        ////lines = c.text.Split('\n');
        ////int min = 30;

        ////int mediumStart = 0, hardStart = 0;

        ////for (int i = 0; i < lines.Length; ++i)
        ////{
        ////    string line = lines[i].TrimEnd('\r');
        ////    string[] l = line.Split('\t');

        ////    if (mediumStart == 0 && int.Parse(l[3]) < 2000)
        ////    {
        ////        mediumStart = i;
        ////    }

        ////    if (hardStart == 0 && int.Parse(l[3]) < 500)
        ////    {
        ////        hardStart = i;
        ////        break;
        ////    }
        ////}

        ////int counter = 1;

        ////for (int i = 0; i < 300; i += 10)
        ////{
        ////    ChallengeInformation ci = new ChallengeInformation();
        ////    string line = lines[i].TrimEnd('\r');
        ////    string[] l = line.Split('\t');

        ////    min += 5;
        ////    min = Mathf.Clamp(min, 30, 150);

        ////    ci.Letters = new char[] { l[0][0], l[1][0], l[2][0] };
        ////    ci.LevelName = counter.ToString();
        ////    ci.Target = min;
        ////    ci.difficulty = Difficulty.Easy;
        ////    ci.Time = 60;
        ////    ci.Completed = false;
        ////    cis.Add(ci);

        ////    ++counter;
        ////    ////Info info = new Info();
        ////    ////info.letters = l[0].ToString() + l[1].ToString() + l[2].ToString();
        ////    ////info.matchingWords = int.Parse(l[3]);

        ////    ////List<string> matches = dictData.GetMatchingWords(new Word(l[0][0], l[1][0], l[2][0]));
        ////    ////int total = 0;
        ////    ////foreach (var match in matches)
        ////    ////{
        ////    ////    total += ScrabbleScore(match);
        ////    ////}

        ////    ////info.scrabbleScore = total;
        ////    ////infos.Add(info);
        ////}

        ////min = 30;

        ////for (int i = mediumStart; i < mediumStart + 150; i += 5)
        ////{
        ////    ChallengeInformation ci = new ChallengeInformation();
        ////    string line = lines[i].TrimEnd('\r');
        ////    string[] l = line.Split('\t');

        ////    min += 5;
        ////    min = Mathf.Clamp(min, 30, 125);

        ////    ci.Letters = new char[] { l[0][0], l[1][0], l[2][0] };
        ////    ci.LevelName = counter.ToString();
        ////    ci.Target = min;
        ////    ci.difficulty = Difficulty.Medium;
        ////    ci.Time = 60;
        ////    ci.Completed = false;
        ////    cis.Add(ci);
        ////    ++counter;
        ////}

        ////min = 30;

        ////for (int i = hardStart; i < hardStart + 20; ++i)
        ////{
        ////    ChallengeInformation ci = new ChallengeInformation();
        ////    string line = lines[i].TrimEnd('\r');
        ////    string[] l = line.Split('\t');

        ////    min += 5;
        ////    min = Mathf.Clamp(min, 30, 75);

        ////    ci.Letters = new char[] { l[0][0], l[1][0], l[2][0] };
        ////    ci.LevelName = counter.ToString();
        ////    ci.Target = min;
        ////    ci.difficulty = Difficulty.Hard;
        ////    ci.Time = 60;
        ////    ci.Completed = false;
        ////    cis.Add(ci);
        ////    ++counter;
        ////}

        ////WriteToFile();

        int x = 0;
    }

    private int ScrabbleScore(string s)
    {
        int scrabbleScore = 0;

        for (int i = 0; i < s.Length; ++i)
        {
            switch (s[i])
            {
                case 'A':
                case 'E':
                case 'I':
                case 'O':
                case 'L':
                case 'N':
                case 'R':
                case 'S':
                case 'T':
                case 'U':
                    scrabbleScore += 1;
                    break;
                case 'D':
                case 'G':
                    scrabbleScore += 2;
                    break;
                case 'B':
                case 'C':
                case 'M':
                case 'P':
                    scrabbleScore += 3;
                    break;
                case 'F':
                case 'H':
                case 'V':
                case 'W':
                case 'Y':
                    scrabbleScore += 4;
                    break;
                case 'K':
                    scrabbleScore += 5;
                    break;
                case 'J':
                case 'X':
                    scrabbleScore += 8;
                    break;
                case 'Q':
                case 'Z':
                    scrabbleScore += 10;
                    break;
            }
        }

        return scrabbleScore;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void GenerateLetters()
    {
        Letters = new List<KeyValuePair<int, char[]>>();

        for (int i = System.Convert.ToInt32('A'); i <= System.Convert.ToInt32('Z'); ++i)
        {
            for (int j = System.Convert.ToInt32('A'); j <= System.Convert.ToInt32('Z'); ++j)
            {
                for (int k = System.Convert.ToInt32('A'); k <= System.Convert.ToInt32('Z'); ++k)
                {
                    Word word = new Word((char)i, (char)j, (char)k);

                    int matchCount = dictData.GetMatchingWords(word).Count;
                    if (matchCount > 0)
                    {
                        Letters.Add(new KeyValuePair<int, char[]>(matchCount, new char[] { (char)i, (char)j, (char)k }));
                    }
                }
            }
        }

        Letters.Sort(comparer);

        List<string> linesList = new List<string>();
        foreach (var letter in Letters)
        {
            linesList.Add(letter.Value[0].ToString() + '\t' + letter.Value[1].ToString() + '\t' + letter.Value[2].ToString() + '\t' + letter.Key.ToString());
        }

        File.WriteAllLines(path, linesList.ToArray());
        Debug.Log("Challenge Letters Written To File");
    }

    private void WriteToFile()
    {
        file = new FileStream(path, FileMode.Open, FileAccess.Write);
        file.Dispose();
        List<string> lines = new List<string>();

        foreach (var info in cis)
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
}

class WordPairComparer : IComparer<KeyValuePair<int, char[]>>
{
    public int Compare(KeyValuePair<int, char[]> x, KeyValuePair<int, char[]> y)
    {
        if (x.Key == y.Key)
        {
            return 0;
        }
        else if (x.Key < y.Key)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
}
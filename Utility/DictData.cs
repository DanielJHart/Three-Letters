using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DictData
{
    private Dictionary<char, int> letterIndexes;
    private Dictionary<string, int> combosCounts;
    private string[] words;
    private List<string> matchingWords;

    public DictData(TextAsset ta)
    {
        words = ta.text.Split('\n');
        letterIndexes = new Dictionary<char, int>();
        combosCounts = new Dictionary<string, int>();
    }

    public void Setup()
    {
        letterIndexes.Add('A', 0);
        char lastLetter = 'A';

        for (int i = 0; i < words.Length; ++i)
        {
            string word = words[i];

            if (word[0] != lastLetter)
            {
                letterIndexes.Add(word[0], i);
                ++lastLetter;

                if (lastLetter == 'Z' + 1)
                {
                    break;
                }
            }
        }
    }

    public void CleanLetterCombos()
    {
        for (int i = 0; i < words.Length - 1; ++i)
        {
            string word = words[i].TrimEnd('\r');
            string[] l = word.Split('\t');
            combosCounts.Add(l[0] + l[1] + l[2], int.Parse(l[3]));
        }
    }

    public List<string> GetMatchingWords(Word word)
    {
        List<string> retWords = new List<string>();
        bool wordFinished = false;

        for (int w = 0; w < words.Length; ++w)
        {
            wordFinished = false;

            for (int i = 0; i < words[w].Length - 2; ++i)
            {
                if (words[w][i] == word.A)
                {
                    for (int j = i + 1; j < words[w].Length - 1; ++j)
                    {
                        if (words[w][j] == word.B)
                        {
                            for (int k = j + 1; k < words[w].Length; ++k)
                            {
                                if (words[w][k] == word.C)
                                {
                                    retWords.Add(words[w]);
                                    wordFinished = true;
                                    break;
                                }
                            }
                        }

                        if (wordFinished)
                        {
                            break;
                        }
                    }
                }

                if (wordFinished)
                {
                    break;
                }
            }
        }

        matchingWords = retWords;
        return retWords;
    }

    public int GetMatchingNumberOfWords(Word word)
    {
        int ret = 0;

        string search = word.A.ToString() + word.B.ToString() + word.C.ToString();

        if (combosCounts.ContainsKey(search))
        {
            ret = combosCounts[search];
        }

        return ret;
    }

    public bool IsWordValid(string word)
    {
        return matchingWords.Contains(word.ToUpper());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Word
{
    public Word()
    {
        
    }

    public Word(char a, char b, char c)
    {
        A = a;
        B = b;
        C = c;
        CombinedLetters = string.Empty;
        CombinedLetters += a;
        CombinedLetters += b;
        CombinedLetters += c;
    }

    public char A, B, C;
    public string CombinedLetters;
    public List<string> MatchingWords;
}

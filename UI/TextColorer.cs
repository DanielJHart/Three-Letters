using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TextColorer : MonoBehaviour
{
    [SerializeField]
    private InputField inputField;

    [SerializeField]
    private Text text, A, B, C;

    [SerializeField]
    private LetterBox lbA, lbB, lbC;

    private AudioManager audio;
    private bool aFound = false, bFound = false, cFound = false, wordComplete = false;
    private Game game;

    // Use this for initialization
    void Start()
    {
        inputField.onValueChanged.AddListener(TextChanged);
        game = FindObjectOfType<Game>();
        game.Correct.AddListener(SetComplete);
        audio = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Reset()
    {
        lbA.Reset();
        lbB.Reset();
        lbC.Reset();
        aFound = false;
        bFound = false;
        cFound = false;
    }

    public void TextChanged(string word)
    {
        word = word.ToUpper();
        string replacementText = string.Empty;
        replacementText = word;

        int a = word.IndexOf(A.text);

        if (a >= 0)
        {
            // A is found
            if (!aFound)
            {
                aFound = true;
                lbA.LetterTyped();
                audio.PlaySoundEffect("ATyped");
            }
            
            int b = -1;
            int[] bPositions = GetPositionsOf(B.text[0], word);

            foreach (int pos in bPositions)
            {
                if (pos > a)
                {
                    b = pos;
                    break;
                }
            }

            if (b > a)
            {
                if (!bFound)
                {
                    bFound = true;
                    lbB.LetterTyped();
                    audio.PlaySoundEffect("BTyped");
                }

                int c = -1;
                int[] cPositions = GetPositionsOf(C.text[0], word);

                foreach (int pos in cPositions)
                {
                    if (pos > b)
                    {
                        c = pos;
                        break;
                    }
                }

                if (c > b)
                {
                    if (!cFound)
                    {
                        cFound = true;
                        lbC.LetterTyped();
                        audio.PlaySoundEffect("CTyped");
                    }

                    replacementText = replacementText.Insert(c + 1, "</color>");
                    replacementText = replacementText.Insert(c, "<color=#237de8>");
                }
                else
                {
                    if (cFound)
                    {
                        cFound = false;
                        lbC.Reset();
                    }
                }

                replacementText = replacementText.Insert(b + 1, "</color>");
                replacementText = replacementText.Insert(b, "<color=#eeba4c>");
            }
            else
            {
                if (bFound)
                {
                    bFound = false;
                    lbC.Reset();
                    lbB.Reset();
                }
            }

            replacementText = replacementText.Insert(a + 1, "</color>");
            replacementText = replacementText.Insert(a, "<color=#e3493b>");
        }
        else
        {
            if (!wordComplete)
            {
                // A not found
                if (aFound)
                {
                    Reset();
                }
            }
            else
            {
                wordComplete = false;
            }
        }

        text.text = replacementText;
    }

    private void SetComplete()
    {
        wordComplete = true;
        aFound = false;
    }

    private int[] GetPositionsOf(char c, string word)
    {
        List<int> positions = new List<int>();

        for (int i = 0; i < word.Length; ++i)
        {
            if (word[i] == c)
            {
                positions.Add(i);
            }
        }

        return positions.ToArray();
    }
}

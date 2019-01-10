using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    [SerializeField]
    private List<Image> images;

    [SerializeField]
    private List<Text> texts;

    private List<Color> colors;

    [SerializeField]
    private Color correct, incorrect;

    private bool fading = false;

    private float t;

    // Use this for initialization
    void Start()
    {
        colors = new List<Color>();

        foreach (Image img in images)
        {
            colors.Add(img.color);
        }

        foreach (Text txt in texts)
        {
            colors.Add(txt.color);
        }

        Game game = FindObjectOfType<Game>();
        game.Correct.AddListener(Correct);
        game.Incorrect.AddListener(Incorrect);
    }

    // Update is called once per frame
    void Update()
    {
        if (fading)
        {
            t += Time.deltaTime;

            foreach (Image img in images)
            {
                img.color = correct;
            }

            foreach (Text txt in texts)
            {
                txt.color = correct;
            }
        }
    }

    private void Correct()
    {
        foreach (Image img in images)
        {
            img.color = correct;
        }

        foreach (Text txt in texts)
        {
            txt.color = correct;
        }
    }

    private void Incorrect()
    {
        foreach (Image img in images)
        {
            img.color = incorrect;
        }

        foreach (Text txt in texts)
        {
            txt.color = incorrect;
        }
    }
}

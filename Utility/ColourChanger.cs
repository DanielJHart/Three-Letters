using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColourChanger : MonoBehaviour {

    public Color color;
    float a = 0f;
    bool running = false;
    Image image;
    float timer;

	// Use this for initialization
	void Start ()
    {
        image = GetComponent<Image>();
        color.a = a;
        image.color = color;
	}

    private void OnEnable()
    {
        if (!image)
        {
            image = GetComponent<Image>();
        }

        a = 1;
        color.a = a;
        image.color = color;
        running = true;
        timer = Time.time;
    }

    // Update is called once per frame
    void Update ()
    {
        if (running)
        {
            if (Time.time - timer > 1)
            {
                a -= 0.05f;

                if (a < 0)
                {
                    running = false;
                    gameObject.SetActive(false);
                }
                else
                {
                    color.a = a;
                    image.color = color;
                }
            }
        }
	}

    public void ChangeColour()
    {
        a = 1;
        color.a = a;
        image.color = color;
        running = true;
        timer = Time.time;
    }
}

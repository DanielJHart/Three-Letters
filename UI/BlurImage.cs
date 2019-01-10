using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlurImage : MonoBehaviour
{
    private bool isRunning = false, entering = true;
    Image image;
    private Material mat;
    private float f;

    // Use this for initialization
    void Start()
    {
        image = GetComponent<Image>();
        mat = image.material;
        f = 0;
        mat.SetFloat("_Size", f);
        image.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            if (entering)
            {
                f += 0.1f;
            }
            else
            {
                f -= 0.1f;
            }

            if (f > 0 && f < 3)
            {
                mat.SetFloat("_Size", f);
            }
            else
            {
                isRunning = false;
                if (f <= 0)
                {
                    image.enabled = false;
                }
            }
        }
    }

    public void Setup()
    {
        f = 0;
        entering = true;
        isRunning = true;
        image.enabled = true;
    }

    public void Close()
    {
        f = 3;
        entering = false;
        isRunning = true;
    }
}

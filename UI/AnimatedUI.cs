using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.UI;

public class AnimatedUI : MonoBehaviour
{
    [SerializeField]
    public string Name;

    public float ANIMSPEED = 1.5f;

    public AnimationCurve AC;

    [SerializeField]
    private Vector2 startPos, endPos;

    private Vector2 originalStartPos, originalEndPos;

    private bool isEntering = false, isExiting = false;
    private RectTransform rt;
    private float time;


    public UnityEvent completed;

    [SerializeField]
    private bool WillScale;

    [SerializeField]
    private Vector2 startScale = new Vector2(1, 1), endScale = new Vector2(1, 1);

    [SerializeField]
    private bool WillFade;

    [SerializeField]
    private float startAlpha = 1, endAlpha = 1;

    private int ScreenWidth, ScreenHeight;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
		completed = new UnityEvent();

        ScreenWidth = Screen.width;
        ScreenHeight = Screen.height;

        originalStartPos = startPos;
        originalEndPos = endPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEntering)
        {
            time += ANIMSPEED * Time.deltaTime;
            float f = AC.Evaluate(time);

            Vector2 sPos = new Vector2(startPos.x * ScreenWidth, startPos.y * ScreenHeight);
            Vector2 ePos = new Vector2(endPos.x * ScreenWidth, endPos.y * ScreenHeight);

            Vector2 newPos = Lerp(startPos, endPos, f);
            rt.localPosition = newPos;

            if (WillScale)
            {
                Vector2 newScale = Vector2.Lerp(startScale, endScale, f);
                rt.localScale = newScale;
            }

            if (WillFade)
            {
                float newAlpha = Mathf.Lerp(startAlpha, endAlpha, f);

                Image img = GetComponent<Image>();

                if (img)
                {
                    Color col = img.color;
                    img.color = new Color(col.r, col.g, col.b, newAlpha);
                }
                else
                {
                    Text txt = GetComponent<Text>();

                    if (txt)
                    {
                        Color col = txt.color;
                        txt.color = new Color(col.r, col.g, col.b, newAlpha);
                    }
                }
            }

            if (time >= 1)
            {
                isEntering = false;
                completed.Invoke();
            }
        }

        if (isExiting)
        {
            time += ANIMSPEED * Time.deltaTime;
            float f = AC.Evaluate(time);
            Vector2 newPos = Vector2.Lerp(endPos, startPos, f);
            rt.localPosition = newPos;

            if (f >= 1)
            {
                isExiting = false;
                completed.Invoke();
            }

            if (WillScale)
            {
                Vector2 newScale = Vector2.Lerp(endScale, startScale, f);
                rt.localScale = newScale;
            }

            if (WillFade)
            {
                float newAlpha = Mathf.Lerp(endAlpha, startAlpha, f);

                Image img = GetComponent<Image>();

                if (img)
                {
                    Color col = img.color;
                    img.color = new Color(col.r, col.g, col.b, newAlpha);
                }
                else
                {
                    Text txt = GetComponent<Text>();

                    if (txt)
                    {
                        Color col = txt.color;
                        txt.color = new Color(col.r, col.g, col.b, newAlpha);
                    }
                }
            }
        }
    }

    public void Play()
    {
        if (isExiting)
        {
            isExiting = false;
        }

        Image img = GetComponent<Image>();

        if (img)
        {
            if (WillFade)
            {
                Color col = img.color;
                img.color = new Color(col.r, col.g, col.b, startAlpha);
            }
        }
        else
        {
            Text txt = GetComponent<Text>();

            if (txt)
            {
                Color col = txt.color;
                txt.color = new Color(col.r, col.g, col.b, startAlpha);
            }
        }

        rt.localPosition = startPos;
        isEntering = true;
        isExiting = false;
        time = 0;
    }

    public void Exit()
    {
        if (isEntering)
        {
            isEntering = false;
        }

        Image img = GetComponent<Image>();

        if (img)
        {
            if (WillFade)
            {
                Color col = img.color;
                img.color = new Color(col.r, col.g, col.b, endAlpha);
            }
        }
        else
        {
            Text txt = GetComponent<Text>();

            if (txt)
            {
                Color col = txt.color;
                txt.color = new Color(col.r, col.g, col.b, endAlpha);
            }
        }


        rt.localPosition = endPos;
        isExiting = true;
        isEntering = false;
        time = 0;
    }

    private Vector2 Lerp(Vector2 A, Vector2 B, float t)
    {
        float x = A.x + ((B.x - A.x) * t);
        float y = A.y + ((B.y - A.y) * t);
        return new Vector2(x, y);
    }

    public void Scale(Vector2 sf)
    {
        startPos *= sf;
        endPos *= sf;
    }

    public void SetAtStart()
    {
        rt.localPosition = startPos;

        if (WillScale)
        {
            rt.localScale = startScale;
        }

        if (WillFade)
        {
            Image img = GetComponent<Image>();

            if (img)
            {
                Color col = img.color;
                img.color = new Color(col.r, col.g, col.b, startAlpha);
            }
            else
            {
                Text txt = GetComponent<Text>();

                if (txt)
                {
                    Color col = txt.color;
                    txt.color = new Color(col.r, col.g, col.b, startAlpha);
                }
            }
        }
    }

    public void Set(Vector2 start, Vector2 end, bool reposition)
    {
        bool atStart = true;
        if (reposition)
        {
            if (rt.localPosition == (Vector3)startPos)
            {
                atStart = true;
            }
            else
            {
                atStart = false;
            }
        }

        startPos = start;
        endPos = end;

        if (reposition)
        {
            if (atStart)
            {
                transform.localPosition = startPos;
            }
            else
            {
                transform.localPosition = endPos;
            }
        }
    }

    public void Reset()
    {
        startPos = originalStartPos;
        endPos = originalEndPos;
    }
}

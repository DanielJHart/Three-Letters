using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> scalableObjects;
    private ScreenOrientation currentOri;
    private Vector2 sf;

    public bool Landscape = false;

    // Use this for initialization
    void Start()
    {
        currentOri = Screen.orientation;
        sf = GetComponent<RectTransform>().sizeDelta / new Vector2(639f, 1135f);
        ScaleAtStart();

        if (currentOri == ScreenOrientation.LandscapeLeft || currentOri == ScreenOrientation.LandscapeRight)
        {
            Landscape = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ////if (Input.GetKeyDown(KeyCode.R))
        ////{
        ////    if (currentOri == ScreenOrientation.Portrait)
        ////    {
        ////        currentOri = ScreenOrientation.Landscape;
        ////        Landscape = true;
        ////    }
        ////    else
        ////    {
        ////        currentOri = ScreenOrientation.Portrait;
        ////        Landscape = false;
        ////    }

        ////    Unscale();
        ////    sf = GetComponent<RectTransform>().sizeDelta / new Vector2(639f, 1135f);
        ////    Scale();
        ////    MoveObjects();
        ////}

        if (currentOri != Screen.orientation)
        {
            currentOri = Screen.orientation;
            if (currentOri == ScreenOrientation.LandscapeLeft || currentOri == ScreenOrientation.LandscapeRight)
            {
                Landscape = true;
            }
            else
            {
                Landscape = false;
            }

            Unscale();
            sf = GetComponent<RectTransform>().sizeDelta / new Vector2(639f, 1135f);
            Scale();
            MoveObjects();
        }
    }

    private void ScaleAtStart()
    {
        List<AnimatedUI> animatedUIs = FindObjectsOfType<AnimatedUI>().ToList();
        foreach (AnimatedUI anim in animatedUIs)
        {
            anim.Scale(sf);
        }

        SetAnimsAtStart();

        float width = GetComponent<RectTransform>().sizeDelta.x;
        foreach (var scalableObject in scalableObjects)
        {
            scalableObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, scalableObject.GetComponent<RectTransform>().sizeDelta.y);
        }
    }

    private void Scale()
    {
        List<AnimatedUI> animatedUIs = FindObjectsOfType<AnimatedUI>().ToList();
        foreach (AnimatedUI anim in animatedUIs)
        {
            anim.Scale(sf);
        }

        SetAnimNewPos();

        float width = GetComponent<RectTransform>().sizeDelta.x;
        foreach (var scalableObject in scalableObjects)
        {
            scalableObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, scalableObject.GetComponent<RectTransform>().sizeDelta.y);
        }
    }

    private void Unscale()
    {
        List<AnimatedUI> animatedUIs = FindObjectsOfType<AnimatedUI>().ToList();
        foreach (AnimatedUI anim in animatedUIs)
        {
            anim.Reset();
        }
    }

    private void MoveObjects()
    {
        if (FindObjectOfType<Game>().GetGameState() == GameState.Game)
        {
            AnimatedUIHandler handler = FindObjectOfType<AnimatedUIHandler>();

            if (Landscape)
            {
                handler.GetAnimator("Timer Circle Exit Left Landscape").SetAtStart();
                handler.GetAnimator("Reroll Button Exit Landscape").SetAtStart();
            }
            else
            {
                handler.GetAnimator("Timer Circle Exit Left").SetAtStart();
                handler.GetAnimator("Reroll Button Exit").SetAtStart();
                handler.GetAnimator("A Image Exit Left").SetAtStart();
                handler.GetAnimator("C Image Exit Left").SetAtStart();
            }
        }
    }

    private void SetAnimNewPos()
    {
        Debug.Log(sf);
        FindObjectOfType<AnimatedUIHandler>().SetAnimatorsNewPosition(
            sf,
            "Points Enter",
            "A Image Enter",
            "B Image Enter",
            "C Image Enter",
            "Game Bar Enter",
            "Timer Circle Enter",
            "Reroll Button Enter",
            "Burger Enter",
            "Easy Enter",
            "Medium Enter",
            "Hard Enter",
            "Difficulty Close Button Enter",
            "Classic Enter",
            "Time Trial Enter",
            "Challenge Enter",
            "Red Bar Enter",
            "Options Enter",
            "Continue Enter",
            "Watch Ad Text Enter",
            "Watch Ad Enter",
            "No Thanks Enter",
            "New High Score Text Enter",
            "Challenge Complete Enter",
            "Challenge Failed Enter",
            "Score Text Enter",
            "Points Text Enter",
            "Best Score Text Enter",
            "Best Points Text Enter",
            "Times Text Enter",
            "Up Text Enter",
            "Challenges Enter",
            "Pause Enter",
            "Welcome Complete Enter",
            "Instructions Enter",
            "Challenges Text Enter",
            "Logo Exit"
            );
    }

    private void SetAnimsAtStart()
    {
        FindObjectOfType<AnimatedUIHandler>().SetAnimatorsAtStart(
            "Points Enter",
            "A Image Enter",
            "B Image Enter",
            "C Image Enter",
            "Game Bar Enter",
            "Timer Circle Enter",
            "Reroll Button Enter",
            "Burger Enter",
            "Easy Enter",
            "Medium Enter",
            "Hard Enter",
            "Difficulty Close Button Enter",
            "Classic Enter",
            "Time Trial Enter",
            "Challenge Enter",
            "Red Bar Enter",
            "Options Enter",
            "Continue Enter",
            "Watch Ad Text Enter",
            "Watch Ad Enter",
            "No Thanks Enter",
            "New High Score Text Enter",
            "Challenge Complete Enter",
            "Challenge Failed Enter",
            "Score Text Enter",
            "Points Text Enter",
            "Best Score Text Enter",
            "Best Points Text Enter",
            "Times Text Enter",
            "Up Text Enter",
            "Challenges Enter",
            "Pause Enter",
            "Welcome Complete Enter",
            "Instructions Enter",
            "Challenges Text Enter"
            );
    }
}

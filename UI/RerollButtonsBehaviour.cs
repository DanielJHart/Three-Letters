using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RerollButtonsBehaviour : MonoBehaviour
{
    [SerializeField]
    GameObject rerollButton, rerollA, rerollB, rerollC, A, B, C;

    [SerializeField]
    Sprite rerollNormal, rerollSelected, rerollBox, normalBox;
    
    private AnimatedUIHandler anim;
    private Game game;
    private bool rerolling = false;

    // Use this for initialization
    void Start()
    {
        game = FindObjectOfType<Game>();
        anim = FindObjectOfType<AnimatedUIHandler>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Reset()
    {
        rerollButton.GetComponent<Image>().rectTransform.localScale = new Vector3(1, 1, 1);
        anim.RemoveListenersFrom("Timer Circle Exit Left Centered");
        rerolling = false;
    }

    public void EnterReroll()
    {
        if (!rerolling)
        {
            game.Pause();
            game.EnterReroll();
            rerollButton.GetComponent<Image>().sprite = rerollSelected;
            A.GetComponent<Image>().sprite = rerollBox;
            A.GetComponent<Image>().color = Colors.Red;
            B.GetComponent<Image>().sprite = rerollBox;
            B.GetComponent<Image>().color = Colors.Red;
            C.GetComponent<Image>().sprite = rerollBox;
            C.GetComponent<Image>().color = Colors.Red;
            rerollA.SetActive(true);
            rerollB.SetActive(true);
            rerollC.SetActive(true);
            rerolling = true;

            foreach (var w in FindObjectsOfType<Wiggle>())
            {
                w.Begin();
            }
        }
        else
        {
            game.Resume();
            game.ExitReroll(false);
            rerollButton.GetComponent<Image>().sprite = rerollNormal;
            A.GetComponent<Image>().sprite = normalBox;
            A.GetComponent<Image>().color = Colors.OffWhite;
            B.GetComponent<Image>().sprite = normalBox;
            B.GetComponent<Image>().color = Colors.OffWhite;
            C.GetComponent<Image>().sprite = normalBox;
            C.GetComponent<Image>().color = Colors.OffWhite;
            rerollA.SetActive(false);
            rerollB.SetActive(false);
            rerollC.SetActive(false);
            rerolling = false;

            foreach (var w in FindObjectsOfType<Wiggle>())
            {
                w.Stop();
            }
        }
    }

    public void RerollLetter(int letter)
    {
        game.RerollLetter(letter);
        game.Resume();
        game.ExitReroll(false);
        rerollButton.GetComponent<Image>().sprite = rerollNormal;
        A.GetComponent<Image>().sprite = normalBox;
        A.GetComponent<Image>().color = Colors.OffWhite;
        B.GetComponent<Image>().sprite = normalBox;
        B.GetComponent<Image>().color = Colors.OffWhite;
        C.GetComponent<Image>().sprite = normalBox;
        C.GetComponent<Image>().color = Colors.OffWhite;
        rerollA.SetActive(false);
        rerollB.SetActive(false);
        rerollC.SetActive(false);

        if (!FindObjectOfType<Scaler>().Landscape)
        {
            anim.PlayAnimation("Center Timer Circle", 0.1f);
            anim.PlayAnimation("Hide Reroll Button", 0.1f);
        }
        else
        {
            anim.PlayAnimation("Hide Reroll Button Landscape", 0.1f);
        }

        rerolling = false;

        foreach (var w in FindObjectsOfType<Wiggle>())
        {
            w.Stop();
        }
    }
}

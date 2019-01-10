using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform rt;

    [SerializeField]
    private UnityEvent trigger, firstGame;
    private float alpha = 0, timer = 0;
    private bool fadingIn = false, latched = false, playedBefore = true;

    // Use this for initialization
    void Start()
    {
        rt = GetComponent<RectTransform>();
        FindObjectOfType<LogoOpening>().LogoAnimationComplete.AddListener(FadeIn);
    }

    // Update is called once per frame
    void Update()
    {
        if (fadingIn)
        {
            alpha += 0.015f;
            Image img = GetComponent<Image>();
            img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);

            if (alpha >= 1)
            {
                fadingIn = false;
            }
        }

        if (latched)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                latched = false;
            }
        }
    }

    public void FadeIn()
    {
        fadingIn = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!fadingIn && !latched)
        {
            rt.localScale = Vector3.one;
            Animator anim = GetComponent<Animator>();
            anim.Play("PlayButtonAnimTest");
            latched = true;
            timer = 10;

            if (playedBefore)
            {
                trigger.Invoke();
                FindObjectOfType<MenuStateManager>().SetState(MenuState.Main);
            }
            else
            {
                FindObjectOfType<MenuStateManager>().SetState(MenuState.Play);
                firstGame.Invoke();
            }

            FindObjectOfType<AudioManager>().PlaySoundEffect("Blip");
        }
    }

    public void SetNotPlayedBefore()
    {
        playedBefore = false;
    }
}

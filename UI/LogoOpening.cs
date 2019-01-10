using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LogoOpening : MonoBehaviour
{
    private Animator animator;
    private float timer;
    private bool hasFiredEvent = false;
    public UnityEvent LogoAnimationComplete;
    private bool playing = false, playedBefore = true;
    private Image img;
    AudioManager audio;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        img = GetComponent<Image>();
        audio = FindObjectOfType<AudioManager>();
        timer = 0.5f;
        ////clipLength = animator.GetCurrentAnimatorStateInfo(0).length;
    }

    // Update is called once per frame
    void Update()
    {
        if (animator)
        {
            if (playing && !hasFiredEvent)
            {
                if (img.sprite.name == "LOGO 2_00007")
                {
                    audio.PlaySoundEffect("ATyped");
                }
                else if (img.sprite.name == "LOGO 2_00010")
                {
                    audio.PlaySoundEffect("BTyped");
                }
                else if (img.sprite.name == "LOGO 2_00013")
                {
                    audio.PlaySoundEffect("CTyped");
                }
                else if (img.sprite.name == "LOGO 2_00053")
                {
                    audio.PlaySoundEffect("Good");
                }
                else if (img.sprite.name == "LOGO 2_00073")
                {
                    timer -= Time.deltaTime;

                    if (timer < 0)
                    {
                        hasFiredEvent = true;
                        FindObjectOfType<MenuStateManager>().SetState(MenuState.Main);

                        if (playedBefore)
                        {
                            FindObjectOfType<Menu>().PressPlay();

                        }
                        else
                        {
                            FindObjectOfType<Menu>().OpenWelcomeGame();
                        }
                    }
                }
            }
            else
            {
                timer -= Time.deltaTime;

                if (timer < 0)
                {
                    animator.Play("NewLogoAnim");
                    playing = true;
                    timer = 1f;
                }

            }
        }
        else
        {
            timer -= Time.deltaTime;
            LogoAnimationComplete.Invoke();

            if (timer < 0 && !hasFiredEvent)
            {
                hasFiredEvent = true;
                FindObjectOfType<MenuStateManager>().SetState(MenuState.Main);

                if (playedBefore)
                {
                    FindObjectOfType<Menu>().PressPlay();

                }
                else
                {
                    FindObjectOfType<Menu>().OpenWelcomeGame();
                }
            }
        }
    }

    public void SetNotPlayedBefore()
    {
        playedBefore = false;
    }
}

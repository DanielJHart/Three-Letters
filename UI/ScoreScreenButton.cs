using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScoreScreenButton : ImageButton 
{
    Animator animator;
    bool latched = false, clickable = true;
    float timer = 0;

	// Use this for initialization
	override public void Start () 
    {
        base.Start();
        animator = GetComponent<Animator>();

        FindObjectOfType<Menu>().ScoreMenuOpen.AddListener(PlayAnimation);
	}
	
	// Update is called once per frame
	override public void Update () 
    {
        base.Update();

        if (latched)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                latched = false;
            }
        }
	}

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!latched && clickable)
        {
            base.OnPointerDown(eventData);
            transform.localScale = new Vector3(0.9f, 0.9f, 1);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!latched && clickable)
        {
            base.OnPointerUp(eventData);
            transform.localScale = Vector3.one;
            Latch();
        }
    }

    private void Latch()
    {
        ScoreScreenButton[] buttons = FindObjectsOfType<ScoreScreenButton>();
        foreach (var button in buttons)
        {
            button.SetLatched();
        }

        BigButton[] btns = FindObjectsOfType<BigButton>();
        foreach (var button in btns)
        {
            button.SetLatched();
        }
    }

    public void SetLatched()
    {
        latched = true;
        timer = 2;
    }

    public void SetClickable(bool clickable)
    {
        this.clickable = clickable;
    }

    private void PlayAnimation()
    {
        transform.localScale = Vector3.one;
        animator.Play("Animation");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BigButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    protected UnityEvent Trigger;
    
    protected Image image;
    protected Text text;
    protected Color color;

    [SerializeField]
    protected List<AnimatedUI> uis;

    protected bool Latched = false;
    protected float timeLeft = 0;

    // Use this for initialization
    void Start()
    {
        image = GetComponent<Image>();
        color = image.color;

        text = transform.parent.Find("Text").GetComponent<Text>();
    }

    public void Update()
    {
        if (Latched)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                Latched = false;
            }
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (!Latched)
        {
            image.color = Colors.White;
            text.color = color;
        }
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (!Latched)
        {
            transform.parent.SetAsLastSibling();
            image.color = color;
            text.color = Colors.White;
            uis[uis.Count - 1].completed.AddListener(InvokeTrigger);
            FindObjectOfType<Menu>().MenuClosed.AddListener(Reset);

            foreach (AnimatedUI ui in uis)
            {
                StartCoroutine(WaitForOpen(0.1f, ui));
            }

            Latch();

            FindObjectOfType<AudioManager>().PlaySoundEffect("Blip");
        }
    }

    protected virtual void Reset()
    {
        uis[uis.Count - 1].completed.RemoveListener(Trigger.Invoke);
        FindObjectOfType<Menu>().MenuClosed.RemoveListener(Reset);
        foreach (AnimatedUI ui in uis)
        {
            ui.Exit();
        }
    }

    private void Latch()
    {
        BigButton[] buttons = FindObjectsOfType<BigButton>();
        foreach (var button in buttons)
        {
            button.SetLatched();
        }

        ScoreScreenButton[] btns = FindObjectsOfType<ScoreScreenButton>();
        foreach (var button in btns)
        {
            button.SetLatched();
        }
    }

    private void InvokeTrigger()
    {
        uis[uis.Count - 1].completed.RemoveAllListeners();
        Trigger.Invoke();
    }

    public void SetLatched()
    {
        Latched = true;
        timeLeft = 2;
    }

    private IEnumerator WaitForOpen(float time, AnimatedUI anim)
    {
        yield return new WaitForSeconds(time);
        anim.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler 
{
    private Image image;
    private RectTransform rectTransform;

    [SerializeField]
    private UnityEvent trigger;

	// Use this for initialization
	public virtual void Start ()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	public virtual void Update ()
    {
		
	}

    public virtual void OnPointerDown(PointerEventData eventData)
    {

    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        trigger.Invoke();
        FindObjectOfType<AudioManager>().PlaySoundEffect("Blip");
    }
}

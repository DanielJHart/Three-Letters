using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PauseButton : BigButton
{
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!Latched)
        {
            Trigger.Invoke();
            SetLatched();
        }
    }

    private void Start()
    {
        
    }
}

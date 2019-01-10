using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DifficultyButton : BigButton
{
    [SerializeField]
    private Difficulty difficulty;

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        GameInfo.SetDifficulty(difficulty);
        uis[0].completed.AddListener(GoToGame);
        FindObjectOfType<Menu>().MenuClosed.AddListener(Reset);
    }

    private void GoToGame()
    {
        FindObjectOfType<Menu>().CloseDifiiculty();
    }

    override protected void Reset()
    {
        base.Reset();
        uis[0].completed.RemoveListener(GoToGame);
    }
}

using UnityEngine.EventSystems;

public class PauseMenuButton : BigButton
{
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        FindObjectOfType<Menu>().MenuClosed.AddListener(Reset);
    }

    private void Reset()
    {
        uis[0].completed.RemoveListener(Trigger.Invoke);
        FindObjectOfType<Menu>().MenuClosed.RemoveListener(Reset);
        foreach (AnimatedUI ui in uis)
        {
            ui.Exit();
        }
    }
}

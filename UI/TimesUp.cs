using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimesUp : MonoBehaviour
{
    [SerializeField]
    private AnimatedUI EnterUI, ExitUI;

    // Use this for initialization
    void Start()
    {
        FindObjectOfType<Game>().TimesUp.AddListener(Enter);
        FindObjectOfType<Game>().EndTimesUp.AddListener(Exit);
    }

    private void Enter()
    {
        FindObjectOfType<Menu>().TimesUp();
    }

    private void Exit()
    {
        ExitUI.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

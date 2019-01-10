using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStateManager : MonoBehaviour
{
    MenuState menuState;

	// Use this for initialization
	void Start ()
    {
        menuState = MenuState.Play;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public MenuState GetState()
    {
        return menuState;
    }

    public void SetState(MenuState state)
    {
        menuState = state;
    }
}

public enum MenuState
{
    Play,

    Main,

    Difficulty,

    Challenges
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SystemHandler : MonoBehaviour
{
    [SerializeField]
    private Image img;

    [SerializeField]
    private GameObject game;

	// Use this for initialization
	void Start ()
    {
		if (SystemInfo.systemMemorySize >= 900)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            game.SetActive(true);
            img.enabled = true;
        }

        Debug.Log("RAM: " + SystemInfo.systemMemorySize);
        Debug.Log("VRAM: " + SystemInfo.graphicsMemorySize);
    }
}

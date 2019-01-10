using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChallengeButton : MonoBehaviour
{
    Button btn;
    Menu menu;
    private bool latched = false;
    float timer = 0;

    // Use this for initialization
    void Start()
    {
        btn = GetComponent<Button>();
        menu = FindObjectOfType<Menu>();
        btn.onClick.AddListener(Clicked);
    }

    private void Clicked()
    {
        if (!latched)
        {
            FindObjectOfType<Game>().SetChallengeInfo(gameObject.name);
            GetComponent<ChallengeInformationText>().SetUpdatingFalse();
            FindObjectOfType<Game>().SetGameMode(GameMode.Challenge);
            FindObjectOfType<Game>().SetupGame();
            menu.CloseChallenges();
            menu.GoToGame();
            latched = true;
            timer = 2;
            FindObjectOfType<AudioManager>().PlaySoundEffect("Blip");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (latched)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                latched = false;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MenuButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum ButtonType { CLASSIC, TIMETRIAL, CHALLENGE, OPTIONS, CHALLENGELEVEL };

    private Text text;
    private Color color;
    private RectTransform rt;
    private Menu menu;
    private bool latched = false;
    private float timer = 0;
    private MenuStateManager menuStateManager;

    [SerializeField]
    private Color SelectedColor;

    [SerializeField]
    private ButtonType type;

    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
        color = text.color;
        text.color = new Color(color.r, color.g, color.b, 0);
        rt = GetComponent<RectTransform>();
        menu = FindObjectOfType<Menu>();
        menuStateManager = FindObjectOfType<MenuStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (latched)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                latched = false;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        MenuState ms = menuStateManager.GetState();
        if (!latched && menuStateManager.GetState() == MenuState.Main)
        {
            rt.localScale = new Vector3(0.9f, 0.9f, 1);
            text.color = SelectedColor;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!eventData.dragging)
        {
            if (!latched && menuStateManager.GetState() == MenuState.Main)
            {
                rt.localScale = Vector3.one;
                text.color = color;

                switch (type)
                {
                    case ButtonType.CLASSIC:
                        StartCoroutine(WaitForDifficulties(0.1f));
                        GameInfo.SetGameMode(GameMode.Classic);
                        break;
                    case ButtonType.TIMETRIAL:
                        StartCoroutine(WaitForDifficulties(0.1f));
                        GameInfo.SetGameMode(GameMode.TimeTrial);
                        break;
                    case ButtonType.CHALLENGE:
                        menu.OpenChallenges();
                        GameInfo.SetGameMode(GameMode.Challenge);
                        break;
                    case ButtonType.OPTIONS:
                        menu.OpenPauseMenu();
                        FindObjectOfType<BlurImage>().Setup();
                        break;
                    case ButtonType.CHALLENGELEVEL:
                        FindObjectOfType<Game>().SetChallengeInfo(gameObject.name);
                        GetComponent<ChallengeInformationText>().SetUpdatingFalse();
                        menu.CloseChallenges();
                        menu.GoToGame();
                        break;
                    default:
                        break;
                }

                Latch();
                FindObjectOfType<AudioManager>().PlaySoundEffect("Blip");
            }
        }
    }

    private void Latch()
    {
        MenuButton[] buttons = FindObjectsOfType<MenuButton>();
        foreach (var button in buttons)
        {
            button.SetLatched();
        }
    }

    public void SetLatched()
    {
        latched = true;
        timer = 2;
    }

    private IEnumerator WaitForDifficulties(float time)
    {
        yield return new WaitForSeconds(time);
        menu.OpenDifficulty();
    }
}

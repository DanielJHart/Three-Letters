using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeInformationText : MonoBehaviour
{
    private Text text;
    private Color color;
    private float alpha;
    public bool updating = true;

    public Image tick;

    // Use this for initialization
    void Start()
    {
        tick.enabled = false;
        text = GetComponent<Text>();
        color = text.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (updating)
        {
            float ans = ((transform.position.x - (Screen.width / 2)) / (Screen.width / 2)) * (Mathf.PI / 2);

            if (transform.position.x < 0 || transform.position.x > Screen.width)
            {
                alpha = 0;
            }
            else
            {
                alpha = Mathf.Cos(ans);
            }

            text.color = new Color(color.r, color.g, color.b, alpha);

            float s = Mathf.Clamp(alpha, 0.8f, 1);

            transform.localScale = new Vector3(s, s, s);

            if (tick.isActiveAndEnabled)
            {
                tick.color = new Color(tick.color.r, tick.color.g, tick.color.b, alpha);
                tick.transform.localScale = new Vector3(s, s, s);
            }
        }
    }

    public void SetUpdatingTrue()
    {
        ChallengeInformationText[] cits = FindObjectsOfType<ChallengeInformationText>();
        foreach (var cit in cits)
        {
            cit.updating = true;
        }
    }

    public void SetUpdatingFalse()
    {
        ChallengeInformationText[] cits = FindObjectsOfType<ChallengeInformationText>();
        foreach (var cit in cits)
        {
            cit.updating = false;
        }
    }

    public void UpdateInfo()
    {
        ChallengeInformationText[] cits = FindObjectsOfType<ChallengeInformationText>();
        ChallengeGenerator cg = FindObjectOfType<ChallengeGenerator>();

        foreach (var cit in cits)
        {
            ChallengeInformation info = cg.GetChallengeInfo(cit.gameObject.name);

            if (info.Completed)
            {
                cit.tick.enabled = true;
            }
        }
    }
}

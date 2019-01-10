using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TargetsText : MonoBehaviour
{

    private Text text;
    string activeChallenge = string.Empty;
    private List<ChallengeInformationText> content;


    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
        content = new List<ChallengeInformationText>();
    }

    // Update is called once per frame
    void Update()
    {
        if (content.Count == 0)
        {
            content = FindObjectsOfType<ChallengeInformationText>().ToList();
        }

        foreach (ChallengeInformationText cit in content)
        {
            if (cit.transform.position.x > Screen.width / 5 && cit.transform.position.x < (Screen.width / 5) * 4 && cit.name != activeChallenge)
            {
                activeChallenge = cit.name;
                ChallengeInformation info = FindObjectOfType<ChallengeGenerator>().GetChallengeInfo(cit.name);
                text.text = info.Letters[0].ToString() + info.Letters[1].ToString() + info.Letters[2].ToString() + "\n" + info.Target + "pts\n" + info.Time + "s";
            }
        }
    }
}

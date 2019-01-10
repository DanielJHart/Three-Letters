using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedUIHandler : MonoBehaviour
{
    private Dictionary<int, AnimatedUI> Animators;

    // Use this for initialization
    void Start()
    {
        int x = 0;

        Animators = new Dictionary<int, AnimatedUI>();
        AnimatedUI[] uis = FindObjectsOfType<AnimatedUI>();

        foreach (AnimatedUI ui in uis)
        {
            if (Animators.ContainsKey(ui.Name.GetHashCode()))
            {
                Animators.Add(x, ui);
                ++x;
            }
            else
            {
                Animators.Add(ui.Name.GetHashCode(), ui);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayAnimation(string name, float delay)
    {
        if (Animators.ContainsKey(name.GetHashCode()))
        {
            StartCoroutine(WaitForOpen(Animators[name.GetHashCode()], delay));
        }
        else
        {
            Debug.Log("Can't find animation: " + name);
        }
    }

    public void PlayAnimationExit(string name, float delay)
    {
        if (Animators.ContainsKey(name.GetHashCode()))
        {
            StartCoroutine(WaitForClose(Animators[name.GetHashCode()], delay));
        }
        else
        {
            Debug.Log("Can't find animation: " + name);
        }
    }

    public AnimatedUI GetAnimator(string name)
    {
        return Animators[name.GetHashCode()];
    }

    public void RemoveListenersFrom(string name)
    {
        Animators[name.GetHashCode()].completed.RemoveAllListeners();
    }

    public void SetAnimatorsAtStart(params string[] names)
    {
        foreach (string name in names)
        {
            if (Animators.ContainsKey(name.GetHashCode()))
            {
                Animators[name.GetHashCode()].SetAtStart();
            }
            else
            {
                Debug.Log("Can't find animation: " + name);
            }
        }
    }

    public void SetAnimatorsNewPosition(Vector2 sf, params string[] names)
    {
        foreach (string name in names)
        {
            if (name == "A Image Enter")
            {
                int x = 0;
            }

            if (Animators.ContainsKey(name.GetHashCode()))
            {
                var rt = Animators[name.GetHashCode()].GetComponent<RectTransform>();
                rt.localPosition = new Vector3(rt.localPosition.x * sf.x, rt.localPosition.y * sf.y);
                int x = 0;
            }
            else
            {
                Debug.Log("Can't find animation: " + name);
            }
        }
    }

    private IEnumerator WaitForOpen(AnimatedUI anim, float time)
    {
        yield return new WaitForSeconds(time);
        anim.Play();
    }

    private IEnumerator WaitForClose(AnimatedUI anim, float time)
    {
        yield return new WaitForSeconds(time);
        anim.Exit();
    }
}

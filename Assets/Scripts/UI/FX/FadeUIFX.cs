using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FadeUIFX : MonoBehaviour
{
    [Header("Fade Config")]
    [SerializeField] private AnimationCurve fadeCurve;
    [SerializeField] [Range(0.0f, 20.0f)] private float fadeTransitionInSecs = 1.0f;
    private BaseFadeFX[] canFadeUIArray;

    private void Awake()
    {
        canFadeUIArray = FindFadeUIInChildren();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(FadeOutRoutine(null));
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(FadeInRoutine(null));
        }
    }

    private BaseFadeFX[] FindFadeUIInChildren()
    {
        var list = new List<BaseFadeFX>();
        foreach(Transform t in this.transform)
        {
            var canFade = t.GetComponent<BaseFadeFX>();
            if(canFade)
            {
                list.Add(canFade);
            }
        }

        return list.ToArray();
    }

    private IEnumerator FadeInRoutine(Action endOfAnimationCallback)
    {
        yield return FadeRoutine(false);
        endOfAnimationCallback?.Invoke();
    }

    private IEnumerator FadeOutRoutine(Action endOfAnimationCallback)
    {
        yield return FadeRoutine(true);
        endOfAnimationCallback?.Invoke();
    }

    private IEnumerator FadeRoutine(bool isFadeOut)
    {
        var elapsed = 0.0f;
        while(elapsed <= fadeTransitionInSecs)
        {
            var percentage = elapsed/fadeTransitionInSecs;
            var fadePercentage = isFadeOut ? (1.0f - percentage) : percentage;
            SetAll(fadePercentage);
            yield return new WaitForEndOfFrame();
            elapsed += Time.deltaTime;
        }

        // ensure it completely fades
        SetAll(isFadeOut ? 0.0f : 1.0f);
    }

    protected virtual float GetFadeValue(float fadePercentage)
    {
        return fadeCurve.Evaluate(fadePercentage);
    }

    private void SetAll(float fadePercentage)
    {
        foreach(var item in canFadeUIArray)
        {
            item.SetFadePercentage(GetFadeValue(fadePercentage));
        }
    }
}

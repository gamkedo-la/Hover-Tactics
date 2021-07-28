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
    private bool isFading = false;

    [Header("Debug")]
    [SerializeField] private bool logDebug;

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

    public void Fade(bool isVisible, Action onEndCallback)
    {
        if(isVisible)
        {
            FadeIn(onEndCallback);
        }
        else
        {
            FadeOut(onEndCallback);
        }
    }

    protected void FadeIn(Action onEndCallback)
    {
        if(logDebug) Debug.Log("FadeIn Called");

        if(isFading)
        {
            return;
        }

        if(logDebug) Debug.Log("FadeIn Started");

        isFading = true;
        SetAll(0.0f);
        this.gameObject.SetActive(true);
        StartCoroutine(FadeInRoutine(() => {
            isFading = false;
            onEndCallback?.Invoke();
            if(logDebug) Debug.Log("FadeIn Ended");
        }));
    }

    protected void FadeOut(Action onEndCallback)
    {
        if(logDebug) Debug.Log("FadeOut Called");
        if(isFading)
        {
            return;
        }

        if(logDebug) Debug.Log("FadeOut Started");

        isFading = true;
        SetAll(1.0f);
        this.gameObject.SetActive(true);
        StartCoroutine(FadeOutRoutine(() => {
            if(logDebug) Debug.Log("FadeOut Started");
            isFading = false;
            onEndCallback?.Invoke();
        }));
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
        yield return FadeRoutine(true);
        endOfAnimationCallback?.Invoke();
    }

    private IEnumerator FadeOutRoutine(Action endOfAnimationCallback)
    {
        yield return FadeRoutine(false);
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
        if(canFadeUIArray == null)
        {
            canFadeUIArray = FindFadeUIInChildren();
        }

        foreach(var item in canFadeUIArray)
        {
            item.SetFadePercentage(GetFadeValue(fadePercentage));
        }
    }
}

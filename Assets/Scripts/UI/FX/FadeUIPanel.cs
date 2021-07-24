using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeUIPanel : BaseFadeFX
{
    [Header("Fade Config")]
    [SerializeField] private AnimationCurve fadeCurve;
    
    private Image[] imageArray;
    private TMPro.TMP_Text[] textArray;

    private Color[] imgStartColors;
    private Color[] imageEndColors;

    private Color[] textStartColors;
    private Color[] textEndColors;

    private void Awake()
    {
       InitImagesArray();
       InitTextArray();
    }

    private void InitImagesArray()
    {
        var imageList = new List<Image>();
        GetAllComponentsInChildren<Image>(this.transform, ref imageList);
        imageArray = imageList.ToArray();

        var imageCount = imageArray.Length;
        imgStartColors = new Color[imageCount];
        imageEndColors = new Color[imageCount];
        for(int i = 0; i < imageCount; i++)
        {
            var color = imageArray[i].color;
            imgStartColors[i] = color;
            imageEndColors[i] = new Color(color.r, color.g, color.b, 0.0f);
        }
    }

    private void InitTextArray()
    {
        var textList = new List<TMPro.TMP_Text>();
        GetAllComponentsInChildren<TMPro.TMP_Text>(this.transform, ref textList);
        textArray = textList.ToArray();

        var textCount = textArray.Length;
        textStartColors = new Color[textCount];
        textEndColors = new Color[textCount];
        for(int i = 0; i < textCount; i++)
        {
            var color = textArray[i].color;
            textStartColors[i] = color;
            textEndColors[i] = new Color(color.r, color.g, color.b, 0.0f);
        }
    }
    public override void SetFadePercentage(float fadePercentage)
    {
        SetFadePercentageImages(fadePercentage);
        SetFadePercentageTexts(fadePercentage);
    }

    protected virtual void SetFadePercentageImages(float fadePercentage)
    {
        var imageCount = imageArray.Length;
        for(int i = 0; i < imageCount; i++)
        {
            imageArray[i].color = Color.Lerp(
                imgStartColors[i],
                imageEndColors[i],
                GetFadeValue(fadePercentage));
        }
    }

    protected virtual void SetFadePercentageTexts(float fadePercentage)
    {
        var textCount = textArray.Length;
        for(int i = 0; i < textCount; i++)
        {
            textArray[i].color = Color.Lerp(
                textStartColors[i],
                textEndColors[i],
                GetFadeValue(fadePercentage));
        }
    }

    protected virtual float GetFadeValue(float fadePercentage)
    {
        return fadeCurve.Evaluate(fadePercentage);
    }

    protected static void GetAllComponentsInChildren<TComponent>(Transform parent, ref List<TComponent> list)
    {
        var components = parent.GetComponents<TComponent>();
        foreach(var c in components)
        {
            list.Add(c);
        }

        foreach(Transform child in parent)
        {
            GetAllComponentsInChildren(child, ref list);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Prologue : MonoBehaviour
{
    public string[] beforeMechLines;
    public string[] afterMechLines;
    public Image[] covers;

    [Space]
    public float startDelay = 0.5f;
    public float charDelay = 0.1f;

    [Space]
    public TextMeshProUGUI dlgText;

    [Space]
    public AudioClip[] dlgSounds;

    private float charTimer = 0.0f;

    private int lineIndex = 0;
    private int charIndex = 0;
    private bool mechDisplay = false;

    private bool proceed = false;

    private AudioSource audioSource;

    void Start()
    {
        charTimer = startDelay;
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = 1.5f;
        dlgText.color = Color.white;
    }

    void Update()
    {
        if(Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            if(!mechDisplay
            && lineIndex < beforeMechLines.Length
            && charIndex < beforeMechLines[lineIndex].Length)
            {
                while(charIndex < beforeMechLines[lineIndex].Length)
                {
                    dlgText.text += (beforeMechLines[lineIndex])[charIndex];
                    charIndex++;
                }
            }
            else if(mechDisplay
            && lineIndex < afterMechLines.Length
            && charIndex < afterMechLines[lineIndex].Length)
            {
                while(charIndex < afterMechLines[lineIndex].Length)
                {
                    dlgText.text += (afterMechLines[lineIndex])[charIndex];
                    charIndex++;
                }
            }
            else
            {
                proceed = true;
            }
        }

        if(!mechDisplay)
        {
            if(lineIndex < beforeMechLines.Length)
            {
                if(charIndex < beforeMechLines[lineIndex].Length)
                {
                    if(charTimer <= 0.0f)
                    {
                        dlgText.text += (beforeMechLines[lineIndex])[charIndex];
                        if(SoundFXManager.state) audioSource.PlayOneShot(dlgSounds[UnityEngine.Random.Range(0, dlgSounds.Length - 1)]);
                        charIndex++;
                        charTimer = charDelay;
                    }
                }
                else if(proceed)
                {
                    dlgText.text += "<br><br>";
                    lineIndex++;
                    charIndex = 0;
                    proceed = false;
                }
            }
            else
            {
                lineIndex = 0;
                charIndex = 0;
                mechDisplay = true;
                proceed = false;

                dlgText.text = "";
                dlgText.rectTransform.anchoredPosition = new Vector2(0.0f, 200.0f);
            }
        }
        else if(mechDisplay)
        {
            if(lineIndex < afterMechLines.Length)
            {
                if(lineIndex > 0 && lineIndex <= covers.Length)
                    covers[lineIndex - 1].color = Color.Lerp(covers[lineIndex - 1].color, Color.clear, Time.deltaTime);
                
                if(charIndex < afterMechLines[lineIndex].Length)
                {
                    if(charTimer <= 0.0f)
                    {
                        dlgText.text += (afterMechLines[lineIndex])[charIndex];

                        if(lineIndex <= 1) audioSource.pitch = 1.0f;
                        else if(lineIndex == 2) audioSource.pitch = 2.0f;
                        else if(lineIndex == 3) audioSource.pitch = 0.6f;
                        else audioSource.pitch = 1.5f;

                        if(SoundFXManager.state) audioSource.PlayOneShot(dlgSounds[UnityEngine.Random.Range(0, dlgSounds.Length - 1)]);
                        charIndex++;
                        charTimer = charDelay;
                    }
                }
                else if(proceed)
                {
                    if(lineIndex > 0 && lineIndex <= covers.Length)
                        covers[lineIndex - 1].color = Color.clear;

                    dlgText.text = "";
                    lineIndex++;
                    charIndex = 0;
                    proceed = false;

                    if(lineIndex <= 1) dlgText.color = Color.green;
                    else if(lineIndex == 2) dlgText.color = Color.cyan;
                    else if(lineIndex == 3) dlgText.color = Color.yellow;
                    else dlgText.color = Color.white;
                }
            }
            else
            {
                FadeToScene.Load(GameMenuController.playNight ? "PlayNight" : "Play");
            }
        }

        charTimer -= Time.deltaTime;
    }
}

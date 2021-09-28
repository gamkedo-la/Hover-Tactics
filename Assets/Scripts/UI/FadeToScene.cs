using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeToScene : MonoBehaviour
{
    public float delay = 0.25f;

    private Image image;

    private static FadeToScene instance = null;
    private static bool show = false;
    private static float timer = 0.0f;
    private static string fadeToSceneName = "";

    void Start()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(gameObject.transform.parent);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        image = GetComponent<Image>();
        SetToFullFade();
    }

    void Update()
    {
        if(fadeToSceneName != "" && timer <= 0.0f)
        {
            timer = delay;
            show = true;
            StartCoroutine("LoadSceneDelayed");
        }

        if(timer != 0.0f)
        {
            Color color = image.color;
            if(show)
                color.a = 1.0f - (timer / delay);
            else if (!show)
                color.a = (timer / delay);
            image.color = color;

            timer -= Time.unscaledDeltaTime;
        }
    }

    public static void Load(string sceneName)
    {
        if(timer <= 0.0f && sceneName != "" && !show)
            fadeToSceneName = sceneName;
    }

    IEnumerator LoadSceneDelayed()
    {
        yield return new WaitForSeconds(delay);
        string scene = fadeToSceneName;
        if(scene != "")
        {
            SetToFullFade();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(scene);
            fadeToSceneName = "";
        }
        yield return null;
    }

    void SetToFullFade()
    {
        Color color = image.color;
        color.a = 1.0f;
        image.color = color;
        show = false;
        timer = delay * 2.0f;
    }
}

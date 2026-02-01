using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject creditsPanel;

    public void ButtonPlay()
    {
        StartCoroutine(WaitForTransition());
        
    }

    public void ButtonMenu()
    {
       StartCoroutine(WaitForTransitionMenu()); 
    }

    public IEnumerator WaitForTransition()
    { 
        yield return StartCoroutine(AudioManager.Instance.AnimeTransition());
        SceneManager.LoadScene("GameSceneFinal");
    }

    public IEnumerator WaitForTransitionMenu()
    { 
        yield return StartCoroutine(AudioManager.Instance.AnimeTransition());
        SceneManager.LoadScene("MenuScene");
    }

    public void ButtonSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void ButtonCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void ButtonClose(string panel)
    {
        if (panel == "settings") settingsPanel.SetActive(false);
        if (panel == "credits") creditsPanel.SetActive(false);
    }

    public void ButtonQuit()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
            Debug.Log("Application Exit");
        #else
            Application.Quit();
        #endif
    }
}

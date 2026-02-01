using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject creditsPanel;

    public void ButtonPlay()
    {
         SceneManager.LoadScene("GameScene");
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

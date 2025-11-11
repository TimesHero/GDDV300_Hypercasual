using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuManager: MonoBehaviour
{
    public GameObject settingsPanel;

    public void SwapScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void MuteVolume(bool isMuted)
    {
        if (isMuted)
        {
            AudioListener.volume = 0;
        }else
        {
            AudioListener.volume = 1;
        }
    }

    public void ShowPanel()
    {
        settingsPanel.SetActive(true);
    }
    
    public void HidePanel()
    {
        settingsPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

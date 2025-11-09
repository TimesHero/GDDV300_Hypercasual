using UnityEngine;

public class GameUI : MonoBehaviour
{
    public GameObject pausePanel;
    public void PauseOrUnpase(bool paused)
    {
        Time.timeScale = 0;
        if (paused==false)
            Time.timeScale = 1;
        pausePanel.SetActive(paused);
    }
}

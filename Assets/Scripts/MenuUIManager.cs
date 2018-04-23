using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : MonoBehaviour {

    [SerializeField]
    private GameObject mainMenuPanel;
    [SerializeField]
    private GameObject playPanel;
    [SerializeField]
    private GameObject creditsPanel;
    [SerializeField]
    private GameObject pausePanel;


    public void HideMenus()
    {
        HideMainMenuPanel();
        HidePlayPanel();
    }

    public void HidePlayPanel () {
        playPanel.SetActive(false);
	}
    public void ShowPlayPanel()
    {
        playPanel.SetActive(true);
    }
    public void ShowMainMenuPanel () {
        mainMenuPanel.SetActive(true);
	}

    public void HideMainMenuPanel()
    {
        mainMenuPanel.SetActive(true);
    }

    public void HideCreditsPanel()
    {
         creditsPanel.SetActive(false);
    }
    public void ShowCreditsPanel()
    {
        creditsPanel.SetActive(true);
    }

    public void HidePausePanel()
    {
        pausePanel.SetActive(false);
    }
    public void ShowPausePanel()
    {
        pausePanel.SetActive(true);
    }

}

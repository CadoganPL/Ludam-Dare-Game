﻿using System;
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
    [SerializeField]
    private GameObject howToPlayPanel;
    [SerializeField]
    private AudioClip mainBGM;
    public AudioClip gameBGM;
    public AudioSource source;

    //yes, this is shitcode. Written fast to do the job, not be pretty.

    void Awake()
    {
        source = GameObject.Find("Main Camera").GetComponent<AudioSource>();
    }

    public void HideMenus()
    {
        HideMainMenuPanel();
        HidePlayPanel();
        HideCreditsPanel();
        HideHowToPlayPanel();
    }

    public void HideHowToPlayPanel()
    {
        howToPlayPanel.SetActive(false);
    }
    
    public void ShowHowToPlayPanel()
    {
        HideMenus();
        howToPlayPanel.SetActive(true);
    }

    public void HidePlayPanel () {
        playPanel.SetActive(false);
	}
    public void ShowPlayPanel()
    {
        HideMenus();
        playPanel.SetActive(true);
    }
    public void ShowMainMenuPanel () {
        if(source.clip!=mainBGM)
        {
            source.clip = mainBGM;
            source.Play();
        }
        HideMenus();
        mainMenuPanel.SetActive(true);
	}

    public void HideMainMenuPanel()
    {
        mainMenuPanel.SetActive(false);
    }

    public void HideCreditsPanel()
    {
         creditsPanel.SetActive(false);
    }
    public void ShowCreditsPanel()
    {
        HideMenus();
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

    public void MuteAudio()
    {
        AudioListener.volume = 0;
    }
    public void ResumeAudio()
    {
        AudioListener.volume = 1;
    }


}

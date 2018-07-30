using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfigComp : MonoBehaviour {

    [Header("Game Sounds")]

    [SerializeField]
    [Tooltip("Soundtrack On/Off")]
    public bool soundtrack = true;

    [SerializeField]
    [Tooltip("Sound effects On/Off")]
    public bool soundEffects = true;

    [Header("GameObjects References")]

    [SerializeField]
    [Tooltip("Levels AudioClip Reference")]
    public AudioClip[] levelAudioClip;

    [SerializeField]
    [Tooltip("GameOver AudioClip Reference")]
    public AudioClip gameOverAudioClip;

    private AudioSource audioSource;

    private int currentIndexAudioSource;
    private bool currentSoundtrackConfig;

    [Header("Current game info:")]

    private bool shieldActivated = false;
    public bool IsShieldActivated()
    {
        return shieldActivated;
    }
    public void ActivateShield()
    {
        shieldActivated = true;
    }
    public void DeactivateSield()
    {
        shieldActivated = false;
    }

    //public static int countAsteroids = 0;

    public int numTypeShield = 0;

    public int numTypeBonus = 0;

    public static bool activedDebug = true;

    public static ConfigComp configComp = null;

    private void Awake()
    {
        if (configComp != null)
        {
            Destroy(gameObject);
        }
        else
        {
            configComp = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentIndexAudioSource = -1;
        //ConfigComp.PrintDebug("ConfigComp.Start soundtrack " + soundtrack);
        //ConfigComp.PrintDebug("ConfigComp.Start soundEffects " + soundEffects);
        currentSoundtrackConfig = soundtrack;
        if (soundtrack)
        {
            playSoundByScene();
        }

        initValues();
    }

    public void initValues()
    {
        numTypeBonus = 0;
        shieldActivated = false;
    }
    // Update is called once per frame
    void Update () {
        if (audioSource.isPlaying && !soundtrack)
            audioSource.Stop();
        else if (!audioSource.isPlaying && soundtrack)
            audioSource.Play();
    }

    public void playSoundByScene()
    {
        int indexScene = SceneManager.GetActiveScene().buildIndex;
        playSoundByScene(indexScene);
    }

    public void playSoundByScene(int indexScene)
    {
        //ConfigComp.PrintDebug("ConfigComp.playSoundByScene [in]");

        //ConfigComp.PrintDebug("ConfigComp.playSoundByScene - currentIndexAudioSource " + currentIndexAudioSource);
        //ConfigComp.PrintDebug("ConfigComp.playSoundByScene - indexScene " + indexScene);
        if (currentIndexAudioSource != indexScene || currentSoundtrackConfig != configComp.soundtrack)
        {
            currentIndexAudioSource = indexScene;
            if ( audioSource.isPlaying )
            {
                //ConfigComp.PrintDebug("ConfigComp.playSoundByScene - audioSource.Stop() ");
                audioSource.Stop();
            }
            audioSource.clip = levelAudioClip[indexScene];
            //ConfigComp.PrintDebug("ConfigComp.playSoundByScene - audioSource.name " + audioSource.name);
            //ConfigComp.PrintDebug("ConfigComp.playSoundByScene - audioSource.Play() ");
            if(soundtrack)
            audioSource.Play();

        }
        //ConfigComp.PrintDebug("ConfigComp.playSoundByScene [out]");
    }

    public void playSoundByGameOver()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        currentIndexAudioSource = -1;
        audioSource.clip = gameOverAudioClip;
        audioSource.Play();
    }

    public void playSoundContinue()
    {
        playSoundByScene();
    }

    public static void PrintDebug(string msg)
    {
        if (activedDebug)
            print(msg);
    }

}

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
    private bool soundtrack = true;
    public bool Soundtrack
    {
        get
        {
            return soundtrack;
        }
    }

    [SerializeField]
    [Tooltip("Sound effects On/Off")]
    private bool soundeffects = true;
    public bool Soundeffects
    {
        get
        {
            return soundeffects;
        }
    }

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

    private AudioSource audioSource;

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

        if (Soundtrack)
            audioSource.Play();

        initValues();
    }

    public void initValues()
    {
        numTypeBonus = 0;
        shieldActivated = false;
    }
    // Update is called once per frame
    void Update () {
        if (audioSource.isPlaying && !Soundtrack)
            audioSource.Stop();
        else if (!audioSource.isPlaying && Soundtrack)
            audioSource.Play();
    }

    public static void PrintDebug(string msg)
    {
        if (activedDebug)
            print(msg);
    }

}

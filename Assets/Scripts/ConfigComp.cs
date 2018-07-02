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

    public int numAsteroidsDestroyed = 0;

    public int numTypeShield = 0;

    public int numTypeBonus = 0;

    public int numAsteroidsDestroyedPerBonus = 0;

    public static bool activedDebug = true;

    private static bool gamePaused;
    public bool GamePaused
    {
        get
        {
            return gamePaused;
        }
    }

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

    private void initValues()
    {
        numAsteroidsDestroyed = 0;
        numTypeBonus = 0;
        numAsteroidsDestroyedPerBonus = 0;
        shieldActivated = false;
        //countAsteroids = 0;
        gamePaused = false;
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


    public static void PauseGame(bool isPaused)
    {
        ConfigComp.PrintDebug("ConfigComp.PauseGame " + isPaused);
        gamePaused = isPaused;
        //Se o jogo estiver paused, timescale recebe 0
        Time.timeScale = (isPaused) ? 0 : 1;
    }

    public void Restart()
    {
        ConfigComp.PrintDebug("ConfigComp.Restart ");
        PauseGame(false);
        initValues();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadSceneByName(string nameScene)
    {
        ConfigComp.PrintDebug("ConfigComp.LoadSceneByName ");
        PauseGame(false);
        SceneManager.LoadScene(nameScene);
    }

    public void DestroyObj(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    public IEnumerator ShowContinue(Button continueButton)
    {
        ConfigComp.PrintDebug("ConfigComp.ShowContinue ");
        var btnText = continueButton.GetComponentInChildren<Text>();
        while (true)
        {
            if (UnityAdControler.nextTimeReward.HasValue && (DateTime.Now < UnityAdControler.nextTimeReward.Value))
            {
                continueButton.interactable = false;

                TimeSpan restante = UnityAdControler.nextTimeReward.Value - DateTime.Now;

                var contagemRegressiva = string.Format("{0:D2}:{1:D2}", restante.Minutes, restante.Seconds);
                btnText.text = contagemRegressiva;
                yield return new WaitForSeconds(1f);
            }
            else
            {
                continueButton.interactable = true;
                continueButton.onClick.AddListener(UnityAdControler.ShowRewardAd);
                btnText.text = "Continue (Ver Ad)";
                break;
            }
        }
    }

}

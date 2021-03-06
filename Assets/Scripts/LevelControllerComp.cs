﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelControllerComp : MonoBehaviour {

    [Header("GameObjects References")]

    [SerializeField]
    [Tooltip("Environment Reference")]
    private Transform environment;

    [SerializeField]
    [Tooltip("Asteroid Type1 Reference")]
    private Transform[] asteroidsType1;

    [SerializeField]
    [Tooltip("Asteroid Type2 Reference")]
    private Transform[] asteroidsType2;

    [SerializeField]
    [Tooltip("Asteroid Type3 Reference")]
    private Transform[] asteroidsType3;

    [SerializeField]
    [Tooltip("Shield Reference")]
    private Transform shield;

    [Header("Painels References")]

    [SerializeField]
    [Tooltip("PauseMenuPainel Reference")]
    public GameObject pauseMenuPainel;

    [SerializeField]
    [Tooltip("GameOverMenuPainel Reference")]
    public GameObject gameOverMenuPainel;

    [SerializeField]
    [Tooltip("InfoPanel Reference")]
    public GameObject infoPanel;


    [Header("Game Environment Config")]

    [SerializeField]
    [Tooltip("Inteval time between asteroids")]
    [Range(100, 5000)]
    public long intevalTimeMilliseconds = 1000;

    [SerializeField]
    [Tooltip("Number of asteroids type1 per Level")]
    [Range(0, 50)]
    public int numAsteroidsType1 = 1;

    [SerializeField]
    [Tooltip("Number of asteroids type2 per Level")]
    [Range(0, 50)]
    public int numAsteroidsType2 = 2;

    [SerializeField]
    [Tooltip("Number of asteroids type3 per Level")]
    [Range(0, 50)]
    public int numAsteroidsType3 = 3;

    [SerializeField]
    [Tooltip("Number of shields per Level")]
    [Range(0, 50)]
    public int numShields = 3;


    [SerializeField]
    [Tooltip("Number of asteroids to win the bonus 1")]
    [Range(1, 1000)]
    public int numAsteroidsPerBonus1 = 6;

    [SerializeField]
    [Tooltip("Number of asteroids to win the bonus 2")]
    [Range(1, 1000)]
    public int numAsteroidsPerBonus2 = 12;

    [SerializeField]
    [Tooltip("Number of asteroids to win the bonus 3")]
    [Range(1, 1000)]
    public int numAsteroidsPerBonus3 = 18;

    [SerializeField]
    [Tooltip("Number maximum of asteroids that can be lost")]
    [Range(1, 1000)]
    public int numMaxLostAsteroids = 0;


    [Header("keyboard Config")]

    [SerializeField]
    [Tooltip("The speed that the spaceship will moviment laterally")]
    [Range(1, 10)]
    public float lateralSpeed = 5.0f;

    private Text goText;
    public Text GoText
    {
        get
        {
            if (!goText)
                goText = GameObject.FindGameObjectWithTag("GoText").GetComponent<Text>();

            return goText;
        }
    }

    private Text infoLevelValue;
    public Text InfoLevelValue
    {
        get
        {
            if (!infoLevelValue)
                infoLevelValue = GameObject.FindGameObjectWithTag("InfoLevelValue").GetComponent<Text>();

            return infoLevelValue;
        }
    }

    private Text infoAsteroidsValue;
    public Text InfoAsteroidsValue
    {
        get
        {
            if (!infoAsteroidsValue)
                infoAsteroidsValue = GameObject.FindGameObjectWithTag("InfoAsteroidsValue").GetComponent<Text>();

            return infoAsteroidsValue;
        }
    }

    private Text infoMaxAsteroidsValue;
    public Text InfoMaxAsteroidsValue
    {
        get
        {
            if (!infoMaxAsteroidsValue)
                infoMaxAsteroidsValue = GameObject.FindGameObjectWithTag("InfoMaxAsteroidsValue").GetComponent<Text>();

            return infoMaxAsteroidsValue;
        }
    }

    private Text infoMaxLostAsteroidsValue;
    public Text InfoMaxLostAsteroidsValue
    {
        get
        {
            if (!infoMaxLostAsteroidsValue)
                infoMaxLostAsteroidsValue = GameObject.FindGameObjectWithTag("InfoMaxLostAsteroidsValue").GetComponent<Text>();

            return infoMaxLostAsteroidsValue;
        }
    }

    private Text infoShieldValue;
    public Text InfoShieldValue
    {
        get
        {
            if (!infoShieldValue)
                infoShieldValue = GameObject.FindGameObjectWithTag("InfoShieldValue").GetComponent<Text>();

            return infoShieldValue;
        }
    }

    private Text infoBonusValue;
    public Text InfoBonusValue
    {
        get
        {
            if (!infoBonusValue)
                infoBonusValue = GameObject.FindGameObjectWithTag("InfoBonusValue").GetComponent<Text>();

            return infoBonusValue;
        }
    }

    private Text soundtrackText;
    //public Text SoundtrackText
    //{
    //    get
    //    {
    //        if (!soundtrackText)
    //            soundtrackText = GameObject.FindGameObjectWithTag("SoundtrackText").GetComponent<Text>();

    //        return soundtrackText;
    //    }
    //}


    private Text soundEffectsText;
    //public Text SoundEffectsText
    //{
    //    get
    //    {
    //        if (!soundEffectsText)
    //            soundEffectsText = GameObject.FindGameObjectWithTag("SoundEffectsText").GetComponent<Text>();

    //        return soundEffectsText;
    //    }
    //}
    [Header("Current game info:")]

    
    public int numAsteroidsType1Added = 0;

    public int numAsteroidsType2Added = 0;

    public int numAsteroidsType3Added = 0;

    public int numShieldsAdded = 0;

    public int numMaxAsteroidPerLevel = 0;

    public int numAsteroidsDestroyed = 0;

    public int numAsteroidsDestroyedPerBonus = 0;

    private long timeReference;

    private List<GameObject> spots;

    private SpaceShipComp spaceShipComp;
    public SpaceShipComp SpaceShip
    {
        get
        {
            if (!spaceShipComp)
                spaceShipComp = FindObjectOfType<SpaceShipComp>();
            
            return spaceShipComp;
        }
    }

    private bool gameStarted = false;
    public bool GameStarted
    {
        get
        {
            return gameStarted;
        }
    }

    private static bool gamePaused;
    public bool GamePaused
    {
        get
        {
            return gamePaused;
        }
    }

    private ConfigComp configComp;
    public ConfigComp Config
    {
        get
        {
            if(configComp==null)
                configComp = FindObjectOfType<ConfigComp>();
            return configComp;
        }
    }

    // Use this for initialization
    void Start()
    {
        //ConfigComp.PrintDebug("LevelControllerComp.Start");

        configComp = FindObjectOfType<ConfigComp>();

        spaceShipComp = FindObjectOfType<SpaceShipComp>();

        goText = GameObject.FindGameObjectWithTag("GoText").GetComponent<Text>();
        infoLevelValue = GameObject.FindGameObjectWithTag("InfoLevelValue").GetComponent<Text>();
        infoAsteroidsValue = GameObject.FindGameObjectWithTag("InfoAsteroidsValue").GetComponent<Text>();
        infoMaxAsteroidsValue = GameObject.FindGameObjectWithTag("InfoMaxAsteroidsValue").GetComponent<Text>();
        infoMaxLostAsteroidsValue = GameObject.FindGameObjectWithTag("InfoMaxLostAsteroidsValue").GetComponent<Text>();
        infoShieldValue = GameObject.FindGameObjectWithTag("InfoShieldValue").GetComponent<Text>();
        infoBonusValue = GameObject.FindGameObjectWithTag("InfoBonusValue").GetComponent<Text>();

        Initialize();

        
        numMaxAsteroidPerLevel = (numAsteroidsType1 + numAsteroidsType2 + numAsteroidsType3);
        InfoMaxAsteroidsValue.text = numMaxAsteroidPerLevel < 0 ? "0" : numMaxAsteroidPerLevel.ToString();

        InfoMaxLostAsteroidsValue.text = numMaxLostAsteroids.ToString();

        InfoLevelValue.text = SceneManager.GetActiveScene().buildIndex.ToString();

        UpdateValueAsteroids();
        UpdateValueLostAsteroids();
        UpdateValueShield();
        UpdateValueBonus();

        ListSpots();

        if (Config.IsShieldActivated())
        {
            if (Config.numTypeShield > 0)
            {
                spaceShipComp.loadShieldSpaceShip();
            }
        }
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (!gameStarted)
            {
                timeReference = DateTime.Now.ToFileTime();
                GoText.gameObject.SetActive(false);
                gameStarted = true;
                //ConfigComp.PrintDebug("LevelControllerComp.Update GameStarted " + GameStarted);
            }
        }
       
        if (GameStarted && !GamePaused)
        {
            UpdateValueAsteroids();
            UpdateValueLostAsteroids();
            UpdateValueShield();
            UpdateValueBonus();
            //SpaceShip.SendLaserShot();
            InserComponets();
            Config.playSoundByScene();
        }
    }

    private void InserComponets()
    {
        DateTime dataTime = DateTime.Now;
        long currentTimeNow = dataTime.ToFileTime();
        long timeDifference = (currentTimeNow - timeReference);
        long interval = (intevalTimeMilliseconds * 10000);
        if (timeDifference >= interval)
        {
            timeReference = currentTimeNow;
            int comp = UnityEngine.Random.Range(1, 10);
            if (comp > 8 && numShieldsAdded < numShields)
            {
                AddShield();
            }
            else
            {
                AddAsteroids();
            }

        }
    }

    void Initialize()
    {
        //ConfigComp.PrintDebug("LevelControllerComp.Initialize");
        gamePaused = false;
        GoText.gameObject.SetActive(true);
        numAsteroidsDestroyed = 0;
        numAsteroidsDestroyedPerBonus = 0;
        gameStarted = false;
        numAsteroidsType1Added = 0;
        numAsteroidsType2Added = 0;
        numAsteroidsType3Added = 0;
        numShieldsAdded = 0;
        timeReference = DateTime.Now.ToFileTime();
    }

    public void OnOffSountrack()
    {
        Config.soundtrack = !Config.soundtrack;
        changeTextSoundtrack();
    }

    public void OnOffSoundeffects()
    {
        Config.soundEffects = !Config.soundEffects;
        changeTextSoundEffects();
    }

    private void changeTextSoundtrack()
    {
        //ConfigComp.PrintDebug("LevelControllerComp.changeTextSoundtrack Config.Soundtrack " + Config.soundtrack);
        if (Config.soundtrack)
        {
            //ConfigComp.PrintDebug("LevelControllerComp.changeTextSoundtrack Soundtrack ON ");
            soundtrackText.text = "Soundtrack ON";
        }
        else
        {
            //ConfigComp.PrintDebug("LevelControllerComp.changeTextSoundtrack Soundtrack OFF ");
            soundtrackText.text = "Soundtrack OFF";
        }
    }

    private void changeTextSoundEffects()
    {
        //ConfigComp.PrintDebug("LevelControllerComp.changeTextSoundEffects Config.Soundeffects " + Config.soundEffects);
        if (Config.soundEffects)
        {
            //ConfigComp.PrintDebug("LevelControllerComp.changeTextSoundEffects Sound Effects ON ");
            soundEffectsText.text = "Sound Effects ON";
        }
        else
        {
            //ConfigComp.PrintDebug("LevelControllerComp.changeTextSoundEffects Sound Effects OFF ");
            soundEffectsText.text = "Sound Effects OFF";
        }
    }

    private void ListSpots()
    {
        spots = new List<GameObject>();
        foreach (Transform coponent in environment)
        {
            if (coponent.CompareTag("Spot"))
            {
                spots.Add(coponent.gameObject);
            }
        }
    }

    public void LoadLevel(string sceneName)
    {
        //ConfigComp.PrintDebug("LevelControllerComp.LoadLevel " + sceneName);
        SceneManager.LoadScene(sceneName);
        Initialize();
    }

    public void LoadNextLevel()
    {
        //ConfigComp.PrintDebug("LevelControllerComp.LoadNextLevel "+ (SceneManager.GetActiveScene().buildIndex + 1));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Initialize();
    }

    private void AddShield()
    {
        if (numShieldsAdded <= numShields)
        {
            if (spots.Count > 0)
            {
                var spot = spots[UnityEngine.Random.Range(0, spots.Count)];
                var pointSpawnPos = spot.transform.position;
                var newShield = Instantiate(shield, pointSpawnPos, Quaternion.identity);
                newShield.SetParent(spot.transform);
                ++numShieldsAdded;
            }
        }
    }

    private void AddAsteroids()
    {
        if (numAsteroidsType1Added <= numAsteroidsType1 ||
            numAsteroidsType2Added <= numAsteroidsType2 ||
            numAsteroidsType3Added <= numAsteroidsType3)
        {
            int asteroidType = UnityEngine.Random.Range(1, 4);
            //ConfigComp.PrintDebug("LevelControllerComp.AddAsteroids numAsteroidsType1 " + numAsteroidsType1);
            //ConfigComp.PrintDebug("LevelControllerComp.AddAsteroids numAsteroidsType2 " + numAsteroidsType2);
            //ConfigComp.PrintDebug("LevelControllerComp.AddAsteroids numAsteroidsType3 " + numAsteroidsType3);
            //ConfigComp.PrintDebug("LevelControllerComp.AddAsteroids asteroidType " + asteroidType);
            if (asteroidType == 1 && numAsteroidsType1Added <= numAsteroidsType1)
            {
                numAsteroidsType1Added++;
                AddAsterois(asteroidsType1);
            }
            else if (asteroidType == 2 && numAsteroidsType2Added <= numAsteroidsType2)
            {
                numAsteroidsType2Added++;
                AddAsterois(asteroidsType2);
            }
            else if (asteroidType == 3 && numAsteroidsType3Added <= numAsteroidsType3)
            {
                numAsteroidsType3Added++;
                AddAsterois(asteroidsType3);
            }
        }
    }

    private void AddAsterois(Transform[] asteroids)
    {
        int asteroidModel = UnityEngine.Random.Range(0, asteroids.Length);
        //ConfigComp.PrintDebug("LevelControllerComp.AddAsteroids asteroidModel " + asteroidModel);
        var asteroid = asteroids[asteroidModel];
        if (spots.Count > 0)
        {
            var spot = spots[UnityEngine.Random.Range(0, spots.Count)];
            var pointSpawnPos = spot.transform.position;
            var newAsteroid = Instantiate(asteroid, pointSpawnPos, Quaternion.identity);
            newAsteroid.SetParent(spot.transform);
        }
    }

    public void UpdateValueAsteroids()
    {
        InfoAsteroidsValue.text = numAsteroidsDestroyed < 0 ? "0" : numAsteroidsDestroyed.ToString();
    }

    public void UpdateValueLostAsteroids()
    {
        InfoMaxLostAsteroidsValue.text = numMaxLostAsteroids < 0 ? "0" : numMaxLostAsteroids.ToString();
    }

    public void UpdateValueShield()
    {
        InfoShieldValue.text = Config.numTypeShield < 0 ? "0": Config.numTypeShield.ToString();
    }

    public void UpdateValueBonus()
    {
        InfoBonusValue.text = Config.numTypeBonus < 0 ? "0" : Config.numTypeBonus.ToString();
    }

    public void DecreaseAsteroidCounter()
    {
        numMaxAsteroidPerLevel--;
        if (numMaxAsteroidPerLevel <= 0 )
        {
            LoadNextLevel();
        }
    }

    public void DecreaseNumMaxLostAsteroids()
    {
        numMaxLostAsteroids--;

        if (numMaxLostAsteroids <= 0)
        {
            numMaxLostAsteroids = 0;
            ResetGame();
        }
    }

    /// <summary>
    /// Method to pause the game
    /// </summary>
    /// <param name="isPaused"></param>
    public void SetPauseMenu(bool isPaused)
    {
        //ConfigComp.PrintDebug("LevelControllerComp.SetPauseMenu "+ isPaused);
        PauseGame(isPaused);
        pauseMenuPainel.SetActive(isPaused);

        var texts = pauseMenuPainel.transform.GetComponentsInChildren<Text>();

        soundtrackText = null;
        soundEffectsText = null;
        foreach (var text in texts)
        {
            if (text.tag.Equals("SoundtrackText"))
            {
                //ConfigComp.PrintDebug("LevelControllerComp.SetPauseMenu SoundtrackText ");
                soundtrackText = text;
            }else
            if (text.tag.Equals("SoundEffectsText"))
            {
                //ConfigComp.PrintDebug("LevelControllerComp.SetPauseMenu SoundEffectsText ");
                soundEffectsText = text;
            }
        }
        changeTextSoundtrack();
        changeTextSoundEffects();
    }



    //////////////////////////////////////////////////////////////////////
    

    public void ResetGame()
    {
        ConfigComp.PrintDebug("LevelControllerComp.ResetGame ");
        gameOverMenuPainel.SetActive(true);

        var texts = gameOverMenuPainel.transform.GetComponentsInChildren<Text>();

        soundtrackText = null;
        soundEffectsText = null;
        foreach (var text in texts)
        {
            if (text.tag.Equals("SoundtrackText"))
            {
                ConfigComp.PrintDebug("LevelControllerComp.ResetGame SoundtrackText ");
                soundtrackText = text;
            }
            else
            if (text.tag.Equals("SoundEffectsText"))
            {
                ConfigComp.PrintDebug("LevelControllerComp.ResetGame SoundEffectsText ");
                soundEffectsText = text;
            }
        }
        changeTextSoundtrack();
        changeTextSoundEffects();

        PauseGame(true);

        Config.playSoundByGameOver();

        var buttons = gameOverMenuPainel.transform.GetComponentsInChildren<Button>();

        Button continueButton = null;
        foreach (var button in buttons)
        {
            if (button.name.Equals("ContinueButton"))
            {
                continueButton = button;
                break;
            }
        }

        if (continueButton != null)
        {
#if UNITY_ADS
            //Se o button continue for clicado, iremos tocar o anúncio
            StartCoroutine(ShowContinue(continueButton));
            //buttonContinue.onClick.AddListener(UnityAdControler.ShowRewardAd);
#else
            //Se nao existe add, nao precisa mostrar o botao Continue
            continueButton.gameObject.SetActive(false);
#endif
        }

    }

    /// <summary>
    /// Metodo para reiniciar o jogo
    /// </summary>
    private void Reset()
    {
        //ConfigComp.PrintDebug("LevelControllerComp.Reset ");
        Initialize();
        //Reinicia o level
        string sceneName = SceneManager.GetActiveScene().name;
        LoadSceneByName(sceneName);
    }


    /// <summary>
    /// Faz o reset do jogo
    /// </summary>
    public void Continue()
    {
        //ConfigComp.PrintDebug("LevelControllerComp.Continue ");
        gameOverMenuPainel.SetActive(false);

        PauseGame(false);

        Config.playSoundByScene();
    }


    public void IncreaseAsteroidsDestroyed()
    {
        numAsteroidsDestroyed++;
        numAsteroidsDestroyedPerBonus++;
        if ((Config.numTypeBonus == 0 && numAsteroidsDestroyedPerBonus >= numAsteroidsPerBonus1) ||
            (Config.numTypeBonus == 1 && numAsteroidsDestroyedPerBonus >= numAsteroidsPerBonus2) ||
            (Config.numTypeBonus == 2 && numAsteroidsDestroyedPerBonus >= numAsteroidsPerBonus3))
        {
            int maxBonus = spaceShipComp.laserShots.Length - 1;
            numAsteroidsDestroyedPerBonus = 0;
            Config.numTypeBonus++;
            if (Config.numTypeBonus > maxBonus)
                Config.numTypeBonus = maxBonus;
        }
    }

    public static void PauseGame(bool isPaused)
    {
        //ConfigComp.PrintDebug("LevelControllerComp.PauseGame " + isPaused);
        gamePaused = isPaused;
        //Se o jogo estiver paused, timescale recebe 0
        Time.timeScale = (isPaused) ? 0 : 1;
    }

    public void Restart()
    {
        //ConfigComp.PrintDebug("LevelControllerComp.Restart ");
        PauseGame(false);
        Initialize();
        Config.initValues();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadSceneByName(string sceneName)
    {
        //ConfigComp.PrintDebug("LevelControllerComp.LoadSceneByName ");
        PauseGame(false);
        SceneManager.LoadScene(sceneName);
    }

    public void DestroyObj(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    public IEnumerator ShowContinue(Button continueButton)
    {
        //ConfigComp.PrintDebug("LevelControllerComp.ShowContinue ");
        var btnText = continueButton.GetComponentInChildren<Text>();
        while (true)
        {
            if (UnityAdControler.nextTimeReward.HasValue && (DateTime.Now < UnityAdControler.nextTimeReward.Value))
            {
                continueButton.interactable = false;
                

                TimeSpan restante = UnityAdControler.nextTimeReward.Value - DateTime.Now;

                var contagemRegressiva = string.Format("{0:D2}:{1:D2}", restante.Minutes, restante.Seconds);
                btnText.text = contagemRegressiva;
                yield return new WaitForSecondsRealtime(1f);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainControllerComp : MonoBehaviour {

    private ConfigComp configComp;

    [Header("GameObjects References")]

    [SerializeField]
    [Tooltip("Sprite Sound ON Reference")]
    private Sprite spritesSoundON;

    [SerializeField]
    [Tooltip("Sprite Sound OFF Reference")]
    private Sprite spritesSoundOFF;

    private Image soundEffects;

    private Image soundtrack;

    public void LoadSceneByNameWithAds(string sceneName)
    {

        SceneManager.LoadScene(sceneName);

        if (UnityAdControler.showAds)
        {
            UnityAdControler.ShowAd();
        }
        
    }

    public void OnOffSountrack()
    {
        configComp.soundtrack = !configComp.soundtrack;
        changeImageSountrack();
    }

    public void OnOffSoundeffects()
    {
        configComp.soundEffects = !configComp.soundEffects;
        changeImageSoundEffects();
    }

    private void changeImageSountrack()
    {
        //ConfigComp.PrintDebug("MainControllerComp.changeImageSountrack configComp.Soundtrack " + configComp.soundtrack);
        if (configComp.soundtrack)
        {
            //ConfigComp.PrintDebug("MainControllerComp.changeImageSountrack spritesSoundON ");
            soundtrack.sprite = spritesSoundON;
        }
        else
        {
            //ConfigComp.PrintDebug("MainControllerComp.changeImageSountrack spritesSoundOFF ");
            soundtrack.sprite = spritesSoundOFF;
        }
    }

    private void changeImageSoundEffects()
    {
        //ConfigComp.PrintDebug("MainControllerComp.changeImageSoundEffects configComp.Soundeffects " + configComp.soundEffects);
        if (configComp.soundEffects)
        {
            //ConfigComp.PrintDebug("MainControllerComp.changeImageSoundEffects spritesSoundON ");
            soundEffects.sprite = spritesSoundON;
        }
        else
        {
            //ConfigComp.PrintDebug("MainControllerComp.changeImageSoundEffects spritesSoundOFF ");
            soundEffects.sprite = spritesSoundOFF;
        }
    }

    // Use this for initialization
    void Start () {
        configComp = FindObjectOfType<ConfigComp>();

        if(GameObject.FindGameObjectWithTag("SoundtrackImage"))
        soundtrack = GameObject.FindGameObjectWithTag("SoundtrackImage").GetComponent<Image>();
        if(GameObject.FindGameObjectWithTag("SoundEffectsImage"))
        soundEffects = GameObject.FindGameObjectWithTag("SoundEffectsImage").GetComponent<Image>();

        changeImageSountrack();
        changeImageSoundEffects();
    }
	
	// Update is called once per frame
	void Update () {

        configComp.playSoundByScene();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadSceneByNameWithAds("Level_1");
        }

    }
}

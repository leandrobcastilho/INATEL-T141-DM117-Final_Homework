using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainControllerComp : MonoBehaviour {

    private ConfigComp configComp;

    public void LoadSceneByNameWithAds(string sceneName)
    {

        SceneManager.LoadScene(sceneName);

        if (UnityAdControler.showAds)
        {
            UnityAdControler.ShowAd();
        }
        
    }

    // Use this for initialization
    void Start () {
        configComp = FindObjectOfType<ConfigComp>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

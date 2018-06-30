using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControllerComp : MonoBehaviour {

    public static MusicControllerComp musicControllerComp = null;

    private void Awake()
    {
        if (musicControllerComp != null)
        {
            Destroy(gameObject);
        }
        else
        {
            musicControllerComp = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}

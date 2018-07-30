using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShotComp : MonoBehaviour {

    private AudioSource audioSource;

    private SpaceShipComp spaceShip;

    private Rigidbody2D rv2D;
    public Rigidbody2D Rv2D
    {
        get
        {
            if (!rv2D)
            {
                rv2D = GetComponent<Rigidbody2D>();
            }
            return rv2D;
        }
    }

    private LevelControllerComp levelControllerComp;

    // Use this for initialization
    void Start()
    {
        levelControllerComp = FindObjectOfType<LevelControllerComp>();
        audioSource = GetComponent<AudioSource>();

        spaceShip = FindObjectOfType<SpaceShipComp>();
        transform.position = spaceShip.transform.position;

        if (levelControllerComp.Config.soundEffects)
        {
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
        }
        StarShot();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StarShot()
    {
        Rv2D.velocity = new Vector2(0, 5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.GetComponent<SpaceShipComp>())
        {
            //ConfigComp.PrintDebug("LaserShotComp.OnCollisionEnter2D !SpaceShip " + gameObject.name + " - " + collision.gameObject.name);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<GameOverComp>())
        {
            //ConfigComp.PrintDebug("LaserShotComp.OnTriggerEnter2D GameOver " + gameObject.name + " - " + collision.gameObject.name);
            Destroy(gameObject);
        }
    }
}

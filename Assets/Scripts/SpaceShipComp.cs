using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipComp : MonoBehaviour {

    [SerializeField]
    private Sprite[] sprites;

    [SerializeField]
    private ParticleSystem explosion;

    [SerializeField]
    [Tooltip("Environment Laser Shot")]
    public GameObject[] laserShots;

    [SerializeField]
    [Tooltip("Inteval time between Laser Shot")]
    [Range(0, 1)]
    public float intevalTimeSeconds = 0.1f;

    private AudioSource audioSource;

    private LevelControllerComp levelControllerComp;

    private SpriteRenderer spriteRenderer;

    private int numMaxHits;
    
	private bool isKeyBoard = false;

    // Use this for initialization
    void Start()
    {
        levelControllerComp = FindObjectOfType<LevelControllerComp>();
        //ConfigComp.PrintDebug("SpaceShipComp.Start ");

        numMaxHits = 0;

        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
		 if (Input.GetKeyDown(KeyCode.Space)) {
            isKeyBoard = true;
           
        }else if (Input.GetButtonDown("Fire1")) {
            isKeyBoard = false;
        }

        if (isKeyBoard) {
            KeyboardMovement();
        }else
            MouseMovement();
		
        //MouseMovement();
        //TouchMovement();
        //KeyboardMovement();
    }

    public void SendLaserShot()
    {
        if (levelControllerComp.GameStarted && !levelControllerComp.GamePaused)
        {
            if (Input.GetButtonDown("Fire1") || (Input.GetKeyDown(KeyCode.Space)))
            {
                GameObject laserShot = laserShots[levelControllerComp.Config.numTypeBonus];
                SendLaserShot(laserShot);
            }
        }
    }
	
    private void SendLaserShot(GameObject laserShot)
    {
		Vector3 goposition = this.gameObject.transform.position;
		GameObject newLaserShot = Instantiate(laserShot, goposition, Quaternion.identity);
		LaserShotComp laserShotComp = newLaserShot.GetComponent<LaserShotComp>();
		laserShotComp.StarShot();
    }

    private void MouseMovement()
    {
        //ConfigComp.PrintDebug("SpaceShipComp.MouseMovement ");
        float mousePosWorldUnitX = ((Input.mousePosition.x) / Screen.width * 16);
        Vector2 spaceShipPos = new Vector2(0, transform.position.y);
        spaceShipPos.x = Mathf.Clamp(mousePosWorldUnitX, 1f, 15f);
        transform.position = spaceShipPos;
    }

    private void TouchMovement()
    {
        if (Input.touchCount > 0)
        {
            //ConfigComp.PrintDebug("SpaceShipComp.TouchMovement ");
            Touch touch = Input.touches[0];
            float touchPosWorldUnitX = ((touch.position.x) / Screen.width * 16);
            Vector2 spaceShipPos = new Vector2(Mathf.Clamp(touchPosWorldUnitX, 0f, 15f), transform.position.y);
            transform.position = spaceShipPos;
        }
    }

    private void KeyboardMovement()
    {		
		float eixoX = Input.GetAxis("Horizontal");
        

        Vector2 direcao = new Vector2(eixoX, 0);

        transform.Translate(direcao * 10 * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<ShieldComp>())
        {
            //ConfigComp.PrintDebug("SpaceShipComp.OnCollisionEnter2D - Shield = " + gameObject.name + " - " + collision.gameObject.name);
            ShieldComp shieldComp = collision.gameObject.GetComponent<ShieldComp>();

            /*
            * intact shield number 2
            * broken shield number 1
            */
            int typeShield = 2 - shieldComp.numShot;
            
            /*
            * if the shield is active you can upgrade if the new shield is better
            */
            if (levelControllerComp.Config.IsShieldActivated())
            {
                if (levelControllerComp.Config.numTypeShield < typeShield)
                {
                    levelControllerComp.Config.numTypeShield = typeShield;
                    loadShieldSpaceShip();
                }
            }
            else
            {
                levelControllerComp.Config.ActivateShield();
                levelControllerComp.Config.numTypeShield = typeShield;
                loadShieldSpaceShip();
            }
            
            return;
        }
        else if (collision.gameObject.GetComponent<AsteroidComp>())
        {
           // ConfigComp.PrintDebug("SpaceShipComp.OnCollisionEnter2D - Asteroid = " + gameObject.name + " - " + collision.gameObject.name);
            numMaxHits--;

            levelControllerComp.Config.numTypeBonus--;
            levelControllerComp.Config.numTypeShield--;
            if ( levelControllerComp.Config.numTypeBonus < 0 )
                levelControllerComp.Config.numTypeBonus = 0;

            if (numMaxHits < 0)
            {
                numMaxHits = 0;
                levelControllerComp.Config.numTypeBonus = 0;
                ExplosionEffect();
                levelControllerComp.ResetGame();
            }
            LoadSprite();
            if (levelControllerComp.Config.Soundeffects)
                AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
            return;
        }
        
    }

    public void loadShieldSpaceShip()
    {
        numMaxHits = levelControllerComp.Config.numTypeShield;
        LoadSprite();
    }

    private void ExplosionEffect()
    {
        //ConfigComp.PrintDebug("SpaceShipComp.ExplosionEffect ");
        if (explosion)
        {
            ParticleSystem ps = Instantiate<ParticleSystem>(explosion, transform.position, Quaternion.identity);
            ParticleSystem.MainModule main = ps.main;
            main.startColor = spriteRenderer.color;
            Destroy(ps, 1.0f);
        }
    }

    private void LoadSprite()
    {
        //ConfigComp.PrintDebug("SpaceShipComp.LoadSprite ");
        int spriteIndex = numMaxHits;
        if (spriteIndex > sprites.Length || spriteIndex < 0)
            spriteIndex = 0;
        if (sprites[spriteIndex])
        {
            spriteRenderer.sprite = sprites[spriteIndex];
        }
    }

}

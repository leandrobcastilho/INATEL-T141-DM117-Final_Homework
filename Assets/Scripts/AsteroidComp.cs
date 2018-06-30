using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidComp : MonoBehaviour {

    [SerializeField]
    private Sprite[] sprites;

    [SerializeField]
    private ParticleSystem explosion;

    private AudioSource audioSource;

    private int numShot;

    private SpriteRenderer spriteRenderer;

    private LevelControllerComp levelControllerComp;

    // Use this for initialization
    void Start()
    {
        levelControllerComp = FindObjectOfType<LevelControllerComp>();
        LevelControllerComp.PrintDebug("AsteroidComp.Start ");
        numShot = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        if (transform.CompareTag("Type3") || transform.CompareTag("Type2") || transform.CompareTag("Type1"))
        {
            levelControllerComp.IncreaseAsteroidCounter();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<LaserShotComp>())
        {
            //LevelControllerComp.PrintDebug("SpaceShipComp.OnCollisionEnter2D - LaserShot = " + gameObject.name + " - " + collision.gameObject.name);
   
            if (transform.CompareTag("Type3") || transform.CompareTag("Type2") || transform.CompareTag("Type1"))
            {
                AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
                ExplosionEffect();
                ApplyDamages();
            }

        }
        else if(collision.gameObject.GetComponent<LaserShotComp>())
        {
            numShot = sprites.Length + 1;
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
            ExplosionEffect();
            ApplyDamages();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<GameOverComp>())
        {
            //LevelControllerComp.PrintDebug("AsteroidComp.OnTriggerEnter2D - GameOver = " + gameObject.name + " - " + collision.gameObject.name);
            levelControllerComp.DecreaseAsteroidCounter();
            Destroy(gameObject);
        }
    }

    private void ExplosionEffect()
    {
        LevelControllerComp.PrintDebug("AsteroidComp.ExplosionEffect ");
        if (explosion)
        {
            ParticleSystem ps = Instantiate<ParticleSystem>(explosion, transform.position, Quaternion.identity);
            ParticleSystem.MainModule main = ps.main;
            main.startColor = spriteRenderer.color;
        }
    }

    private void ApplyDamages()
    {
        LevelControllerComp.PrintDebug("AsteroidComp.ApplyDamages ");
        numShot++;
        int maxShot = sprites.Length + 1;
        if (numShot >= maxShot)
        {
            levelControllerComp.IncreaseAsteroidsDestroyed();
            levelControllerComp.DecreaseAsteroidCounter();
            Destroy(gameObject);
        }
        else
        {
            LoadSprite();
        }
    }

    private void LoadSprite()
    {
        LevelControllerComp.PrintDebug("AsteroidComp.LoadSprite ");
        int spriteIndex = numShot - 1;
        if (spriteIndex > sprites.Length || spriteIndex < 0)
            spriteIndex = 0;
        if (sprites[spriteIndex])
        {
            spriteRenderer.sprite = sprites[spriteIndex];
        }
    }
}

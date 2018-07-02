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
        ConfigComp.PrintDebug("AsteroidComp.Start ");
        numShot = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        //if (transform.CompareTag("Type3") || transform.CompareTag("Type2") || transform.CompareTag("Type1"))
        //{
        //    levelControllerComp.IncreaseAsteroidCounter();
        //}
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<LaserShotComp>())
        {
            //ConfigComp.PrintDebug("SpaceShipComp.OnCollisionEnter2D - LaserShot = " + gameObject.name + " - " + collision.gameObject.name);
   
            if (transform.CompareTag("Type3") || transform.CompareTag("Type2") || transform.CompareTag("Type1"))
            {
                /*
                 * for each type of laser a different power
                 */
                int power = 1;
                if (collision.gameObject.transform.CompareTag("LaserShot1"))
                    power = 1;
                if (collision.gameObject.transform.CompareTag("LaserShot2"))
                    power = 2;
                if (collision.gameObject.transform.CompareTag("LaserShot3"))
                    power = 3;

                /*
                * for each power of laser a different damage shot
                */
                for (int i = 1; i <= power; i++)
                {
                    if (levelControllerComp.Config.Soundeffects)
                    AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
                    ExplosionEffect();
                    if (!ApplyDamages(power))
                        break;
                }
            }

        }
        else if (collision.gameObject.GetComponent<AsteroidComp>())
        {
            if (transform.CompareTag("Type3") || transform.CompareTag("Type2") || transform.CompareTag("Type1"))
            {
                if (levelControllerComp.Config.Soundeffects)
                    AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
                ExplosionEffect();
                ApplyDamages(1);
            }
        }
        else if (collision.gameObject.GetComponent<SpaceShipComp>())
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<GameOverComp>())
        {
            //ConfigComp.PrintDebug("AsteroidComp.OnTriggerEnter2D - GameOver = " + gameObject.name + " - " + collision.gameObject.name);
            levelControllerComp.DecreaseAsteroidCounter();
            Destroy(gameObject);
        }
    }

    private void ExplosionEffect()
    {
        ConfigComp.PrintDebug("AsteroidComp.ExplosionEffect ");
        if (explosion)
        {
            ParticleSystem ps = Instantiate<ParticleSystem>(explosion, transform.position, Quaternion.identity);
            ParticleSystem.MainModule main = ps.main;
            main.startColor = spriteRenderer.color;
            Destroy(ps, 1.0f);
        }
    }

    private bool ApplyDamages(int power)
    {
        bool canContinue = true;
        numShot++;
        int maxShot = sprites.Length + 1;
        ConfigComp.PrintDebug("AsteroidComp.ApplyDamages power " + power + " - numShot " + numShot);
        ConfigComp.PrintDebug("AsteroidComp.ApplyDamages maxShot " + maxShot);
        if (numShot >= maxShot)
        {
            levelControllerComp.IncreaseAsteroidsDestroyed();
            levelControllerComp.DecreaseAsteroidCounter();
            Destroy(gameObject);
            canContinue = false;
        }
        else
        {
            LoadSprite();
        }
        return canContinue;
    }

    private void LoadSprite()
    {
        int spriteIndex = numShot - 1;
        if (spriteIndex > sprites.Length || spriteIndex < 0)
            spriteIndex = 0;
        ConfigComp.PrintDebug("AsteroidComp.LoadSprite spriteIndex " + spriteIndex);
        if (sprites[spriteIndex])
        {
            spriteRenderer.sprite = sprites[spriteIndex];
        }
    }
}

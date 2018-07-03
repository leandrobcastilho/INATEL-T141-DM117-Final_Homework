﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldComp : MonoBehaviour {

    [SerializeField]
    private Sprite[] sprites;

    [SerializeField]
    private ParticleSystem explosion;

    private AudioSource audioSource;

    public int numShot;

    private SpriteRenderer spriteRenderer;

    private LevelControllerComp levelControllerComp;

    // Use this for initialization
    void Start()
    {
        levelControllerComp = FindObjectOfType<LevelControllerComp>();
        //ConfigComp.PrintDebug("ShieldComp.Start ");
        numShot = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<LaserShotComp>())
        {
            if (levelControllerComp.Config.Soundeffects)
                AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
            ExplosionEffect();
            ApplyDamages();
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
            //ConfigComp.PrintDebug("ShieldComp.OnTriggerEnter2D SpaceShip " + gameObject.name + " - " + collision.gameObject.name);
            Destroy(gameObject);
        }
    }

    private void ExplosionEffect()
    {
        if (explosion)
        {
            ParticleSystem ps = Instantiate<ParticleSystem>(explosion, transform.position, Quaternion.identity);
            ParticleSystem.MainModule main = ps.main;
            main.startColor = spriteRenderer.color;
            Destroy(ps, 1.0f);
        }
    }

    private void ApplyDamages()
    {
        numShot++;
        int maxShot = sprites.Length + 1;
        if (numShot >= maxShot)
        {
            Destroy(gameObject);
        }
        else
        {
            LoadSprite();
        }
    }

    private void LoadSprite()
    {
        int spriteIndex = numShot - 1;
        if (spriteIndex > sprites.Length || spriteIndex < 0)
            spriteIndex = 0;
        if (sprites[spriteIndex])
        {
            spriteRenderer.sprite = sprites[spriteIndex];
        }
    }
}

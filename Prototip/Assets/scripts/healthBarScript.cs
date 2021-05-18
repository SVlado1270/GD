﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class healthBarScript : MonoBehaviour
{
    public Slider bar;
    public TextMeshProUGUI barText;
    public TextMeshProUGUI endText;
    public TextMeshProUGUI shieldText;
    public GameObject shieldIcon;
    public int maxHealth = 30;
    public int shield = 0;
    public int health;
    public bool isPlayer = false;

    float HeroHitAudioLength;

    public GameObject avatar;
    public GameObject corpseAvatar; 
    Animator PlayerAnimator, EnemyAnimator;
    AudioSource[] nAudio;
    AudioSource v_audio;

    

    // Start is called before the first frame update
    void Start()
    {
        //fetch components
        bar = transform.Find("statsCanvas").Find("Slider").GetComponent<Slider>();
        barText = transform.Find("statsCanvas").Find("Health").GetComponent<TextMeshProUGUI>();
        shieldText = transform.Find("statsCanvas").Find("shieldCountText").GetComponent<TextMeshProUGUI>();
        shieldIcon = transform.Find("statsCanvas").Find("shieldIcon").gameObject;

        avatar = transform.Find("avatar").gameObject;

        endText = GameObject.Find("infoUI/Canvas/EndGame").GetComponent<TextMeshProUGUI>();

        //put health bar above character 
        var old = bar.transform.position;
        Vector3 worldHealthBarPos = new Vector3(transform.position.x, transform.position.y + 2.5f);
        bar.transform.position = Camera.main.WorldToScreenPoint(worldHealthBarPos);
        var delta = bar.transform.position - old;

        //translate the other stats components so their placement is the same relative to the healthbar
        barText.transform.Translate(delta);
        shieldText.transform.Translate(delta);
        shieldIcon.transform.Translate(delta);

        this.health = maxHealth;
        updateSliderValue();
        UpdateShield();


        PlayerAnimator = GameObject.Find("Character/Hero/Silent/avatar").GetComponent<Animator>();
        EnemyAnimator = GameObject.Find("Character/Villains/cultist/avatar").GetComponent<Animator>();
        nAudio = GameObject.Find("Character/Hero/Silent/avatar").GetComponents<AudioSource>();
        v_audio = GameObject.Find("Character/Villains/cultist/avatar").GetComponent<AudioSource>();

    }

    void updateSliderValue()
    {
        float value = (float)health / (float)maxHealth;
        if (value < 0f)
        {
            value = 0;
        }
        else if (value > 1f)
        {
            value = 1f;
        }
        bar.value = value;

        barText.SetText("{0}/{1}", health, maxHealth);
    }

    public void applyDamage(int damage)
    {

        if (shield > 0)
        {
            if (shield > damage)
            {
                shield -= damage;
            }
            else
            {
                damage -= shield;
                health -= damage;
                shield = 0;
            }
        }
        else
        {
            health -= damage;
        }
        updateSliderValue();
        UpdateShield();

        if (health <= 0)
        {
            endText.gameObject.SetActive(true);

            if (isPlayer)
            {
                corpseAvatar.SetActive(true);
                avatar.SetActive(false);
                endText.SetText("Game over!");
            }
            else
            {
                avatar.SetActive(false);
                endText.SetText("You win!");
            }
            
            transform.Find("statsCanvas").gameObject.SetActive(false);
            GameObject.FindGameObjectWithTag("EndTurnButton").SetActive(false);
            GameObject.FindGameObjectWithTag("CardManager").GetComponent<CardManagerScript>().gameState = GameState.GameOver;
        }
    }

    public void UpdateShield(int delta = 0)
    {
        shield += delta;
        shieldText.SetText(shield.ToString());
    }

    public void consumeEffect(Effect e)
    {
        CardManagerScript cardManager = GameObject.FindGameObjectWithTag("CardManager").GetComponent<CardManagerScript>();
        int damage = e.damage;
        if (e.isShiv)
        {
            damage += cardManager.shivsBonusDamage;
        }
        if (damage > 0)
        {
            // ANIMATIE PLAYER + AUDIO ATAC
            if (!isPlayer)
            {
                PlayerAnimator.SetBool("isPlayer", true);

                PlayerAnimator.Play("Hero_hit");
                nAudio[0].PlayOneShot(nAudio[0].clip, nAudio[0].clip.length);

                PlayerAnimator.SetBool("isPlayer", false);
            }
        }

        if (isPlayer && damage > 0)
        {
            EnemyAnimator.SetBool("isAttacking", true);

            EnemyAnimator.Play("Cultist_hit");
            v_audio.PlayOneShot(v_audio.clip, v_audio.clip.length);

            EnemyAnimator.SetBool("isAttacking", false);
        }

        //AUDIO SHIELD
        if (e.shield > 0)
        {
            nAudio[1].PlayOneShot(nAudio[1].clip, nAudio[1].clip.length);
        }

        applyDamage(damage);
        UpdateShield(e.shield);
        e.ApplyMeta();
    }


    // Update is called once per frame
    void Update()
    {

    }
}

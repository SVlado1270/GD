using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class healthBarScript : MonoBehaviour
{
    public Slider bar;
    public TextMeshProUGUI barText;
    public TextMeshProUGUI endText;
    public int maxHealth = 30;
    public int shield = 0;
    public int health;

    public Vector3 porpsPivotPoint;
    public int weak = 0;
    public int retain = 0;
    public int dexterity = 0;
    public int strength = 0;
    public int accuracy = 0;
    public int blades = 0;
    public int poison = 0;
    public int intagible = 0;
    public int wraith = 0;
    public int ritual = 0;

    public bool isPlayer = false;

    float HeroHitAudioLength;

    public GameObject avatar;
    public GameObject corpseAvatar;
    GameObject intent;

    Animator PlayerAnimator, EnemyAnimator;
    AudioSource[] nAudio;
    AudioSource v_audio;
    

    public Transform getProp_UI(string propName)
    {
        return transform.Find("statsCanvas").Find(propName + "Prop");
    }

    public Transform getPropCountText(string propName)
    {
        return transform.Find("statsCanvas").Find(propName + "Prop").Find(propName + "CountText");
    }

    public Transform getPropIcon(string propName)
    {
        return transform.Find("statsCanvas").Find(propName + "Prop").Find(propName + "Icon");
    }

    // Start is called before the first frame update

    public void UpdateIntent(string spriteName, int counter, string hover_text)
    {
        if (isPlayer == false)
        {
            intent.SetActive(true);
            SpriteManagerScript SPRITES = GameObject.FindGameObjectWithTag("SpriteManager").GetComponent<SpriteManagerScript>();

            intent.transform.Find("icon").GetComponent<Image>().sprite = SPRITES.GetSpriteByName(spriteName);
            intent.transform.Find("icon").GetComponent<StaticTooltip>().message = hover_text;

            intent.transform.Find("count").GetComponent<TextMeshProUGUI>().SetText(counter.ToString());
        }
    }

    public bool isDead()
    {
        return health <= 0;
    }

    public void UpdatePropsUI()
    {
        var pivot = this.porpsPivotPoint;
        var pivotDelta = new Vector3(60, 0, 0);

        var props = new List<(int, string)>
        {
            (shield, "shield"),
            (weak, "weak"),
            (dexterity, "dexterity"),
            (strength, "strength"),
            (accuracy, "accuracy"),
            (blades, "blades"),
            (poison, "poison"),
            (retain, "retain"),
            (intagible, "intangible"),
            (wraith, "wraith"),
            (ritual, "ritual")
        };

        foreach (var pair in props)
        {
            var prop = getProp_UI(pair.Item2);
            if(pair.Item1 != 0)
            {
                prop.gameObject.SetActive(true);
                prop.transform.position = pivot;
                pivot += pivotDelta;
                getPropCountText(pair.Item2).GetComponent<TextMeshProUGUI>().SetText(pair.Item1.ToString());
            }
            else
            {
                prop.gameObject.SetActive(false);
            }
        }

    }

    public void resetStats()
    {
        weak = 0;
        retain = 0;
        dexterity = 0;
        strength = 0;
        accuracy = 0;
        blades = 0;
        poison = 0;
        intagible = 0;
        wraith = 0;
        ritual = 0;
        shield = 0;
    }
    void Start()
    {
        //fetch components
        bar = transform.Find("statsCanvas").Find("Slider").GetComponent<Slider>();
        barText = transform.Find("statsCanvas").Find("Health").GetComponent<TextMeshProUGUI>();

        avatar = transform.Find("avatar").gameObject;
        intent = transform.Find("statsCanvas").Find("intent").gameObject;
        endText = GameObject.Find("infoUI/Canvas/EndGame").GetComponent<TextMeshProUGUI>();

        //put health bar above character 
        var old = bar.transform.position;
        Vector3 worldHealthBarPos = new Vector3(transform.position.x, transform.position.y + 2.5f);
        bar.transform.position = Camera.main.WorldToScreenPoint(worldHealthBarPos);
        var delta = bar.transform.position - old;

        //translate the other stats components so their placement is the same relative to the healthbar
        barText.transform.Translate(delta);


        this.porpsPivotPoint = getProp_UI("shield").position + delta;
        UpdatePropsUI();


        this.health = maxHealth;
        updateSliderValue();


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

    public void applyDamage(int damage, bool ignoreShield=false)
    {
        if(ignoreShield == false)
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
        }
        else
        {
            health -= damage;
        }
        updateSliderValue();
        UpdatePropsUI();

        if (health <= 0)
        {
            NewTurnScript.onKill();
            if (isPlayer)
            {
                corpseAvatar.SetActive(true);
                avatar.SetActive(false);
            }
        }
    }

    public void startTurnEffects()
    {
        if (wraith > 0)
        {
            dexterity -= wraith;
        }
        shield = 0;
        if (poison > 0)
        {
            applyDamage(poison, true); //ignores shield
            poison -= 1;
            updateSliderValue();
        }
    }

    public void endTurnEffects()
    {
        if (ritual > 0)
        {
            strength += ritual;
        }
    }
    public void consumeEffect(Effect e, healthBarScript sender)
    {
        CardManagerScript cardManager = GameObject.FindGameObjectWithTag("CardManager").GetComponent<CardManagerScript>();
        int damage = e.damage;

        if(e.ritual > 0)
        {
            ritual += e.ritual;
        }
        if (e.poison > 0)
        {
            poison += e.poison;
        }
        if (e.shivsToSpawn > 0)
        {
            cardManager.InstantiateShiv(e.shivsToSpawn);
        }
        if (e.shivsAtTurnStart > 0)
        {
            blades += e.shivsAtTurnStart;
        }

        if (e.cardsToRetain > 0)
        {
            cardManager.hasRetained = false;
            cardManager.retainUpToNCards = e.cardsToRetain;
            retain = e.cardsToRetain;
        }


        if (e.shivBonusDmg > 0)
        {
            accuracy += e.shivBonusDmg;
        }

        if (e.isShiv)
        {
            damage += GameObject.FindGameObjectWithTag("Player").GetComponent<healthBarScript>().accuracy;
        }

        dexterity += e.dexterity;
        intagible += e.intangible;
        wraith += e.wraith;

        
        if (intagible > 0 && damage > 1)
        {
            damage = 1;
            intagible -= 1;
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

        strength += e.strength;

        if(damage > 0)
        {
            damage += sender.strength;
            if (damage < 0)
            {
                damage = 0;
            }
        }
        

        if (e.weak > 0)
        {
            weak += e.weak;
        }
        if( sender.weak > 0)
        {
            sender.weak -= 1;
            damage = (int)(damage * 0.75);
            sender.UpdatePropsUI();
        }

        if (e.heal > 0)
        {
            int newHealth = health + e.heal;
            health = newHealth > maxHealth ? maxHealth : newHealth;
        }
        applyDamage(damage);

        if(e.shield > 0)
        {
            int delta_shield = e.shield + dexterity;
            if (delta_shield > 0)
            {
                shield += delta_shield;
            }
        }


        e.ApplyMeta();

        UpdatePropsUI();
    }


    // Update is called once per frame
    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class healthBarScript : MonoBehaviour
{
    public Slider bar;
    public TextMeshProUGUI barText;
    public TextMeshProUGUI shieldText;
    public GameObject shieldIcon;
    public int maxHealth = 30;
    public int shield = 0;
    public int health;
    public bool isPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        //fetch components
        bar = transform.Find("statsCanvas").Find("Slider").GetComponent<Slider>();
        barText = transform.Find("statsCanvas").Find("Health").GetComponent<TextMeshProUGUI>();
        shieldText = transform.Find("statsCanvas").Find("shieldCountText").GetComponent<TextMeshProUGUI>();
        shieldIcon = transform.Find("statsCanvas").Find("shieldIcon").gameObject;

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
    }

    void updateSliderValue()
    {
        float value = (float)health / (float)maxHealth;
        if(value < 0f)
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
        if(shield > 0)
        {
            if(shield > damage)
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
    }
    
    public void UpdateShield(int delta=0)
    {
        shield += delta;
        shieldText.SetText(shield.ToString());
    }
    public void consumeEffect(Effect e)
    {
        applyDamage(e.damage);
        UpdateShield(e.shield);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

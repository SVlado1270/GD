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
    public int maxHealth = 30;
    public int shield = 0;
    public int health;
    public bool isPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        bar = transform.Find("Canvas").Find("Slider").GetComponent<Slider>();
        barText = transform.Find("Canvas").Find("Health").GetComponent<TextMeshProUGUI>();
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
        if (isPlayer) //TODO: remove this if
        {
            shield += delta;
            shieldText.SetText(shield.ToString()); //TODO: function to update all UI components
        }
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

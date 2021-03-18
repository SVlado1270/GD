using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTurnScript : MonoBehaviour
{
    energyManagerScript energyManager;
    CardManagerScript cardManager;
    healthBarScript healthBar;

    private void Start()
    {
        energyManager = GameObject.FindGameObjectWithTag("Player").GetComponent<energyManagerScript>();
        cardManager = GameObject.FindGameObjectWithTag("CardManager").GetComponent<CardManagerScript>();
        healthBar = GameObject.FindGameObjectWithTag("Player").GetComponent<healthBarScript>();
    }

    public void OnButtonPress()
    {
        EnemyTurn();
        PlayerNewTurn();
    }

    public void PlayerNewTurn()
    {
        energyManager.ResetEnergy();
        cardManager.newHand();
        healthBar.UpdateShield();
    }

    public void EnemyTurn()
    {
        healthBar.applyDamage(Random.Range(4, 9));
        healthBar.shield = 0;
        healthBar.UpdateShield();
    }
}

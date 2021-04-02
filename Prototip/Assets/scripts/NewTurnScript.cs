using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTurnScript : MonoBehaviour
{
    energyManagerScript energyManager;
    CardManagerScript cardManager;
    healthBarScript playerStats;

    private void Start()
    {
        energyManager = GameObject.FindGameObjectWithTag("Player").GetComponent<energyManagerScript>();
        cardManager = GameObject.FindGameObjectWithTag("CardManager").GetComponent<CardManagerScript>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<healthBarScript>();
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
        playerStats.UpdateShield();
    }

    public void EnemyTurn()
    {
        playerStats.applyDamage(Random.Range(4, 9));
        playerStats.shield = 0;
        playerStats.UpdateShield();
    }
}

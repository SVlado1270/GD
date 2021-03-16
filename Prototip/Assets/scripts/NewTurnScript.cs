using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTurnScript : MonoBehaviour
{
    public void OnButtonPress()
    {
        EnemyTurn();
        PlayerNewTurn();
    }

    public void PlayerNewTurn()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<energyManagerScript>().ResetEnergy();
        GameObject.FindGameObjectWithTag("CardManager").GetComponent<CardManagerScript>().newHand();
        var playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<healthBarScript>();
        playerStats.shield = 0;
        playerStats.UpdateShield();
    }

    public void EnemyTurn()
    {
        var playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<healthBarScript>();
        playerStats.applyDamage(Random.Range(4, 9));
        playerStats.shield = 0;
        playerStats.UpdateShield();
    }
}

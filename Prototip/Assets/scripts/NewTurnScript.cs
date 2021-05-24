using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewTurnScript : MonoBehaviour
{
    energyManagerScript energyManager;
    CardManagerScript cardManager;
    healthBarScript playerStats;
    healthBarScript enemyStats;


    //temporary intent ui fix 
    public GameObject intentUI;
    Effect enemyEffect;
    int currentTurnIndex = 0;

    public void ChooseAnotherActionForEnemy()
    {
        if(currentTurnIndex == 0)
        {
            int ritual = 3;
            string hover_text = "This enemy intends to use a Buff.";
            enemyStats.UpdateIntent("buff_intent", ritual, hover_text);
            enemyEffect = new Effect(TargetType.Enemy) { ritual = ritual };
        }
        else
        {
            int enemyDamage = 6;
            string hover_text = "This enemy intends to attack for " + enemyDamage.ToString() + " damage.";
            enemyStats.UpdateIntent("attack_intent", enemyDamage, hover_text);
            enemyEffect = new Effect(TargetType.Player) { damage = enemyDamage };
        }
        currentTurnIndex++;
    }

    //



    private void Start()
    {
        currentTurnIndex = 0;
        energyManager = GameObject.FindGameObjectWithTag("Player").GetComponent<energyManagerScript>();
        cardManager = GameObject.FindGameObjectWithTag("CardManager").GetComponent<CardManagerScript>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<healthBarScript>();
        enemyStats = GameObject.FindGameObjectWithTag("Enemy").GetComponent<healthBarScript>();
        ChooseAnotherActionForEnemy();

        //temporary placement for ui intent
        //TODO: remove this
    }

    public void OnButtonPress()
    {
        NewTurn();
    }
    public void NewTurn()
    {
        if (cardManager.hasRetained == false && cardManager.retainUpToNCards > 0)
        {
            cardManager.SelectCardsMode(cardManager.retainUpToNCards, CardSelectionType.Retain);
            return;
        }

        if (cardManager.gameState != GameState.Combat)
        {
            TooltipScript.ShowTooltip("Finish card selection first", 3f);
            return;
        }

        
        EnemyTurn();
        PlayerNewTurn();
        cardManager.hasRetained = false;
    }

    public void PlayerNewTurn()
    {
        playerStats.startTurnEffects(); 
        energyManager.ResetEnergy();
        
        cardManager.newHand();
        
        playerStats.UpdatePropsUI();
        enemyStats.UpdatePropsUI();

        playerStats.endTurnEffects();
    }

    public void EnemyTurn()
    {
        enemyStats.startTurnEffects(); //reset previous block


        //act acoording to last choice
        if (enemyEffect.targetType == TargetType.Player)
        {
            playerStats.consumeEffect(enemyEffect, enemyStats);
        }
        else
        {
            enemyStats.consumeEffect(enemyEffect, enemyStats);
        }

        //choose action for next turn
        ChooseAnotherActionForEnemy();


        enemyStats.endTurnEffects();
        playerStats.UpdatePropsUI();
        enemyStats.UpdatePropsUI();
    }
}

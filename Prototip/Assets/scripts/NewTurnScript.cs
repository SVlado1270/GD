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

    public GameObject[] enemies;
    int currentEnemyIndex;
    healthBarScript enemyStats;

    int cardsToUnlock;

    Effect enemyEffect;
    int currentTurnIndex = 0;

    public static void onKill()
    {
        //Debug.Log("kill happened");
    }

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
        cardsToUnlock = 1;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject e in enemies)
        {
            e.SetActive(false);
        }



        energyManager = GameObject.FindGameObjectWithTag("Player").GetComponent<energyManagerScript>();
        cardManager = GameObject.FindGameObjectWithTag("CardManager").GetComponent<CardManagerScript>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<healthBarScript>();


        currentEnemyIndex = -1;
        NextLevel();
    }

    public void OnButtonPress()
    {
        NewTurn();
    }

    public bool checkForKill()
    {
        if (enemyStats.isDead())
        {
            NextLevel();
            return true;
        }
        else if (playerStats.isDead())
        {
            cardManager.gameState = GameState.GameOver;
            return true;
        }
        return false;
    }

    public void NewTurn()
    {
        print("NEW TURN CALLED");
        if (enemyStats.isDead())
        {
            if (cardManager.nCardsToSelect == 0 && cardManager.hasUnlocked == false)
            {
                return;
            }
            NextLevel();
        }

        if (cardManager.hasRetained == false && cardManager.retainUpToNCards > 0)
        {
            cardManager.SelectCardsMode(cardManager.retainUpToNCards, CardSelectionType.Retain);
            return;
        }

        if (cardManager.gameState != GameState.Combat && cardManager.gameState != GameState.GameOver)
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

    public void playerResetAtNewLevel()
    {
        playerStats.resetStats();
        energyManager.ResetEnergy();

        cardManager.newHand();
        playerStats.UpdatePropsUI();
        enemyStats.UpdatePropsUI();
    }

    public void EnemyTurn()
    {
        if (checkForKill())
        {
            return;
        }
        enemyStats.startTurnEffects(); //reset previous block


        if (checkForKill())
        {
            return;
        }
        //act acoording to last choice
        if (enemyEffect.targetType == TargetType.Player)
        {
            playerStats.consumeEffect(enemyEffect, enemyStats);
        }
        else
        {
            enemyStats.consumeEffect(enemyEffect, enemyStats);
        }
        if(checkForKill())
        {
            return;
        }


        //choose action for next turn
        ChooseAnotherActionForEnemy();

        enemyStats.endTurnEffects();
        if (checkForKill())
        {
            return;
        }

        playerStats.UpdatePropsUI();
        enemyStats.UpdatePropsUI();
    }

    public void NextLevel()
    {
        currentTurnIndex = 0;
        if(currentEnemyIndex >= 0)
        {
            cardManager.SelectCardsMode(cardsToUnlock, CardSelectionType.Unlock);
            enemies[currentEnemyIndex].SetActive(false);
        }
        if(currentEnemyIndex + 1 < enemies.Length)
        {
            currentEnemyIndex++;
            enemies[currentEnemyIndex].SetActive(true);
            enemyStats = enemies[currentEnemyIndex].GetComponent<healthBarScript>();
            ChooseAnotherActionForEnemy();
        }
        else
        {
            print("GAMEOVER");
        }
    }
}

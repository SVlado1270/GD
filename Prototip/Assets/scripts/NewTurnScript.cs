﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewTurnScript : MonoBehaviour
{
    public TextMeshProUGUI gameOverMessage;
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
        switch(enemyStats.name)
        {
            case "cultist":
                {
                    if (currentTurnIndex == 0)
                    {
                        int ritual = 3;
                        string hover_text = "This enemy intends to use a Buff.";
                        enemyStats.UpdateIntent("buff_intent", ritual, hover_text);
                        enemyEffect = new Effect(TargetType.Enemy) { ritual = ritual };
                    }
                    else
                    {
                        int damage = 6;
                        string hoverText = "This enemy intends to attack for " + damage.ToString() + " damage.";
                        enemyStats.UpdateIntent("attack_intent", damage, hoverText);
                        enemyEffect = new Effect(TargetType.Player) { damage = damage };
                    }
                    break;
                }
            case "slaver":
                {
                    int damage;
                    if (currentTurnIndex % 2 == 0)
                    {
                        damage = Random.Range(4, 11);
                    }
                    else
                    {
                        damage = Random.Range(12, 15);
                    }
                    string hoverText = "This enemy intends to attack for " + damage.ToString() + " damage.";
                    enemyStats.UpdateIntent("attack_intent", damage, hoverText);
                    enemyEffect = new Effect(TargetType.Player) { damage = damage };
                    break;
                }
            case "mantisLord":
                {
                    int damage;
                    if (currentTurnIndex % 2 == 0)
                    {
                        damage = Random.Range(6, 15);
                    }
                    else
                    {
                        damage = Random.Range(16, 20);
                    }
                    string hoverText = "This enemy intends to attack for " + damage.ToString() + " damage.";
                    enemyStats.UpdateIntent("attack_intent", damage, hoverText);
                    enemyEffect = new Effect(TargetType.Player) { damage = damage };
                    break;
                }
            case "fungi":
                {
                    float p = Random.Range(0f, 100f);
                    if (p <= 55f)
                    {
                        int poison = Random.Range(3, 8);
                        string hoverText = "This enemy intends to apply " + poison.ToString() + " poison.";
                        enemyStats.UpdateIntent("poison_intent", poison, hoverText);
                        enemyEffect = new Effect(TargetType.Player) { poison = poison };
                    }
                    else
                    {
                        int shield = Random.Range(3, 10);
                        string hoverText = "This enemy intends to defend.";
                        enemyStats.UpdateIntent("shield_intent", shield, hoverText);
                        enemyEffect = new Effect(TargetType.Enemy) { shield = shield };
                    }
                    break;
                }
            case "umu":
                {
                    float p = Random.Range(0f, 100f);
                    if (p <= 60f)
                    {
                        int damage = 6;
                        string hoverText = "This enemy intends to attack for " + damage.ToString() + " damage.";
                        enemyStats.UpdateIntent("attack_intent", damage, hoverText);
                        enemyEffect = new Effect(TargetType.Player) { damage = damage };
                    }
                    else
                    {
                        int strength = 3;
                        string hoverText = "This enemy intends to use a Buff.";
                        enemyStats.UpdateIntent("buff_intent", strength, hoverText);
                        enemyEffect = new Effect(TargetType.Enemy) { strength = strength };
                    }
                    break;
                }
            case "radiance":
                {
                    float p = Random.Range(0f, 100f);
                    if (p < 55f)
                    {
                        int damage = 10;
                        string hoverText = "This enemy intends to attack and use a debuff.";
                        enemyStats.UpdateIntent("attack_debuff_intent", damage, hoverText);
                        enemyEffect = new Effect(TargetType.Player) { damage = damage, wraith = 1 };
                        break;
                    }
                    else if(p < 90f)
                    {
                        int shield = 10;
                        string hoverText = "This enemy intends to defend.";
                        enemyStats.UpdateIntent("shield_intent", shield, hoverText);
                        enemyEffect = new Effect(TargetType.Enemy) { shield = shield };
                        break;
                    }
                    else
                    {
                        int value = Random.Range(1, 3);
                        string hoverText = "This enemy intends to apply a debuff.";
                        enemyStats.UpdateIntent("debuff_intent", value, hoverText);
                        enemyEffect = new Effect(TargetType.Player) { weak = value, strength = -value };
                        break;
                    }
                }
            case "corruptHeart":
                {
                    float p = Random.Range(0f, 100f);
                    if(p < 45f)
                    {
                        int damage = 10;
                        string hoverText = "This enemy intends to attack for " + damage.ToString() + " damage.";
                        enemyStats.UpdateIntent("attack_intent", damage, hoverText);
                        enemyEffect = new Effect(TargetType.Player) { damage = damage };
                        break;
                    }
                    else
                    {
                        int shield = 10;
                        int strength = Random.Range(1, 6);
                        string hoverText = "This enemy intends to defend and use a buff.";
                        enemyStats.UpdateIntent("defend_buff_intent", shield, hoverText);
                        enemyEffect = new Effect(TargetType.Enemy) { shield = shield, strength = strength };
                        break;
                    }
                }
            case "collector":
                {
                    float p = Random.Range(0f, 100f);
                    if (p < 50f)
                    {
                        int shield = 15;
                        int strength = 3;
                        string hoverText = "This enemy intends to defend and use a buff.";
                        enemyStats.UpdateIntent("defend_buff_intent", shield, hoverText);
                        enemyEffect = new Effect(TargetType.Enemy) { shield = shield, strength = strength };
                        break;
                    }
                    else
                    {
                        int strength = -1;
                        int damage = Random.Range(4, 7);
                        string hoverText = "This enemy intends to attack and use a debuff.";
                        enemyStats.UpdateIntent("attack_debuff_intent", damage, hoverText);
                        enemyEffect = new Effect(TargetType.Player) { damage = damage, strength = strength };
                        break;
                    }
                }
        }
        currentTurnIndex++;
    }




    private void Start()
    {
        cardsToUnlock = 1;
        
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
            transform.Find("Text").GetComponent<Text>().text = "RESTART";
            cardManager.gameState = GameState.GameOver;
            return true;
        }
        return false;
    }

    public void RestartGame()
    {
        for(int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<healthBarScript>().resetStats(true);
            enemies[i].SetActive(false);
        }
        currentEnemyIndex = -1;
        playerStats.resetStats(true);
        energyManager.ResetEnergy();

        cardManager.ChangeCardsWithState(CardState.Exhausted, CardState.InDrawPile);
        cardManager.ChangeCardsWithState(CardState.InDiscardPile, CardState.InDrawPile);
        cardManager.ChangeCardsWithState(CardState.InHand, CardState.InDrawPile);
        cardManager.newHand();
        playerStats.UpdatePropsUI();
        playerStats.updateSliderValue();

        enemyStats.UpdatePropsUI();

        playerStats.corpseAvatar.SetActive(false);
        playerStats.avatar.SetActive(true);
        NextLevel();
        transform.Find("Text").GetComponent<Text>().text = "END TURN";
        cardManager.gameState = GameState.Combat;
    }

    public void NewTurn()
    {
        print("NEW TURN CALLED");

        if(cardManager.gameState == GameState.GameOver)
        {
            TooltipScript.ShowTooltip("You died.", 3f);
            RestartGame();
            return;
        }

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
        playerStats.resetStats(false);
        energyManager.ResetEnergy();

        cardManager.ChangeCardsWithState(CardState.Exhausted, CardState.InDrawPile);
        cardManager.ChangeCardsWithState(CardState.InDiscardPile, CardState.InDrawPile);
        cardManager.ChangeCardsWithState(CardState.InHand, CardState.InDrawPile);
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
            if(currentEnemyIndex + 1 < enemies.Length && cardManager.CountCardsWithState(CardState.Locked) > 0)
            {
                cardManager.SelectCardsMode(cardsToUnlock, CardSelectionType.Unlock);
            }
            enemies[currentEnemyIndex].SetActive(false);
        }
        if(currentEnemyIndex + 1 < enemies.Length)
        {
            currentEnemyIndex++;

            if(currentTurnIndex > 5)
            {
                playerStats.applyHeal(5);
            }

            enemies[currentEnemyIndex].SetActive(true);
            enemyStats = enemies[currentEnemyIndex].GetComponent<healthBarScript>();
            enemyStats.UpdatePropsUI();
            enemyStats.updateSliderValue();
            ChooseAnotherActionForEnemy();
        }
        else
        {
            cardManager.hasUnlocked = true;
            gameOverMessage.gameObject.SetActive(true);
            print("GAMEOVER");
            gameObject.SetActive(false);
        }
        print(enemyStats.name);
    }
}

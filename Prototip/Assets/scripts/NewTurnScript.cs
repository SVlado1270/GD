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


    //temporary intent ui fix 
    public GameObject intentUI;
    int enemyDamage;

    public void ChooseAnotherActionForEnemy()
    {
        enemyDamage = Random.Range(4, 15);
        intentUI.transform.Find("text").GetComponent<TextMeshProUGUI>().SetText(enemyDamage.ToString());
        intentUI.GetComponent<StaticTooltip>().message = "This enemy intends to attack for " + enemyDamage.ToString() + " damage.";
    }

    //



    private void Start()
    {
        ChooseAnotherActionForEnemy();
        energyManager = GameObject.FindGameObjectWithTag("Player").GetComponent<energyManagerScript>();
        cardManager = GameObject.FindGameObjectWithTag("CardManager").GetComponent<CardManagerScript>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<healthBarScript>();

        //temporary placement for ui intent
        //TODO: remove this
        intentUI.transform.position = Camera.main.WorldToScreenPoint(GameObject.Find("Character/Villains/cultist/avatar").transform.position + new Vector3(0f, 5f));
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
        energyManager.ResetEnergy();
        cardManager.newHand();
        playerStats.UpdateShield();
    }

    public void EnemyTurn()
    {
        playerStats.consumeEffect(new Effect(TargetType.Player) { damage = enemyDamage });
        playerStats.shield = 0;
        playerStats.UpdateShield();
        ChooseAnotherActionForEnemy();
    }
}

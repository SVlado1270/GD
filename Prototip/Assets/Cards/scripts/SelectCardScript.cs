﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SelectCardScript : MonoBehaviour
{
    private bool isDragging = false;
    private CardManagerScript cardManager;
    private cardPrefabScript controller;


    private void Start()
    {
        cardManager = GameObject.FindGameObjectWithTag("CardManager").GetComponent<CardManagerScript>();
        controller = GetComponent<cardPrefabScript>();
    }



    public void OnMouseDown()
    {
        switch (cardManager.gameState)
        {
            case GameState.Combat:
                isDragging = true;
                controller.PutOnTop();
                break;
            case GameState.SelectCards:
                if (controller.state == CardState.InHand)
                {
                    controller.state = CardState.Selected;
                }
                else if (controller.state == CardState.Selected) 
                {
                    controller.state = CardState.InHand;
                }
                cardManager.PlaceCards();
                break;
        }
    }

    public void OnMouseUp()
    {
        switch (cardManager.gameState)
        {
            case GameState.Combat:
                isDragging = false;
                castCardIfNeeded();
                break;
        }
    }
    void OnMouseOver()
    {
        switch (controller.state)
        {
            case CardState.InHand:
                controller.Highlight();
                break;
        }
    }

    void OnMouseExit()
    {
        switch (controller.state)
        {
            case CardState.InHand:
                controller.removeHighlight();
                break;
        }
    }

    bool CardIsTuchingCharacter(GameObject character)
    {
        return GetComponent<BoxCollider2D>().IsTouching(character.GetComponent<BoxCollider2D>());
    }

    void castCardIfNeeded()
    {
        var energyManager = GameObject.FindGameObjectWithTag("Player").GetComponent<energyManagerScript>();

        if (energyManager.canAfford(controller) == false)
        {
            TooltipScript.ShowTooltip("not enough energy", 1f);
            return;
        }

        GameObject receiverObject = FindClosestHoveredCharacter();
        if (receiverObject)
        {
            healthBarScript receiver = receiverObject.transform.parent.GetComponent<healthBarScript>();
            receiver.consumeEffect(controller.effect);
            controller.state = CardState.InDiscardPile;
            energyManager.consumeCard(controller);
        }
    }

    GameObject FindClosestHoveredCharacter()
    {
        var allCharacters = GameObject.FindGameObjectsWithTag("Character");
        GameObject focusedCharacter = null;
        float closestHoveringDistance = float.MaxValue;
        foreach (GameObject character in allCharacters)
        {
            character.GetComponent<receiveCardEffectsScript>().SetHighlight(false);
            float distance = (character.transform.position - transform.position).magnitude;
            if (distance < closestHoveringDistance && CardIsTuchingCharacter(character))
            {
                closestHoveringDistance = distance;
                focusedCharacter = character;
            }
        }
        return focusedCharacter;
    }
    void HighlightCharacterOnHover()
    {
        GameObject characterToHighlight = FindClosestHoveredCharacter();
        if (characterToHighlight)
        {
            characterToHighlight.GetComponent<receiveCardEffectsScript>().SetHighlight(true);
        }
    }

    void Update()
    {
        if(cardManager.gameState == GameState.Combat)
        {
            if (isDragging)
            {
                HighlightCharacterOnHover();
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                transform.Translate(mousePosition);
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SelectCardScript : MonoBehaviour
{
    private bool isDragging;


    public void OnMouseDown()
    {
        isDragging = true;
        GetComponent<cardPrefabScript>().PutOnTop();
    }

    public void OnMouseUp()
    {
        isDragging = false;
        castCardIfNeeded();
    }

    bool CardIsTuchingCharacter(GameObject character)
    {
        return GetComponent<BoxCollider2D>().IsTouching(character.GetComponent<BoxCollider2D>());
    }

    void castCardIfNeeded()
    {
        var energyManager = GameObject.FindGameObjectWithTag("Player").GetComponent<energyManagerScript>();
        var controller = GetComponent<cardPrefabScript>();
        if (energyManager.canAfford(controller))
        {
            GameObject receiverObject = FindClosestHoveredCharacter();
            if (receiverObject)
            {
                healthBarScript receiver = receiverObject.transform.parent.GetComponent<healthBarScript>();
                receiver.consumeEffect(controller.effect);
                controller.state = CardState.InDiscardPile;
                energyManager.consumeCard(controller);
            }
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
        if (isDragging)
        {
            HighlightCharacterOnHover();
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePosition);
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }
}

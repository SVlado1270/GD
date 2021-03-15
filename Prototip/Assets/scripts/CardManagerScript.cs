using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManagerScript : MonoBehaviour
{
    public GameObject cardPrefab;
    public List<GameObject> allCards;
    public float cardWidth;

    void Start()
    {
        //some random cards for testing purposes
        //params:       title,          flavour,            sprite,     description,    energy, effect(dmg, shield)
        InstantiateCard("Strike",       CardFlavour.Attack, "silent1",  "deals 6 damage",   1,  new Effect(6, 0));
        InstantiateCard("Strike",       CardFlavour.Attack, "silent1",  "deals 6 damage",   1,  new Effect(6, 0));
        InstantiateCard("Defend",       CardFlavour.Skill,  "silent5",  "gain 5 block",     1,  new Effect(0, 5));
        InstantiateCard("Strike",       CardFlavour.Attack, "silent1",  "deals 6 damage",   1,  new Effect(6, 0));
        InstantiateCard("Defend",       CardFlavour.Skill,  "silent5",  "gain 5 block",     1,  new Effect(0, 5));

        //TODO add count parameter, allow having the same card multiple times in the deck
        PlaceCards();
    }

    void InstantiateCard(string title, CardFlavour flavour, string spriteName, string description, int energy, Effect effect)
    {
        GameObject card = Instantiate(cardPrefab, this.transform.position, Quaternion.identity);
        cardPrefabScript cardController = card.GetComponent<cardPrefabScript>();
        cardController.Init(title, flavour, spriteName, description, energy, effect);
        allCards.Add(card);
    }

    public void PlaceCards()
    {
        float cardScale = 0.7f;
        float maxRotation = 15f;
        float perCardRotation;

        if (allCards.Count == 1)
        {
            perCardRotation = 0f;
        }
        else
        {
            perCardRotation = 2 * maxRotation / (allCards.Count - 1);
        }
        float maxWidth = 800f;  // TODO: remove hardcoded value
        //spacing = cardWidth - some overlapping
        float cardWidth = 4f;  // TODO: remove hardcoded value
        cardWidth *= cardScale;
        float spacing = cardWidth * 0.9f; // spacing is less than the card width so they overlap a little bit
        if (spacing * (allCards.Count - 1)> maxWidth)
        {
            spacing = maxWidth / (allCards.Count - 1);
        }
        Vector2 center = transform.position;
        float leftCoord = center.x - spacing * (allCards.Count - 1) / 2;

        for (int i = 0; i < allCards.Count; i++)
        {
            GameObject card = allCards[i];
            card.transform.rotation = Quaternion.Euler(0, 0, maxRotation - i * perCardRotation);
            card.transform.localPosition = new Vector2(leftCoord + i * spacing, center.y);
            card.transform.localScale = new Vector3(cardScale, cardScale, cardScale);
            card.GetComponent<cardPrefabScript>().PutOnTop();
        }
    }
    void Update()
    {
        
    }
}

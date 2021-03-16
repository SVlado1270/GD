using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardManagerScript : MonoBehaviour
{
    public TextMeshProUGUI discardPileCountText;
    public TextMeshProUGUI drawPileCountText;
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
        InstantiateCard("Strike2", CardFlavour.Attack, "silent1", "deals 6 damage", 1, new Effect(6, 0));
        InstantiateCard("Strike2", CardFlavour.Attack, "silent1", "deals 6 damage", 1, new Effect(6, 0));
        InstantiateCard("Defend2", CardFlavour.Skill, "silent5", "gain 5 block", 1, new Effect(0, 5));
        InstantiateCard("Strike2", CardFlavour.Attack, "silent1", "deals 6 damage", 1, new Effect(6, 0));
        InstantiateCard("Defend2", CardFlavour.Skill, "silent5", "gain 5 block", 1, new Effect(0, 5));
        InstantiateCard("Strike3", CardFlavour.Attack, "silent1", "deals 6 damage", 1, new Effect(6, 0));
        InstantiateCard("Strike3", CardFlavour.Attack, "silent1", "deals 6 damage", 1, new Effect(6, 0));
        InstantiateCard("Defend3", CardFlavour.Skill, "silent5", "gain 5 block", 1, new Effect(0, 5));
        InstantiateCard("Strike3", CardFlavour.Attack, "silent1", "deals 6 damage", 1, new Effect(6, 0));
        InstantiateCard("Defend3", CardFlavour.Skill, "silent5", "gain 5 block", 1, new Effect(0, 5));

        //TODO add count parameter, allow having the same card multiple times in the deck

        newHand();
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

        int inHandCards = 0;
        for (int i = 0; i < allCards.Count; i++)
        {
            cardPrefabScript controller = allCards[i].GetComponent<cardPrefabScript>();
            if (controller.state == CardState.InHand)
            {
                inHandCards++;
            }
        }

        if (inHandCards == 1)
        {
            perCardRotation = 0f;
        }
        else
        {
            perCardRotation = 2 * maxRotation / (inHandCards - 1);
        }
        float maxWidth = 800f;  // TODO: remove hardcoded value
        //spacing = cardWidth - some overlapping
        float cardWidth = 4f;  // TODO: remove hardcoded value
        cardWidth *= cardScale;
        float spacing = cardWidth * 0.9f; // spacing is less than the card width so they overlap a little bit
        if (spacing * (inHandCards - 1)> maxWidth)
        {
            spacing = maxWidth / (inHandCards - 1);
        }
        Vector2 center = transform.position;
        float leftCoord = center.x - spacing * (inHandCards - 1) / 2;

        int drawPileCount = 0;
        int discardPileCount = 0;
        int inHandIndex = 0;
        for (int i = 0; i < allCards.Count; i++)
        {
            GameObject card = allCards[i];
            var controller = card.GetComponent<cardPrefabScript>();
            switch (controller.state)
            {
                case CardState.InHand:
                    card.gameObject.SetActive(true);
                    card.transform.rotation = Quaternion.Euler(0, 0, maxRotation - inHandIndex * perCardRotation);
                    card.transform.localPosition = new Vector2(leftCoord + inHandIndex * spacing, center.y);
                    card.transform.localScale = new Vector3(cardScale, cardScale, cardScale);
                    inHandIndex++;
                    controller.PutOnTop();
                    break;
                case CardState.InDrawPile:
                    card.gameObject.SetActive(false);
                    drawPileCount++;
                    break;
                case CardState.InDiscardPile:
                    card.gameObject.SetActive(false);
                    discardPileCount++;
                    break;
            }
            discardPileCountText.SetText("Discard pile: {0}", discardPileCount);
            drawPileCountText.SetText("Draw pile: {0}", drawPileCount);
        }
    }

    public void newHand()
    {
        int inHandCount = 0;
        for (int i = 0; i < allCards.Count; i++)
        {
            GameObject card = allCards[i];
            var controller = card.GetComponent<cardPrefabScript>();
            switch (controller.state)
            {
                case CardState.InHand:
                    controller.state = CardState.InDiscardPile;
                    break;
                case CardState.InDrawPile:
                    if (inHandCount < 5)
                    {
                        inHandCount++;
                        controller.state = CardState.InHand;
                    }
                    break;
                case CardState.InDiscardPile:
                    break;
            }
        }
        PlaceCards();
    }
    void Update()
    {

    }
};

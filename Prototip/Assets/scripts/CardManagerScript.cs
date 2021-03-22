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
    public int cardsToDraw = 5; 


    void Start()
    {
        //some random cards for testing purposes
        //params:       title,          flavour,            sprite,     description,    energy, effect(dmg, shield),    count
        InstantiateCard("Strike",       CardFlavour.Attack, "silent1",  "deals 6 damage",   1,  new Effect(6, 0),       15);
        InstantiateCard("Defend",       CardFlavour.Skill,  "silent5",  "gain 5 block",     1,  new Effect(0, 5),       15);


        newHand();
        PlaceCards();
    }



    void InstantiateCard(string title, CardFlavour flavour, string spriteName, string description, int energy, Effect effect, int count=1)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject card = Instantiate(cardPrefab, this.transform.position, Quaternion.identity);
            cardPrefabScript cardController = card.GetComponent<cardPrefabScript>();
            cardController.Init(title, flavour, spriteName, description, energy, effect);
            allCards.Add(card);
        }
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
        if (spacing * (inHandCards - 1) > maxWidth)
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
            discardPileCountText.SetText(discardPileCount.ToString());
            drawPileCountText.SetText(drawPileCount.ToString());
        }
    }

    public int CountCardsWithState(CardState state)
    {
        int counter = 0;
        for (int i = 0; i < allCards.Count; i++)
        {
            if (allCards[i].GetComponent<cardPrefabScript>().state == state)
            {
                counter++;
            }
        }
        return counter;
    }

    public void MoveDiscardedCardsToDrawPile()
    {
        for (int i = 0; i < allCards.Count; i++)
        {
            var controller = allCards[i].GetComponent<cardPrefabScript>();
            if(controller.state == CardState.InDiscardPile)
            {
                controller.state = CardState.InDrawPile;
                //TODO: trigger ? animation
            }
        }
    }

    public void DiscardAllInHandCards()
    {
        for (int i = 0; i < allCards.Count; i++)
        {
            GameObject card = allCards[i];
            var controller = card.GetComponent<cardPrefabScript>();
            if(controller.state == CardState.InHand)
            {
                controller.state = CardState.InDiscardPile;
                //TODO: trigger discard animation
            }
        }
    }

    public bool DrawRandomCard()
    {
        var indexes = new List<int>();
        for (int i = 0; i < allCards.Count; i++)
        {
            GameObject card = allCards[i];
            var controller = card.GetComponent<cardPrefabScript>();
            if (controller.state == CardState.InDrawPile)
            {
                indexes.Add(i);
            }
        }
        if (indexes.Count <= 0)
        {
            return false;
        }
        var random = Random.Range(0, indexes.Count);
        allCards[indexes[random]].GetComponent<cardPrefabScript>().state = CardState.InHand;
        //TODO: trigger draw animation
        return true;
    }

    public void newHand()
    {
        DiscardAllInHandCards();
        for (int i = 0; i < cardsToDraw; i++)
        {
            var drawPileNotEmpty = DrawRandomCard();
            if (drawPileNotEmpty == false)
            {
                MoveDiscardedCardsToDrawPile();
                DrawRandomCard();
            }
        }
        PlaceCards();
    }
    void Update()
    {

    }
};

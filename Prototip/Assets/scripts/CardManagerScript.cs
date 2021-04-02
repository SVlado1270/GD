using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum GameState
{
    Combat,
    SelectCards,
    DisplayDeck
}

public class CardManagerScript : MonoBehaviour
{
    public GameState gameState;
    public TextMeshProUGUI discardPileCountText;
    public TextMeshProUGUI drawPileCountText;
    public GameObject cardPrefab;
    public List<GameObject> allCards;
    public int cardsToDraw = 5;
    int nCardsToSelect;

    public GameObject confirmButton;


    //UI placement constants
    const float cardScale = 0.7f;
    const float maxRotation = 15f;
    const float maxWidth = 800f;  // TODO: remove hardcoded value
    //spacing = cardWidth - some overlapping
    const float cardWidth = 4f * cardScale;  // TODO: remove hardcoded value
    const float spacing = cardWidth * 0.9f; // spacing is less than the card width so they overlap a little bit

    void Start()
    {
        gameState = GameState.SelectCards;
        nCardsToSelect = 0;
        //some random cards for testing purposes
        //params:       title,          flavour,            sprite,     description,    energy, effect(dmg, shield),    count
        InstantiateCard("Strike",       CardFlavour.Attack, "silent1",  "deals 6 damage",   1,  new Effect(6, 0),       15);
        InstantiateCard("Defend",       CardFlavour.Skill,  "silent5",  "gain 5 block",     1,  new Effect(0, 5),       15);


        newHand();
        PlaceCards();
    }

    public void SelectCardsMode(int nCardsToSelect) 
    {
        gameState = GameState.SelectCards;
        this.nCardsToSelect = nCardsToSelect;
        confirmButton.SetActive(true);
    }

    public void DiscardCardsWithState(CardState state)
    {
        for (int i = 0; i < allCards.Count; i++)
        {
            GameObject card = allCards[i];
            var controller = card.GetComponent<cardPrefabScript>();
            if (controller.state == state)
            {
                controller.state = CardState.InDiscardPile;
                //TODO: trigger discard animation
            }
        }
    }

    public void ConfirmSelected()
    {
        gameState = GameState.Combat;
        this.nCardsToSelect = 0;
        DiscardCardsWithState(CardState.Selected);
        confirmButton.SetActive(false);
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
        int inHandCards = CountCardsWithState(CardState.InHand);
        float perCardRotation = inHandCards == 1 ? 0f : 2 * maxRotation / (inHandCards - 1);

        float inHandSpacing = spacing;
        if (inHandSpacing * (inHandCards - 1) > maxWidth) // if current spacing makes cards exit screen boundries --> increase overlapping
        {
            inHandSpacing = maxWidth / (inHandCards - 1);
        }
        Vector2 center = transform.position;
        float leftCoord = center.x - inHandSpacing * (inHandCards - 1) / 2;

        int nSelectedCards = CountCardsWithState(CardState.Selected);
        Vector2 selectedCenter = transform.position + new Vector3(0, 10f);
        float selectedSpacing = 2 * inHandSpacing;
        float selectedLeftCoord = selectedCenter.x - selectedSpacing * (nSelectedCards - 1) / 2;

        int inHandIndex = 0;
        int selectedIndex = 0;
        for (int i = 0; i < allCards.Count; i++)
        {
            GameObject card = allCards[i];
            var controller = card.GetComponent<cardPrefabScript>();
            switch (controller.state)
            {
                case CardState.InHand:
                    card.gameObject.SetActive(true);
                    card.transform.rotation = Quaternion.Euler(0, 0, maxRotation - inHandIndex * perCardRotation);
                    card.transform.localPosition = new Vector2(leftCoord + inHandIndex * inHandSpacing, center.y);
                    card.transform.localScale = new Vector3(cardScale, cardScale, cardScale);
                    inHandIndex++;
                    controller.PutOnTop();
                    break;
                case CardState.Selected:
                    card.gameObject.SetActive(true);
                    card.transform.localPosition = new Vector2(selectedLeftCoord + selectedIndex * selectedSpacing, selectedCenter.y);
                    card.transform.localScale = new Vector3(cardScale, cardScale, cardScale);
                    selectedIndex++;
                    controller.PutOnTop();
                    break;
                case CardState.InDrawPile:
                    card.gameObject.SetActive(false);
                    break;
                case CardState.InDiscardPile:
                    card.gameObject.SetActive(false);
                    break;
            }
            discardPileCountText.SetText(CountCardsWithState(CardState.InDiscardPile).ToString());
            drawPileCountText.SetText(CountCardsWithState(CardState.InDrawPile).ToString());
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
        DiscardCardsWithState(CardState.InHand);
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

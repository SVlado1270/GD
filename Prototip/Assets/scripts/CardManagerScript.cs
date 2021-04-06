using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public enum GameState
{
    Combat,
    SelectCards,
    DisplayDeck,
    GameOver
}

public enum CardSelectionType
{
    Discard,
    Retain
}

public class CardManagerScript : MonoBehaviour
{
    public GameState gameState;
    public TextMeshProUGUI discardPileCountText;
    public TextMeshProUGUI drawPileCountText;
    public TextMeshProUGUI exhaustPileCountText;
    public GameObject cardPrefab;
    public List<GameObject> allCards;
    public int cardsToDraw = 5;
    public int nCardsToSelect;
    public int retainUpToNCards = 0;
    public bool hasRetained = true;
    public CardSelectionType cardSelectionType;

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
        gameState = GameState.Combat;
        nCardsToSelect = 0;
        //some random cards for testing purposes
        //params:       title,          flavour,            sprite,     description,        energy, effect,                                         count
        InstantiateCard("Strike",       CardFlavour.Attack, "silent1",  "Deal 6 damage.",   1,      new Effect(TargetType.Enemy) {damage = 6},       6);
        InstantiateCard("Defend",       CardFlavour.Skill,  "silent5",  "Gain 5 block.",     1,      new Effect(TargetType.Player) {shield = 5},      6);
        InstantiateCard("Draw", CardFlavour.Skill, "silent2", "Draw two cards.", 1, new Effect(TargetType.Player) { cardsToDraw = 2 },  2);
        InstantiateCard("Compromise", CardFlavour.Skill, "silent4", "Gain 8 block, discard one card.", 1, new Effect(TargetType.Player) { cardsToDiscard = 1, shield = 8 }, 3);
        InstantiateCard("Retain", CardFlavour.Power, "silent7", "At the end of every turn retain up to one card. Exhaust.", 2, new Effect(TargetType.Player) { cardsToRetain = 1, exhaust = true }, 1);

        newHand();
        PlaceCards();
    }

    public void SelectCardsMode(int nCardsToSelect, CardSelectionType cardSelectionType) 
    {
        gameState = GameState.SelectCards;

        this.nCardsToSelect = nCardsToSelect;
        this.cardSelectionType = cardSelectionType;
        confirmButton.SetActive(true);

        switch (cardSelectionType)
        {
            case CardSelectionType.Discard:
                confirmButton.GetComponentInChildren<Text>().text = "DISCARD";
                break;
            case CardSelectionType.Retain:
                confirmButton.GetComponentInChildren<Text>().text = "RETAIN";
                break;
        }
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
        if(CountCardsWithState(CardState.InHand) + CountCardsWithState(CardState.Selected) > nCardsToSelect 
            && CountCardsWithState(CardState.Selected) < nCardsToSelect)
        {
            TooltipScript.ShowTooltip("select atleast " + nCardsToSelect.ToString() + " cards", 2.5f);
            return;
        }
        gameState = GameState.Combat;
        this.nCardsToSelect = 0;

        switch (cardSelectionType)
        {
            case CardSelectionType.Discard:
                DiscardCardsWithState(CardState.Selected);
                break;
            case CardSelectionType.Retain:
                ChangeCardsWithState(CardState.Selected, CardState.Retained);
                hasRetained = true;
                GameObject.FindGameObjectWithTag("EndTurnButton").GetComponent<NewTurnScript>().NewTurn();
                break;
        }
        confirmButton.SetActive(false);
        PlaceCards();
    }
    public int ChangeCardsWithState(CardState oldState, CardState newState)
    {
        int counter = 0;
        for (int i = 0; i < allCards.Count; i++)
        {
            var controller = allCards[i].GetComponent<cardPrefabScript>();
            if (controller.state == oldState)
            {
                controller.state = newState;
            }
        }
        return counter;
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
                    card.transform.rotation = Quaternion.identity;
                    selectedIndex++;
                    controller.PutOnTop();
                    break;
                case CardState.InDrawPile:
                    card.gameObject.SetActive(false);
                    break;
                case CardState.InDiscardPile:
                    card.gameObject.SetActive(false);
                    break;
                case CardState.Exhausted:
                    card.gameObject.SetActive(false);
                    break;
            }
            discardPileCountText.SetText(CountCardsWithState(CardState.InDiscardPile).ToString());
            drawPileCountText.SetText(CountCardsWithState(CardState.InDrawPile).ToString());
            exhaustPileCountText.SetText(CountCardsWithState(CardState.Exhausted).ToString());
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

    public void ForceDrawRandomCard()
    {
        var drawPileNotEmpty = DrawRandomCard();
        if (drawPileNotEmpty == false)
        {
            MoveDiscardedCardsToDrawPile();
            DrawRandomCard();
        }
    }

    public void newHand()
    {
        DiscardCardsWithState(CardState.InHand);
        ChangeCardsWithState(CardState.Retained, CardState.InHand);
        for (int i = 0; i < cardsToDraw; i++)
        {
            ForceDrawRandomCard();
        }
        PlaceCards();
    }
};

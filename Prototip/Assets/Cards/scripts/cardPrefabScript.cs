using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum TargetType
{
    Player,
    Enemy
}
public class Effect
{
    public Effect(TargetType targetType)
    {
        this.targetType = targetType;
        damage = 0;
        shield = 0;
        cardsToDraw = 0;
        cardsToDiscard = 0;
        cardsToRetain = 0;
        exhaust = false;
        shivsToSpawn = 0;
        replaceHandWithShivs = false;
        shivsAtTurnStart = 0;
        isShiv = false;
        shivBonusDmg = 0;
        dexterity = 0;
        weak = 0;
        strength = 0;
        poison = 0;
        intangible = 0;
        wraith = 0;
        ritual = 0;
    }
    public int damage;
    public int shield;
    public int cardsToDraw;
    public int cardsToDiscard;
    public int cardsToRetain;
    public bool exhaust;
    public int shivsToSpawn;
    public bool replaceHandWithShivs;
    public int shivsAtTurnStart;
    public bool isShiv;
    public int shivBonusDmg;
    public int dexterity;
    public int weak;
    public int strength;
    public int poison;
    public int intangible;
    public int wraith;
    public int ritual;
    public TargetType targetType;

    public void ApplyMeta()
    {
        CardManagerScript cardManager = GameObject.FindGameObjectWithTag("CardManager").GetComponent<CardManagerScript>();
        for (int i = 0; i < cardsToDraw; i++)
        {
            cardManager.ForceDrawRandomCard();
        }
        if (cardsToDiscard > 0)
        {
            cardManager.SelectCardsMode(cardsToDiscard, CardSelectionType.Discard);
        }
        if (replaceHandWithShivs)
        {
            int cardsInHand = cardManager.CountCardsWithState(CardState.InHand);
            cardManager.DiscardCardsWithState(CardState.InHand);
            cardManager.InstantiateShiv(cardsInHand);
        }
    }
};

public enum CardFlavour
{
    Attack,
    Skill,
    Power
};

public enum CardState
{
    InDrawPile,
    InHand,
    InDiscardPile,
    Selected,
    Retained,
    Exhausted,
    Locked,
    ToBeUnlocked
}

public class cardPrefabScript : MonoBehaviour
{
    public Effect effect;
    public CardState state = CardState.InDrawPile;
    //used for the "on hover" effect
    public Vector3 scaleOnHighlight = new Vector3(1f, 1f, 1f);
    public Vector3 scaleOnIdle = new Vector3(0.7f, 0.7f, 0.7f);
    public bool isHighlighted = false; 

    public int energy;
    public string description;
    public string spriteName; //name of the sprite used to render the artwork
    public CardFlavour flavour;


    // these components are used so that we can change the artwork and metadata without regenrating the whole PNG
    private SpriteRenderer art;
    private TextMeshPro descriptionText;
    private TextMeshPro energyText;
    private TextMeshPro titleText;
    //rendering a different body depending on the flavour (attack, skill, power)
    public SpriteRenderer cardBody;

    //used to manipulate perception (cards that are on top will have higher order index)
    public static int layerOrderIndex = 0;

    public static int GetLatestOrderingIndex()
    {
        layerOrderIndex++;
        return layerOrderIndex;
    }

    public void PutOnTop()
    {
        //updating the layer order index for each component so that it's the highest, therefore on top
        art.sortingOrder = GetLatestOrderingIndex(); //must be 1st
        cardBody.sortingOrder = GetLatestOrderingIndex(); //must be 2nd

        //the order for these 3 rows doesn't matter
        titleText.sortingOrder = GetLatestOrderingIndex(); 
        descriptionText.sortingOrder = GetLatestOrderingIndex();
        energyText.sortingOrder = GetLatestOrderingIndex();
    }

    public void Init(string title, CardFlavour flavour, string spriteName, string description, int energy, Effect effect) 
    {
        //using a single component to load the sprites so that we dont have to load them for each card
        SpriteManagerScript SPRITES = GameObject.FindGameObjectWithTag("SpriteManager").GetComponent<SpriteManagerScript>();

        //child components
        titleText = transform.Find("title").GetComponent<TextMeshPro>();
        cardBody = transform.Find("cardBody").GetComponent<SpriteRenderer>();
        art = transform.Find("art").GetComponent<SpriteRenderer>();
        descriptionText = transform.Find("description").GetComponent<TextMeshPro>();
        energyText = transform.Find("energy").GetComponent<TextMeshPro>();

        //render according to given parameters
        titleText.SetText(title);
        art.sprite = SPRITES.GetSpriteByName(spriteName);
        cardBody.sprite = SPRITES.GetFlavourSprite(flavour);
        descriptionText.SetText(description);
        energyText.SetText(energy.ToString());

        //temporary placement until deck functionality is implemented
        GameObject cardContainer = GameObject.FindGameObjectWithTag("Cards");
        transform.position = cardContainer.transform.position;
        transform.parent = GameObject.FindGameObjectWithTag("Cards").transform;
        PutOnTop();

        //
        this.energy = energy;
        this.description = description;
        this.flavour = flavour;
        this.effect = effect;
    }

    public void removeHighlight()
    {
        if (isHighlighted)
        {
            isHighlighted = false;
            GameObject.FindGameObjectWithTag("CardManager").GetComponent<CardManagerScript>().PlaceCards();
        }
    }

    public void Highlight()
    {
        if (isHighlighted == false)
        {
            isHighlighted = true;
            PutOnTop();
            transform.localScale = scaleOnHighlight;
            transform.position += new Vector3(0, 2.5f);
            transform.rotation = Quaternion.identity;
        }
    }

    void Start()
    {
    }
    void Update()
    {
    }
}

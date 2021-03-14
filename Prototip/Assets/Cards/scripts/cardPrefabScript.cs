using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum CardFlavour // your custom enumeration
{
    Attack,
    Skill,
    Power
};

public class cardPrefabScript : MonoBehaviour
{

    public int energy;
    public string description;
    public string spriteName;
    public CardFlavour flavour;

    private TextMeshPro descriptionText;
    private TextMeshPro energyText;
    private TextMeshPro titleText;
    private SpriteRenderer cardBody;
    private SpriteRenderer art;

    public static int layerOrderIndex = 0;

    public static int GetLatestOrderingIndex()
    {
        layerOrderIndex++;
        return layerOrderIndex;
    }
    public void PutOnTop()
    {
        art.sortingOrder = GetLatestOrderingIndex();
        cardBody.sortingOrder = GetLatestOrderingIndex();
        titleText.sortingOrder = GetLatestOrderingIndex();
        descriptionText.sortingOrder = GetLatestOrderingIndex();
        energyText.sortingOrder = GetLatestOrderingIndex();
    }

    public void Init(string title, CardFlavour flavour, string spriteName, string description, int energy) 
    {
        SpriteManagerScript SPRITES = GameObject.FindGameObjectWithTag("SpriteManager").GetComponent<SpriteManagerScript>();

        titleText = transform.Find("title").GetComponent<TextMeshPro>();
        cardBody = transform.Find("cardBody").GetComponent<SpriteRenderer>();
        art = transform.Find("art").GetComponent<SpriteRenderer>();
        descriptionText = transform.Find("description").GetComponent<TextMeshPro>();
        energyText = transform.Find("energy").GetComponent<TextMeshPro>();

        titleText.SetText(title);
        art.sprite = SPRITES.GetSpriteByName(spriteName);
        cardBody.sprite = SPRITES.GetFlavourSprite(flavour);

        descriptionText.SetText(description);
        energyText.SetText(energy.ToString());

        this.energy = energy;
        this.description = description;
        this.flavour = flavour;

        GameObject cardContainer = GameObject.FindGameObjectWithTag("Cards");
        this.transform.position = cardContainer.transform.position;
        this.transform.parent = GameObject.FindGameObjectWithTag("Cards").transform;

        this.PutOnTop();
    }

    void Start()
    {
    }
    void Update()
    {

    }
}

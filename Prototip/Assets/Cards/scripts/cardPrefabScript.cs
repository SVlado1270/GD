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

    public int energy = 69;
    public string description = "pls work";
    public string spriteName = "bonk2";
    public string title = "Untitled";
    public CardFlavour flavour;

    private TextMeshPro descriptionText;
    private TextMeshPro energyText;


    void Start()
    {
        Transform cardRenderer = transform.Find("cardRenderer");
        SpriteManagerScript SPRITES = GameObject.FindGameObjectWithTag("SpriteManager").GetComponent<SpriteManagerScript>();

        TextMeshPro titleText = cardRenderer.Find("title").GetComponent<TextMeshPro>();
        SpriteRenderer cardBody = cardRenderer.GetComponent<SpriteRenderer>();
        SpriteRenderer art = cardRenderer.Find("art").GetComponent<SpriteRenderer>();
        descriptionText = cardRenderer.Find("description").GetComponent<TextMeshPro>();
        energyText = cardRenderer.Find("energy").GetComponent<TextMeshPro>();

        titleText.SetText(title);
        cardBody.sprite = SPRITES.GetFlavourSprite(flavour);
        art.sprite = SPRITES.GetSpriteByName(spriteName);
        descriptionText.SetText(description);
        energyText.SetText(this.energy.ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }
}

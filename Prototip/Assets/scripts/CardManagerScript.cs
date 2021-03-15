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
        //params:       title,          flavour,            sprite,     description,    energy
        InstantiateCard("bonker",       CardFlavour.Attack, "bonk",     "it bonks1",    1);
        InstantiateCard("bonk",         CardFlavour.Power,  "bonk",     "it bonks2",    2);
        InstantiateCard("bonk2",        CardFlavour.Skill,  "bonk2",    "it bonks3",    3);
        InstantiateCard("OBNK", CardFlavour.Power, "bonk2", "it bonks5", 4);
        PlaceCards();
    }

    void InstantiateCard(string title, CardFlavour flavour, string spriteName, string description, int energy)
    {
        GameObject card = Instantiate(cardPrefab, this.transform.position, Quaternion.identity);
        cardPrefabScript cardController = card.GetComponent<cardPrefabScript>();
        cardController.Init(title, flavour, spriteName, description, energy);
        allCards.Add(card);
    }

    public void PlaceCards()
    {
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
        float maxWidth = 800f;
        float overlap = 10f;
        //spacing = cardWidth - some overlapping
        float spacing = allCards[0].GetComponent<cardPrefabScript>().cardBody.sprite.bounds.size.x - overlap;
        if (spacing * (allCards.Count - 1)> maxWidth)
        {
            spacing = maxWidth / (allCards.Count - 1);
        }
        Vector2 center = transform.position;
        float rightCoord = center.x + spacing * (allCards.Count - 1) / 2;
        for (int i = 0; i < allCards.Count; i++)
        {
            GameObject card = allCards[i];
            card.transform.rotation = Quaternion.Euler(0, 0, maxRotation - i * perCardRotation);
            card.transform.localPosition = new Vector2(rightCoord - i * spacing, center.y);
            card.GetComponent<cardPrefabScript>().PutOnTop();
        }
    }
    void Update()
    {
        
    }
}

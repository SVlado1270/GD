using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManagerScript : MonoBehaviour
{
    public GameObject cardPrefab;
    // Start is called before the first frame update
    void Start()
    {
        InstantiateCard("bonker", CardFlavour.Attack, "bonk", "it bonks1", 1);
        InstantiateCard("bonk", CardFlavour.Skill, "bonk", "it bonks2", 1);
        InstantiateCard("bonk2", CardFlavour.Skill, "bonk2", "it bonks3", 3);
        InstantiateCard("bonkas", CardFlavour.Attack, "bonk", "it bonks4", 1);
        InstantiateCard("alpha bonk", CardFlavour.Power, "bonk2", "it bonks5", 2);
    }

    void InstantiateCard(string title, CardFlavour flavour, string spriteName, string description, int energy)
    {
        GameObject card = Instantiate(cardPrefab, this.transform.position, Quaternion.identity);
        cardPrefabScript cardController = card.GetComponent<cardPrefabScript>();
        cardController.Init(title, flavour, spriteName, description, energy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

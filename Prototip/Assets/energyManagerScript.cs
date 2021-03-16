using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class energyManagerScript : MonoBehaviour
{

    public TextMeshProUGUI energyCountText;
    int maxEnergy = 3;
    int energy = 3;

    public void ResetEnergy()
    {
        energy = maxEnergy;
        energyCountText.SetText(energy.ToString());
    }

    public bool canAfford(cardPrefabScript card)
    {
        return card.energy <= energy;
    }
    public void consumeCard(cardPrefabScript card)
    {
        if (canAfford(card))
        {
            energy -= card.energy;
        }
        energyCountText.SetText(energy.ToString());
    }
    // Start is called before the first frame update
    void Start()
    {
        energyCountText.SetText(energy.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

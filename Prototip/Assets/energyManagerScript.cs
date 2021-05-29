using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class energyManagerScript : MonoBehaviour
{

    public TextMeshProUGUI energyCountText;
    int maxEnergy = 3;
    int energy = 3;
    public int energyNextTurn = 0;

    public void ResetEnergy()
    {
        energy = maxEnergy;
        if(energyNextTurn > 0)
        {
            energy += energyNextTurn;
            energyNextTurn = 0;
        }
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
        maxEnergy = 3;
        energy = 3;
        energyNextTurn = 0;
        energyCountText.SetText(energy.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

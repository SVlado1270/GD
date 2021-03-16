using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTurnScript : MonoBehaviour
{
    public void OnButtonPress()
    {
        GameObject.FindGameObjectWithTag("CardManager").GetComponent<CardManagerScript>().newHand();
    }
}

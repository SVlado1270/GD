using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class receiveCardEffectsScript : MonoBehaviour
{
    public bool onFocus = false;

    void OnMouseEnter()
    {
        onFocus = true;
    }

    void OnMouseExit()
    {
        onFocus = false;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

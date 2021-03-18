using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class receiveCardEffectsScript : MonoBehaviour
{
    public bool highlight = false;
    GameObject highlightRenderer;

    void Start()
    {
        highlightRenderer = transform.Find("highlight").gameObject;
    }


    public void SetHighlight(bool highlight)
    {
        if (this.highlight != highlight)
        {
            this.highlight = highlight;
            highlightRenderer.SetActive(highlight);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SelectCardScript : MonoBehaviour
{
    private bool isDragging;

    public void OnMouseDown()
    {
        isDragging = true;
        this.GetComponent<cardPrefabScript>().PutOnTop();
    }

    public void OnMouseUp()
    {
        isDragging = false;
        castCardIfNeeded();
    }

    void castCardIfNeeded()
    {
        var potentialReceivers = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject potentialReceiver in potentialReceivers)
        {
            if ((potentialReceiver.transform.position - transform.position).magnitude < 1f)
            {
                // TODO: validate receiver (dont allow casting a shield on the enemy or self inflicted damage)
                healthBarScript receiver = potentialReceiver.transform.parent.GetComponent<healthBarScript>();
                var controller = GetComponent<cardPrefabScript>();
                receiver.consumeEffect(controller.effect);
                controller.state = CardState.InDiscardPile;
                // TODO: move to discard pile
                break;
            }
        }
    }

    void Update()
    {
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePosition);

            //Debug.Log(mousePosition);

           // this..SetActive(false); // false to hide, true to show
        }
    }
}

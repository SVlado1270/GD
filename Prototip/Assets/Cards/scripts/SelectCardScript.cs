using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCardScript : MonoBehaviour
{
    GameObject gameObject;
    private bool isDragging;

    private void Start()
    {
        Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
    }

    public void OnMouseDown()
    {
        isDragging = true;
        Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
    }

    public void OnMouseUp()
    {
        isDragging = false;

    }

    void Update()
    {
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePosition);

            Debug.Log(mousePosition);

           // this..SetActive(false); // false to hide, true to show
        }
    }
}

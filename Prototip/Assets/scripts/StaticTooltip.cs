using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class StaticTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string message;

    private void Start()
    {
    }

    private void Update()
    {
    }

    public void OnPointerEnter(PointerEventData _)
    {
        TooltipScript.ShowTooltip(message);
    }

    public void OnPointerExit(PointerEventData _)
    {
        TooltipScript.HideTooltip();
    }
}

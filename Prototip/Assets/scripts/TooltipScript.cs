using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipScript : MonoBehaviour
{
    private static Text tooltipText;
    private static RectTransform backgroundRectTransform;
    private static Transform tooltip;
    private static float timeLeft;
    private static bool isTimed;

    void Awake()
    {
        tooltip = transform;
        backgroundRectTransform = tooltip.Find("background").GetComponent<RectTransform>();
        tooltipText = tooltip.Find("text").GetComponent<Text>();
        HideTooltip();
    }

    public static void ShowTooltip(string tooltipString)
    {
        isTimed = false;
        tooltip.gameObject.SetActive(true);

        tooltipText.text = tooltipString;

        float padding = 4f;
        float width = tooltipText.preferredWidth + 2 * padding;
        float height = tooltipText.preferredHeight + 2 * padding;
        Vector2 backgroundSize = new Vector2(width, height);

        backgroundRectTransform.sizeDelta = backgroundSize;
    }

    public static void ShowTooltip(string tooltipString, float time)
    {
        timeLeft = time;
        isTimed = true;
        ShowTooltip(tooltipString);
    }

    public static void HideTooltip()
    {
        tooltip.gameObject.SetActive(false);
    }


    private void Update()
    {
        if (isTimed)
        {
            if (timeLeft < 0f)
            {
                HideTooltip();
            }
            else
            {
                timeLeft -= Time.deltaTime;
            }
        }
        Vector3 padding = new Vector3(10f, 10f);
        tooltip.position = Input.mousePosition + padding;
    }
}

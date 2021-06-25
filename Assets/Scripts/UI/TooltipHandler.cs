using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject tooltip;

    private void Start()
    {
        tooltip.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
      
        ITooltip stuffToDescribe = (ITooltip)GetComponentInChildren(typeof(ITooltip));
        if (stuffToDescribe != null && stuffToDescribe.GetTooltip() != null)
        {
            tooltip.GetComponentInChildren<TextMeshProUGUI>().text = stuffToDescribe.GetTooltip();

            tooltip.SetActive(true);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);
    }
}

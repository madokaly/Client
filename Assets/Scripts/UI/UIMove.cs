using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMove : MonoBehaviour,IDragHandler
{
    public RectTransform rt;

    public void OnDrag(PointerEventData eventData)
    {
        rt.anchoredPosition += eventData.delta;
    }
}

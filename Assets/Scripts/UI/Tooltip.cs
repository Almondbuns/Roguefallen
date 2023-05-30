using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string text;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject.Find("MousePointer").GetComponent<MousePointer>().AddInfoPanel(text);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameObject.Find("MousePointer").GetComponent<MousePointer>().RemoveInfoPanel(text);
    }
}

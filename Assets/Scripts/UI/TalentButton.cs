using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TalentButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TalentData talent_data;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (talent_data != null)
            GameObject.Find("MousePointer").GetComponent<MousePointer>().AddInfoPanel(talent_data);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (talent_data != null)
            GameObject.Find("MousePointer").GetComponent<MousePointer>().RemoveInfoPanel(talent_data);
    }
}

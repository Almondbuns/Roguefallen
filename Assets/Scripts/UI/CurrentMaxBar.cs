using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentMaxBar : MonoBehaviour
{
    public void SetValues(int current, int max)
    {
        transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = current.ToString() + "/" + max.ToString();
        float ratio = Mathf.Max(0,Mathf.Min(1,current / (float) max));
        float width = GetComponent<RectTransform>().sizeDelta.x;
        float border = transform.Find("CurrentBar").GetComponent<RectTransform>().localPosition.x;
        transform.Find("CurrentBar").GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Max(0,width * ratio - 2 * border), transform.Find("CurrentBar").GetComponent<RectTransform>().sizeDelta.y);
    }
}

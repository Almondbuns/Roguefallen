using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorHealthBar : MonoBehaviour
{
    public void SetValues(int current, int max)
    {
        float ratio = Mathf.Max(0,Mathf.Min(1,current / (float) max));
        float height = GetComponent<RectTransform>().sizeDelta.y;
        transform.Find("CurrentBar").GetComponent<RectTransform>().sizeDelta = new Vector2(transform.Find("CurrentBar").GetComponent<RectTransform>().sizeDelta.x, Mathf.Max(0,height * ratio));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TextInfo : MonoBehaviour
{
    public string text;
  
    RectTransform rect;
    public Vector2 start_position;
    
    public void Create()
    {
        if (text == null)
        {
            Debug.Log("Error: Start TextInfo without TextData");
            return;
        }

        transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = text;

        float width = transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().GetPreferredValues(text).x;
        float height = transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().GetPreferredValues(text).y;

        transform.Find("Text").GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        GetComponent<RectTransform>().sizeDelta = new Vector2(width + 20,height + 10);

        start_position.x = Mouse.current.position.x.ReadValue();
        start_position.y = Mouse.current.position.y.ReadValue();
    }

    public void CheckScreenPosition()
    {
        rect = GetComponent<RectTransform>();

        if (rect.position.x < 0)
        {
            rect.position = new Vector3(0,rect.position.y,rect.position.z);
            return;
        }

        if (rect.position.x + rect.sizeDelta.x > Screen.width)
        {
            rect.position = new Vector3(Screen.width - rect.sizeDelta.x, rect.position.y, rect.position.z);
            return;
        }

       if (rect.position.y > Screen.height)
        {
            rect.position = new Vector3(rect.position.x, Screen.height, rect.position.z);
            return;
        }

        if (rect.position.y - rect.sizeDelta.y < 0)
        {
            rect.position = new Vector3(rect.position.x, rect.sizeDelta.y + 10 , rect.position.z);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(Mouse.current.position.ReadValue().x - start_position.x) > 150
        || Mathf.Abs(Mouse.current.position.ReadValue().y - start_position.y) > 150)
        {
            GameObject.Find("MousePointer").GetComponent<MousePointer>().RemoveInfoPanel(text);
        }
    }

    void FixedUpdate()
    {
        CheckScreenPosition();
    }
}

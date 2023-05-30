using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropPanel : MonoBehaviour
{
    public ItemActionPanel item_action_panel;
    public void Create(ItemActionPanel item_action_panel)
    {
        this.item_action_panel = item_action_panel;
        transform.Find("OkButton").GetComponent<Button>().onClick.AddListener(Select);
        transform.Find("Amount").GetComponent<TMPro.TMP_InputField>().interactable = true;
        transform.Find("Amount").GetComponent<TMPro.TMP_InputField>().text = item_action_panel.item_data.amount.ToString();
    }

    public void Select()
    {
        int amount = 1;
        bool is_number = int.TryParse((string) transform.Find("Amount").GetComponent<TMPro.TMP_InputField>().text, out amount);
        
        if (is_number == false)
            item_action_panel.Drop(1);
        else
            item_action_panel.Drop(amount);
    }
}

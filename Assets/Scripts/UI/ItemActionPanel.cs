using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemActionPanel : MonoBehaviour
{
    public ItemData item_data;
    public GameObject drop_panel_prefab;
    public UIState ui_state;

    // Start is called before the first frame update
    void Start()
    {
        if (item_data == null)
        {
            Debug.LogError("No item set in ItemActionPanel");
            return;
        }

        if (item_data.GetPrototype().effects_when_consumed.Count > 0)
            transform.Find("Consume").GetComponent<Button>().onClick.AddListener(Consume);
        else
            transform.Find("Consume").gameObject.SetActive(false);

        transform.Find("Drop").GetComponent<Button>().onClick.AddListener(Drop);
    }

    public void Consume()
    {
        PlayerData player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;
        ui_state.DestroyState();
        player_data.PrepareConsume(item_data);
    }

    public void Drop()
    { 
        if (item_data.amount <= 1)
        {
            Drop(item_data.amount);
        }
        else
        {
            GameObject drop_panel = GameObject.Instantiate(drop_panel_prefab, gameObject.transform, false);
            drop_panel.GetComponent<DropPanel>().Create(this);   
        }

    }

    public void Drop(int amount)
    {
        PlayerData player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;
      
        ui_state.DestroyState();
        player_data.PrepareDrop(item_data, amount);
    }
}

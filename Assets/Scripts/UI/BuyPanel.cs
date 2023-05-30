using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyPanel : MonoBehaviour
{
    // Start is called before the first frame update
    public ItemData item_data;

    void Start()
    {
        if (item_data == null)
            return;

        transform.Find("ItemInfo").GetComponent<ItemInfo>().item_data = item_data;
        transform.Find("ItemInfo").GetComponent<ItemInfo>().Create();

        transform.Find("PlayerGold").Find("GoldText").GetComponent<TMPro.TextMeshProUGUI>().text = GameObject.Find("GameData").GetComponent<GameData>().player_data.gold_amount.ToString();
        transform.Find("ItemGold").Find("GoldText").GetComponent<TMPro.TextMeshProUGUI>().text = item_data.GetGoldValue().ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, transform.Find("ItemInfo").GetComponent<RectTransform>().sizeDelta.y + 130);
    }
}

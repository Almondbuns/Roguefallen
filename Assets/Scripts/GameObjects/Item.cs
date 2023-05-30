using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemData item_data;
   
    // Start is called before the first frame update
    void Start()
    {
        Texture2D texture = Resources.Load<Texture2D>(item_data.GetPrototype().icon);
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f,0.5f));

        name = item_data.GetName();
        transform.position = new Vector3(item_data.x + 0.5f, item_data.y + 0.5f, 0);
        if (item_data.amount > 1)
            transform.Find("Canvas").Find("Amount").GetComponent<TMPro.TextMeshProUGUI>().text = item_data.amount.ToString();
        else
            transform.Find("Canvas").Find("Amount").gameObject.SetActive(false);

        item_data.HandleRemove += OnRemove;
    }

    public void OnRemove()
    {
        Destroy(gameObject);
        GameObject.Find("Map").GetComponent<Map>().items.Remove(this);
    }

    public void UpdateVisibility(MapData map)
    {
        if (this == null)
            return;
            
        if (map.tiles[item_data.x, item_data.y].visibility == Visibility.None)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(item_data.x + 0.5f, item_data.y + 0.5f, 0), 10f * Time.deltaTime);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject.Find("MousePointer").GetComponent<MousePointer>().AddInfoPanel(item_data);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameObject.Find("MousePointer").GetComponent<MousePointer>().RemoveInfoPanel(item_data);
    }

    public void OnDestroy()
    {
        item_data.HandleRemove -= OnRemove;
    }
}

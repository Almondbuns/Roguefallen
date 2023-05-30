using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum InventorySlotType
{
    INVENTORY,
    EQUIPMENT
}

public class InventorySlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public InventorySlotType type;
    public InventorySlotData inventory_data;
    public EquipmentSlotData equipment_data;

    private Transform drag_icon;

    public void SetData(EquipmentSlotData slot_data)
    {
        type = InventorySlotType.EQUIPMENT;
        equipment_data = slot_data;
    }

    public void SetData(InventorySlotData slot_data)
    {
        type = InventorySlotType.INVENTORY;
        inventory_data = slot_data;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        drag_icon = transform.Find("Icon");
        drag_icon.SetParent(GameObject.Find("MousePointer").transform,true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        drag_icon.SetParent(transform, false);
        drag_icon.localPosition = new Vector3(50, -50, 0);
        GetComponentInParent<InventoryPanel>().DoActivationAndDragAndDrop(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ItemData item_data;
        if (type == InventorySlotType.INVENTORY)
            item_data = inventory_data.item;
        else
            item_data = equipment_data.item;

        if (item_data != null)
            GameObject.Find("MousePointer").GetComponent<MousePointer>().AddInfoPanel(item_data);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemData item_data;
        if (type == InventorySlotType.INVENTORY)
            item_data = inventory_data.item;
        else
            item_data = equipment_data.item;

        if (item_data != null)
            GameObject.Find("MousePointer").GetComponent<MousePointer>().RemoveInfoPanel(item_data);
    }
}

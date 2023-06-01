using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryPanelSide
{
    public List<GameObject> slots;
    public GameObject inventory_panel;

    public InventoryPanelSide(GameObject parent)
    {
        inventory_panel = parent;
        slots = new();
    }

    public virtual void Refresh(GameObject inventory_slot_prefab)
    {

    }
}

public class PanelSidePlayerInventory : InventoryPanelSide
{
    public PlayerData player_data;

    public PanelSidePlayerInventory(GameObject parent) : base(parent)
    {
        player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;
    }

    public override void Refresh(GameObject inventory_slot_prefab)
    {
        
        foreach (GameObject slot in slots)
            GameObject.Destroy(slot);

        slots.Clear();

        int counter = 0;

        foreach (InventorySlotData slot in player_data.inventory.slots)
        {
            GameObject ui_slot = GameObject.Instantiate(inventory_slot_prefab, inventory_panel.transform, false);
            ui_slot.name = "InventorySlot " + counter;
            ui_slot.GetComponent<RectTransform>().localPosition = new Vector3(50 + 150 * (counter % 5), 320 - 180 * (counter / 5));
            ui_slot.GetComponent<InventorySlot>().SetData(slot);

            slots.Add(ui_slot);
            ++counter;
        }

        foreach (GameObject ui_slot in slots)
        {
            if (ui_slot.GetComponent<InventorySlot>().inventory_data.item != null)
            {
                ItemData item = ui_slot.GetComponent<InventorySlot>().inventory_data.item;

                string quality_string = "";
                switch (item.quality)
                {
                    case ItemQuality.Normal:
                        quality_string = "<color=#ffffff>";
                        break;
                    case ItemQuality.Magical1:
                        quality_string = "<color=#0000aa>";
                        break;
                    case ItemQuality.Magical2:
                        quality_string = "<color=#4444ff>";
                        break;
                    case ItemQuality.Unique:
                        quality_string = "<color=#00aa00>";
                        break;
                    case ItemQuality.Artefact:
                        quality_string = "<color=#aa2222>";
                        break;
                }

                ui_slot.transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = quality_string + item.GetName() + "</color>";
                if (item.GetPrototype().tier > 0)
                    ui_slot.transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text += " +" + item.GetPrototype().tier;
                Texture2D texture = Resources.Load<Texture2D>(item.GetPrototype().icon);
                ui_slot.transform.Find("Icon").GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                ui_slot.transform.Find("Icon").GetComponent<Image>().color = new Color(1, 1, 1, 1);
                if (item.amount > 1)
                    ui_slot.transform.Find("Amount").GetComponent<TMPro.TextMeshProUGUI>().text = item.amount.ToString();
                else
                    ui_slot.transform.Find("Amount").gameObject.SetActive(false);
            }
            else
            {
                ui_slot.transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = "";
                ui_slot.transform.Find("Icon").GetComponent<Image>().sprite = null;
                ui_slot.transform.Find("Icon").GetComponent<Image>().color = new Color(1, 1, 1, 0);
                ui_slot.transform.Find("Amount").gameObject.SetActive(false);
            }
        }

        inventory_panel.transform.Find("Gold").Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.gold_amount.ToString();
        inventory_panel.transform.Find("Weight").Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = player_data.GetCurrentWeight() + "/" + player_data.GetMaxWeight();

    }
}

public class PanelSideChestInventory : InventoryPanelSide
{
    public ActorData chest;

    public PanelSideChestInventory(GameObject parent, ActorData chest) : base(parent)
    {
        this.chest = chest;
    }

    public override void Refresh(GameObject inventory_slot_prefab)
    {

        foreach (GameObject slot in slots)
            GameObject.Destroy(slot);

        slots.Clear();

        int counter = 0;

        foreach (InventorySlotData slot in chest.inventory.slots)
        {
            GameObject ui_slot = GameObject.Instantiate(inventory_slot_prefab, inventory_panel.transform, false);
            ui_slot.name = "ChestInventorySlot " + counter;
            ui_slot.GetComponent<RectTransform>().localPosition = new Vector3(-700 + 150 * (counter % 4), 140 - 180 * (counter / 4));
            ui_slot.GetComponent<InventorySlot>().SetData(slot);

            slots.Add(ui_slot);
            ++counter;
        }

        foreach (GameObject ui_slot in slots)
        {
            if (ui_slot.GetComponent<InventorySlot>().inventory_data.item != null)
            {
                ItemData item = ui_slot.GetComponent<InventorySlot>().inventory_data.item;
                string quality_string = "";
                switch (item.quality)
                {
                    case ItemQuality.Normal:
                        quality_string = "<color=#ffffff>";
                        break;
                    case ItemQuality.Magical1:
                        quality_string = "<color=#0000aa>";
                        break;
                    case ItemQuality.Magical2:
                        quality_string = "<color=#4444ff>";
                        break;
                    case ItemQuality.Unique:
                        quality_string = "<color=#00aa00>";
                        break;
                    case ItemQuality.Artefact:
                        quality_string = "<color=#aa2222>";
                        break;
                }

                ui_slot.transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = quality_string + item.GetName() + "</color>";
                if (item.GetPrototype().tier > 0)
                    ui_slot.transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text += " +" + item.GetPrototype().tier;
                Texture2D texture = Resources.Load<Texture2D>(item.GetPrototype().icon);
                ui_slot.transform.Find("Icon").GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                ui_slot.transform.Find("Icon").GetComponent<Image>().color = new Color(1, 1, 1, 1);
                if (item.amount > 1)
                    ui_slot.transform.Find("Amount").GetComponent<TMPro.TextMeshProUGUI>().text = item.amount.ToString();
                else
                    ui_slot.transform.Find("Amount").gameObject.SetActive(false);
            }
            else
            {
                ui_slot.transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = "";
                ui_slot.transform.Find("Icon").GetComponent<Image>().sprite = null;
                ui_slot.transform.Find("Icon").GetComponent<Image>().color = new Color(1, 1, 1, 0);
                ui_slot.transform.Find("Amount").gameObject.SetActive(false);
            }
        }

        Texture2D chest_texture = Resources.Load<Texture2D>(chest.prototype.icon);
        inventory_panel.transform.Find("Image").GetComponent<Image>().sprite= Sprite.Create(chest_texture, new Rect(0, 0, chest_texture.width, chest_texture.height), new Vector2(0.5f, 0.5f)); ;

    }
}

public class PanelSidePlayerEquipment : InventoryPanelSide
{
    public PlayerData player_data;

    public PanelSidePlayerEquipment(GameObject parent) : base(parent)
    {
        player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;
    }

    public override void Refresh(GameObject inventory_slot_prefab)
    {        
        foreach (GameObject slot in slots)
            GameObject.Destroy(slot);

        slots.Clear();

        foreach (EquipmentSlotData slot in player_data.equipment)
        {
            GameObject ui_slot = GameObject.Instantiate(inventory_slot_prefab, inventory_panel.transform, false);
            ui_slot.name = "EquipmentSlot" + slot.name;
            ui_slot.GetComponent<InventorySlot>().SetData(slot);
            int x = 0;
            int y = 0;
            if (slot.name == "Head")
            {
                x = -450;
                y = 180;
            }
            else if (slot.name == "Shield")
            {
                x = 45 - 800;
                y = 430 - 450;
            }
            else if (slot.name == "Weapon")
            {
                x = 570 - 800;
                y = 180;
            }
            else if (slot.name == "Weapon 2L")
            {
                x = 500 - 800;
                y = 830 - 450;
            }
            else if (slot.name == "Weapon 2R")
            {
                x = 650 - 800;
                y = 830 - 450;
            }
            else if (slot.name == "Chest")
            {
                x = 350 - 800;
                y = 430 - 450;
            }
            else if (slot.name == "Feet")
            {
                x = 350 - 800;
                y = 115 - 450;
            }
            else if (slot.name == "Hands")
            {
                x = 570 - 800;
                y = 430 - 450;
            }
            else if (slot.name == "Neck")
            {
                x = 45 - 800;
                y = 630 - 450;
            }
            else if (slot.name == "Finger 1")
            {
                x = 45 - 800;
                y = 260 - 450;
            }
            else if (slot.name == "Finger 2")
            {
                x = 570 - 800;
                y = 260 - 450;
            }

            ui_slot.GetComponent<RectTransform>().localPosition = new Vector3(x, y);
            slots.Add(ui_slot);
        }

        foreach (GameObject ui_slot in slots)
        {
            if (ui_slot.GetComponent<InventorySlot>().equipment_data.item != null)
            {
                ItemData item = ui_slot.GetComponent<InventorySlot>().equipment_data.item;
                string quality_string = "";
                switch (item.quality)
                {
                    case ItemQuality.Normal:
                        quality_string = "<color=#ffffff>";
                        break;
                    case ItemQuality.Magical1:
                        quality_string = "<color=#0000aa>";
                        break;
                    case ItemQuality.Magical2:
                        quality_string = "<color=#4444ff>";
                        break;
                    case ItemQuality.Unique:
                        quality_string = "<color=#00aa00>";
                        break;
                    case ItemQuality.Artefact:
                        quality_string = "<color=#aa2222>";
                        break;
                }

                ui_slot.transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = quality_string + item.GetName() + "</color>";
                if (item.GetPrototype().tier > 0)
                    ui_slot.transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text += " +" + item.GetPrototype().tier;
                Texture2D texture = Resources.Load<Texture2D>(ui_slot.GetComponent<InventorySlot>().equipment_data.item.GetPrototype().icon);
                ui_slot.transform.Find("Icon").GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                ui_slot.transform.Find("Icon").GetComponent<Image>().color = new Color(1, 1, 1, 1);
                if (item.amount > 1)
                    ui_slot.transform.Find("Amount").GetComponent<TMPro.TextMeshProUGUI>().text = item.amount.ToString();
                else
                    ui_slot.transform.Find("Amount").gameObject.SetActive(false);
            }
            else
            {
                ui_slot.transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = ui_slot.GetComponent<InventorySlot>().equipment_data.name;
                ui_slot.transform.Find("Icon").GetComponent<Image>().sprite = null;
                ui_slot.transform.Find("Icon").GetComponent<Image>().color = new Color(1, 1, 1, 0);
                ui_slot.transform.Find("Amount").gameObject.SetActive(false);
            }
        }
    }
}

public class InventoryPanel : MonoBehaviour
{
    public GameObject inventory_slot_prefab;
    public GameObject item_action_prefab;

    public InventoryPanelSide left_side;
    public InventoryPanelSide right_side;
   
    PlayerData player_data;

    public UIState ui_state;

    void Start()
    {
        gameObject.name = "Inventory Panel";

        player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;

        Refresh();
    }

    public void Refresh()
    {
        left_side.Refresh(inventory_slot_prefab);
        right_side.Refresh(inventory_slot_prefab);
    }

    internal void DoActivationAndDragAndDrop(InventorySlot inventory_slot_src)
    {
        //Mouse Pointer is at position of target inventory slot (or not)
        for (int i = 0; i < transform.childCount; ++i)
        {
            GameObject go = transform.GetChild(i).gameObject;
            if (go.GetComponent<InventorySlot>() == null) // No suitable target 
                continue;

            if (Mouse.current.position.x.ReadValue() < go.GetComponent<RectTransform>().position.x
                || Mouse.current.position.x.ReadValue() > go.GetComponent<RectTransform>().position.x + 100)
                continue;

            if (Mouse.current.position.y.ReadValue() < go.GetComponent<RectTransform>().position.y - 100
                || Mouse.current.position.y.ReadValue() > go.GetComponent<RectTransform>().position.y)
                continue;

            
            //Find Source
            ItemData item_src = null;
            if (inventory_slot_src.type == InventorySlotType.INVENTORY)
            {
                item_src = inventory_slot_src.inventory_data.item;
            }
            else
            {
                item_src = inventory_slot_src.equipment_data.item;
            }

            ItemData item_target;
            //Find Target
            if (go.GetComponent<InventorySlot>().type == InventorySlotType.INVENTORY)
            {
                item_target = go.GetComponent<InventorySlot>().inventory_data.item;
            }
            else
            {
                item_target = go.GetComponent<InventorySlot>().equipment_data.item;
            }

            if (item_src == null) return;

            if (go.GetComponent<InventorySlot>() == inventory_slot_src)
            {
                // Same item - interpret as mouse click
                ClickOnItem(go.GetComponent<InventorySlot>());
                return;
            }

            //If both are inventory slots 
            if (go.GetComponent<InventorySlot>().type == InventorySlotType.INVENTORY
                && inventory_slot_src.type == InventorySlotType.INVENTORY)
            {
                //Stack Items if possible
                if (item_src != null && item_target != null && item_src.GetType() == item_target.GetType() && item_src.GetPrototype().is_stackable == true)
                {
                    if (item_src.GetTier() == item_target.GetTier() && item_src.amount + item_target.amount <= item_src.GetPrototype().stack_max)
                    {
                        item_target.amount += item_src.amount;
                        inventory_slot_src.inventory_data.item = null;
                        GameObject.Find("UI").GetComponent<UI>().Refresh();
                        return;
                    }
                }

                //Else Swap placement
                go.GetComponent<InventorySlot>().inventory_data.item = item_src;
                inventory_slot_src.inventory_data.item = item_target;
                GameObject.Find("UI").GetComponent<UI>().Refresh();
                return;
            }

            //If both are equipment slots of the same type just swap placement
            if (go.GetComponent<InventorySlot>().type == InventorySlotType.EQUIPMENT
                && inventory_slot_src.type == InventorySlotType.EQUIPMENT)
            {
                //Only Equip if correct equip type
                if (item_src != null && go.GetComponent<InventorySlot>().equipment_data.item_type.Contains(item_src.GetPrototype().type) == false)
                    return;
                if (item_target != null && inventory_slot_src.equipment_data.item_type.Contains(item_target.GetPrototype().type) == false)
                    return;
               
                go.GetComponent<InventorySlot>().equipment_data.item = item_src;
                inventory_slot_src.equipment_data.item = item_target;
                GameObject.Find("UI").GetComponent<UI>().Refresh();
                return;
            }
            
            if (item_target != null) return;

            //Only Equip if correct equip type
            if (go.GetComponent<InventorySlot>().type == InventorySlotType.EQUIPMENT)
            {
                if (go.GetComponent<InventorySlot>().equipment_data.item_type.Contains(item_src.GetPrototype().type) == false)
                    return;
            }

            

            // Equipment change has to be done in data structures
            if (go.GetComponent<InventorySlot>().type == InventorySlotType.EQUIPMENT
                && inventory_slot_src.type == InventorySlotType.INVENTORY)
            {
                GameObject.Find("GameData").GetComponent<GameData>().player_data.PrepareEquip(inventory_slot_src.inventory_data, go.GetComponent<InventorySlot>().equipment_data); ;
                //OnDestroy();
            }

            if (go.GetComponent<InventorySlot>().type == InventorySlotType.INVENTORY
                && inventory_slot_src.type == InventorySlotType.EQUIPMENT)
            {
                GameObject.Find("GameData").GetComponent<GameData>().player_data.PrepareUnequip(inventory_slot_src.equipment_data, go.GetComponent<InventorySlot>().inventory_data); ;
                //OnDestroy();
            }

            GameObject.Find("UI").GetComponent<UI>().Refresh();

            break;
        }
    }

    public void ClickOnItem(InventorySlot slot)
    {
        if (slot.type != InventorySlotType.INVENTORY)
            return;
        if (slot.inventory_data.item == null)
            return;

        /*if (slot.inventory_data.item.GetPrototype().effects_when_consumed.Count == 0
            && slot.inventory_data.item.GetPrototype().effects_when_used.Count == 0)
            return;*/

        GameObject item_action_panel = GameObject.Instantiate(item_action_prefab, slot.gameObject.transform, false);
        item_action_panel.name = "ItemActionPanel";
        item_action_panel.GetComponent<RectTransform>().localPosition = new Vector3(-100,0);
        item_action_panel.GetComponent<ItemActionPanel>().item_data = slot.inventory_data.item;
        item_action_panel.GetComponent<ItemActionPanel>().ui_state = ui_state;
    }

    /*void OnDestroy()
    {
        if (GameObject.Find("UI").GetComponent<UI>().ui_state != null)
            GameObject.Find("UI").GetComponent<UI>().ui_state.DestroyState();
    }*/
}

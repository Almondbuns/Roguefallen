using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EquipmentSlotData
{
    public string name;
    public ItemData item;

    public List<ItemType> item_type;

    internal void Save(BinaryWriter save)
    {
        save.Write(name);

        if (item == null)
        {
            save.Write(false);
        }
        else
        {
            save.Write(true);
            item.Save(save);
        }

        save.Write(item_type.Count);
        foreach(var v in item_type)
        {
            save.Write((int)v);
        }
    }

    internal void Load(BinaryReader save)
    {
        name = save.ReadString();

        bool b = save.ReadBoolean();
        if (b == true)
        {
            item = new ItemData(null);
            item.Load(save);
        }

        int size = save.ReadInt32();
        item_type = new(size);
        for (int i = 0; i < size; ++i)
        {
            item_type.Add((ItemType) save.ReadInt32());
        }
    }
}

public class InventorySlotData
{
    public ItemData item;

    internal void Save(BinaryWriter save)
    {
        if (item == null)
        {
            save.Write(false);
        }
        else
        {
            save.Write(true);
            item.Save(save);
        }
    }

    internal void Load(BinaryReader save)
    {
        bool b = save.ReadBoolean();
        if (b == true)
        {
            item = new ItemData(null);
            item.Load(save);
        }
    }
}

public class InventoryData
{
    public List<InventorySlotData> slots;

    internal void Save(BinaryWriter save)
    {
        save.Write(slots.Count);
        foreach (var v in slots)
            v.Save(save);
    }

    internal void Load(BinaryReader save)
    {
        int size = save.ReadInt32();
        slots = new(size);
        for (int i = 0; i < size; ++i)
        {
            InventorySlotData v = new();
            v.Load(save);
            slots.Add(v);
        }
    }

    public InventoryData(int size)
    {
        slots = new List<InventorySlotData>();
        for (int i = 0; i < size; ++i)
        {
            slots.Add(new InventorySlotData());
        }
    }

    public bool AddItem(ItemData item, int slot_index = -1, int amount = -1)
    {
        if (amount == -1)
            amount = item.amount;

        if (slot_index == -1) // Next free slot
        {
            //First check if stackable existing item
            if (item.GetPrototype().is_stackable == true)
            {
                int counter = 0;
                foreach (InventorySlotData slot in slots)
                {
                    if (slot.item != null && slot.item.GetPrototype().name == item.GetPrototype().name && slot.item.GetPrototype().tier == item.GetPrototype().tier && slot.item.amount + amount <= slot.item.GetPrototype().stack_max)
                    {
                        slot_index = counter;
                        break;
                    }
                    ++counter;
                }
            }
        }

        if (slot_index == -1) // Next free slot
        {
            int counter = 0;
            foreach (InventorySlotData slot in slots)
            {
                if (slot.item == null)
                {
                    slot_index = counter;
                    break;
                }
                ++counter;
            }
        }

        if (slot_index < 0 || slot_index >= slots.Count)
            return false;

        if (slots[slot_index].item != null)
        {
            if (item.GetPrototype().is_stackable == false)
                return false;

            if(slots[slot_index].item.GetPrototype().name != item.GetPrototype().name)
                return false;

            if (slots[slot_index].item.amount + amount > slots[slot_index].item.GetPrototype().stack_max)
                return false;
        }

        if (slots[slot_index].item == null)
        {
            slots[slot_index].item = item;
        }
        else
        {
            slots[slot_index].item.amount += amount;
        }

        return true;
    }

    public int FindSlotIndex(ItemData item_data)
    {
        int slot_index = slots.FindIndex(x => x.item == item_data);
        return slot_index;
    }

    public ItemData RemoveItem(int slot_index, int? amount = null)
    {
        if (slot_index < 0 || slot_index >= slots.Count)
            return null;

        if (slots[slot_index].item == null)
            return null;

        ItemData item;
        if (amount.HasValue == false)
        {
            item = slots[slot_index].item;
            slots[slot_index].item = null;
        }
        else
        {
            item = slots[slot_index].item;
            int item_amount = Mathf.Min(amount.Value, item.amount);
            item.amount -= item_amount;
            if (item.amount == 0)
                slots[slot_index].item = null;
        }

        return item;
    }

    public void RemoveItem(ItemData item, int amount = 1)
    {
        int counter = 0;
        foreach(InventorySlotData slot in slots)
        {
            if (slot.item == item)
            {
                RemoveItem(counter, amount);
                return;
            }
            ++counter;
        }
    }

    internal ItemData GetItem(long item_id)
    {
        foreach (InventorySlotData slot in slots)
        {
            if (slot.item != null && slot.item.id == item_id)
            {
                return slot.item;
            }
        }

        return null;
    }

    public int GetWeight()
    {
        int value = 0;
        foreach (InventorySlotData slot in slots)
        {
            if (slot.item != null)
            {
                value += slot.item.GetWeight();
            }
        }

        return value;
    }
}

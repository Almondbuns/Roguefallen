using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class MFTavern : MapFeatureData
{
    public int capacity;
    public List<Type> questgiver_types;
    public List<(int x, int y)> questgiver_positions;

    public List<(ActorData questgiver, (int x, int y) position, QuestData quest)> questgivers;

    internal override void Save(BinaryWriter save)
    {
  
    }

    internal override void Load(BinaryReader save)
    {
    }

    public MFTavern(MapData map) : base(map)
    {
        dimensions = (15, 10);
        capacity = 0; // UnityEngine.Random.Range(2,7);

        questgivers = new();
      
        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("house_floor"));
        floors["floor"] = collection;

        collection = new();
        collection.Add(new MapObjectData("tavern_corner_wall_SW"));
        objects["wall_SW"] = collection;

        collection = new();
        collection.Add(new MapObjectData("tavern_corner_wall_NW"));
        objects["wall_NW"] = collection;

        collection = new();
        collection.Add(new MapObjectData("tavern_corner_wall_NE"));
        objects["wall_NE"] = collection;

        collection = new();
        collection.Add(new MapObjectData("tavern_corner_wall_SE"));
        objects["wall_SE"] = collection;

        collection = new();
        collection.Add(new MapObjectData("tavern_wall_S"));
        objects["wall_S"] = collection;

        collection = new();
        collection.Add(new MapObjectData("tavern_wall_W"));
        objects["wall_W"] = collection;

        collection = new();
        collection.Add(new MapObjectData("tavern_wall_N"));
        objects["wall_N"] = collection;

        collection = new();
        collection.Add(new MapObjectData("tavern_wall_E"));
        objects["wall_E"] = collection;

        collection = new();
        collection.Add(new MapObjectData("shop_counter"));
        objects["shop_counter"] = collection;

        collection = new();
        collection.Add(new MapObjectData("tavern_table"));
        objects["table"] = collection;

        collection = new();
        collection.Add(new MapObjectData("tavern_chair_L"));
        objects["chair_L"] = collection;

        collection = new();
        collection.Add(new MapObjectData("tavern_chair_R"));
        objects["chair_R"] = collection;

    }

    internal void RemoveQuestgiver(ActorData questgiver)
    {
        questgivers.RemoveAll(x => x.questgiver == questgiver);
        map.Remove(questgiver);
        GameObject.Find("Map").GetComponent<Map>().Kill(questgiver);
    }

    public override void Generate()
    {
        questgiver_types = new List<Type> { typeof(Questgiver1) };

        questgiver_positions = new List<(int x, int y)>
        { (position.x + 2, position.y + dimensions.y - 5),
        (position.x + 2, position.y + dimensions.y - 6),
        (position.x + 4, position.y + dimensions.y - 5),
        (position.x + 4, position.y + dimensions.y - 6),
        (position.x + 2, position.y + dimensions.y - 8),
        (position.x + 2, position.y + dimensions.y - 9),
        (position.x + 4, position.y + dimensions.y - 8),
        (position.x + 4, position.y + dimensions.y - 9),
        };


        for (int x = position.x; x < position.x + dimensions.x; ++x)
        {
            for (int y = position.y; y < position.y + dimensions.y; ++y)
            {
                map.tiles[x, y].objects.Clear();

                if (x == position.x && y == position.y)
                    map.tiles[x, y].objects.Add(objects["wall_SW"].Random());
                if (x == position.x + dimensions.x - 1 && y == position.y)
                    map.tiles[x, y].objects.Add(objects["wall_SE"].Random());
                if (x == position.x && y == position.y + dimensions.y - 1)
                    map.tiles[x, y].objects.Add(objects["wall_NW"].Random());
                if (x == position.x + dimensions.x - 1 && y == position.y + dimensions.y - 1)
                    map.tiles[x, y].objects.Add(objects["wall_NE"].Random());

                if (x == position.x && y > position.y && y < position.y + dimensions.y - 1)
                    map.tiles[x, y].objects.Add(objects["wall_W"].Random());
                if (x == position.x + dimensions.x - 1 && y > position.y && y < position.y + dimensions.y - 1)
                    map.tiles[x, y].objects.Add(objects["wall_E"].Random());
                if (x > position.x && x < position.x + dimensions.x - 1 && y == position.y)
                    map.tiles[x, y].objects.Add(objects["wall_S"].Random());
                if (x > position.x && x < position.x + dimensions.x - 1 && y == position.y + dimensions.y - 1)
                    map.tiles[x, y].objects.Add(objects["wall_N"].Random());

                if (x == position.x + dimensions.x / 2 + 2 && y == position.y)
                    map.tiles[x, y].objects.Clear();

                map.tiles[x, y].floor = floors["floor"].Random();
            }
        }

        map.tiles[position.x + 6, position.y + dimensions.y - 3].objects.Add(objects["shop_counter"].Random());
        map.tiles[position.x + 7, position.y + dimensions.y - 3].objects.Add(objects["shop_counter"].Random());
        map.tiles[position.x + 8, position.y + dimensions.y - 3].objects.Add(objects["shop_counter"].Random());
        map.tiles[position.x + 9, position.y + dimensions.y - 3].objects.Add(objects["shop_counter"].Random());
        map.tiles[position.x + 9, position.y + dimensions.y - 2].objects.Add(objects["shop_counter"].Random());
        map.tiles[position.x + 6, position.y + dimensions.y - 2].objects.Add(objects["shop_counter"].Random());

        map.tiles[position.x + 3, position.y + dimensions.y - 5].objects.Add(objects["table"].Random());
        map.tiles[position.x + 3, position.y + dimensions.y - 6].objects.Add(objects["table"].Random());
        map.tiles[position.x + 2, position.y + dimensions.y - 5].objects.Add(objects["chair_L"].Random());
        map.tiles[position.x + 2, position.y + dimensions.y - 6].objects.Add(objects["chair_L"].Random());
        map.tiles[position.x + 4, position.y + dimensions.y - 5].objects.Add(objects["chair_R"].Random());
        map.tiles[position.x + 4, position.y + dimensions.y - 6].objects.Add(objects["chair_R"].Random());

        map.tiles[position.x + 3, position.y + dimensions.y - 8].objects.Add(objects["table"].Random());
        map.tiles[position.x + 3, position.y + dimensions.y - 9].objects.Add(objects["table"].Random());
        map.tiles[position.x + 2, position.y + dimensions.y - 8].objects.Add(objects["chair_L"].Random());
        map.tiles[position.x + 2, position.y + dimensions.y - 9].objects.Add(objects["chair_L"].Random());
        map.tiles[position.x + 4, position.y + dimensions.y - 8].objects.Add(objects["chair_R"].Random());
        map.tiles[position.x + 4, position.y + dimensions.y - 9].objects.Add(objects["chair_R"].Random());

        map.tiles[position.x + 7, position.y + dimensions.y - 5].objects.Add(objects["table"].Random());
        map.tiles[position.x + 7, position.y + dimensions.y - 6].objects.Add(objects["table"].Random());
        map.tiles[position.x + 6, position.y + dimensions.y - 5].objects.Add(objects["chair_L"].Random());
        map.tiles[position.x + 6, position.y + dimensions.y - 6].objects.Add(objects["chair_L"].Random());
        map.tiles[position.x + 8, position.y + dimensions.y - 5].objects.Add(objects["chair_R"].Random());
        map.tiles[position.x + 8, position.y + dimensions.y - 6].objects.Add(objects["chair_R"].Random());

        map.tiles[position.x + 7, position.y + dimensions.y - 8].objects.Add(objects["table"].Random());
        map.tiles[position.x + 7, position.y + dimensions.y - 9].objects.Add(objects["table"].Random());
        map.tiles[position.x + 6, position.y + dimensions.y - 8].objects.Add(objects["chair_L"].Random());
        map.tiles[position.x + 6, position.y + dimensions.y - 9].objects.Add(objects["chair_L"].Random());
        map.tiles[position.x + 8, position.y + dimensions.y - 8].objects.Add(objects["chair_R"].Random());
        map.tiles[position.x + 8, position.y + dimensions.y - 9].objects.Add(objects["chair_R"].Random());

        map.tiles[position.x + 11, position.y + dimensions.y - 5].objects.Add(objects["table"].Random());
        map.tiles[position.x + 11, position.y + dimensions.y - 6].objects.Add(objects["table"].Random());
        map.tiles[position.x + 10, position.y + dimensions.y - 5].objects.Add(objects["chair_L"].Random());
        map.tiles[position.x + 10, position.y + dimensions.y - 6].objects.Add(objects["chair_L"].Random());
        map.tiles[position.x + 12, position.y + dimensions.y - 5].objects.Add(objects["chair_R"].Random());
        map.tiles[position.x + 12, position.y + dimensions.y - 6].objects.Add(objects["chair_R"].Random());

        map.tiles[position.x + 11, position.y + dimensions.y - 8].objects.Add(objects["table"].Random());
        map.tiles[position.x + 11, position.y + dimensions.y - 9].objects.Add(objects["table"].Random());
        map.tiles[position.x + 10, position.y + dimensions.y - 8].objects.Add(objects["chair_L"].Random());
        map.tiles[position.x + 10, position.y + dimensions.y - 9].objects.Add(objects["chair_L"].Random());
        map.tiles[position.x + 12, position.y + dimensions.y - 8].objects.Add(objects["chair_R"].Random());
        map.tiles[position.x + 12, position.y + dimensions.y - 9].objects.Add(objects["chair_R"].Random());

        ActorData vendor = new MonsterData(position.x + 7, position.y + dimensions.y - 2, new Barkeeper(10));
        map.Add(vendor);

        for (int i = 0; i < capacity; ++ i)
        {
            int rand_type = UnityEngine.Random.Range(0, questgiver_types.Count);
            int rand_position = UnityEngine.Random.Range(0, questgiver_positions.Count);

            ActorPrototype prototype = (ActorPrototype)Activator.CreateInstance(questgiver_types[rand_type], 10);
            ActorData actor = new MonsterData(questgiver_positions[rand_position].x, questgiver_positions[rand_position].y, prototype);
            map.Add(actor);

            QuestData quest;
            if (UnityEngine.Random.Range(0, 2) == 0)
                quest = new QDKillMonster();
            else
                quest = new QDFetchItem();

            quest.GenerateQuest(1, QuestComplexity.Short);

            questgivers.Add((actor,questgiver_positions[rand_position],quest));
        }
    }

    public override bool OnPlayerMovement(int move_destination_x, int move_destination_y)
    {
        for (int i = 0; i < questgivers.Count; ++i)
        {
            if (move_destination_x == questgivers[i].position.x && move_destination_y == questgivers[i].position.y)
            {
                UIStateQuestStartDialog quest_state = new UIStateQuestStartDialog(this, questgivers[i].quest, questgivers[i].questgiver);
                GameObject.Find("UI").GetComponent<UI>().AddUIState(quest_state);
                return false;
            }
        }

        return true;
    }
}


public class MFStoreWeapons : MFStore
{
    public MFStoreWeapons(MapData map) : base(map)
    {
        capacity = 28;

        //item_types.Add(typeof(ItemSword1H));
        item_types.Add(typeof(ItemHandAxe1H));
        item_types.Add(typeof(ItemDoubleAxe1H));
        item_types.Add(typeof(ItemPickaxe1H));
        item_types.Add(typeof(ItemHammer1H));
        item_types.Add(typeof(ItemMace1H));
        item_types.Add(typeof(ItemFlail1H));
        //item_types.Add(typeof(ItemSword2H));
        item_types.Add(typeof(ItemAxe2H));
        item_types.Add(typeof(ItemWarHammer2H));
        //item_types.Add(typeof(ItemSpear1H));
        //item_types.Add(typeof(ItemSpear2H));
        item_types.Add(typeof(ItemShieldHeavy));
        item_types.Add(typeof(ItemShieldMedium));
    }
}

public class MFStoreArmor : MFStore
{
    public MFStoreArmor(MapData map) : base(map)
    {
        capacity = 18;

        item_types.Add(typeof(ItemBootsHeavy));
        //item_types.Add(typeof(ItemBootsMedium));
        //item_types.Add(typeof(ItemBootsLight));
        item_types.Add(typeof(ItemChestHeavy));
        //item_types.Add(typeof(ItemChestMedium));
        //item_types.Add(typeof(ItemChestLight));
        item_types.Add(typeof(ItemHandsHeavy));
        //item_types.Add(typeof(ItemHandsMedium));
        //item_types.Add(typeof(ItemHandsLight));
        item_types.Add(typeof(ItemHeadHeavy));
        //item_types.Add(typeof(ItemHeadMedium));
        //item_types.Add(typeof(ItemHeadLight));
    }
}

public class MFStoreConsumables : MFStore
{
    public MFStoreConsumables(MapData map) : base(map)
    {
        capacity = 28;

        item_types.Add(typeof(ItemMeatHorn));
        item_types.Add(typeof(ItemStaminaPotion));
        item_types.Add(typeof(ItemAlmondBun));
        item_types.Add(typeof(ItemHealthPotion));
        item_types.Add(typeof(ItemFirebomb));
        item_types.Add(typeof(ItemThrowingKnife));
        item_types.Add(typeof(ItemBlueberries));        
        item_types.Add(typeof(ItemManaPotion));
        item_types.Add(typeof(ItemAcidFlask));
    }
}

public class MFStoreJewelry : MFStore
{
    public MFStoreJewelry(MapData map) : base(map)
    {
        capacity = 15;

        item_types.Add(typeof(ItemRing));
        item_types.Add(typeof(ItemRing));
        item_types.Add(typeof(ItemAmulet));
    }
}

public class MFStoreUsables : MFStore
{
    public MFStoreUsables(MapData map) : base(map)
    {
        capacity = 23;

        item_types.Add(typeof(ItemPoemOfReturn));
        item_types.Add(typeof(ItemPoemOfAJourney));
        item_types.Add(typeof(ItemPoemOfAWalk));
        item_types.Add(typeof(ItemPeppermintTea));
        item_types.Add(typeof(ItemStrawberryTea));
        item_types.Add(typeof(ItemCamomileTea));
        item_types.Add(typeof(ItemFluteOfHealing));
        item_types.Add(typeof(ItemFluteOfBraveness));
        item_types.Add(typeof(ItemFluteOfEndurance));
        item_types.Add(typeof(ItemRepairPowder));
    }
}


public abstract class MFStore : MapFeatureData
{
    public int capacity;
    public List<Type> item_types;
    public List<ItemData> items;
    public List<(int x, int y)> display_positions;
    public bool use_walls = true;
    public bool use_small_version = false;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);
        save.Write(capacity);

        save.Write(item_types.Count);
        foreach (var o in item_types)
            save.Write(o.Name);

        save.Write(items.Count);
        foreach (var o in items) // items are stored in map. We only reference here
        {
            save.Write(o.GetName());
            save.Write(o.x);
            save.Write(o.y);
        }

        save.Write(display_positions.Count);
        foreach (var o in display_positions)
        {
            save.Write(o.x);
            save.Write(o.y);
        }

        save.Write(use_walls);
        save.Write(use_small_version);
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);
        capacity = save.ReadInt32();

        int size = save.ReadInt32();
        item_types = new(size);
        for (int i = 0; i < size; ++i)
        {
            item_types.Add(Type.GetType(save.ReadString()));
        }

        size = save.ReadInt32();
        items = new(size);
        for (int i = 0; i < size; ++i)
        {
            string name = save.ReadString();
            int x = save.ReadInt32();
            int y = save.ReadInt32();
            //We need to find the corresponding item in the map
            ItemData item_data = map.items.Find(item => item.GetName() == name && item.x == x && item.y == y);
            if (item_data == null)
            {
                Debug.Log("Cannot find item " + name + " at (" + x + "/" + y + ")");
                continue;
            }
            items.Add(item_data);
        }

        size = save.ReadInt32();
        display_positions = new(size);
        for (int i = 0; i < size; ++i)
        {
            display_positions.Add((save.ReadInt32(), save.ReadInt32()));
        }

        use_walls = save.ReadBoolean();
        use_small_version = save.ReadBoolean();
    }


    public MFStore(MapData map) : base(map)
    {
        dimensions = (11, 11);
        
        capacity = UnityEngine.Random.Range(33, 33);
        item_types = new();
        items = new List<ItemData>();
        distribute_general_actors = false;
        distribute_general_items = false;

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("house_floor"){});
        floors["floor"] = collection;

        collection = new();
        collection.Add(new MapObjectData("house_corner_wall_SW"));
        objects["wall_SW"] = collection;

        collection = new();
        collection.Add(new MapObjectData("house_corner_wall_NW"));
        objects["wall_NW"] = collection;

        collection = new();
        collection.Add(new MapObjectData("house_corner_wall_NE"));
        objects["wall_NE"] = collection;

        collection = new();
        collection.Add(new MapObjectData("house_corner_wall_SE"));
        objects["wall_SE"] = collection;

        collection = new();
        collection.Add(new MapObjectData("house_wall_S"));
        objects["wall_S"] = collection;

        collection = new();
        collection.Add(new MapObjectData("house_wall_W"));
        objects["wall_W"] = collection;

        collection = new();
        collection.Add(new MapObjectData("house_wall_N"));
        objects["wall_N"] = collection;

        collection = new();
        collection.Add(new MapObjectData("house_wall_E"));
        objects["wall_E"] = collection;

        collection = new();
        collection.Add(new MapObjectData("shop_counter"));
        objects["shop_counter"] = collection;

        collection = new();
        collection.Add(new MapObjectData("shop_display"){emits_light = true, light_color = new Color(1,1,1) });
        objects["shop_display"] = collection;

    }

    void GenerateDisplayItems()
    {
        if (use_small_version == false)
        {
            display_positions = new List<(int x, int y)>
            {
                (position.x + 6, position.y + 8),
                (position.x + 6, position.y + 7),
                (position.x + 7, position.y + 8),
                (position.x + 7, position.y + 7),
                (position.x + 8, position.y + 8),
                (position.x + 8, position.y + 7),
                (position.x + 9, position.y + 7),
                (position.x + 9, position.y + 8),
                (position.x + 9, position.y + 1),
                (position.x + 9, position.y + 2),
                (position.x + 9, position.y + 3),
                (position.x + 9, position.y + 4),
                (position.x + 9, position.y + 5),
                (position.x + 1, position.y + 1),
                (position.x + 1, position.y + 2),
                (position.x + 1, position.y + 3),
                (position.x + 1, position.y + 4),
                (position.x + 1, position.y + 5),
                (position.x + 3, position.y + 1),
                (position.x + 3, position.y + 2),
                (position.x + 3, position.y + 3),
                (position.x + 3, position.y + 4),
                (position.x + 3, position.y + 5),
                (position.x + 5, position.y + 1),
                (position.x + 5, position.y + 2),
                (position.x + 5, position.y + 3),
                (position.x + 5, position.y + 4),
                (position.x + 5, position.y + 5),
                (position.x + 7, position.y + 1),
                (position.x + 7, position.y + 2),
                (position.x + 7, position.y + 3),
                (position.x + 7, position.y + 4),
                (position.x + 7, position.y + 5),
            };
        }
        else
        {
             display_positions = new List<(int x, int y)>
            {
                (position.x + 6, position.y + 2),
                (position.x + 6, position.y + 3),
                (position.x + 7, position.y + 2),
                (position.x + 7, position.y + 3),
                (position.x + 8, position.y + 2),
                (position.x + 8, position.y + 3),
                (position.x + 9, position.y + 2),
                (position.x + 9, position.y + 3),
            };
        }

        for (int i = 0; i < capacity; i += 1)
        {
            ItemData item = GenerateItem();
            items.Add(item);
        }

        int slot_counter = 0;
        foreach (ItemData item in items)
        {
            item.x = display_positions[slot_counter].x;
            item.y = display_positions[slot_counter].y;
            map.tiles[display_positions[slot_counter].x, display_positions[slot_counter].y].objects.Add(objects["shop_display"].Random());
            map.Add(item);
            ++slot_counter;
        }
    }

    private ItemData GenerateItem()
    {
        int rand = UnityEngine.Random.Range(0, item_types.Count);

        ItemPrototype prototype = (ItemPrototype) Activator.CreateInstance(item_types[rand], UnityEngine.Random.Range(1,11));

        ItemData item = new ItemData(prototype, -1, -1);

        //Currently possible item quality depends on item level. So to get higher item quality in shops increase probs
        int r = UnityEngine.Random.Range(1, 101);
        if (r <= 20)
            item.SetQuality(ItemQuality.Unique);
        else if (r <= 60)
            item.SetQuality(ItemQuality.Magical2);
        else if (r <= 90)
            item.SetQuality(ItemQuality.Magical1);

        return item;
    }

    public override void Generate()
    {
        for (int x = position.x; x < position.x + dimensions.x; ++x)
        {
            for (int y = position.y; y < position.y + dimensions.y; ++y)
            {
                map.tiles[x, y].objects.Clear();

                if (use_walls == true)
                {
                    if (x == position.x && y == position.y)
                        map.tiles[x, y].objects.Add(objects["wall_SW"].Random());
                    if (x == position.x + dimensions.x - 1 && y == position.y)
                        map.tiles[x, y].objects.Add(objects["wall_SE"].Random());
                    if (x == position.x && y == position.y + dimensions.y - 1)
                        map.tiles[x, y].objects.Add(objects["wall_NW"].Random());
                    if (x == position.x + dimensions.x - 1 && y == position.y + dimensions.y - 1)
                        map.tiles[x, y].objects.Add(objects["wall_NE"].Random());

                    if (x == position.x && y > position.y && y < position.y + dimensions.y - 1)
                        map.tiles[x, y].objects.Add(objects["wall_W"].Random());
                    if (x == position.x + dimensions.x - 1 && y > position.y && y < position.y + dimensions.y - 1)
                        map.tiles[x, y].objects.Add(objects["wall_E"].Random());
                    if (x > position.x && x < position.x + dimensions.x - 1 && y == position.y)
                        map.tiles[x, y].objects.Add(objects["wall_S"].Random());
                    if (x > position.x && x < position.x + dimensions.x - 1 && y == position.y + dimensions.y - 1)
                        map.tiles[x, y].objects.Add(objects["wall_N"].Random());

                    if (x == position.x + dimensions.x/2 + 2 && y == position.y)
                        map.tiles[x, y].objects.Clear();
                }

                map.tiles[x, y].floor = floors["floor"].Random();
                map.tiles[x,y].light = Color.white;
            }
        }

        if (use_small_version == false)
        {
            map.tiles[position.x + 1, position.y + dimensions.y - 3].objects.Add(objects["shop_counter"].Random());
            map.tiles[position.x + 2, position.y + dimensions.y - 3].objects.Add(objects["shop_counter"].Random());
            map.tiles[position.x + 3, position.y + dimensions.y - 3].objects.Add(objects["shop_counter"].Random());
            map.tiles[position.x + 4, position.y + dimensions.y - 3].objects.Add(objects["shop_counter"].Random());
            map.tiles[position.x + 4, position.y + dimensions.y - 2].objects.Add(objects["shop_counter"].Random());
        }

        GenerateDisplayItems();

        ActorData vendor = new MonsterData(position.x + 1, position.y + dimensions.y - 2, new Shopkeeper(20));
        //vendor.is_friendly = true;
        map.Add(vendor);
    }

    public override bool OnPlayerMovement(int move_destination_x, int move_destination_y)
    {
        foreach (ItemData item in items)
        {
            if (move_destination_x == item.x && move_destination_y == item.y)
            {
                UIStateShopBuy buy_state = new UIStateShopBuy(this, item);
                GameObject.Find("UI").GetComponent<UI>().AddUIState(buy_state);
                return false;
            }
        }
        return true;
    }

    public bool ReplaceItem(ItemData item_data)
    {
        map.Remove(item_data);
        GameObject.Find("Map").GetComponent<Map>().Kill(item_data);

        int item_index = items.FindIndex(x => x == item_data);

        if (item_index == -1)
            return false;
        
        ItemData new_item = GenerateItem();
        items[item_index] = new_item;
        new_item.x = display_positions[item_index].x;
        new_item.y = display_positions[item_index].y;
        map.Add(new_item);
        GameLogger.Log("The shopkeeper places a new item in the shop.");
        return true;
    }
}

public class MFVillageField : MapFeatureData
{
    public MFVillageField(MapData map) : base(map)
    {
        dimensions = (UnityEngine.Random.Range(8,16), 8);
       
        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("field_empty"));
        collection.Add(new MapObjectData("radish_field_1"));
        collection.Add(new MapObjectData("radish_field_2"));
        floors["floor"] = collection;

      
    }

    public override void Generate()
    {
        for (int x = position.x; x < position.x + dimensions.x; ++x)
        {
            for (int y = position.y; y < position.y + dimensions.y; ++y)
            {
                map.tiles[x, y].objects.Clear();

                
                map.tiles[x, y].floor = floors["floor"].Random();
            }
        }
    }

}

public class MFCaveEntrance : MFChangeDungeon
{
    public MFCaveEntrance(MapData map) : base(map)
    {
       
    }

    public MFCaveEntrance(MapData map, DungeonChangeData dcd) : base(map, dcd)
    {
        dimensions = (5,5);

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("cave_wall_1"));
        collection.Add(new MapObjectData("cave_wall_2"));
        collection.Add(new MapObjectData("cave_wall_3"));
        collection.Add(new MapObjectData("cave_wall_4"));
        objects["wall"] = collection;

        collection = new();
        collection.Add(new MapObjectData("cave_entrance_down"));
        objects["entrance"] = collection;
    }

    public override void Generate()
    {
        for (int x = position.x; x < position.x + dimensions.x; ++x)
        {
            for (int y = position.y; y < position.y + dimensions.y; ++y)
            {
                map.tiles[x, y].objects.Clear();
                if (y > position.y)
                    map.tiles[x, y].objects.Add(objects["wall"].Random());
            }
        }

        map.tiles[position.x + dimensions.x / 2, position.y + 1].objects.Add(objects["entrance"].Random());

        enter_tiles.Add((position.x + dimensions.x / 2, position.y+ 1));
        exit_tile = (position.x + dimensions.x / 2, position.y);
    }
}

public class MFVillageSunflowers : MapFeatureData
{
    public MFVillageSunflowers(MapData map) : base(map)
    {
        dimensions = (6, 3);

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("field_empty"));
        floors["floor"] = collection;
    }

    public override void Generate()
    {
        for (int i = 0; i < 6; ++ i)
        {
            map.tiles[position.x + i, position.y].objects.Clear();
            map.tiles[position.x + i, position.y+1].objects.Clear();
            map.tiles[position.x + i, position.y+2].objects.Clear();
            map.tiles[position.x + i, position.y].floor = floors["floor"].Random();
            map.tiles[position.x + i, position.y+1].floor = floors["floor"].Random();

            MonsterData flower = new MonsterData(0,0, new Flower(0));
            flower.MoveTo(position.x + i,position.y+1);
            map.Add(flower);        
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EncounterData
{
    public List<(Type type, int amount_min, int amount_max)> type_amounts;
    public int level_min;
    public int level_max;

    public EncounterData()
    {
        type_amounts = new();
    }

    public void Save(System.IO.BinaryWriter save)
    {
        save.Write(type_amounts.Count);
        foreach(var v in type_amounts)
        {
            save.Write(v.type.ToString());
            save.Write(v.amount_min);
            save.Write(v.amount_max);
        }

        save.Write(level_min);
        save.Write(level_max);
    }

    public void Load(System.IO.BinaryReader save)
    {
        int size = save.ReadInt32();
        
        for (int i = 0; i < size; ++ i)
        {
            type_amounts.Add((Type.GetType(save.ReadString()), save.ReadInt32(), save.ReadInt32()));
        }

        level_min = save.ReadInt32();
        level_max = save.ReadInt32();

        size = save.ReadInt32();
    }
}

public class ItemPlacementData
{
    public Type type;
    public List<(float probability, int amount)> prob_amount;
    public int level_min;
    public int level_max;

    public ItemPlacementData()
    {
        prob_amount = new();
    }

    public void Save(System.IO.BinaryWriter save)
    {
        save.Write(type.ToString());

        save.Write(prob_amount.Count);
        foreach(var v in prob_amount)
        {
            
            save.Write(v.probability);
            save.Write(v.amount);
        }

        save.Write(level_min);
        save.Write(level_max);
    }

    public void Load(System.IO.BinaryReader save)
    {
        type = Type.GetType(save.ReadString());

        int size = save.ReadInt32();
        for (int i = 0; i < size; ++ i)
        {
            prob_amount.Add((save.ReadSingle(), save.ReadInt32()));
        }

        level_min = save.ReadInt32();
        level_max = save.ReadInt32();
    }

    public int GetRandomAmount()
    {   
        float rand = UnityEngine.Random.value;
        foreach(var v in prob_amount)
        {
            if (rand <= v.probability)
            return v.amount;

            rand -= v.probability;
        }
        //This code branch should not happen if probs are set correctly
        Debug.Log("Warning: ItemPlacementData Probability Overflow: using last amount entry.");
        return prob_amount[prob_amount.Count-1].amount;
    }
}

public class DungeonLevelData
{
    public int dungeon_level;
    public int difficulty_level;
    public int biome_index;
    public (int x, int y) dimensions;
    public bool has_enemies = true;
    public bool has_items = true;
    public bool is_always_visible = false;
    public (int min, int max) number_of_rooms;
    public (int min, int max) number_of_encounters;
    public List<ItemPlacementData> items;
    public (int min, int max) number_of_gold_items;

    public List<(Type type, int count_min, int count_max)> map_features;
    public List<DungeonChangeData> dungeon_changes;
    public List<(int weight, EncounterData encounter)> encounters;
    public List<(Type type, int min, int max)> dynamic_objects;

    public MapData map;

    public bool regeneration_needed = true;
    public bool regeneration_quest_goals = false;

    public List<(int x, int y, int w, int h)> room_list;

    internal void Save(System.IO.BinaryWriter save)
    {
        save.Write(dungeon_level);
        save.Write(difficulty_level);
        save.Write(biome_index);
        save.Write(dimensions.x);
        save.Write(dimensions.y);
        save.Write(has_enemies);
        save.Write(has_items);
        save.Write(is_always_visible);
        save.Write(number_of_rooms.min);
        save.Write(number_of_rooms.max);
        save.Write(number_of_encounters.min);
        save.Write(number_of_encounters.max);
        save.Write(number_of_gold_items.min);
        save.Write(number_of_gold_items.max);

        save.Write(map_features.Count);
        foreach(var v in map_features)
        {
            save.Write(v.type.Name);
            save.Write(v.count_min);
            save.Write(v.count_max);
        }

        save.Write(items.Count);
        foreach(var v in items)
        {
            v.Save(save);
        }

        save.Write(dungeon_changes.Count);
        foreach (var v in dungeon_changes)
        {
            v.Save(save);
        }

        save.Write(encounters.Count);
        foreach (var v in encounters)
        {
            save.Write(v.weight);
            v.encounter.Save(save);
        }

        save.Write(dynamic_objects.Count);
        foreach (var v in dynamic_objects)
        {
            save.Write(v.type.Name);
            save.Write(v.min);
            save.Write(v.max);
        }

        if (map == null)
            save.Write(false);
        else
        {
            save.Write(true);
            save.Write(map.w);
            save.Write(map.h);
            map.Save(save);
        }

        save.Write(regeneration_needed);
        save.Write(regeneration_quest_goals);

        save.Write(room_list.Count);
        foreach (var v in room_list)
        {
            save.Write(v.x);
            save.Write(v.y);
            save.Write(v.w);
            save.Write(v.h);
        }
    }
    
    internal void Load(System.IO.BinaryReader save)
    {
        dungeon_level = save.ReadInt32();
        difficulty_level = save.ReadInt32();
        biome_index = save.ReadInt32();
        dimensions = (save.ReadInt32(), save.ReadInt32());
        has_enemies = save.ReadBoolean();
        has_items = save.ReadBoolean();
        is_always_visible = save.ReadBoolean();
        number_of_rooms = (save.ReadInt32(), save.ReadInt32());
        number_of_encounters = (save.ReadInt32(), save.ReadInt32());
        number_of_gold_items = (save.ReadInt32(), save.ReadInt32());
        
        int size = save.ReadInt32();
        map_features = new(size);
        for (int i = 0; i < size; ++i)
        {
            map_features.Add((Type.GetType(save.ReadString()), save.ReadInt32(), save.ReadInt32()));
        }

        size = save.ReadInt32();
        items = new(size);
        for (int i = 0; i < size; ++i)
        {
            ItemPlacementData v = new();
            v.Load(save);
            items.Add(v);
        }

        size = save.ReadInt32();
        dungeon_changes = new(size);
        for (int i = 0; i < size; ++i)
        {
            DungeonChangeData v = new DungeonChangeData();
            v.Load(save);
            dungeon_changes.Add(v);
        }

        size = save.ReadInt32();
        encounters= new(size);
        for (int i = 0; i < size; ++i)
        {
            int weight = save.ReadInt32();
            EncounterData v = new();
            v.Load(save);
            encounters.Add((weight,v));
        }

        size = save.ReadInt32();
        dynamic_objects = new(size);
        for (int i = 0; i < size; ++i)
        {
            dynamic_objects.Add((Type.GetType(save.ReadString()), save.ReadInt32(), save.ReadInt32()));
        }

        bool b = save.ReadBoolean();
        if (b == true)
        {
            map = new MapData(save.ReadInt32(), save.ReadInt32());
            map.Load(save);
        }
        else
        {
            map = null;
        }

        regeneration_needed = save.ReadBoolean();
        regeneration_quest_goals = save.ReadBoolean();

        room_list = new(size);
        for (int i = 0; i < size; ++i)
        {
            room_list.Add((save.ReadInt32(), save.ReadInt32(), save.ReadInt32(), save.ReadInt32()));
        }
    }

    public DungeonLevelData()
    {
        map_features = new();
        dungeon_changes = new();
        encounters = new();
        number_of_encounters = (0, 0);
        dynamic_objects = new();
        items = new();
    }

    public void SetRegenerationNeeded(bool regeneration_quest_items)
    {
        regeneration_needed = true;
        this.regeneration_quest_goals = regeneration_quest_items;
    }

    public void CreateMapLevel(List<ItemData> quest_items, List<ActorData> quest_actors)
    {
        int n_rooms = UnityEngine.Random.Range(number_of_rooms.min, number_of_rooms.max + 1);
        room_list = new();
        map = GameObject.Find("GameData").GetComponent<GameData>().biomes[biome_index].CreateMapLevel(dungeon_level, dimensions.x, dimensions.y, n_rooms, map_features, dungeon_changes, room_list);

        if (quest_items != null)
            DistributeQuestItems(quest_items);
        if (has_items == true)
            DistributeItems();

        //GuaranteeConnectivity();

        if (quest_actors != null)
            DistributeQuestActors(quest_actors);
        if (has_enemies == true)
            DistributeMonsters();

        DistributeObjects();

        if (is_always_visible == true)
            SetExploredVisibility();

        map.CalculateLight(GameObject.Find("GameData").GetComponent<GameData>().biomes[biome_index].ambience_light);

        regeneration_needed = false;
        regeneration_quest_goals = false;
    }

    public void DistributeObjects()
    {
        foreach((Type type, int min, int max) dynamic_object in dynamic_objects)
        {
            int number_of_objects = UnityEngine.Random.Range(dynamic_object.min, dynamic_object.max + 1);
            for (int i = 0; i < number_of_objects; ++i)
            {
                int object_level = Mathf.Max(1, UnityEngine.Random.Range(difficulty_level - 1, difficulty_level + 2));
                ActorData object_data = new DynamicObjectData(-1, -1, (ActorPrototype)Activator.CreateInstance(dynamic_object.type, object_level));

                //search for position
                int number_tries = 0;
                bool found_position = false;
                while (found_position == false && number_tries < 1000)
                {
                    int x = UnityEngine.Random.Range(0, map.tiles.GetLength(0));
                    int y = UnityEngine.Random.Range(0, map.tiles.GetLength(1));
                    if (map.IsAccessableTile(x, y) == true)
                    {
                        bool is_in_forbidden_feature = false;
                        
                        int border_size = 5;
                        foreach(MapFeatureData mfd in map.features)
                        {
                            if (mfd.distribute_general_actors == false &&
                                x >= mfd.position.x - border_size && x <= mfd.position.x + mfd.dimensions.x + border_size&&
                                y >= mfd.position.y - border_size && y <= mfd.position.y + mfd.dimensions.y + border_size)
                            {
                            is_in_forbidden_feature = true;
                            }
                        }          
                        if (is_in_forbidden_feature == false)
                        {
                            object_data.MoveTo(x,y);                        
                            map.Add(object_data);
                            found_position = true;
                        }
                    }
                    ++number_tries;
                }
            }
        }
    }

    public void SetExploredVisibility()
    {
        for (int x = 0; x < map.tiles.GetLength(0); ++x)
        {
            for (int y = 0; y < map.tiles.GetLength(1); ++y)
            {
                map.tiles[x, y].visibility = Visibility.Active;
            }
        }

        map.is_always_visible = true;
    }

    public void GuaranteeConnectivity()
    {
        foreach ((int x, int y) tile in map.important_connect_tiles)
        {
            map.tiles[tile.x, tile.y].objects.RemoveAll(x => x.movement_blocked == true);

            Path path = Algorithms.AStar(map, tile, map.important_connect_tiles[0], false, true);
            foreach (var p in path.path)
            {
                if (p.cumulated_cost >= 99900)
                {
                    map.tiles[p.x, p.y].objects.RemoveAll(x => x.movement_blocked == true);
                }
            }
        }
    }

    public void DistributeItems()
    {      
        foreach(var v in items)
        {
            int number_of_items = v.GetRandomAmount();
            for (int i = 0; i < number_of_items; ++i)
            {
                ItemData item = null;
                //Item levels start at one now
                int item_level = Mathf.Max(1, UnityEngine.Random.Range(difficulty_level - 1, difficulty_level + 2));

                ItemPrototype p = (ItemPrototype) Activator.CreateInstance(v.type, item_level);
                item = new(p,-1,1);
            
                int number_tries = 0;
                bool found_position = false;
                while (found_position == false && number_tries < 1000)
                {
                    int x = UnityEngine.Random.Range(0, map.tiles.GetLength(0));
                    int y = UnityEngine.Random.Range(0, map.tiles.GetLength(1));
                    if (map.IsAccessableTile(x, y) == true && map.GetItem(x,y) == null)
                    {
                        bool is_in_forbidden_feature = false;
                        
                        int border_size = 5;
                        foreach(MapFeatureData mfd in map.features)
                        {
                            if (mfd.distribute_general_items == false &&
                                x >= mfd.position.x - border_size && x <= mfd.position.x + mfd.dimensions.x + border_size&&
                                y >= mfd.position.y - border_size && y <= mfd.position.y + mfd.dimensions.y + border_size)
                            {
                            is_in_forbidden_feature = true;
                            }
                        }          
                        if (is_in_forbidden_feature == false)
                        {
                            item.x = x;
                            item.y = y;
                            map.items.Add(item);
                            found_position = true;
                            map.important_connect_tiles.Add((x, y));

                            int r = UnityEngine.Random.Range(1, 101);
                            if (r <= 10)
                                item.SetQuality(ItemQuality.Unique);
                            else if (r <= 40)
                                item.SetQuality(ItemQuality.Magical2);
                            else if (r <= 70)
                                item.SetQuality(ItemQuality.Magical1);
                        }
                    }
                    ++number_tries;
                }
            }
        }

        int number_of_gold = UnityEngine.Random.Range(number_of_gold_items.min, number_of_gold_items.max + 1);
        for (int i = 0; i < number_of_gold; ++i)
        {
            ItemData item = null;
            //Item levels start at zero
            int item_level = Mathf.Max(0, UnityEngine.Random.Range(difficulty_level - 2, difficulty_level + 1));

            item = new ItemData( new ItemGold(item_level), -1, -1);

            int number_tries = 0;
            bool found_position = false;
            while (found_position == false && number_tries < 1000)
            {
                int x = UnityEngine.Random.Range(0, map.tiles.GetLength(0));
                int y = UnityEngine.Random.Range(0, map.tiles.GetLength(1));
                if (map.IsAccessableTile(x, y) == true)
                {
                    item.x = x;
                    item.y = y;
                    map.items.Add(item);
                    found_position = true;
                    map.important_connect_tiles.Add((x, y));
                }
                ++number_tries;
            }

        }

    }

    public void DistributeQuestItems(List<ItemData> quest_items)
    {
        foreach(ItemData item in quest_items)
        { 
            int number_tries = 0;
            bool found_position = false;
            while (found_position == false && number_tries < 10000)
            {
                int x = UnityEngine.Random.Range(0, map.tiles.GetLength(0));
                int y = UnityEngine.Random.Range(0, map.tiles.GetLength(1));
                if (map.IsAccessableTile(x, y) == true)
                {
                    item.x = x;
                    item.y = y;
                    map.items.Add(item);
                    found_position = true;
                    map.important_connect_tiles.Add((x, y));

                    int r = UnityEngine.Random.Range(1, 101);
                    if (r <= 5)
                        item.SetQuality(ItemQuality.Unique);
                    else if (r <= 20)
                        item.SetQuality(ItemQuality.Magical2);
                    else if (r <= 50)
                        item.SetQuality(ItemQuality.Magical1);
                }
                ++number_tries;
            }

            if (found_position == false)
            {
                GameLogger.Log("Error: Could not place quest item " + item.GetName() + " in dungeon.");
            }
        }
    }

    public void DistributeMonsters()
    {
        var possible_encounters = encounters.FindAll(x => x.encounter.level_min <= difficulty_level && x.encounter.level_max >= difficulty_level);
        
        int sum_weight = 0;
        foreach(var v in possible_encounters)
            sum_weight += v.weight;

        int num_of_encounters = UnityEngine.Random.Range(number_of_encounters.min, number_of_encounters.max +1 );

        for (int n = 0; n < num_of_encounters; ++ n)
        {
            int encounter_index = UnityEngine.Random.Range(1, sum_weight+1);
            EncounterData encounter = null;
            int counter = 0;
            foreach (var v in possible_encounters)
            {
                if (encounter_index <= counter + v.weight)
                {
                    encounter = v.encounter;
                    break;
                }
                
                counter += v.weight;
            }

            //search for position of encounter
            int number_tries = 0;
            bool found_position = false;
            (int x, int y)? encounter_position = null;
            
            while (found_position == false && number_tries < 1000)
            {
                int x = UnityEngine.Random.Range(0, map.tiles.GetLength(0));
                int y = UnityEngine.Random.Range(0, map.tiles.GetLength(1));
                
                if (map.IsAccessableTile(x, y) == true)
                {
                    bool is_in_forbidden_feature = false;
                    //Don't place monsters near dungeon exits
                    int border_size = 5;
                    foreach(MapFeatureData mfd in map.features)
                    {
                        if (mfd.distribute_general_actors == false &&
                            x >= mfd.position.x - border_size && x <= mfd.position.x + mfd.dimensions.x + border_size&&
                            y >= mfd.position.y - border_size && y <= mfd.position.y + mfd.dimensions.y + border_size)
                        {
                           is_in_forbidden_feature = true;
                        }
                    }          
                    if (is_in_forbidden_feature == false)
                    {
                        encounter_position = (x,y);
                        found_position = true;
                    }
                }
                ++number_tries;
            }
            if (found_position == false)
                return;

            foreach(var type_data in encounter.type_amounts)
            {
                int num_of_monsters = UnityEngine.Random.Range(type_data.amount_min, type_data.amount_max +1 );

                for (int i = 0; i < num_of_monsters; ++i)
                {
                    int monster_level = -1;
                    float random_number = UnityEngine.Random.Range(0.0f,1.0f);
                    if (random_number <= 0.78)
                        monster_level = difficulty_level;
                    else if (random_number <= 0.88)
                        monster_level = Mathf.Max(0,difficulty_level - 1);
                    else if (random_number <= 0.98)
                        monster_level = difficulty_level + 1;
                    else if (random_number <= 0.99)
                        monster_level = Mathf.Max(0,difficulty_level - 2);
                    else
                        monster_level = difficulty_level + 2;          
                    
                    ActorData monster = new MonsterData(-1, -1, (ActorPrototype)Activator.CreateInstance(type_data.type, monster_level));
                    
                    //search for position of monster
                    number_tries = 0;
                    found_position = false;
                    while (found_position == false && number_tries < 1000)
                    {
                        int x = UnityEngine.Random.Range(encounter_position.Value.x - 5, encounter_position.Value.x + 6);
                        int y = UnityEngine.Random.Range(encounter_position.Value.y - 5, encounter_position.Value.y + 6);
                        if (map.IsAccessableTile(x, y) == true)
                        {
                            bool is_in_forbidden_feature = false;
                            //Don't place monsters near dungeon exits
                            int border_size = 5;
                            foreach(MapFeatureData mfd in map.features)
                            {
                                if (mfd.distribute_general_actors == false &&
                                    x >= mfd.position.x - border_size && x <= mfd.position.x + mfd.dimensions.x + border_size&&
                                    y >= mfd.position.y - border_size && y <= mfd.position.y + mfd.dimensions.y + border_size)
                                {
                                    is_in_forbidden_feature = true;
                                }
                            }          
                            if (is_in_forbidden_feature == false)
                            {
                                monster.MoveTo(x,y);                    
                                map.actors.Add(monster);
                                found_position = true;
                            }
                        }
                        ++number_tries;
                    }
                }
            }
        }
    }

    public void DistributeQuestActors(List<ActorData> quest_actors)
    {
        foreach (ActorData monster in quest_actors)
        {
            //search for position of monster
            int number_tries = 0;
            bool found_position = false;
            while (found_position == false && number_tries < 10000)
            {
                int x = UnityEngine.Random.Range(0, map.tiles.GetLength(0));
                int y = UnityEngine.Random.Range(0, map.tiles.GetLength(1));
                if (map.CanBeMovedInByActor(x, y, monster) == true)
                {
                    monster.MoveTo(x,y);                    
                    map.actors.Add(monster);
                    found_position = true;
                    Debug.Log("Placing quest actor " + monster.prototype.name + ".");

                }
                ++number_tries;
            }

            if (found_position == false)
            {
                GameLogger.Log("Error: Could not place quest actor " + monster.prototype.name + " in dungeon.");
            }
        }
    }
}

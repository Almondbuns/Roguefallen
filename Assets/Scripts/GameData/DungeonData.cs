using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class DungeonChangeData
{
    public string name;
    public Type dungeon_change_type;    
    public string dungeon_change_image;
    public string target_dungeon_name = "";
    public string target_entrance_name = "";
    public string target_entrance_parameter = "";

    internal void Save(BinaryWriter save)
    {
        save.Write(name);
        save.Write(dungeon_change_type.Name);
        save.Write(dungeon_change_image);
        save.Write(target_dungeon_name);
        save.Write(target_entrance_name);
        save.Write(target_entrance_parameter);
    }

    internal void Load(BinaryReader save)
    {
        name = save.ReadString();
        dungeon_change_type = Type.GetType(save.ReadString());
        dungeon_change_image = save.ReadString();
        target_dungeon_name = save.ReadString();
        target_entrance_name = save.ReadString();
        target_entrance_parameter = save.ReadString();
    }
}

public class DungeonData
{
    public string name;
    public bool is_persistent = false;
    public long tick_counter = 0;

    public List<DungeonLevelData> dungeon_levels;

    internal void Save(BinaryWriter save)
    {
        save.Write(name);
        save.Write(is_persistent);

        save.Write(dungeon_levels.Count);
        foreach (DungeonLevelData d in dungeon_levels)
            d.Save(save);

        save.Write(tick_counter);

    }

    internal void Load(BinaryReader save)
    {
        name = save.ReadString();
        is_persistent = save.ReadBoolean();

        int size = save.ReadInt32();
        dungeon_levels = new List<DungeonLevelData>(size);
        for (int i = 0; i < size; ++i)
        {
            DungeonLevelData d = new DungeonLevelData();
            d.Load(save);
            dungeon_levels.Add(d);
        }

        tick_counter = save.ReadInt64();
    }



    public DungeonData()
    {
        dungeon_levels = new List<DungeonLevelData>();
    }

    public void SetRegenerationNeeded()
    {
        int counter = 0;
        foreach (DungeonLevelData level_data in dungeon_levels)
        {
            level_data.dungeon_level = counter;
            if (counter == dungeon_levels.Count - 1) // Quest items will always generate at the last level of a dungeon
                level_data.SetRegenerationNeeded(true);
            else
                level_data.SetRegenerationNeeded(false);
            ++counter;
        }
    }
    
    public void RegenerateLevel(DungeonLevelData level)
    {
        List<ItemData> quest_items = null;
        List<ActorData> quest_actors = null;
        if (level.regeneration_quest_goals == true)
        {
            quest_items = CheckQuestItemRequirements();
            quest_actors = CheckQuestActorRequirements();
        }

        level.CreateMapLevel(quest_items, quest_actors);                
    }
    public List<ItemData> CheckQuestItemRequirements()
    {
        GameData game_data = GameObject.Find("GameData").GetComponent<GameData>();

        List<ItemData> quest_items = new();

        foreach(QuestData quest in game_data.player_data.active_quests)
        {
            List<ItemData> items = quest.GetDungeonItems(name);
            if (items != null)
            {
                foreach (var v in items)
                    quest_items.Add(v);
            }
        }

        return quest_items;
    }

    public List<ActorData> CheckQuestActorRequirements()
    {
        GameData game_data = GameObject.Find("GameData").GetComponent<GameData>();

        List<ActorData> quest_actors = new();

        foreach (QuestData quest in game_data.player_data.active_quests)
        {
            List<ActorData> actors = quest.GetDungeonActors(name);
            if (actors != null)
            {
                foreach (var v in actors)
                    quest_actors.Add(v);
            }
        }

        return quest_actors;
    }

    public MapData GetMapData(int z)
    {
        return dungeon_levels[z].map;
    }

    public DungeonLevelData GetMapLevelData(int x)
    {
        return dungeon_levels[x];
    }

    internal DungeonLevelData GetDungeonLevelDataOf(string target_entrance_name)
    {
        foreach(var level in dungeon_levels)
        {
            foreach(DungeonChangeData dcd in level.dungeon_changes)
            {
                if (dcd.name == target_entrance_name)
                {
                    if (level.regeneration_needed == true)
                        RegenerateLevel(level);
                    return level;
                }
            }
        }

        return null;
    }

    public virtual void Tick()
    {
        //Dungeons may implement special time-based features
    }
}
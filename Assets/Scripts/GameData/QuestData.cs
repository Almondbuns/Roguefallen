using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class QuestRewardData
{
    public int xp;
    public int gold;
    public List<ItemData> items;

    public QuestRewardData()
    {
        items = new();
    }

    internal void Save(BinaryWriter save)
    {
        save.Write(xp);
        save.Write(gold);
    
        save.Write(items.Count);
        foreach (var v in items)
            v.Save(save);

    }

    internal void Load(BinaryReader save)
    {
        xp = save.ReadInt32();
        gold = save.ReadInt32();
    
        int size = save.ReadInt32();
        items = new(size);
        for (int i = 0; i < size; ++i)
        {
            ItemData v = new(null);
            v.Load(save);
            items.Add(v);
        }
    }
}

public enum QuestComplexity
{
    Short,
    Medium,
    Long
}

public abstract class QuestData 
{
    public static long id_counter = 0;
    public long id;

    public int difficulty_level;
    public QuestComplexity complexity_level;

    public QuestRewardData reward;
    public List<QuestMissionData> missions;

    public string name;
    public string start_quest_dialog;
    public string end_quest_dialog;

    public QuestData()
    {
        missions = new();
        reward = new();

        id = id_counter;
        ++id_counter;
    }

    internal void Save(BinaryWriter save)
    {
        save.Write(id);
        save.Write(name);
        save.Write(difficulty_level);
        save.Write((int) complexity_level);
        save.Write(start_quest_dialog);
        save.Write(end_quest_dialog);

        reward.Save(save);

        save.Write(missions.Count);
        foreach (var v in missions)
        {
            save.Write(v.GetType().Name);
            v.Save(save);
        }

    }

    internal void Load(BinaryReader save)
    {
        id = save.ReadInt64();
        name = save.ReadString();
        difficulty_level = save.ReadInt32();
        complexity_level = (QuestComplexity) save.ReadInt32();
        start_quest_dialog = save.ReadString();
        end_quest_dialog = save.ReadString();

        reward.Load(save);

        int size = save.ReadInt32();
        missions = new(size);
        for (int i = 0; i < size; ++i)
        {
            Type a_type = Type.GetType(save.ReadString());
            QuestMissionData v = (QuestMissionData)Activator.CreateInstance(a_type);
            v.Load(save);
            missions.Add(v);
        }
    }

    public abstract void GenerateQuest(int difficulty, QuestComplexity complexity);

    internal List<ItemData> GetDungeonItems(string name)
    {
        List<ItemData> quest_items = new();
        foreach (QuestMissionData mission in missions)
        {
            if (mission.location == name)
            {
                List<ItemData> mission_items = mission.GetQuestItems();
                if (mission_items != null)
                {
                    foreach (ItemData item in mission_items)
                        quest_items.Add(item);
                }
            }
        }

        return quest_items;
    }

    internal List<ActorData> GetDungeonActors(string name)
    {
        List<ActorData> quest_actors = new();
        foreach (QuestMissionData mission in missions)
        {
            if (mission.location == name)
            {
                List<ActorData> mission_actors = mission.GetQuestActors();
                if (mission_actors != null)
                {
                    foreach (ActorData item in mission_actors)
                        quest_actors.Add(item);
                }
            }
        }

        return quest_actors;
    }

    internal void OnItemPickup(ItemData item)
    {
        foreach(QuestMissionData mission in missions)
        {
            mission.OnItemPickup(item);
        }

        bool completed = IsCompleted();
        if (completed == true)
        {
            GameLogger.Log("The Player completed the quest: " + name + "!");
        }
    }

    internal void OnActorKill(ActorData actor)
    {
        foreach (QuestMissionData mission in missions)
        {
            mission.OnActorKill(actor);
        }

        bool completed = IsCompleted();
        if (completed == true)
        {
            GameLogger.Log("The Player completed the quest: " + name + "!");
        }
    }

    internal void OnLocationChange(string location)
    {
        foreach (QuestMissionData mission in missions)
        {
            mission.OnLocationChange(location);
        }

        bool completed = IsCompleted();
    }

    public bool IsCompleted()
    {

        foreach (QuestMissionData mission in missions)
        {
            bool is_completed = mission.IsCompleted();
            if (is_completed == false)
                return false;
        }

        GameLogger.Log("The Player completed the quest: " + name + "!");
        UIStateQuestEndDialog quest_state = new UIStateQuestEndDialog(this);
        GameObject.Find("UI").GetComponent<UI>().AddUIState(quest_state);
       
        return true;
    }
}

public class QDFetchItem : QuestData
{
    public override void GenerateQuest(int difficulty, QuestComplexity complexity)
    {
        name = "Fetch the guinea pig";
        this.difficulty_level = difficulty;
        this.complexity_level = complexity;

        reward.gold = difficulty_level * 250;

        QMDFetchItems mission = new QMDFetchItems();
        ItemData guinea_pig = new ItemData(new ItemGuineaPig(1))
        {
            is_quest_item = true,
            quest_id = id,
        };

        mission.items.Add((false, guinea_pig));

        GameData game_data = GameObject.Find("GameData").GetComponent<GameData>();
        int random_dungeon_index = 1; // UnityEngine.Random.Range(0, game_data.dungeons.Count);
        mission.location = game_data.dungeons[random_dungeon_index].name;
        missions.Add(mission);

        QMDBeInLocation mission2 = new();
        mission2.location = game_data.dungeons[0].name;
        missions.Add(mission2);

        start_quest_dialog = "I need someone to fetch something for me.<br> <br>I lost my <color=white> guinea pig </color> deep within <color=red>" + mission.location + "</color>.<br> <br>I am deeply afraid of <color=green>spiders</color>. So I cannot get it myself.<br> <br>Are you going to help me?";

        end_quest_dialog = "You did it! Thank the gods that my guinea pig is alright. You surely deserve your reward! Thank you for all your troubles.";
    }
}

public class QDKillMonster : QuestData
{
    public override void GenerateQuest(int difficulty, QuestComplexity complexity)
    {
        name = "Kill the Lost Explorer";
        this.difficulty_level = difficulty;
        this.complexity_level = complexity;

        reward.gold = difficulty_level * 250;

        QMDKillMonsters mission = new QMDKillMonsters();
        MonsterData monster = new MonsterData(-1,-1, new LostExplorer(1))
        {
            is_quest_actor = true,
            quest_id = id,
        };

        mission.monsters.Add((false, monster));

        GameData game_data = GameObject.Find("GameData").GetComponent<GameData>();
        int random_dungeon_index = 1; // UnityEngine.Random.Range(0, game_data.dungeons.Count);
        mission.location = game_data.dungeons[random_dungeon_index].name;
        missions.Add(mission);

        QMDBeInLocation mission2 = new();
        mission2.location = game_data.dungeons[0].name;
        missions.Add(mission2);

        start_quest_dialog = "I need a sword for hire.<br> <br>The evil creature <color=white>The Lost Explorer</color> wanders around in <color=red>" + mission.location + "</color>.<br> <br>It took a lot of lifes and needs to be killed.<br> <br>Are you going to help us?";

        end_quest_dialog = "You killed the creature. You surely deserve your reward! Thank you for all your troubles.";
    }
}
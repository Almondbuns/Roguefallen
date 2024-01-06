using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        int random_dungeon_index = UnityEngine.Random.Range(0, game_data.dungeons.Count);
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
        int random_dungeon_index = UnityEngine.Random.Range(0, game_data.dungeons.Count);
        mission.location = game_data.dungeons[random_dungeon_index].name;
        missions.Add(mission);

        QMDBeInLocation mission2 = new();
        mission2.location = game_data.dungeons[0].name;
        missions.Add(mission2);

        start_quest_dialog = "I need a sword for hire.<br> <br>The evil creature <color=white>The Lost Explorer</color> wanders around in <color=red>" + mission.location + "</color>.<br> <br>It took a lot of lifes and needs to be killed.<br> <br>Are you going to help us?";

        end_quest_dialog = "You killed the creature. You surely deserve your reward! Thank you for all your troubles.";
    }
}

public class QDMain : QuestData
{
    public override void GenerateQuest(int difficulty, QuestComplexity complexity)
    {
        name = "Main Quest";
        this.difficulty_level = difficulty;
        this.complexity_level = complexity;

        reward.gold = 5000;
        reward.xp = 5000;

        QMDFetchItems mission = new QMDFetchItems();
        ItemData family_symbol = new ItemData(new FamilySymbol(1))
        {
            is_quest_item = true,
            quest_id = id,
        };

        mission.items.Add((false, family_symbol));

        GameData game_data = GameObject.Find("GameData").GetComponent<GameData>();
        int random_dungeon_index = 1; // UnityEngine.Random.Range(0, game_data.dungeons.Count);
        mission.location = game_data.dungeons[random_dungeon_index].name;
        missions.Add(mission);

        QMDBeInLocation mission2 = new();
        mission2.location = game_data.dungeons[0].name;
        missions.Add(mission2);

        start_quest_dialog = "You will find the family symbol in <color=red>" + mission.location + "</color>. Good Luck!";

        end_quest_dialog = "You killed the creature. You surely deserve your reward! Thank you for all your troubles.";
    }
}
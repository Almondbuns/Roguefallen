using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;


public class QDFetchItem : QuestData
{
    public override void GenerateQuest(int difficulty, QuestComplexity complexity)
    {
        name = "Fetch the guinea pig";
        type = QuestType.SideQuest;
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
        mission.journal_description = "Retrieve the guinea pig from the " + mission.location + ".";

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
        type = QuestType.SideQuest;
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
        mission.journal_description = "Kill the Lost Explorer in the " + mission.location + ".";
        missions.Add(mission);
        

        QMDBeInLocation mission2 = new();
        mission2.location = game_data.dungeons[0].name;
        mission2.journal_description = "Come back to " + mission2.location + ".";
        missions.Add(mission2);
        

        start_quest_dialog = "I need a sword for hire.<br> <br>The evil creature <color=white>The Lost Explorer</color> wanders around in <color=red>" + mission.location + "</color>.<br> <br>It took a lot of lifes and needs to be killed.<br> <br>Are you going to help us?";

        end_quest_dialog = "You killed the creature. You surely deserve your reward! Thank you for all your troubles.";
    }
}

public class QDMain : QuestData
{
    public override void GenerateQuest(int difficulty, QuestComplexity complexity)
    {
        GameData game_data = GameObject.Find("GameData").GetComponent<GameData>();

        name = "Main Quest";
        type = QuestType.MainQuest;
        this.difficulty_level = difficulty;
        this.complexity_level = complexity;

        reward.gold = 5000;
        reward.xp = 5000;

        QMDTalkToNPCs mission_1 = new();
        mission_1.location = game_data.dungeons[1].name;
        mission_1.journal_description = "Talk to the village elder in " + mission_1.location + ".";
        mission_1.npc = new();
        MonsterData questgiver = new MonsterData(-1,-1, new Questgiver1(50))
        {
            is_quest_actor = true,
            quest_id = id,
        };

        DialogueData dialogue = new();
        dialogue.dialogue.Add((
            "Welcome, brave soul.<br><br>Our land is under a shadow, a creeping darkness that threatens to consume us. In our direst hour, we look to the past for salvation.<br><br>Deep within the Whispering Caves lies a relic of great importance – an ancient shield bearing the Medallion of the royal family, once ruled by the queen who now dwells in madness. This symbol holds the power to counter her dark influence.",
            "A shield from a fallen queen's lineage? What makes you think I can retrieve it?"
        ));
        dialogue.dialogue.Add((
            "I've heard tales of your deeds, your strength and courage.<br><br>The caves you must venture into are fraught with danger, not just from creatures corrupted by darkness but also riddled with traps left from a time long past.<br><br>Your skills in combat and keen awareness will be crucial in navigating these perils.",
            "Traps and dark creatures, then. What is the reward?"
        ));
        dialogue.dialogue.Add((
            "Recovering the shield is a feat that will earn you great honor and wealth from our village.<br><br>But more than that, you will be wielding a piece of history, a key to saving our realm from a darkness that grows stronger each day. Your name will be sung alongside the heroes of old.",
            "I'll take on this task. For the shield, the reward, and to put an end to this creeping darkness."
        ));

        mission_1.npc.Add((false,questgiver,dialogue));
        missions.Add(mission_1);

        QMDFetchItems mission_2 = new QMDFetchItems();
        ItemData family_symbol = new ItemData(new FamilySymbol(1))
        {
            is_quest_item = true,
            quest_id = id,
        };

        mission_2.items.Add((false, family_symbol));

        int random_dungeon_index = 1; // UnityEngine.Random.Range(0, game_data.dungeons.Count);
        mission_2.location = game_data.dungeons[random_dungeon_index].name;
        mission_2.journal_description = "Retrieve the family symbol from the " + mission_2.location + ".";
        missions.Add(mission_2);

        QMDBeInLocation mission_3 = new();
        mission_3.location = game_data.dungeons[0].name;
        mission_3.journal_description = "Come back to " + mission_3.location + ".";
        missions.Add(mission_3);

        start_quest_dialog = "You will find the family symbol in <color=red>" + mission_2.location + "</color>. Good Luck!";

        end_quest_dialog = "You killed the creature. You surely deserve your reward! Thank you for all your troubles.";
    }
}
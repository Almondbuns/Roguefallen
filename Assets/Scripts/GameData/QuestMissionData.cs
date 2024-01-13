using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public abstract class QuestMissionData
{
    public string location;
    public string journal_description;

    internal virtual void Save(BinaryWriter save)
    {
        save.Write(location);
        save.Write(journal_description);
    }

    internal virtual void Load(BinaryReader save)
    {
        location = save.ReadString();
        journal_description = save.ReadString();
    }

    public virtual void OnItemPickup(ItemData item)
    {
    }

    public virtual void OnActorKill(ActorData actor)
    {
    }

    public virtual void OnLocationChange(string location)
    {
    }

    public virtual bool OnStopPlayerMovement(ActorData actor)
    {
        return false;
    }

    internal virtual List<ItemData> GetQuestItems()
    {
        return null;
    }

    internal virtual List<ActorData> GetQuestActors()
    {
        return null;
    }

    internal virtual bool IsCompleted()
    {
        return true;
    }
}

public class QMDFetchItems : QuestMissionData
{
    public List<(bool in_possession, ItemData item)> items;

    public QMDFetchItems()
    {
        items = new();
    }

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(items.Count);
        foreach (var v in items)
        {
            save.Write(v.in_possession);
            v.item.Save(save);
        }
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        int size = save.ReadInt32();
        items = new(size);
        for (int i = 0; i < size; ++i)
        {
            bool in_posession = save.ReadBoolean();
            ItemData v = new(null);
            v.Load(save);
            items.Add((in_posession, v));
        }
    }

    public override void OnItemPickup(ItemData item)
    {
        for (int i = 0; i < items.Count; ++i)
        {
            if (items[i].in_possession == false)
            {
                if (item.id == items[i].item.id)
                {
                    items[i] = (true, items[i].item);
                    GameLogger.Log("You completed part of a quest.");
                }
            }
        }
    }

    internal override List<ItemData> GetQuestItems()
    {
        List<ItemData> questitems = new();
        foreach (var v in items)
            questitems.Add(v.item);
        return questitems;
    }

    internal override bool IsCompleted()
    {
        foreach (var v in items)
        {
            if (v.in_possession == false)
                return false;
        }

        return true;
    }
}

public class QMDBeInLocation : QuestMissionData
{
    public QMDBeInLocation()
    {
    }

    internal override bool IsCompleted()
    {
        if (GameObject.Find("GameData").GetComponent<GameData>().current_dungeon.name == location)
            return true;

        else
            return false;
    }
}

public class QMDKillMonsters: QuestMissionData
{
    public List<(bool is_killed, ActorData monster)> monsters;

    public QMDKillMonsters()
    {
        monsters = new();
    }

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(monsters.Count);
        foreach (var v in monsters)
        {
            save.Write(v.is_killed);
            //v.monster.Save(save);
        }
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        int size = save.ReadInt32();
        monsters = new(size);
        for (int i = 0; i < size; ++i)
        {
            bool is_killed = save.ReadBoolean();
            //ActorData v = new(null);
            //v.Load(save);
            //monsters.Add((is_killed, v));
        }
    }

    public override void OnActorKill(ActorData actor)
    {
        for (int i = 0; i < monsters.Count; ++i)
        {
            if (monsters[i].is_killed == false)
            {
                if (actor.id == monsters[i].monster.id)
                {
                    monsters[i] = (true, monsters[i].monster);
                    GameLogger.Log("You completed part of a quest.");
                }
            }
        }
    }

    internal override List<ActorData> GetQuestActors()
    {
        List<ActorData> questmonsters = new();
        foreach (var v in monsters)
            questmonsters.Add(v.monster);
        return questmonsters;
    }

    internal override bool IsCompleted()
    {
        foreach (var v in monsters)
        {
            if (v.is_killed == false)
                return false;
        }

        return true;
    }
}

public class DialogueData
{
    public List<(string npc, string player)> dialogue;

    public DialogueData()
    {
        dialogue = new();
    }

    public void Save(BinaryWriter save)
    {
        save.Write(dialogue.Count);
        foreach (var v in dialogue)
        {
            save.Write(v.npc);
            save.Write(v.player);
        }
    }

    public void Load(BinaryReader save)
    {
        int size = save.ReadInt32();
        dialogue = new(size);
        for (int i = 0; i < size; ++i)
        {
            string npc = save.ReadString();
            string player = save.ReadString();
            dialogue.Add((npc,player));
        }
    }
}
public class QMDTalkToNPCs: QuestMissionData
{
    public List<(bool is_talked_to, ActorData npc, DialogueData dialogue)> npc;

    public QMDTalkToNPCs()
    {
        npc = new();
    }

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(npc.Count);
        foreach (var v in npc)
        {
            save.Write(v.is_talked_to);
            //v.monster.Save(save);
            v.dialogue.Save(save);
        }
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        int size = save.ReadInt32();
        npc = new(size);
        for (int i = 0; i < size; ++i)
        {
            bool is_talked_to = save.ReadBoolean();
            //ActorData v = new(null);
            //v.Load(save);
            //monsters.Add((is_killed, v));
            DialogueData dialogue = new();
            dialogue.Load(save);
            npc.Add((is_talked_to,null,dialogue));
        }
    }

    internal override bool IsCompleted()
    {
        foreach (var v in npc)
        {
            if (v.is_talked_to == false)
                return false;
        }

        return true;
    }

    internal override List<ActorData> GetQuestActors()
    {
        List<ActorData> npcs = new();
        foreach (var v in npc)
            npcs.Add(v.npc);
        return npcs;
    }

    public override bool OnStopPlayerMovement(ActorData actor)
    {
        for (int i = 0; i < npc.Count; ++i)
        {
            if (actor.id == npc[i].npc.id)
            {
                if (npc[i].is_talked_to == false)
                {
                    npc[i] = (true, npc[i].npc, npc[i].dialogue);
                    GameObject.Find("UI").GetComponent<UI>().ActivateQuestDialogue(npc[i].dialogue);

                    GameLogger.Log("You completed part of a quest.");
                    return true;
                }
                else
                {
                    GameLogger.Log("You already spoke to that person.");
                    return true;
                }
            }
        }
        return false;
    }
}
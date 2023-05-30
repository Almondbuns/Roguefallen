using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum ActionType
{
    UNKNOWN,
    MOVEMENT
}

public enum ActionState
{
    UNKNOWN,
    PREPARE,
    ACTION,
    RECOVER,
    FINISHED
}

public class ActionData
{
    public string name = "";
    public string icon = "";
    public string prepare_message = "";
    public string action_message = "";
    public bool log = true;
    public bool show_on_map = true;
    public ActionType type;
    public int prepare_time;
    public int recover_time;

    public int current_tick = 0;
    public bool regain_stamina = false;
    public ActionState state = ActionState.UNKNOWN;

    public List<CommandData> commands;


    internal void Save(BinaryWriter save)
    {
        save.Write(name);
        save.Write(icon);
        save.Write(prepare_message);
        save.Write(action_message);
        save.Write(log);
        save.Write(show_on_map);
        save.Write((int) type);
        save.Write(prepare_time);
        save.Write(recover_time);
        save.Write(current_tick);
        save.Write(regain_stamina);
        save.Write((int) state);

        save.Write(commands.Count);
        foreach (var v in commands)
        {
            save.Write(v.GetType().Name);
            v.Save(save);
        }
    }

    internal void Load(BinaryReader save)
    {
        name = save.ReadString();
        icon = save.ReadString();
        prepare_message = save.ReadString();
        action_message = save.ReadString();
        log = save.ReadBoolean();
        show_on_map = save.ReadBoolean();
        type = (ActionType)save.ReadInt32();
        prepare_time = save.ReadInt32();
        recover_time = save.ReadInt32();
        current_tick = save.ReadInt32();
        regain_stamina = save.ReadBoolean();
        state = (ActionState)save.ReadInt32();

        int size = save.ReadInt32();
        commands = new(size);
        for (int i = 0; i < size; ++i)
        {
            Type type = Type.GetType(save.ReadString());
            CommandData c = (CommandData) Activator.CreateInstance(type);
            c.Load(save);
            commands.Add(c);
            Debug.Log(type.ToString());
        }
    }

    public ActionData()
    {
        commands = new List<CommandData>();
    }

    public ActionData(TalentData talent)
    {
        commands = new List<CommandData>();
        name = talent.prototype.name;
        icon = talent.prototype.icon;
        prepare_message = talent.prototype.prepare_message;
        action_message = talent.prototype.action_message;
        prepare_time = talent.prototype.prepare_time;
        recover_time = talent.prototype.recover_time;
    }

    public float Tick()
    {
        ++current_tick; // Starts with 1 !
            
        float wait_time = 0;
        long calculated_tick = 0;

        if (current_tick <= prepare_time)
        {
            state = ActionState.PREPARE;
            return wait_time;
        }

        calculated_tick += prepare_time;

        foreach (CommandData command in commands)
        {          
            if (current_tick <= calculated_tick + command.duration)
            {
                wait_time += command.Execute();
                state = ActionState.ACTION;
                return wait_time;
            }

            calculated_tick += command.duration;
        }

        if (recover_time > 0)
            state = ActionState.RECOVER;

        if (HasFinished())
            state = ActionState.FINISHED;

        return wait_time;
    }

    public int GetMaxTicks()
    {
        int max_ticks = 0;
        max_ticks += prepare_time;
        foreach (CommandData command in commands)
            max_ticks += command.duration;
        max_ticks += recover_time;

        return max_ticks;
    }

    public bool HasFinished()
    {
        if (current_tick >= GetMaxTicks())
            return true;

        return false;
    }

    
    public int GetTicksToFinish()
    {
        return GetMaxTicks() - current_tick;
    }

    public int GetTicksToPrepare()
    {
        if (current_tick >= prepare_time)
            return 0;

        return prepare_time - current_tick;
    }

    public int GetTicksToRecover()
    {
        return Mathf.Min(recover_time, (Mathf.Max(0, GetMaxTicks() - current_tick)));
    }
}

public class MoveAction : ActionData
{
    public MoveAction()
    {

    }

    public MoveAction(ActorData actor, int destination_x, int destination_y, int duration)
    {
        name = "Movement (" + (destination_x + 1) + "/" + (destination_y + 1)+ ")";
        icon = "images/actions/movement";
        type = ActionType.MOVEMENT;
        log = false;
        prepare_time = 0;
        recover_time = duration - 1;
        commands.Add(new MoveCommand(actor, destination_x, destination_y, 1));
        regain_stamina = true;
        show_on_map = false;
    }
}

public class MoveAndExplodeAction : ActionData
{
    public MoveAndExplodeAction()
    {

    }

    public MoveAndExplodeAction(ActorData actor, int destination_x, int destination_y, int duration, int explosion_radius, List<(DamageType type, int amount, int penetration)> damage, bool explosion_on_impact)
    {
        name = "Explode Movement (" + (destination_x + 1) + "/" + (destination_y + 1)+ ")";
        icon = "images/actions/movement";
        type = ActionType.MOVEMENT;
        log = false;
        prepare_time = 0;
        recover_time = duration - 1;
        commands.Add(new MoveCommand(actor, destination_x, destination_y, 1, true));
        commands.Add(new ExplodeCommand(actor,explosion_radius, damage, explosion_on_impact));
        regain_stamina = true;
        show_on_map = false;
    }
}

public class ExplodeAction : ActionData
{
    public ExplodeAction()
    {

    }

    public ExplodeAction(ActorData actor, int explosion_radius, List<(DamageType type, int amount, int penetration)> damage, bool explosion_on_impact)
    {
        name = "Explode";
        icon = "images/actions/movement";
        type = ActionType.MOVEMENT;
        log = false;
        prepare_time = 0;
        recover_time = 0;
        commands.Add(new ExplodeCommand(actor,explosion_radius, damage, explosion_on_impact));
        regain_stamina = true;
        show_on_map = false;
    }
}

public class ChangeDungeonAction : ActionData
{
    public ChangeDungeonAction()
    {

    }

    public ChangeDungeonAction(DungeonChangeData region_change_data, ActorData actor, int duration)
    {
        icon = "images/actions/movement";
        log = false;
        prepare_time = duration;
        commands.Add(new ChangeDungeonCommand(region_change_data, actor));
        recover_time = 0;
        show_on_map = false;
    }
}

public class WaitAction : ActionData
{
    public WaitAction()
    {

    }
    public WaitAction(int duration)
    {
        icon = "images/actions/sleep";
        name = "Wait";
        log = false;
        prepare_time = 0;
        recover_time = 0;
        commands.Add(new WaitCommand(duration));
        regain_stamina = true;
        show_on_map = false;
    }
}

public class CollectItemAction : ActionData
{
    public CollectItemAction()
    {

    }
    public CollectItemAction(ItemData item, int duration)
    {
        icon = "images/talents/pickup";
        log = false;
        prepare_time = duration / 2 - 1;
        commands.Add(new CollectItemCommand(item));
        recover_time = duration / 2;
    }
}

public class ConsumeItemAction : ActionData
{
    public ConsumeItemAction()
    {

    }
    public ConsumeItemAction(ItemData item, int duration)
    {
        name = "Consume " + item.GetName();
        icon = item.GetPrototype().icon;
        log = true;
        prepare_time = duration / 2 - 1;
        commands.Add(new ConsumeItemCommand(item));
        recover_time = duration / 2;

        prepare_message = "The <name> starts consuming " + item.GetName()+ ".";
        action_message = "The <name> consumes " + item.GetName() + ".";
    }
}

public class UseItemAction : ActionData
{
    public UseItemAction()
    {

    }
    public UseItemAction(ItemData item, int duration)
    {
        name = "Use " + item.GetName();
        icon = item.GetPrototype().icon;
        log = true;
        prepare_time = duration / 2 - 1;
        commands.Add(new UseItemCommand(item));
        recover_time = duration / 2;

        prepare_message = "The <name> starts using " + item.GetName() + ".";
        action_message = "The <name> uses " + item.GetName() + ".";
    }
}

public class DropItemAction : ActionData
{
    public DropItemAction()
    {

    }

    public DropItemAction(ItemData item, int duration, int amount = -1)
    {
        name = "Drop " + item.GetName();
        icon = item.GetPrototype().icon;
        log = true;
        prepare_time = duration / 2 - 1;
        commands.Add(new DropItemCommand(item, amount));
        recover_time = duration / 2;

        prepare_message = "The <name> starts dropping " + item.GetName() + ".";
        action_message = "The <name> drops " + item.GetName() + ".";
    }
}
public class EquipItemAction : ActionData
{
    public EquipItemAction()
    {

    }
    public EquipItemAction(PlayerData player, int inventory_index, int equipment_index, int duration)
    {
        name = "Equip " + player.inventory.slots[inventory_index].item.GetName();
        icon = player.inventory.slots[inventory_index].item.GetPrototype().icon;
        log = true;
        prepare_time = duration / 2 - 1;
        commands.Add(new EquipItemCommand(inventory_index, equipment_index));
        recover_time = duration / 2;

        prepare_message = "The <name> starts equipping " + player.inventory.slots[inventory_index].item.GetName() + ".";
        action_message = "The <name> equips " + player.inventory.slots[inventory_index].item.GetName() + ".";
    }
}

public class UnequipItemAction : ActionData
{
    public UnequipItemAction()
    {

    }
    public UnequipItemAction(PlayerData player, int inventory_index, int equipment_index, int duration)
    {
        name = "Unequip " + player.equipment[equipment_index].item.GetName();
        icon = player.equipment[equipment_index].item.GetPrototype().icon;
        log = true;
        prepare_time = duration / 2 - 1;
        commands.Add(new UnequipItemCommand(inventory_index, equipment_index));
        recover_time = duration / 2;

        prepare_message = "The <name> starts unequipping " + player.equipment[equipment_index].item.GetName() + ".";
        action_message = "The <name> unequips " + player.equipment[equipment_index].item.GetName() + ".";
    }
}
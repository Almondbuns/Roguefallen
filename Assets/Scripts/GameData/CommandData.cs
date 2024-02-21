using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using UnityEngine.XR;
using System.Drawing;

public abstract class CommandData
{
    public int duration = 1;
    public abstract float Execute();  // returns UI wait time

    internal virtual void Save(BinaryWriter save)
    {
        save.Write(duration);
    }

    internal virtual void Load(BinaryReader save)
    {
        duration = save.ReadInt32();
    }
}

public class WaitCommand : CommandData
{
    public WaitCommand()
    {

    }

    public WaitCommand(int duration)
    {
        this.duration = duration;
    }

    public override float Execute()
    {
        return 0;
    }
}

public class MoveCommand : CommandData
{
    public int destination_x;
    public int destination_y;
    public bool ignore_blocked_tiles;

    public long actor_id;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(destination_x);
        save.Write(destination_y);
        save.Write(ignore_blocked_tiles);
        save.Write(actor_id);
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        destination_x = save.ReadInt32();
        destination_y = save.ReadInt32();
        ignore_blocked_tiles = save.ReadBoolean();

        actor_id = save.ReadInt64();
    }

    public MoveCommand()
    {

    }

    public MoveCommand(ActorData actor, int destination_x, int destination_y, int duration, bool ignore_blocked_tiles = false)
    {
        this.actor_id = actor.id;
        this.destination_x = destination_x;
        this.destination_y = destination_y;
        this.duration = duration;
        this.ignore_blocked_tiles = ignore_blocked_tiles;
    }

    public override float Execute()
    {
        MapData map_data = GameObject.Find("GameData").GetComponent<GameData>().current_map;
        ActorData actor = map_data.GetActor(actor_id);
      
        if (actor.current_effects.FindAll(x => x.effect is EffectStun).Count > 0)
        {
            GameLogger.Log("The " + actor.prototype.name + " is stunned and cannot move.");
            return 0;
        }

        if (actor.GetCurrentAdditiveEffectAmount<EffectConfusion>() > 0)
        {
            GameLogger.Log("The " + actor.prototype.name + " is confused and runs into a random direction.");
            destination_x = actor.X + UnityEngine.Random.Range(-1,2);
            destination_y = actor.Y + UnityEngine.Random.Range(-1,2);
        }

        if (ignore_blocked_tiles == false && map_data.CanBeMovedInByActor(destination_x, destination_y,actor) == false)
            return 0;


        map_data.Move(actor_id, destination_x, destination_y);

        return 0.00f;
    }
}

public class KnockbackCommand : CommandData
{
    public int target_tile_x;
    public int target_tile_y;
    public int distance;

    public long src_actor_id;
    public (DamageType, int, int) push_damage;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(target_tile_x);
        save.Write(target_tile_y);
        save.Write(distance);
        save.Write(src_actor_id);
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        target_tile_x = save.ReadInt32();
        target_tile_y = save.ReadInt32();
        distance = save.ReadInt32();
        src_actor_id = save.ReadInt64();
    }

    public KnockbackCommand(ActorData src_actor, (int x,int y) target_tile, int distance, (DamageType, int, int) push_damage)
    {
        this.src_actor_id = src_actor.id;
        this.target_tile_x = target_tile.x;
        this.target_tile_y = target_tile.y;
        this.distance = distance;
        this.push_damage = push_damage;
    }

    public override float Execute()
    {        
        MapData map_data = GameObject.Find("GameData").GetComponent<GameData>().current_map;

        ActorData src_actor = map_data.GetActor(src_actor_id);
        ActorData target_actor = map_data.GetActorOnTile(target_tile_x, target_tile_y);

        if (target_actor == null) return 0f;

        int push_direction_x = target_tile_x - src_actor.X;
        int push_direction_y = target_tile_y - src_actor.Y;

        int push_tile_x = target_tile_x + push_direction_x;
        int push_tile_y = target_tile_y + push_direction_y;

        int current_distance = 1;
        bool is_pushed = false;
        while (current_distance <= this.distance)
        {
            //If no actor is on push tile push the target. If there is an actor on push tile, damage it instead

            ActorData push_tile_actor = map_data.GetActorOnTile(push_tile_x, push_tile_y);
            if (push_tile_actor == null)
            {
                if (map_data.CanBeMovedInByActor(push_tile_x, push_tile_y, target_actor))
                {
                    map_data.Move(target_actor.id, push_tile_x, push_tile_y);
                    is_pushed= true;
                }
                else
                {
                    break;
                }
            }
            else
            {
                GameLogger.Log("The " + target_actor.prototype.name + " is pushed into the " + push_tile_actor.prototype.name + ".");
                var damage = new List<(DamageType, int, int)> { push_damage };
                var effects = new List<EffectData> { };
                push_tile_actor.TryToHit(target_actor,100, damage, effects);
                break;
            }

            push_tile_x += push_direction_x;
            push_tile_y += push_direction_y;
            current_distance += 1;
        }

        if (is_pushed == true)
            target_actor.AddEffect(new EffectInterrupt { damage_type = DamageType.CRUSH });

        //if (src_actor is PlayerData)
        //    map_data.UpdateVisibility();

        return 0.50f;
    }
}

public class PullCommand : CommandData
{
    public int target_tile_x;
    public int target_tile_y;
    public int distance;

    public long src_actor_id;
    public (DamageType, int, int) pull_damage;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(target_tile_x);
        save.Write(target_tile_y);
        save.Write(distance);
        save.Write(src_actor_id);
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        target_tile_x = save.ReadInt32();
        target_tile_y = save.ReadInt32();
        distance = save.ReadInt32();
        src_actor_id = save.ReadInt64();
    }

    public PullCommand(ActorData src_actor, (int x,int y) target_tile, int distance, (DamageType, int, int) pull_damage)
    {
        this.src_actor_id = src_actor.id;
        this.target_tile_x = target_tile.x;
        this.target_tile_y = target_tile.y;
        this.distance = distance;
        this.pull_damage = pull_damage;
    }

    public override float Execute()
    {        
        MapData map_data = GameObject.Find("GameData").GetComponent<GameData>().current_map;

        ActorData src_actor = map_data.GetActor(src_actor_id);
        ActorData target_actor = map_data.GetActorOnTile(target_tile_x, target_tile_y);

        if (target_actor == null) return 0f;

        //Pull as long as distance gets smaller and distance < max_distance

        int pull_direction_x = 0;
        if (target_actor.X < src_actor.X - 1)
            pull_direction_x = 1;
        if (target_actor.X > src_actor.X + 1)
            pull_direction_x = -1;

        int pull_direction_y = 0;
        if (target_actor.Y < src_actor.Y - 1)
            pull_direction_y = 1;
        if (target_actor.Y > src_actor.Y + 1)
            pull_direction_y = -1;       

        int pull_tile_x = target_actor.X + pull_direction_x;
        int pull_tile_y = target_actor.Y + pull_direction_y;

        int current_distance = 1;
        bool is_pulled = false;
        while (current_distance <= this.distance && (pull_direction_x != 0 || pull_direction_y !=0))
        {
            ActorData pull_tile_actor = map_data.GetActorOnTile(pull_tile_x, pull_tile_y);
            if (pull_tile_actor == null)
            {
                if (map_data.IsAccessableTile(pull_tile_x, pull_tile_y))
                {
                    map_data.Move(target_actor.id, pull_tile_x, pull_tile_y);
                    is_pulled= true;
                }
                else
                {
                    break;
                }
            }
            else
            {
                GameLogger.Log("The " + target_actor.prototype.name + " is pulled into the " + pull_tile_actor.prototype.name + ".");
                var damage = new List<(DamageType, int, int)> { pull_damage };
                var effects = new List<EffectData> { };
                pull_tile_actor.TryToHit(target_actor, 100, damage, effects);
                break;
            }

            pull_direction_x = 0;
            if (target_actor.X < src_actor.X - 1)
                pull_direction_x = 1;
            if (target_actor.X > src_actor.X + 1)
                pull_direction_x = -1;

            pull_direction_y = 0;
            if (target_actor.Y < src_actor.Y - 1)
                pull_direction_y = 1;
            if (target_actor.Y > src_actor.Y + 1)
                pull_direction_y = -1;       

            pull_tile_x += pull_direction_x;
            pull_tile_y += pull_direction_y;
            current_distance += 1;
        }

        if (is_pulled == true)
            target_actor.AddEffect(new EffectInterrupt { damage_type = DamageType.CRUSH });

        //if (src_actor is PlayerData || target_actor is PlayerData)
        //    map_data.UpdateVisibility();

        return 0.50f;
    }
}

public class HealActorCommand : CommandData
{
    public long actor_id;
    public int amount;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);
        
        save.Write(actor_id);
        save.Write(amount);
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        actor_id = save.ReadInt64();
        amount = save.ReadInt32();
    }

    public HealActorCommand()
    {

    }
    public HealActorCommand(long actor_id, int amount)
    {
        this.actor_id = actor_id;
        this.amount = amount;
    }

    public override float Execute()
    {
        MapData map_data = GameObject.Find("GameData").GetComponent<GameData>().current_map;
        ActorData actor = map_data.GetActor(actor_id);

        actor.Heal(amount);
        return 0.00f;
    }
}

public class StealLifeCommand : CommandData
{
    public long src_actor_id;
    public int target_tile_x;
    public int target_tile_y;
    public int amount;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);
        
        save.Write(src_actor_id);
        save.Write(target_tile_x);
        save.Write(target_tile_y);
        save.Write(amount);
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        src_actor_id = save.ReadInt64();
        target_tile_x = save.ReadInt32();
        target_tile_y = save.ReadInt32();
        amount = save.ReadInt32();
    }

    public StealLifeCommand()
    {

    }
    public StealLifeCommand(long src_actor_id, int target_tile_x, int target_tile_y, int amount)
    {
        this.src_actor_id = src_actor_id;
        this.target_tile_x = target_tile_x;
        this.target_tile_y = target_tile_y;
        this.amount = amount;
    }

    public override float Execute()
    {
        MapData map_data = GameObject.Find("GameData").GetComponent<GameData>().current_map;
        ActorData src_actor = map_data.GetActor(src_actor_id);
        ActorData target_actor = map_data.GetActorOnTile(target_tile_x, target_tile_y);

        if (src_actor == null || target_actor == null)
            return 0.00f;

        src_actor.Heal(amount);
        target_actor.Damage(amount);

        GameLogger.Log("The " + src_actor.prototype.name + " steals " + amount + " health from the " + target_actor.prototype.name + ".");

        return 0.00f;
    }
}

public class AttackedTileData
{
    public int x;
    public int y;

    public List<(DamageType type, int damage, int armor_penetration)> damage_on_hit;
    public List<EffectData> effects_on_hit;
    public List<Type> diseases_on_hit;
    public List<Type> poisons_on_hit;

    public AttackedTileData()
    {
        damage_on_hit = new();
        effects_on_hit = new();
        diseases_on_hit = new();
        poisons_on_hit = new();
    }

    internal void Save(BinaryWriter save)
    {
        save.Write(x);
        save.Write(y);

        save.Write(damage_on_hit.Count);
        foreach (var v in damage_on_hit)
        {
            save.Write((int) v.type);
            save.Write(v.damage);
            save.Write(v.armor_penetration);
        }

        save.Write(effects_on_hit.Count);
        foreach (var v in effects_on_hit)
        {
            save.Write(v.GetType().Name);
        }

        save.Write(diseases_on_hit.Count);
        foreach (var v in diseases_on_hit)
        {
            save.Write(v.ToString());
        }

        save.Write(poisons_on_hit.Count);
        foreach (var v in poisons_on_hit)
        {
            save.Write(v.ToString());
        }

    }

    internal void Load(BinaryReader save)
    {
        x = save.ReadInt32();
        y = save.ReadInt32();

        int size = save.ReadInt32();
        damage_on_hit = new(size);
        for (int i = 0; i < size; ++ i)
        {
            damage_on_hit.Add(((DamageType) save.ReadInt32(), save.ReadInt32(), save.ReadInt32()));
        }

        size = save.ReadInt32();
        effects_on_hit = new(size);
        for (int i = 0; i < size; ++i)
        {
            Type type = Type.GetType(save.ReadString());
            effects_on_hit.Add((EffectData) Activator.CreateInstance(type));
        }

        size = save.ReadInt32();
        diseases_on_hit = new(size);
        for (int i = 0; i < size; ++i)
        {
            string name = save.ReadString();
            diseases_on_hit.Add(Type.GetType(name));
        }

        size = save.ReadInt32();
        poisons_on_hit = new(size);
        for (int i = 0; i < size; ++i)
        {
            string name = save.ReadString();
            poisons_on_hit.Add(Type.GetType(name));
        }
    }
}

public class AttackTilesCommand : CommandData
{
    public long actor_id;
    public bool melee; //show melee attack animations
    public List<AttackedTileData> attacked_tiles;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(actor_id);
        save.Write(melee);
        save.Write(attacked_tiles.Count);
        foreach (var v in attacked_tiles)
            v.Save(save);
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        actor_id = save.ReadInt64();
        melee = save.ReadBoolean();

        int size = save.ReadInt32();
        attacked_tiles = new(size);
        for (int i = 0; i < size; ++ i)
        {
            AttackedTileData v = new();
            v.Load(save);
            attacked_tiles.Add(v);
        }
    }

    public AttackTilesCommand()
    {

    }
    public AttackTilesCommand(ActorData actor, List<AttackedTileData> attacked_tiles, int duration, bool melee = false)
    {
        this.actor_id = actor.id;
        this.attacked_tiles = attacked_tiles;
        this.duration = duration;
        this.melee = melee;
    }

    public override float Execute()
    {
        MapData map_data = GameObject.Find("GameData").GetComponent<GameData>().current_map;
        ActorData actor = map_data.GetActor(actor_id);

        if (actor.current_effects.FindAll(x => x.effect is EffectStun).Count > 0)
        {
            GameLogger.Log(actor.prototype.name + " is stunned and cannot attack.");
            return 0.0f;
        }

        foreach (AttackedTileData tile in attacked_tiles)
        {
            map_data.DistributeDamage(actor, tile);
            
        }

        if (melee == true)
            actor.DoMeleeAttack(attacked_tiles);

        if (actor is PlayerData)
            return 0.5f;

        return 0.0f;
    }
}


public class ChangeDungeonCommand : CommandData
{
    public DungeonChangeData dungeon_change_data;
    public long actor_id;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        dungeon_change_data.Save(save);
        save.Write(actor_id);
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        dungeon_change_data = new DungeonChangeData();
        dungeon_change_data.Load(save);
        actor_id = save.ReadInt64();
    }

    public ChangeDungeonCommand()
    {

    }

    public ChangeDungeonCommand(DungeonChangeData dungeon_change_data, ActorData actor)
    {
        this.dungeon_change_data = dungeon_change_data;
        this.actor_id = actor.id;
    }

    public override float Execute()
    {
        MapData map_data = GameObject.Find("GameData").GetComponent<GameData>().current_map;
        ActorData actor = map_data.GetActor(actor_id);

        if (!(actor is PlayerData))
            return 0;

        GameData game_data = GameObject.Find("GameData").GetComponent<GameData>();
        string old_dungeon_name = game_data.current_dungeon.name;

        game_data.current_map.actors.Remove(game_data.player_data);
        game_data.current_dungeon = game_data.dungeons.Find(x => x.name == dungeon_change_data.target_dungeon_name);
        if (game_data.current_dungeon.is_persistent == false && game_data.current_dungeon.name != old_dungeon_name)
            game_data.current_dungeon.SetRegenerationNeeded();

        game_data.current_map_level = game_data.current_dungeon.GetDungeonLevelDataOf(dungeon_change_data.target_entrance_name);

        game_data.current_map = game_data.current_map_level.map;

        game_data.current_map.actors.Insert(0,game_data.player_data);

        //Find Entrance
        foreach(MapFeatureData feature in game_data.current_map.features)
        {
            if (feature is MFChangeDungeon)
            {
                MFChangeDungeon change_feature = (MFChangeDungeon)feature;
                if (change_feature.dungeon_change_data.name == dungeon_change_data.target_entrance_name)
                {
                    game_data.player_data.MoveTo(change_feature.exit_tile.x, change_feature.exit_tile.y);                    
                    break;
                }
            }
        }
        
        game_data.current_map.visited = true;
        game_data.current_map.tick_entered = true;

        GameObject.Find("Map").GetComponent<Map>().DestroyMap();

        GameObject.Instantiate(GameObject.Find("GameEngine").GetComponent<GameEngine>().map_prefab);
  
        //game_data.current_map.UpdateVisibility();

        GameLogger.Log("The Player enters " + game_data.current_dungeon.name + ".");
        GameObject.Find("DungeonInfo").transform.Find("DungeonName").GetComponent<TMPro.TextMeshProUGUI>().text = game_data.current_dungeon.name;
        GameObject.Find("DungeonInfo").transform.Find("DungeonLevel").GetComponent<TMPro.TextMeshProUGUI>().text = "Level " + (game_data.current_map_level.dungeon_level + 1);

        foreach (QuestData quest in game_data.player_data.active_quests)
            quest.OnLocationChange(game_data.current_dungeon.name);

        if (game_data.current_dungeon.name == game_data.dungeons[0].name)
            game_data.player_data.RefreshAll();

        return 0;
    }
}

public class CollectItemCommand : CommandData
{
    public long item_id;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(item_id);

    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        item_id = save.ReadInt64();
    }

    public CollectItemCommand()
    {

    }

    public CollectItemCommand(ItemData item)
    {
        this.item_id = item.id;
    }

    public override float Execute()
    {
        MapData map_data = GameObject.Find("GameData").GetComponent<GameData>().current_map;
        ItemData item = map_data.GetItem(item_id);
        if (item == null) return 0;

        PlayerData player = GameObject.Find("GameData").GetComponent<GameData>().player_data;

        bool success = player.AddItem(item);

        if (success == true)
        {
            map_data.Remove(item);
            string text = "The Player picks up ";
            if (item.amount == 1)
                text += "a ";
            text += item.GetName().ToLower();
            if (item.amount > 1)
                text += "(" + item.amount + ")";
            text += ".";
            GameLogger.Log(text);
        }

        if (item.is_quest_item == true)
        {
            QuestData quest = player.active_quests.Find(x => x.id == item.quest_id);
            if (quest == null)
            {
                Debug.Log("Error: Quest of quest item " + item.GetName() + " does not exist (anymore).");
                return 0;
            }
            GameLogger.Log("It is a quest item!");
            quest.OnItemPickup(item);
        }

        return 0;
    }
}

public class ConsumeItemCommand : CommandData
{
    public long item_id;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(item_id);

    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        item_id = save.ReadInt64();
    }

    public ConsumeItemCommand()
    {

    }

    public ConsumeItemCommand(ItemData item)
    {
        this.item_id = item.id;
    }

    public override float Execute()
    {
        PlayerData player = GameObject.Find("GameData").GetComponent<GameData>().player_data;
        ItemData item = player.inventory.GetItem(item_id);
        player.Consume(item);
        return 0.0f;
    }
}

public class UseItemCommand : CommandData
{
    public long item_id;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(item_id);

    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        item_id = save.ReadInt64();
    }

    public UseItemCommand()
    {

    }

    public UseItemCommand(ItemData item)
    {
        this.item_id = item.id;
    }

    public override float Execute()
    {
        PlayerData player = GameObject.Find("GameData").GetComponent<GameData>().player_data;
        ItemData item = player.inventory.GetItem(item_id);
        player.Use(item);
        return 0.0f;
    }
}

public class DropItemCommand : CommandData
{
    public long item_id;
    public int amount;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(item_id);
        save.Write(amount);
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        item_id = save.ReadInt64();
        amount = save.ReadInt32();
    }

    public DropItemCommand()
    {

    }

    public DropItemCommand(ItemData item, int amount = -1)
    {
        this.item_id = item.id;
        this.amount = amount;

    }

    public override float Execute()
    {
        PlayerData player = GameObject.Find("GameData").GetComponent<GameData>().player_data;
        ItemData item = player.inventory.GetItem(item_id);
        player.Drop(item, amount);
        return 0.0f;
    }
}

public class EquipItemCommand : CommandData
{
    public int inventory_slot_index;
    public int equipment_slot_index;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(inventory_slot_index);
        save.Write(equipment_slot_index);

    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        inventory_slot_index = save.ReadInt32();
        equipment_slot_index = save.ReadInt32();
    }

    public EquipItemCommand()
    {

    }

    public EquipItemCommand(int inventory_slot_index, int equipment_slot_index)
    {
        this.inventory_slot_index = inventory_slot_index;
        this.equipment_slot_index = equipment_slot_index;
    }

    public override float Execute()
    {
        PlayerData player = GameObject.Find("GameData").GetComponent<GameData>().player_data;
        player.Equip(inventory_slot_index,equipment_slot_index);
        return 0f;
    }
}

public class UnequipItemCommand : CommandData
{
    public int inventory_slot_index;
    public int equipment_slot_index;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(inventory_slot_index);
        save.Write(equipment_slot_index);

    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        inventory_slot_index = save.ReadInt32();
        equipment_slot_index = save.ReadInt32();
    }
    public UnequipItemCommand()
    {

    }
    public UnequipItemCommand(int inventory_slot_index, int equipment_slot_index)
    {
        this.inventory_slot_index = inventory_slot_index;
        this.equipment_slot_index = equipment_slot_index;
    }

    public override float Execute()
    {
        PlayerData player = GameObject.Find("GameData").GetComponent<GameData>().player_data;
        player.UnEquip(inventory_slot_index, equipment_slot_index);
        return 0f;
    }
}

public class MultiplyCommand : CommandData
{
    public long actor_id;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(actor_id);
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        actor_id = save.ReadInt64();
    }

    public MultiplyCommand()
    {

    }

    public MultiplyCommand(ActorData actor)
    {
        this.actor_id = actor.id;
    }

    public override float Execute()
    {
        MapData map_data = GameObject.Find("GameData").GetComponent<GameData>().current_map;
        ActorData actor = map_data.GetActor(actor_id);

        // Look for empty tile
        for (int x = -1; x <= 1; ++x)
        {
            for (int y = -1; y <= 1; ++y)
            {
                if (map_data.CanBeMovedInByActor(actor.X + x, actor.Y + y, actor) == true)
                {                    
                    ActorData actor_data = null;
                    actor_data = new MonsterData(actor.X + x, actor.Y + y, (ActorPrototype)Activator.CreateInstance(actor.prototype.GetType(), actor.prototype.stats.level));
                    map_data.Add(actor_data);

                    return 0.0f;
                }
            }
        }

        return 0;
    }
}
public class CreateProjectileCommand : CommandData
{
    ProjectileData projectile;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(projectile.GetType().ToString());
        projectile.Save(save);
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        Type type = Type.GetType(save.ReadString());
        projectile = (ProjectileData) Activator.CreateInstance(type, -1,-1, null);
        projectile.Load(save);
    }

    public CreateProjectileCommand()
    {

    }

    public CreateProjectileCommand(ProjectileData projectile)
    {
        this.projectile = projectile;
    }

    public override float Execute()
    {
        MapData map_data = GameObject.Find("GameData").GetComponent<GameData>().current_map;
        bool hit = !map_data.IsAccessableTile(projectile.X, projectile.Y);

        GameObject.Find("GameData").GetComponent<GameData>().current_map.Add(projectile);
        if (hit)
            projectile.current_action = new ExplodeAction(projectile, projectile.prototype.projectile.damage_radius, projectile.prototype.projectile.damage, projectile.prototype.projectile.explosion_on_impact);

        return 0.0f;
    }
}

public class CreateProjectileToPlayerCommand : CommandData
{
    long actor_id;
    Type type;

    int level;
    
    internal override void Save(BinaryWriter save)
    {
        base.Save(save);
        save.Write(actor_id);
        save.Write(type.ToString());
        save.Write(level);
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        actor_id = save.ReadInt64();
        type = Type.GetType(save.ReadString());    
        level = save.ReadInt32();    
    }

    public CreateProjectileToPlayerCommand()
    {

    }

    public CreateProjectileToPlayerCommand(long actor_id, Type type, int level)
    {
        this.actor_id = actor_id;
        this.type = type;
        this.level = level;
    }

    public override float Execute()
    {
        MapData map_data = GameObject.Find("GameData").GetComponent<GameData>().current_map;
        PlayerData player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;
        ActorData actor = map_data.GetActor(actor_id);

        //Path path = Algorithms.AStar(map_data, (actor.X, actor.Y), (player_data.X, player_data.Y), true, true, actor);

        var path = Algorithms.LineofSight((actor.X, actor.Y), (player_data.X, player_data.Y));

        ProjectileData projectile = new ProjectileData( path[0].x, path[0].y, (ActorPrototype)Activator.CreateInstance(type, level));
        List<(int x, int y)> p_path = new();
        
        int counter = 0;
        foreach(var tile in path)
        {
            if (counter != 0)
                p_path.Add((tile.x, tile.y));

            ++counter;
        }

        projectile.path = p_path;
        bool hit = !map_data.IsAccessableTile(projectile.X, projectile.Y, false, actor);
        map_data.Add(projectile);

        if (hit)
            projectile.current_action = new ExplodeAction(projectile, projectile.prototype.projectile.damage_radius, projectile.prototype.projectile.damage, projectile.prototype.projectile.explosion_on_impact);

        return 0.0f;
    }
}

internal class ActivateSubstainedTalentCommand : CommandData
{
    private long actor_id;
    private long talent_id;

    public ActivateSubstainedTalentCommand(long actor_id, long talent_id)
    {
        this.actor_id = actor_id;
        this.talent_id = talent_id;
    }

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(actor_id);
        save.Write(talent_id);
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        actor_id = save.ReadInt64();
        talent_id = save.ReadInt64();   
    }

    public override float Execute()
    {
        MapData map_data = GameObject.Find("GameData").GetComponent<GameData>().current_map;
        ActorData source_actor = map_data.GetActor(actor_id);

        source_actor.ActivateSubstainedTalent(talent_id);      

        return 0.0f;
    }
}

internal class DeactivateSubstainedTalentCommand : CommandData
{
    private long actor_id;
    private long talent_id;

    public DeactivateSubstainedTalentCommand(long actor_id, long talent_id)
    {
        this.actor_id = actor_id;
        this.talent_id = talent_id;
    }

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(actor_id);
        save.Write(talent_id);
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        actor_id = save.ReadInt64();
        talent_id = save.ReadInt64();
    }

    public override float Execute()
    {
        MapData map_data = GameObject.Find("GameData").GetComponent<GameData>().current_map;
        ActorData source_actor = map_data.GetActor(actor_id);

        source_actor.DeactivateSubstainedTalent(talent_id);       

        return 0.0f;
    }
}

public class ExplodeCommand : CommandData
{
    public long actor_id;
    public int damage_radius;
    public bool explosion_on_impact;

    public List<(DamageType type, int amount, int penetration)> damage;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(actor_id);
        save.Write(damage_radius);
        save.Write(explosion_on_impact);

        save.Write(damage.Count);
        foreach(var v in damage)
        {
            save.Write((int) v.type);
            save.Write(v.amount);
            save.Write(v.penetration);
        }
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        actor_id = save.ReadInt64();
        damage_radius = save.ReadInt32();
        explosion_on_impact = save.ReadBoolean();

        damage = new();
        int size = save.ReadInt32();
        for (int i = 0; i < size; ++i)
        {
            damage.Add(((DamageType) save.ReadInt32(),save.ReadInt32(),save.ReadInt32()));
        }
    }

    public ExplodeCommand()
    {

    }

    public ExplodeCommand(ActorData actor, int damage_radius, List<(DamageType type, int amount, int penetration)> damage, bool explosion_on_impact)
    {
        this.actor_id = actor.id;
        this.damage_radius = damage_radius;
        this.damage = damage;
        this.explosion_on_impact = explosion_on_impact;
    }

    public override float Execute()
    {
        MapData map_data = GameObject.Find("GameData").GetComponent<GameData>().current_map;
        ActorData actor = map_data.GetActor(actor_id);

        for (int i = actor.X - damage_radius; i <= actor.X + damage_radius; ++i)
        {
            for (int j = actor.Y - damage_radius; j <= actor.Y + damage_radius; ++j)
            {
                AttackedTileData attacked_tile = new AttackedTileData
                {
                    x = i,
                    y = j,
                    damage_on_hit = damage,
                };
                map_data.DistributeDamage(actor, attacked_tile);

                if (explosion_on_impact == true)
                    GameObject.Find("Map").GetComponent<Map>().AddVisualEffectToTile(VisualEffect.Fire, (i, j));
            }
        }
        actor.OnKill();

        return 0;
    }
}

public class GetEffectCommand : CommandData
{
    public long actor_id;
    public EffectData effect;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(actor_id);
        save.Write(effect.GetType().ToString());
        effect.Save(save);
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        actor_id = save.ReadInt64();
        effect = (EffectData) Activator.CreateInstance(Type.GetType(save.ReadString()));
        effect.Load(save);
    }

    public GetEffectCommand()
    {

    }

    public GetEffectCommand(ActorData actor, EffectData effect)
    {
        this.actor_id = actor.id;
        this.effect = effect;
    }

    public override float Execute()
    {
        MapData map_data = GameObject.Find("GameData").GetComponent<GameData>().current_map;
        ActorData actor = map_data.GetActor(actor_id);

        actor.AddEffect(effect);
        
        return 0;
    }
}

public class TeleportCommand : CommandData
{
    public long actor_id;
    public (int x, int y) target;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(actor_id);
        save.Write(target.x);
        save.Write(target.y);
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        actor_id = save.ReadInt64();
        target = (save.ReadInt32(), save.ReadInt32());
    }

    public TeleportCommand()
    {

    }

    public TeleportCommand(ActorData actor, (int x, int y) target)
    {
        this.actor_id = actor.id;
        this.target = target;
    }

    public override float Execute()
    {
        MapData map_data = GameObject.Find("GameData").GetComponent<GameData>().current_map;
        ActorData actor = map_data.GetActor(actor_id);
        //Teleport to the specified target. If target is blocked (for example because of player) then teleport to a nearby target
        // Look for empty tile

        (int x, int y)? real_target = null;

        if (map_data.CanBeMovedInByActor(target.x, target.y, actor) == true)
        {
            real_target = target;
        }
        else
        {
            bool found_target = false;
            for (int x = -1; x <= 1; ++x)
            {
                for (int y = -1; y <= 1; ++y)
                {
                    if (found_target == false && map_data.CanBeMovedInByActor(target.x + x, target.y + y, actor) == true)
                    {                    
                        real_target = (target.x + x, target.y + y);    
                        found_target = true;          
                    }
                }
            }
        }

        if (real_target == null)
            return 0;
        
        actor.MoveTo(real_target.Value.x, real_target.Value.y, true);
        return 0;
    }
}

public class TeleportRandomCommand : CommandData
{
    public long actor_id;
    public int distance;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(actor_id);
        save.Write(distance);
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        actor_id = save.ReadInt64();
        distance = save.ReadInt32();
    }

    public TeleportRandomCommand()
    {

    }

    public TeleportRandomCommand(ActorData actor, int distance)
    {
        this.actor_id = actor.id;
        this.distance = distance;
    }

    public override float Execute()
    {
        MapData map_data = GameObject.Find("GameData").GetComponent<GameData>().current_map;
        ActorData actor = map_data.GetActor(actor_id);
        // Look for empty tile

        (int x, int y)? real_target = null;

        int tries = 0;
        while(real_target == null && tries < 1000)
        { 
            int x = actor.X + UnityEngine.Random.Range(-distance,distance+1);
            int y = actor.Y + UnityEngine.Random.Range(-distance,distance+1);

            if (map_data.IsAccessableTile(x, y) == true)
            {
                real_target = (x,y);
            }
            ++tries;
        }

        if (real_target == null)
            return 0;
        
        actor.MoveTo(real_target.Value.x, real_target.Value.y, true);
        return 0;
    }
}

public class RepairItemsCommand : CommandData
{
    public int rel_amount;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(rel_amount);

    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        rel_amount = save.ReadInt32();
    }

    public RepairItemsCommand()
    {

    }

    public RepairItemsCommand(int rel_amount)
    {
        this.rel_amount = rel_amount;
    }

    public override float Execute()
    {
        PlayerData player = GameObject.Find("GameData").GetComponent<GameData>().player_data;
        foreach(var slot in player.equipment)
        {
            if (slot.item != null && slot.item.armor_data != null)
            {
                slot.item.armor_data.durability_current = Mathf.Min(slot.item.armor_data.durability_current + (int) ((rel_amount / 100.0) * slot.item.GetPrototype().armor.durability_max), slot.item.GetPrototype().armor.durability_max);
            }
        }

        foreach(var slot in player.inventory.slots)
        {
            if (slot.item != null && slot.item.armor_data != null)
            {
                slot.item.armor_data.durability_current = Mathf.Min(slot.item.armor_data.durability_current + (int) ((rel_amount / 100.0) * slot.item.GetPrototype().armor.durability_max), slot.item.GetPrototype().armor.durability_max);
            }
        }

        return 0.0f;
    }
}

public class CureDiseasesCommand : CommandData
{
    public long source_actor_id;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(source_actor_id);

    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        source_actor_id = save.ReadInt64();
    }

    public CureDiseasesCommand()
    {

    }

    public CureDiseasesCommand(ActorData source_actor)
    {
        this.source_actor_id = source_actor.id;
    }

    public override float Execute()
    {
        ActorData actor = GameObject.Find("GameData").GetComponent<GameData>().current_map.GetActor(source_actor_id);           
        actor.CureAllDiseases();
        return 0.0f;
    }
}

public class CurePoisonsCommand : CommandData
{
    public long source_actor_id;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(source_actor_id);

    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        source_actor_id = save.ReadInt64();
    }

    public CurePoisonsCommand()
    {

    }

    public CurePoisonsCommand(ActorData source_actor)
    {
        this.source_actor_id = source_actor.id;
    }

    public override float Execute()
    {
        ActorData actor = GameObject.Find("GameData").GetComponent<GameData>().current_map.GetActor(source_actor_id);           
        actor.CureAllPoisons();
        return 0.0f;
    }
}

public class CureInsanityCommand : CommandData
{
    public long source_actor_id;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(source_actor_id);

    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);

        source_actor_id = save.ReadInt64();
    }

    public CureInsanityCommand()
    {

    }

    public CureInsanityCommand(ActorData source_actor)
    {
        this.source_actor_id = source_actor.id;
    }

    public override float Execute()
    {
        ActorData actor = GameObject.Find("GameData").GetComponent<GameData>().current_map.GetActor(source_actor_id);           
        actor.CureInsanity();
        return 0.0f;
    }
}
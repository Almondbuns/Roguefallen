using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectilePrototype
{
    public List<(DamageType type, int amount, int penetration)> damage;
    public int damage_radius = 0;
    public bool explosion_on_impact = false;
}

public class MonsterPrototype
{
    public AIPersonality ai_personality;
}

public class InventoryPrototype
{
    public int size;
}

public class ActorPrototype
{
    public bool is_friendly = false;
    public bool can_move = true;
    public bool blocks_tiles = true;
    public bool is_hidden = false;

    public int prefab_index = -1;

    public ActorStats stats;

    public string name = "";
    public string icon = "";

    public int tile_width = 1;
    public int tile_height = 1;

    public List<TalentPrototype> talents;

    public ProjectilePrototype projectile;

    public MonsterPrototype monster;

    public InventoryPrototype inventory;

    public ActorPrototype(int level)
    {
        stats = new ActorStats();
        stats.level = level;
        talents = new();
    }

    public virtual bool OnPlayerMovementHit(ActorData actor_data)
    {
        return true;
    }

    public virtual void OnKill(ActorData actor_data)
    {
    }

    public virtual void OnEnterTile(ActorData this_actor, ActorData target_actor)
    {
    }

    public virtual void OnCreation(ActorData this_actor)
    {
    }
}

public class PlayerPrototype : ActorPrototype
{
    public int max_weight = 50;
    
    public PlayerPrototype(int level) : base(level)
    {
        name = "Player";
        icon = "images/npc/player";

        is_friendly = true;
    
        stats.health_max = 20;
        stats.stamina_max = 10;
        stats.mana_max = 0;

        if (GameData.GODMODE == true)
        {
            stats.health_max = 10000;
            stats.stamina_max = 10000;
            stats.mana_max = 10000;
        }

        ArmorStats armor_stats = new ArmorStats { body_part = "Chest", percentage = 70 };
        stats.body_armor.Add(armor_stats);
      
        armor_stats = new ArmorStats { body_part = "Head", percentage = 10 };
        stats.body_armor.Add(armor_stats);
      
        armor_stats = new ArmorStats { body_part = "Hands", percentage = 10 };
        stats.body_armor.Add(armor_stats);
      
        armor_stats = new ArmorStats { body_part = "Feet", percentage = 10 };
        stats.body_armor.Add(armor_stats);
     
        stats.movement_time = 50;

        stats.meter_resistances.SetResistance(DamageType.DISEASE, 30);
        stats.meter_resistances.SetResistance(DamageType.POISON, 30);
        stats.meter_resistances.SetResistance(DamageType.INSANITY, 30);

        inventory = new InventoryPrototype{size = 15};
    }
}

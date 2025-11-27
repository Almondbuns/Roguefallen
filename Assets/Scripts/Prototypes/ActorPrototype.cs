using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectilePrototype
{
    public List<(DamageType type, int amount, int penetration)> damage;
    public int damage_radius = 0;
    public bool explosion_on_impact = false;
}

public class AIPrototype
{
    public AIPersonality personality;
    public int prefered_distance = 0;
}

public class MonsterPrototype
{
    public AIPrototype ai_prototype;
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
    public bool has_dialogue = false;

    public bool can_catch_disease = true;
    public bool can_catch_poison = true;
    public bool can_catch_insanity = true;
    public bool can_dodge = true;
    public bool can_take_damage = true;

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

    public DialogueTree dialogue_tree;

    public ActorPrototype(int level)
    {
        stats = new ActorStats();
        stats.level = level;
        talents = new();

        //Standard resistances scale with level - specific monsters may overwrite values
        stats.meter_resistances.SetResistance(DamageType.DISEASE, 10 + 5 * level);
        stats.meter_resistances.SetResistance(DamageType.POISON, 10 + 5 * level);
        stats.meter_resistances.SetResistance(DamageType.INSANITY, 10 + 5 * level);
    }

    public virtual bool OnPlayerMovementHit(ActorData actor_data)
    {
        if (has_dialogue == false)
            return true;
        
        if (GameObject.Find("UI").GetComponent<UI>().current_ui_states.Count > 0) return false;
        GameObject.Find("UI").GetComponent<UI>().AddUIState(new UIStateNPCDialogueTree(actor_data));
        return false;
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

    public virtual void OnDamage(ActorData this_actor, DamageType damage_type, int damage_amount)
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

        ActorArmorStats armor_stats = new ActorArmorStats { body_part = "Chest", percentage = 70 };
        stats.body_armor.Add(armor_stats);
      
        armor_stats = new ActorArmorStats { body_part = "Head", percentage = 10 };
        stats.body_armor.Add(armor_stats);
      
        armor_stats = new ActorArmorStats { body_part = "Hands", percentage = 10 };
        stats.body_armor.Add(armor_stats);
      
        armor_stats = new ActorArmorStats { body_part = "Feet", percentage = 10 };
        stats.body_armor.Add(armor_stats);
     
        stats.movement_time = 50;

        stats.meter_resistances.SetResistance(DamageType.DISEASE, 30);
        stats.meter_resistances.SetResistance(DamageType.POISON, 30);
        stats.meter_resistances.SetResistance(DamageType.INSANITY, 30);

        inventory = new InventoryPrototype{size = 15};
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : ActorPrototype
{
    public Chest(int level) : base(level)
    {
        name = "Chest";
        icon = "images/objects/chest";

        can_catch_disease = false;
        can_catch_poison = false;
        can_catch_insanity = false;

        stats.health_max = 100;

        stats.body_armor.Add(new ArmorStats { body_part = "Chest", percentage = 100, armor = (2, 3, 1), durability_max = 50 });

        inventory = new InventoryPrototype(){size = 8};
    }

    public override void OnCreation(ActorData actor_data)
    {
        int number_of_items = UnityEngine.Random.Range(3,7);

        for (int i = 0; i < number_of_items; ++i)
        {
            float random_float = UnityEngine.Random.value;
            int item_level = stats.level;

            if (random_float > .095f)
                item_level = stats.level + 3;
            else if (random_float > 0.80f)
                item_level = stats.level + 2;
            else if (random_float > 0.50f)
                item_level = stats.level + 1;
                
            ItemData item = ItemData.GetRandomItem(-1,-1, stats.level);

            int r = UnityEngine.Random.Range(1, 101);
            if (r <= 20)
                item.SetQuality(ItemQuality.Unique);
            else if (r <= 60)
                item.SetQuality(ItemQuality.Magical2);
            else if (r <= 90)
                item.SetQuality(ItemQuality.Magical1);

            actor_data.inventory.AddItem(item);
        }
    }

    public override bool OnPlayerMovementHit(ActorData actor_data)
    {
        if (GameObject.Find("UI").GetComponent<UI>().current_ui_states.Count > 0) return false;
        GameObject.Find("UI").GetComponent<UI>().AddUIState(new UIStateInventoryChest(actor_data));

        return false;
    }
}

public class Crate : ActorPrototype
{
    public Crate(int level) : base(level)
    {
        name = "Crate";
        icon = "images/objects/box";

        can_catch_disease = false;
        can_catch_poison = false;
        can_catch_insanity = false;

        stats.health_max = 10;
        stats.dodge = -100;

        stats.body_armor.Add(new ArmorStats { body_part = "Crate", percentage = 100, armor = (0, 0, 0), durability_max = 0 });
    }

    public override void OnKill(ActorData actor_data)
    {
        //Spawn a broken Crate
        MapData map = GameObject.Find("GameData").GetComponent<GameData>().current_map;
        //map.Add(new DynamicObjectData(actor_data.x, actor_data.y, new BrokenCrate(1)));
        
        //May spawn nothing, an item oder a monster
        int random = Random.Range(0, 100);
        if (random < 20)
        {
            //Spawn monster
            (int x, int y)? tile = map.FindRandomEmptyNeighborTile(actor_data.X, actor_data.Y);
            if (tile == null) return;

            ActorData monster;
            float rand = UnityEngine.Random.value;
            if (rand <= 0.25f )
                monster = new MonsterData(tile.Value.x, tile.Value.y, new Rat(stats.level));
            else if (rand <= 0.5f )
                monster = new MonsterData(tile.Value.x, tile.Value.y, new CaveSpider(stats.level));
            else if (rand <= 0.75f )
                monster = new MonsterData(tile.Value.x, tile.Value.y, new Lemming(stats.level));
            else 
                monster = new MonsterData(tile.Value.x, tile.Value.y, new Ooz(stats.level));      
            
            map.Add(monster);
            GameLogger.Log("The Crate breaks. A " + monster.prototype.name + " jumps out.");
        }
        else if (random < 60)
        {
            //Spawn item
            (int x, int y)? tile = map.FindRandomEmptyNeighborTile(actor_data.X, actor_data.Y);
            if (tile == null) return;
            ItemData item = ItemData.GetRandomItem(tile.Value.x, tile.Value.y, stats.level);
            map.Add(item);
            GameLogger.Log("The Crate breaks. You see a " + item.GetName() + ".");
        }
        else
        {
            GameLogger.Log("The Crate breaks. It is empty.");
        }
    }
}

public class Jar : ActorPrototype
{
    public Jar(int level) : base(level)
    {
        name = "Jar";
        icon = "images/objects/jar";

        can_catch_disease = false;
        can_catch_poison = false;
        can_catch_insanity = false;

        stats.health_max = 5;
        stats.dodge = -100;

        stats.body_armor.Add(new ArmorStats { body_part = "Jar", percentage = 100, armor = (0, 0, 0), durability_max = 0 });
    }

    public override void OnKill(ActorData actor_data)
    {
        MapData map = GameObject.Find("GameData").GetComponent<GameData>().current_map;
  
        //May spawn gold, give XP or deal special damage
        int random = Random.Range(0, 100);
        if (random < 33)
        {
            //Spawn gold
            (int x, int y)? tile = map.FindRandomEmptyNeighborTile(actor_data.X, actor_data.Y);
            if (tile == null) return;

            ItemData gold;
            gold = new ItemData(new ItemGold(stats.level), tile.Value.x, tile.Value.y);      
            
            map.Add(gold);
            GameLogger.Log("The jar breaks and drops some gold.");
            return;
        }
        else if (random < 66)
        {
            // Gain XP
            GameObject.Find("GameData").GetComponent<GameData>().player_data.GainExperience(10);
            GameLogger.Log("The jar breaks. The player gains some experience.");
            return;
        }
        else
        {
            //Deal Damage
            int random_damage = UnityEngine.Random.Range(0,100);
            DamageType damage_type = DamageType.DURABILITY;

            if (random_damage < 33)
            {
                damage_type = DamageType.DURABILITY;
                GameLogger.Log("The jar breaks and spills acid.");
            }
            else if (random_damage < 66)
            {
                damage_type = DamageType.POISON;
                GameLogger.Log("The jar breaks and spills poison.");
            }
            else
            {
                damage_type = DamageType.DISEASE;
                GameLogger.Log("The jar breaks and spills a stinky liquid.");
            }

            for (int i = -1; i <= 1; ++ i)
            {
                for (int j = -1; j <= 1; ++ j)
                {
                    if (i == 0 && j == 0)
                    continue;
                    
                    map.DistributeDamage(actor_data, new AttackedTileData(){x = actor_data.X + i, y = actor_data.Y + j, damage_on_hit = {(damage_type,Random.Range(5,11),0)}});
                }
            }
            return;
        }
    }
}

public class BrokenCrate : ActorPrototype
{
    public BrokenCrate(int level) : base(level)
    {
        name = "Broken Crate";
        icon = "images/objects/box_broken";

        can_catch_disease = false;
        can_catch_poison = false;
        can_catch_insanity = false;

        stats.health_max = 10;
        stats.dodge = -100;

        stats.body_armor.Add(new ArmorStats { body_part = "Broken Crate", percentage = 100, armor = (0, 0, 0), durability_max = 0});
    }

}

public class TombPillar : ActorPrototype
{
    public TombPillar(int level) : base(level)
    {
        name = "Pillar";
        icon = "images/objects/tomb_pillar";

        can_catch_disease = false;
        can_catch_poison = false;
        can_catch_insanity = false;

        stats.health_max = 30;
        stats.dodge = -100;

        stats.body_armor.Add(new ArmorStats { body_part = "Pillar", percentage = 100, armor = (0, 0, 0), durability_max = 0});
    }

}

public class TombSarcophagus : ActorPrototype
{
    public TombSarcophagus(int level) : base(level)
    {
        name = "Sarcophagus";
        icon = "images/objects/tomb_sarcophagus";
        tile_width = 1;
        tile_height = 1;

        can_catch_disease = false;
        can_catch_poison = false;
        can_catch_insanity = false;

        stats.health_max = 30;
        stats.dodge = -100;

        stats.body_armor.Add(new ArmorStats { body_part = "Sarcophagus", percentage = 100, armor = (0, 0, 0), durability_max = 0});
    }

}

public class TombGiantBall : ActorPrototype
{
    public TombGiantBall(int level) : base(level)
    {
        name = "Giant Ball";
        icon = "images/npc/tomb_giant_ball";
        tile_width = 2;
        tile_height = 2;

        can_catch_disease = false;
        can_catch_poison = false;
        can_catch_insanity = false;

        stats.health_max = 100;
        stats.dodge = -100;

        stats.body_armor.Add(new ArmorStats { body_part = "Giant Ball", percentage = 100, armor = (0, 0, 0), durability_max = 0});
    }

}


public class BearTrap : ActorPrototype
{
    public BearTrap(int level) : base(level)
    {
        name = "Bear Trap";
        icon = "images/objects/bear_trap";

        can_catch_disease = false;
        can_catch_poison = false;
        can_catch_insanity = false;

        blocks_tiles = false;
        is_hidden = true;

        stats.health_max = 10;
        stats.to_hit = 10;
        stats.dodge = -100;
        stats.stealth = 5;

        stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (0, 0, 0), durability_max = 0 });
    }

    public override void OnEnterTile(ActorData this_actor, ActorData target_actor)
    {
        if (this_actor.is_currently_hidden == true)
            this_actor.SetHidden(false);
        
        GameLogger.Log("The " + name.ToLower() + " snaps.");
        target_actor.TryToHit(this_actor, stats.to_hit, new List<(DamageType type, int damage, int armor_penetration)>() { (DamageType.SLASH, 5, 5)}, null);
    }
}

public class IceSpikeTrap : ActorPrototype
{
    public IceSpikeTrap(int level) : base(level)
    {
        name = "Spike Trap";
        icon = "images/objects/ice_cave_spike_trap";

        can_catch_disease = false;
        can_catch_poison = false;
        can_catch_insanity = false;

        blocks_tiles = false;
        is_hidden = true;

        stats.health_max = 10;
        stats.to_hit = 10;
        stats.dodge = -100;
        stats.stealth = 5;

        stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (0, 0, 0), durability_max = 0 });
    }

    public override void OnEnterTile(ActorData this_actor, ActorData target_actor)
    {
        if (this_actor.is_currently_hidden == true)
            this_actor.SetHidden(false);
        
        GameLogger.Log("A " + name.ToLower() + " shoots out of the ground.");
        target_actor.TryToHit(this_actor, stats.to_hit, new List<(DamageType type, int damage, int armor_penetration)>() { (DamageType.PIERCE, 5, 5)}, null);
    }
}

public class IceWaterTrap : ActorPrototype
{
    public IceWaterTrap(int level) : base(level)
    {
        name = "Water Trap";
        icon = "images/objects/ice_cave_water_trap";

        can_catch_disease = false;
        can_catch_poison = false;
        can_catch_insanity = false;

        blocks_tiles = false;
        is_hidden = true;

        stats.health_max = 10;
        stats.to_hit = 10;
        stats.dodge = -100;
        stats.stealth = 5;

        stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (0, 0, 0), durability_max = 0 });
    }

    public override void OnEnterTile(ActorData this_actor, ActorData target_actor)
    {
        if (this_actor.is_currently_hidden == true)
            this_actor.SetHidden(false);
        
        GameLogger.Log("The ice breaks and reveals a " + name.ToLower() + ".");
        target_actor.TryToHit(this_actor, stats.to_hit, new List<(DamageType type, int damage, int armor_penetration)>() { (DamageType.ICE, 2, 2)}, 
        new List<EffectData> { new EffectAddIceResistance { damage_type = DamageType.ICE, amount = -1, duration = 2000 }});
    }
}

public class SpiderWebTrap : ActorPrototype
{
    public SpiderWebTrap(int level) : base(level)
    {
        name = "Spider Web";
        icon = "images/objects/spider_web";

        can_catch_disease = false;
        can_catch_poison = false;
        can_catch_insanity = false;

        blocks_tiles = false;
        is_hidden = true;

        stats.health_max = 5;
        stats.to_hit = 10;
        stats.dodge = -100;
        stats.stealth = 1;

        stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (0, 0, 0), durability_max = 0 });
    }

    public override void OnEnterTile(ActorData this_actor, ActorData target_actor)
    {
        if (target_actor.prototype is CommonSpider || target_actor.prototype is CaveSpider || target_actor.prototype is PoisonSpider) // Spiders are immune
            return;

        if (this_actor.is_currently_hidden == true)
            this_actor.SetHidden(false);

        if (target_actor is PlayerData)
            GameLogger.Log("The player step into a web.");
        else
            GameLogger.Log("The " + target_actor.prototype.name.ToLower() + " steps into a web.");

        target_actor.TryToHit(this_actor, this_actor.prototype.stats.to_hit, new List<(DamageType type, int damage, int armor_penetration)>() { (DamageType.SLASH, 0, 0) },
            new List<EffectData> { new EffectAddMovementTime { damage_type = DamageType.SLASH, amount = 50, duration = 1000 }});
    }
}

public class OilPuddle : ActorPrototype
{
    public OilPuddle(int level) : base(level)
    {
        name = "Oil Puddle";
        icon = "images/objects/oil_puddle";

        can_catch_disease = false;
        can_catch_poison = false;
        can_catch_insanity = false;

        blocks_tiles = false;

        stats.health_max = 20;
        stats.dodge = -100;

        stats.body_armor.Add(new ArmorStats { body_part = "Oil Puddle", percentage = 100, armor = (0, 0, 0), durability_max = 0});
    }

    public override void OnDamage(ActorData this_actor, DamageType damage_type, int damage_amount)
    {
        if ((damage_type == DamageType.FIRE && damage_amount > 0)
            || (damage_amount > 0 && UnityEngine.Random.value < 0.2f))
        {
            this_actor.current_action = new ExplodeAction(this_actor, 1, new List<(DamageType type, int amount, int penetration)>{(DamageType.FIRE,10,0)}, true)
            {
                prepare_time = 300, 
                prepare_message = "The oil puddle catches fire.", 
                action_message = "The oil puddle explodes."
            };
        }
    }

    public override void OnEnterTile(ActorData this_actor, ActorData target_actor)
    {
        if (UnityEngine.Random.value < 0.1f)
        {
            this_actor.current_action = new ExplodeAction(this_actor, 1, new List<(DamageType type, int amount, int penetration)>{(DamageType.FIRE,10,0)}, true)
            {
                prepare_time = 300, 
                prepare_message = "The oil puddle catches fire.", 
                action_message = "The oil puddle explodes."
            };
        }
    }
}

public class DungeonEntrance : ActorPrototype
{
    public DungeonEntrance(int level, string icon) : base(level)
    {
        name = "Dungeon Entrance";
        this.icon = icon;

        can_catch_disease = false;
        can_catch_poison = false;
        can_catch_insanity = false;

        stats.health_max = 10;
        stats.dodge = -100;

        stats.body_armor.Add(new ArmorStats { body_part = "Broken Crate", percentage = 100, armor = (0, 0, 0), durability_max = 0});
    }

}
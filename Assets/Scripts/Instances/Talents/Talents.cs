using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TalentMultiply : TalentPrototype
{
    public TalentMultiply()
    {
        name = "Multiply";
        target = TalentTarget.Self;
        icon = "images/talents/multiply";
        
        description = "Produces a copy of itself";
        prepare_message = "The <name> starts to vibrate.";
        action_message = "The <name> multiplies.";

        ai_data = new TalentAIInfo
        {
            type = TalentAIInfoType.Special,
            use_probability = 0.9f,
        };
    }
    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
        
        action.commands.Add(new MultiplyCommand(input.source_actor));
       
        return action;
    }
}

public class TalentExplode : TalentPrototype
{
    public int damage_radius;
    public List<(DamageType type, int damage_min, int damage_max, int penetration)> damage;

    public TalentExplode()
    {
        name = "Explode";
        target = TalentTarget.Self;
        icon = "images/talents/multiply";
        
        description = "Explodes";
        prepare_message = "The <name> starts to beep.";
        action_message = "The <name> explodes.";

        ai_data = new TalentAIInfo
        {
            type = TalentAIInfoType.Special,
            player_range = 1,
            use_probability = 0.8f,
        };

        damage = new();
        damage_radius = 0;
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
     
        List<(DamageType type, int damage, int penetration)> real_damage = new();
        foreach(var v in damage)
            real_damage.Add((v.type, UnityEngine.Random.Range(v.damage_min, v.damage_max + 1), v.penetration));

        action.commands.Add(new ExplodeCommand(input.source_actor,damage_radius, real_damage, true));

        return action;
    }
}

public class TalentBlock : TalentPrototype
{
    public TalentBlock()
    {
        name = "Block";
        target = TalentTarget.Tile;
        target_range = 1;
        icon = "images/talents/sword_attack_heavy";
        cost_stamina = 50;
        recover_time = 200;
        description = "Converts all blocked damage to stamina damage";
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
        return action;
    }

}

public class TalentParry : TalentPrototype
{
    public TalentParry()
    {
        name = "Parry";
        target = TalentTarget.Tile;
        target_range = 1;
        icon = "images/talents/sword_attack_heavy";
        cost_stamina = 50;
        recover_time = 200;
        description = "Tries to block the next incoming melee attack and immediatly start a counter attack";
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
        return action;
    }

}
//TODO: Combine all Throwing Talents into one
public class TalentThrowFirebomb : TalentPrototype
{
    public int level = 0;
    public TalentThrowFirebomb(int level)
    {
        this.level = level;
        name = "Throw Firebomb";
        target = TalentTarget.Tile;
        target_range = 5;
        icon = "images/objects/firebomb";
        cost_stamina = 2;
        recover_time = 50;
        if (level <= 4)
            description = "Creates projectile that deals 5 fire damage in an area with radius 1";
        else
            description = "Creates projectile that deals 10 fire damage in an area with radius 1";

        prepare_message = "The <name> holds a firebomb.";
        action_message = "The <name> throws the firebomb.";
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
             
        ProjectileData bomb = new ProjectileData( input.target_tiles[0].Item1, input.target_tiles[0].Item2, new ProjectileFirebomb(level));
        bomb.path = input.target_tiles;
        bomb.is_shoot_by_player = true;

        action.commands.Add(new CreateProjectileCommand(bomb));

        GameObject.Find("GameData").GetComponent<GameData>().player_data.RemoveItem(input.item, 1);
        return action;
    }

}

public class TalentBossTrollThrowFirebomb : TalentPrototype
{
    public TalentBossTrollThrowFirebomb()
    {
        name = "Exploding Charme";
        target = TalentTarget.Tile;
        target_range = 5;
        icon = "images/objects/firebomb";
        cost_stamina = 0;
        prepare_time = 500;
        recover_time = 200;
        description = "Creates four projectiles that deals fire area damage with radius 1";

        ai_data = new TalentAIInfo
        {
            type = TalentAIInfoType.Special,
            use_probability = 0.3f,            
        };
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
       
        Type type = typeof(ProjectileFirebomb);
        action.commands.Add(new CreateProjectileToPlayerCommand(input.source_actor.id, type,10));
        action.commands.Add(new WaitCommand(200));
        action.commands.Add(new CreateProjectileToPlayerCommand(input.source_actor.id,type,10));
        action.commands.Add(new WaitCommand(200));
        action.commands.Add(new CreateProjectileToPlayerCommand(input.source_actor.id,type,10));
        action.commands.Add(new WaitCommand(200));
        action.commands.Add(new CreateProjectileToPlayerCommand(input.source_actor.id,type,10));

        return action;
    }

}

public class TalentItemThrowThrowingKnife : TalentPrototype
{
    public int level = 0;
    public TalentItemThrowThrowingKnife(int level)
    {
        this.level = level;
        name = "Throw Throwing Knife";
        target = TalentTarget.Tile;
        target_range = 10;
        icon = "images/objects/throwing_knife";
        cost_stamina = 2;
        recover_time = 100;
        if (level <= 4)
            description = "Creates projectile that deals 7 pierce damage to a single target";
        else
            description = "Creates projectile that deals 14 pierce damage to a single target";

        prepare_message = "The <name> holds up a throwing knife.";
        action_message = "The <name> throws the throwing knife.";
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);

        ProjectileData bomb = new ProjectileData(input.target_tiles[0].Item1, input.target_tiles[0].Item2, new ProjectileThrowingKnife(level));
        bomb.path = input.target_tiles;
        bomb.is_shoot_by_player = true;

        action.commands.Add(new CreateProjectileCommand(bomb));

        GameObject.Find("GameData").GetComponent<GameData>().player_data.RemoveItem(input.item, 1);
        return action;
    }
}

public class TalentThrowAcidFlask : TalentPrototype
{
    public int level = 0;
    public TalentThrowAcidFlask(int level)
    {
        this.level = level;
        name = "Throw Acid Flask";
        target = TalentTarget.Tile;
        target_range = 5;
        icon = "images/objects/acid_flask";
        cost_stamina = 2;
        recover_time = 100;
        if (level <= 4)
            description = "Projectile that deals 15 durability damage to armor";
        else
            description = "Projectile that deals 25 durability damage to armor";

        prepare_message = "The <name> holds up an acid flask.";
        action_message = "The <name> throws the acid flask.";

    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);

        ProjectileData bomb = new ProjectileData(input.target_tiles[0].Item1, input.target_tiles[0].Item2, new ProjectileAcidFlask(level));
        bomb.path = input.target_tiles;
        bomb.is_shoot_by_player = true;

        action.commands.Add(new CreateProjectileCommand(bomb));
      
        GameObject.Find("GameData").GetComponent<GameData>().player_data.RemoveItem(input.item, 1);
        return action;
    }

}

public class TalentThrowAtPlayer : TalentPrototype
{
    public Type object_type = null;
    public int level = 0;
    public TalentThrowAtPlayer()
    {
        name = "Throw At Player";
        target = TalentTarget.Tile;
        target_range = 8;
        icon = "images/objects/throwing_knife";
        cost_stamina = 2;
        prepare_time = 50;
        recover_time = 100;
        description = "Creates projectile";

        ai_data = new TalentAIInfo
        {
            type = TalentAIInfoType.Special,   
            use_probability = 0.3f,         
        };
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
              
        action.commands.Add(new CreateProjectileToPlayerCommand(input.source_actor.id, object_type, level));
        
        return action;
    }
}

public class TalentPull : TalentPrototype
{

    public TalentPull()
    {
        name = "Pull";
        target = TalentTarget.Tile;
        target_range = 2;
        icon = "images/talents/blunt_stun";
        cost_stamina = 1;
        cooldown = 500;
        description = "";

        prepare_time = 100;
      
        prepare_message = "The <name> tries to pull.";   
        action_message = "The <name> pulls.";    
 
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
        List<AttackedTileData> tiles = new List<AttackedTileData>();

        ActorEffectSpecialCommand effect = new ActorEffectSpecialCommand
        {
            name = "Pulled",
            command = new PullCommand(input.source_actor, (input.target_tiles[0].Item1, input.target_tiles[0].Item2), 9, (DamageType.CRUSH, 5, 0)),
            damage_type = DamageType.SLASH,
        };
        
        tiles.Add(new AttackedTileData
        {
            x = input.target_tiles[0].Item1,
            y = input.target_tiles[0].Item2,
            damage_on_hit = { (DamageType.SLASH, 5, 0)},
            effects_on_hit = { effect },
        });

        action.commands.Add(new AttackTilesCommand(input.source_actor, tiles, 1, true));

        return action;
    }
}

public class TalentStealLife : TalentPrototype
{

    public TalentStealLife()
    {
        name = "Steal Life";
        target = TalentTarget.Tile;
        target_range = 1;
        icon = "images/talents/blunt_stun";
        cost_stamina = 1;
        cooldown = 100;
        description = "";

        prepare_time = 100;
      
        prepare_message = "The <name> tries to steal life.";   
        action_message = "The <name> steals life.";   

        ai_data = new TalentAIInfo
        {
            type = TalentAIInfoType.Special,
            player_range = 1,
            use_probability = 0.9f,
        }; 
 
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
        List<AttackedTileData> tiles = new List<AttackedTileData>();

        ActorEffectSpecialCommand effect = new ActorEffectSpecialCommand
        {
            name = "Steal Life",
            command = new StealLifeCommand(input.source_actor.id, input.target_tiles[0].Item1, input.target_tiles[0].Item2, 2),
            damage_type = DamageType.PIERCE,
        };
        
        tiles.Add(new AttackedTileData
        {
            x = input.target_tiles[0].Item1,
            y = input.target_tiles[0].Item2,
            damage_on_hit = { (DamageType.PIERCE, 2, 0)},
            effects_on_hit = { effect },
        });

        action.commands.Add(new AttackTilesCommand(input.source_actor, tiles, 1, true));
  
        return action;
    }
}

public class ItemTalentReturn : TalentPrototype
{
    public ItemTalentReturn()
    {
        name = "Return to Village";
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/objects/poem";
        cost_stamina = 5;
        prepare_time = 100;
        recover_time = 100;
        description = "Return to village after 20 turns.";
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
        
        DungeonChangeData dcd = new DungeonChangeData
        {
            name = "Cave Exit",
            dungeon_change_type = typeof(MFCaveExit),
            target_dungeon_name = "High Meadow",
            target_entrance_name = "Cave Entrance",
        };

        EffectData effect = new ActorEffectSpecialCommand 
        { 
            name = "Return to Village",
            icon = "images/objects/poem",
            command = new ChangeDungeonCommand(dcd,input.source_actor),
            duration = 2000, 
            execution_time = EffectDataExecutionTime.END,
        };

        action.commands.Add(new GetEffectCommand(input.source_actor, effect));

        GameObject.Find("GameData").GetComponent<GameData>().player_data.RemoveItem(input.item, 1);
        return action;
    }

}

public class ItemTalentCureDiseases : TalentPrototype
{
    public ItemTalentCureDiseases()
    {
        name = "Cure Common Diseases";
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/objects/tea_red";
        cost_stamina = 0;
        prepare_time = 50;
        recover_time = 100;
        description = "Immediately cures all common diseases and resets disease resistance.";

        prepare_message = "The <name> grabs a cup of tea.";
        action_message = "The <name> drinks the tea.";
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
     
        action.commands.Add(new CureDiseasesCommand(input.source_actor));

        GameObject.Find("GameData").GetComponent<GameData>().player_data.RemoveItem(input.item, 1);
        return action;
    }
}

public class ItemTalentCurePoisons: TalentPrototype
{
    public ItemTalentCurePoisons()
    {
        name = "Cure Common Poisons";
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/objects/tea_green";
        cost_stamina = 0;
        prepare_time = 50;
        recover_time = 100;
        description = "Immediately cures all common poisons and resets poison resistance.";

        prepare_message = "The <name> grabs a cup of tea.";
        action_message = "The <name> drinks the tea.";
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
     
        action.commands.Add(new CurePoisonsCommand(input.source_actor));

        GameObject.Find("GameData").GetComponent<GameData>().player_data.RemoveItem(input.item, 1);
        return action;
    }
}

public class ItemTalentCureInsanity: TalentPrototype
{
    public ItemTalentCureInsanity()
    {
        name = "Cure Insanity";
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/objects/tea_blue";
        cost_stamina = 0;
        prepare_time = 50;
        recover_time = 100;
        description = "Immediately cures insanity and resets insanity resistance.";

        prepare_message = "The <name> grabs a cup of tea.";
        action_message = "The <name> drinks the tea.";
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
     
        action.commands.Add(new CureInsanityCommand(input.source_actor));

        GameObject.Find("GameData").GetComponent<GameData>().player_data.RemoveItem(input.item, 1);
        return action;
    }
}

public class ItemTalentRepairItems : TalentPrototype
{
    public ItemTalentRepairItems()
    {
        name = "Repair Items";
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/objects/repair_powder";
        cost_stamina = 5;
        prepare_time = 100;
        recover_time = 100;
        description = "Repairs all items by 25%.";
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
        
        action.commands.Add(new RepairItemsCommand(25));

        action.prepare_message = "The <name> opens a bag of repair powder.";
        action.action_message = "The <name> throws repair powder into the air.";

        GameObject.Find("GameData").GetComponent<GameData>().player_data.RemoveItem(input.item, 1);
        return action;
    }

}

public class TalentTeleportTarget : TalentPrototype
{

    public TalentTeleportTarget()
    {
        name = "Teleport";
        target = TalentTarget.Tile;
        target_range = 8;
        icon = "images/talents/blunt_stun";
        description = "";

        ai_data = new TalentAIInfo
        {
            type = TalentAIInfoType.Special,
            player_range = 8,
            use_probability = 0.9f,
        };
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
        List<AttackedTileData> tiles = new List<AttackedTileData>();

        action.commands.Add(new TeleportCommand(input.source_actor, input.target_tiles[0]));
   
        return action;
    }
}

public class ItemTalentTeleportRandom : TalentPrototype
{

    public ItemTalentTeleportRandom()
    {
        name = "Teleport";
        target = TalentTarget.Self;
        target_range = 8;
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
        action.commands.Add(new TeleportRandomCommand(input.source_actor, target_range));
  
        GameObject.Find("GameData").GetComponent<GameData>().player_data.RemoveItem(input.item, 1);
        return action;
    }
}

public class ItemTalentGainConsumeEffects : TalentPrototype
{

    public ItemTalentGainConsumeEffects()
    {
        name = "Consume";
        target = TalentTarget.Self;
        target_range = 0;
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ConsumeItemAction(input.item, input.source_actor.prototype.stats.usage_time);
        return action;
    }
}

public class ItemTalentGainUsableEffects : TalentPrototype
{

    public ItemTalentGainUsableEffects()
    {
        name = "Gain Effects";
        target = TalentTarget.Self;
        target_range = 0;
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new UseItemAction(input.item, input.source_actor.prototype.stats.usage_time);
        return action;
    }
}

public class TalentBossTrollEarthquake : TalentPrototype
{
    public TalentBossTrollEarthquake()
    {
        target = TalentTarget.Tile;
        target_range = 3;

        ai_data = new TalentAIInfo
        {
            type = TalentAIInfoType.Special,
            use_probability = 0.3f,
        };
    }
    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
      
        List<AttackedTileData> tiles = new List<AttackedTileData>();

        for (int i = -1; i <= 1 + input.source_actor.prototype.tile_width - 1; ++ i)
        {
            for (int j = -1; j <= 1 + input.source_actor.prototype.tile_height - 1; ++j)
            {
                if (i >= 0 && j >= 0 && i <= input.source_actor.prototype.tile_width -1 && j <= input.source_actor.prototype.tile_height -1)
                    continue;

                tiles.Add(
                    new AttackedTileData
                    {
                        x = input.source_actor.x + i,
                        y = input.source_actor.y + j,
                        damage_on_hit = { (DamageType.CRUSH,10,0)},
                        effects_on_hit = { new EffectStun { damage_type = DamageType.CRUSH, duration = 500} },
                        diseases_on_hit = null,
                        poisons_on_hit= null,
                    }
                );
            }
        }
        action.commands.Add(new AttackTilesCommand(input.source_actor, tiles, 1, false));
        action.commands.Add(new WaitCommand(100));

        tiles = new List<AttackedTileData>();

        for (int i = -2; i <= 2 + input.source_actor.prototype.tile_width - 1; ++ i)
        {
            for (int j = -2; j <= 2 + input.source_actor.prototype.tile_height - 1; ++j)
            {
                if (i >= 0 && j >= 0 && i <= input.source_actor.prototype.tile_width -1 && j <= input.source_actor.prototype.tile_height -1)
                    continue;

                tiles.Add(
                    new AttackedTileData
                    {
                        x = input.source_actor.x + i,
                        y = input.source_actor.y + j,
                        damage_on_hit = { (DamageType.CRUSH,10,0)},
                        effects_on_hit = { new EffectStun { damage_type = DamageType.CRUSH, duration = 500} },
                        diseases_on_hit = null,
                        poisons_on_hit= null,
                    }
                );
            }
        }

        action.commands.Add(new AttackTilesCommand(input.source_actor, tiles, 1, false));
        action.commands.Add(new WaitCommand(100));

        tiles = new List<AttackedTileData>();
        for (int i = -3; i <= 3 + input.source_actor.prototype.tile_width - 1; ++ i)
        {
            for (int j = -3; j <= 3 + input.source_actor.prototype.tile_height - 1; ++j)
            {
                if (i >= 0 && j >= 0 && i <= input.source_actor.prototype.tile_width -1 && j <= input.source_actor.prototype.tile_height -1)
                    continue;

                tiles.Add(
                    new AttackedTileData
                    {
                        x = input.source_actor.x + i,
                        y = input.source_actor.y + j,
                        damage_on_hit = { (DamageType.CRUSH,10,0)},
                        effects_on_hit = { new EffectStun { damage_type = DamageType.CRUSH, duration = 500} },
                        diseases_on_hit = null,
                        poisons_on_hit= null,
                    }
                );
            }
        }
        action.commands.Add(new AttackTilesCommand(input.source_actor, tiles, 1, false));
        return action;
    }
}

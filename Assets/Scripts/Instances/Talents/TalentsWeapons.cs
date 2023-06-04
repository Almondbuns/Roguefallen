using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
//using static UnityEditor.Progress;

//Standard Weapon attacks use weapon attack time as recover time scaled by weapon_attack_time_percentage but can define an additional prepare time

public class TalentWeaponAttackStandard : TalentWeaponAttack
{
    public TalentWeaponAttackStandard()
    {
        name = "Standard Attack";
        target = TalentTarget.Tile;
        target_range = 1;
        icon = "images/talents/blunt_standard";
        cost_stamina = 0;
        prepare_time = 0;
        recover_time = 100;
        description = "Deals 100% weapon damage to one tile";

        weapon_damage_percentage = 100.0f;
        weapon_attack_time_percentage = 100.0f;
        weapon_armor_penetration_add = 0;
    }
}

public class TalentWeaponAttackHeavy : TalentWeaponAttack
{
    public TalentWeaponAttackHeavy()
    {
        name = "Blunt Heavy Attack";
        target = TalentTarget.Tile;
        target_range = 1;
        icon = "images/talents/sword_attack_heavy";
        cost_stamina = 1;
        prepare_time = 0;
        recover_time = 125;
        description = "Deals 100% weapon damage to one tile with +3 armor penetration";

        weapon_damage_percentage = 100.0f;
        weapon_attack_time_percentage = 100.0f;
        weapon_armor_penetration_add = 3;

        prepare_message = "<name> starts a heavy attack.";
    }
}

public class TalentBluntHammerAttackHeavy : TalentWeaponAttack
{
    public TalentBluntHammerAttackHeavy()
    {
        name = "Heavy Hammer Attack";
        target = TalentTarget.Tile;
        target_range = 1;
        icon = "images/talents/blunt_heavy";
        cost_stamina = 1;
        prepare_time = 0;
        recover_time = 100;
        description = "Deals 100% weapon damage to one tile with +2 armor penetration";

        weapon_damage_percentage = 100.0f;
        weapon_attack_time_percentage = 100.0f;
        weapon_armor_penetration_add = 2;

        prepare_message = "<name> starts a heavy attack.";
    }
}

public class TalentBluntMaceAttackHeavy : TalentWeaponAttack
{
    public TalentBluntMaceAttackHeavy()
    {
        name = "Heavy Mace Attack";
        target = TalentTarget.Tile;
        target_range = 1;
        icon = "images/talents/blunt_heavy";
        cost_stamina = 1;
        prepare_time = 0;
        recover_time = 100;
        description = "Deals 125% weapon damage to one tile";

        weapon_damage_percentage = 125.0f;
        weapon_attack_time_percentage = 100.0f;
        weapon_armor_penetration_add = 0;

        prepare_message = "<name> starts a heavy attack.";
    }
}

public class TalentBluntFlailAttackHeavy : TalentWeaponAttack
{
    public TalentBluntFlailAttackHeavy()
    {
        name = "Quick Flail Attack";
        target = TalentTarget.Tile;
        target_range = 1;
        icon = "images/talents/blunt_heavy";
        cost_stamina = 1;
        prepare_time = 0;
        recover_time = 100;
        description = "Deals 100% weapon damage to one tile with 75% attack time";

        weapon_damage_percentage = 100.0f;
        weapon_attack_time_percentage = 75.0f;
        weapon_armor_penetration_add = 0;

        prepare_message = "<name> starts a heavy attack.";
    }
}

public class TalentBluntHammer2HAttackHeavy : TalentWeaponAttack
{
    public TalentBluntHammer2HAttackHeavy()
    {
        name = "Heavy 2H Attack";
        target = TalentTarget.Tile;
        target_range = 1;
        icon = "images/talents/blunt_heavy";
        cost_stamina = 1;
        prepare_time = 0;
        recover_time = 100;
        description = "Deals 100% weapon damage to one tile and 4 durability damage";

        weapon_damage_percentage = 100.0f;
        weapon_attack_time_percentage = 100.0f;
        weapon_armor_penetration_add = 0;
        additional_damage = new List<(DamageType type, int damage_min, int damage_max, int armor_penetration)>
        {
            (DamageType.DURABILITY, 4, 4, 0),
        };

        prepare_message = "<name> starts a heavy attack.";
    }
}

public class TalentBluntAttackStun : TalentWeaponAttack
{

    public TalentBluntAttackStun()
    {
        name = "Stun Attack";
        target = TalentTarget.Tile;
        target_range = 1;
        icon = "images/talents/blunt_stun";
        cost_stamina = 4;
        cooldown = 1000;
        description = "100% weapon damage to one tile and chance to stun the target for 7 turns";

        weapon_damage_percentage = 100.0f;
        weapon_attack_time_percentage = 100.0f;
        effects.Add(new EffectStun { damage_type = DamageType.CRUSH, duration = 700 }) ;

        prepare_time = 50;
        recover_time = 100;

        prepare_message = "The <name> aims for the head.";
    }
}

public class TalentBluntAttackMovementDebuff : TalentWeaponAttack
{

    public TalentBluntAttackMovementDebuff()
    {
        name = "To the Knee";
        target = TalentTarget.Tile;
        target_range = 1;
        icon = "images/talents/blunt_slow";
        cost_stamina = 2;
        cooldown = 1000;
        description = "100% weapon damage to one tile and chance to decrease movement speed of target for 15 turns";

        weapon_damage_percentage = 100.0f;
        weapon_attack_time_percentage = 100.0f;
        effects.Add(new EffectAddMovementTime { amount = 100, damage_type = DamageType.CRUSH, duration = 1500 }) ;

        prepare_time = 50;
        recover_time = 100;

        prepare_message = "The <name> aims for the feet.";
    }
}

public class TalentBluntAttackKnockback : TalentWeaponAttack
{

    public TalentBluntAttackKnockback()
    {
        name = "Knockback";
        target = TalentTarget.Tile;
        target_range = 1;
        icon = "images/talents/blunt_knockback";
        cost_stamina = 4;
        cooldown = 500;
        description = "100% weapon damage to one tile and chance to knock back the target. If there is an object behind the target the object will take damage instead.";

        weapon_damage_percentage = 100.0f;
        prepare_time = 25;
        weapon_attack_time_percentage = 100.0f;
      
        prepare_message = "<name> starts a knockback attack.";        
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
        List<AttackedTileData> tiles = new List<AttackedTileData>();

        ItemData weapon = GetWeapon(input);
        List<(DamageType, int, int)> dealt_damage = GetWeaponDamage(weapon, input);
        SetStandardActionParameters(action, weapon, input);

        int push_damage = 0;
        foreach (var v in weapon.GetWeaponDamage())
        {
            push_damage += v.damage_min;
        }

        ActorEffectSpecialCommand effect = new ActorEffectSpecialCommand
        {
            name = "Knockback",
            command = new KnockbackCommand(input.source_actor, (input.target_tiles[0].Item1, input.target_tiles[0].Item2), 3, (DamageType.CRUSH, push_damage, 0)),
            damage_type = DamageType.CRUSH,
        };

        effects.Clear(); //Might include added effects from last creation
        effects.Add(effect);
        
        tiles.Add(new AttackedTileData
        {
            x = input.target_tiles[0].Item1,
            y = input.target_tiles[0].Item2,
            damage_on_hit = dealt_damage,
            effects_on_hit = this.effects,
        });

        action.commands.Add(new AttackTilesCommand(input.source_actor, tiles, 1, true));

        return action;
    }
}

public class TalentWeaponAttackFast : TalentWeaponAttack
{

    public TalentWeaponAttackFast()
    {
        name = "Fast Attack";
        target = TalentTarget.Tile;
        target_range = 1;
        icon = "images/talents/sword_attack_fast";
        cost_stamina = 5;
        recover_time = 50;
        description = "Deals 60% weapon damage to one tile";

        weapon_damage_percentage = 60.0f;
        weapon_attack_time_percentage = 50.0f;
    }
}

public class TalentAxeAttackInterrupt : TalentWeaponAttack
{
    public TalentAxeAttackInterrupt()
    {
        name = "Haft Punch";
        target = TalentTarget.Tile;
        target_range = 1;
        icon = "images/talents/sword_attack_fast";
        cost_stamina = 10;
        recover_time = 25;
        description = "50% weapon damage as crush damage with chance to interrupt target";
        effects.Add(new EffectInterrupt { damage_type = DamageType.CRUSH });

        weapon_damage_percentage = 50.0f;
        weapon_attack_time_percentage = 200.0f;

        prepare_time = 24;
        prepare_message = "<name> grabs the hilt of the weapon.";
    }
}



public class TalentAxeAttackAdjacent : TalentPrototype
{
    public TalentAxeAttackAdjacent()
    {
        name = "Swipe Attack";
        target = TalentTarget.AdjacentTiles;
        target_range = 1;
        icon = "images/talents/sword_attack_heavy";
        cost_stamina = 10;
        recover_time = 50;
        description = "100% weapon damage to target tile and two adjacent tiles";
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
        List<(DamageType, int, int)> dealt_damage = new();
        foreach (var damage_per_type in input.item.GetWeaponDamage())
        {
            dealt_damage.Add((damage_per_type.type, Random.Range(damage_per_type.damage_min, damage_per_type.damage_max + 1), damage_per_type.armor_penetration));
        }

        //Calculate adjacent tiles
        List<(int x, int y)> adjacent_tiles = new();
        int relative_x = input.target_tiles[0].Item1 - input.source_actor.x;
        int relative_y = input.target_tiles[0].Item2 - input.source_actor.y;
        if (relative_x == -1 && relative_y == -1)
        {
            adjacent_tiles.Add((input.source_actor.x, input.source_actor.y - 1));
            adjacent_tiles.Add((input.target_tiles[0].Item1, input.target_tiles[0].Item2));
            adjacent_tiles.Add((input.source_actor.x - 1, input.source_actor.y));
        }
        if (relative_x == -1 && relative_y == 0)
        {
            adjacent_tiles.Add((input.source_actor.x - 1, input.source_actor.y - 1));
            adjacent_tiles.Add((input.target_tiles[0].Item1, input.target_tiles[0].Item2));
            adjacent_tiles.Add((input.source_actor.x - 1, input.source_actor.y + 1));
        }
        if (relative_x == -1 && relative_y == 1)
        {
            adjacent_tiles.Add((input.source_actor.x - 1, input.source_actor.y));
            adjacent_tiles.Add((input.target_tiles[0].Item1, input.target_tiles[0].Item2));
            adjacent_tiles.Add((input.source_actor.x, input.source_actor.y + 1));
        }
        if (relative_x == 0 && relative_y == 1)
        {
            adjacent_tiles.Add((input.source_actor.x - 1, input.source_actor.y + 1));
            adjacent_tiles.Add((input.target_tiles[0].Item1, input.target_tiles[0].Item2));
            adjacent_tiles.Add((input.source_actor.x + 1, input.source_actor.y + 1));
        }
        if (relative_x == 1 && relative_y == 1)
        {
            adjacent_tiles.Add((input.source_actor.x, input.source_actor.y + 1));
            adjacent_tiles.Add((input.target_tiles[0].Item1, input.target_tiles[0].Item2));
            adjacent_tiles.Add((input.source_actor.x + 1, input.source_actor.y));
        }
        if (relative_x == 1 && relative_y == 0)
        {
            adjacent_tiles.Add((input.source_actor.x + 1, input.source_actor.y + 1));
            adjacent_tiles.Add((input.target_tiles[0].Item1, input.target_tiles[0].Item2));
            adjacent_tiles.Add((input.source_actor.x + 1, input.source_actor.y - 1));
        }
        if (relative_x == 1 && relative_y == -1)
        {
            adjacent_tiles.Add((input.source_actor.x + 1, input.source_actor.y));
            adjacent_tiles.Add((input.target_tiles[0].Item1, input.target_tiles[0].Item2));
            adjacent_tiles.Add((input.source_actor.x, input.source_actor.y - 1));
        }
        if (relative_x == 0 && relative_y == -1)
        {
            adjacent_tiles.Add((input.source_actor.x + 1, input.source_actor.y - 1));
            adjacent_tiles.Add((input.target_tiles[0].Item1, input.target_tiles[0].Item2));
            adjacent_tiles.Add((input.source_actor.x - 1, input.source_actor.y - 1));
        }

        foreach ((int x, int y) in adjacent_tiles)
        {
            List<AttackedTileData> tiles = new List<AttackedTileData>();
            tiles.Add(new AttackedTileData
            {
                x = x,
                y = y,
                damage_on_hit = dealt_damage,
            });

            action.commands.Add(new AttackTilesCommand(input.source_actor, tiles, 1, true));
        }

        action.prepare_time = 24;
        action.prepare_message = "The <name> draws an axe.";
        action.action_message = "The <name> attacks with an axe.";
        action.recover_time = 25;
        return action;
    }

}

public class TalentBluntAccuracyBonus : TalentSubstainedEffects
{
    public TalentBluntAccuracyBonus()
    {
        name = "Blunt Concentration";
        type = TalentType.Substained;
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/blunt_concentration";
        cost_stamina = 5;
        prepare_time = 100;
        recover_time = 100;
        description = "A modular stance that increases chance to hit (+10) but also attack time (+25).";

        substained_effects = new List<EffectData>();

        substained_effects.Add(new EffectAddToHit { amount = 10 });
        substained_effects.Add(new EffectAddAttackTime { amount = 25 });
    }
}

public class TalentBluntDoubleAttack : TalentWeaponAttack
{

    public TalentBluntDoubleAttack()
    {
        name = "Double Attack";
        target = TalentTarget.Tile;
        target_range = 1;
        icon = "images/talents/blunt_2x";
        cost_stamina = 2;
        cooldown = 500;
        description = "Two consecutive blows each with 50% attack time and 75% weapon damage to one tile.";

        weapon_damage_percentage = 75.0f;
        prepare_time = 10;
        weapon_attack_time_percentage = 100.0f;

        prepare_message = "<name> starts a double attack.";
        action_message = "<name> attacks two times.";
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);

        ItemData weapon = GetWeapon(input);
        
        SetStandardActionParameters(action, weapon, input);

        List<(DamageType, int, int)> dealt_damage_1 = GetWeaponDamage(weapon,input);
      
        List<AttackedTileData> tiles_1 = new List<AttackedTileData>();
        tiles_1.Add(new AttackedTileData
        {
            x = input.target_tiles[0].Item1,
            y = input.target_tiles[0].Item2,
            damage_on_hit = dealt_damage_1,
            effects_on_hit = this.effects,
        });

        action.commands.Add(new AttackTilesCommand(input.source_actor, tiles_1, 1, true));
        action.commands.Add(new WaitCommand(action.recover_time / 2));

        List<(DamageType, int, int)> dealt_damage_2 = GetWeaponDamage(weapon, input);
        
        List<AttackedTileData> tiles_2 = new List<AttackedTileData>();
        tiles_1.Add(new AttackedTileData
        {
            x = input.target_tiles[0].Item1,
            y = input.target_tiles[0].Item2,
            damage_on_hit = dealt_damage_2,
            effects_on_hit = this.effects,
        });

        action.commands.Add(new AttackTilesCommand(input.source_actor, tiles_2, 1, true));

        action.recover_time = action.recover_time / 2;

        return action;
    }
}

public class TalentBluntEarthquake : TalentWeaponAttack
{

    public TalentBluntEarthquake()
    {
        name = "Earthquake";
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/blunt_earthquake";
        cost_stamina = 10;
        cooldown = 1000;
        description = "50% weapon damage to all tiles in a radius of 3";

        weapon_damage_percentage = 50.0f;
        weapon_attack_time_percentage = 100.0f;
    
        prepare_time = 100;
        recover_time = 100;

        prepare_message = "<name> raises their weapon high up into the air.";
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
        List<AttackedTileData> tiles = new List<AttackedTileData>();

        ItemData weapon = GetWeapon(input);
        List<(DamageType, int, int)> dealt_damage = GetWeaponDamage(weapon,input);
        SetStandardActionParameters(action, weapon, input);

        action.action_message = "The <name> slams the " + weapon.GetName() + "into the ground. The ground shakes.";

        int distance = 3;

        for (int i = -distance; i <= distance; ++i)
        {
            for (int j = -distance; j <= distance; ++j)
            {
                if (i == 0 && j == 0) continue; // don't hurt yourself

                tiles.Add(new AttackedTileData
                {
                    x = input.source_actor.x + i,
                    y = input.source_actor.y + j,
                    damage_on_hit = dealt_damage,
                    effects_on_hit = this.effects,
                });
            }
        }

        action.commands.Add(new AttackTilesCommand(input.source_actor, tiles, 1, false));

        return action;
    }
}

public class TalentBluntAdeptBlunt : TalentPassiveEffects
{
    public TalentBluntAdeptBlunt()
    {
        name = "Adept Bluntoneer";
        type = TalentType.Passive;
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/blunt_plus";

        description = "Increases the damage of blunt weapons by 1.";

        passive_effects = new List<EffectData>();

        passive_effects.Add(new EffectAddMinWeaponDamage { amount = 1 });
        passive_effects.Add(new EffectAddMaxWeaponDamage { amount = 1 });
    }
}

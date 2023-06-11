using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

//Standard Weapon attacks use weapon attack time as recover time scaled by weapon_attack_time_percentage but can define an additional prepare time

public class TalentAxeWarAxeAttackHeavy : TalentWeaponAttack
{
    public TalentAxeWarAxeAttackHeavy()
    {
        name = "Heavy War Axe Attack";
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

public class TalentAxeAttackInterrupt : TalentWeaponAttack
{
    public TalentAxeAttackInterrupt()
    {
        name = "Haft Punch";
        target = TalentTarget.Tile;
        target_range = 1;
        icon = "images/talents/sword_attack_fast";
        cost_stamina = 2;
        description = "50% weapon damage as crush damage with chance to interrupt target";
        effects.Add(new EffectInterrupt { damage_type = DamageType.CRUSH });

        weapon_damage_percentage = 50.0f;
        weapon_attack_time_percentage = 100.0f;

        prepare_message = "<name> grabs the hilt of the weapon.";
    }
}

public class TalentAxeAttackAdjacent : TalentWeaponAttack
{
    public TalentAxeAttackAdjacent()
    {
        name = "Swipe Attack";
        target = TalentTarget.AdjacentTiles;
        target_range = 1;
        icon = "images/talents/sword_attack_heavy";
        cost_stamina = 2;
        prepare_time = 50;
        cooldown = 400;
        description = "100% weapon damage to target tile and two adjacent tiles";
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
        
        ItemData weapon = GetWeapon(input);
        List<(DamageType, int, int)> dealt_damage = GetWeaponDamage(weapon, input);
        SetStandardActionParameters(action, weapon, input);

        //Calculate adjacent tiles
        List<(int x, int y)> adjacent_tiles = new();
        int relative_x = input.target_tiles[0].Item1 - input.source_actor.X;
        int relative_y = input.target_tiles[0].Item2 - input.source_actor.Y;
        if (relative_x == -1 && relative_y == -1)
        {
            adjacent_tiles.Add((input.source_actor.X, input.source_actor.Y - 1));
            adjacent_tiles.Add((input.target_tiles[0].Item1, input.target_tiles[0].Item2));
            adjacent_tiles.Add((input.source_actor.X - 1, input.source_actor.Y));
        }
        if (relative_x == -1 && relative_y == 0)
        {
            adjacent_tiles.Add((input.source_actor.X - 1, input.source_actor.Y - 1));
            adjacent_tiles.Add((input.target_tiles[0].Item1, input.target_tiles[0].Item2));
            adjacent_tiles.Add((input.source_actor.X - 1, input.source_actor.Y + 1));
        }
        if (relative_x == -1 && relative_y == 1)
        {
            adjacent_tiles.Add((input.source_actor.X - 1, input.source_actor.Y));
            adjacent_tiles.Add((input.target_tiles[0].Item1, input.target_tiles[0].Item2));
            adjacent_tiles.Add((input.source_actor.X, input.source_actor.Y + 1));
        }
        if (relative_x == 0 && relative_y == 1)
        {
            adjacent_tiles.Add((input.source_actor.X - 1, input.source_actor.Y + 1));
            adjacent_tiles.Add((input.target_tiles[0].Item1, input.target_tiles[0].Item2));
            adjacent_tiles.Add((input.source_actor.X + 1, input.source_actor.Y + 1));
        }
        if (relative_x == 1 && relative_y == 1)
        {
            adjacent_tiles.Add((input.source_actor.X, input.source_actor.Y + 1));
            adjacent_tiles.Add((input.target_tiles[0].Item1, input.target_tiles[0].Item2));
            adjacent_tiles.Add((input.source_actor.X + 1, input.source_actor.Y));
        }
        if (relative_x == 1 && relative_y == 0)
        {
            adjacent_tiles.Add((input.source_actor.X + 1, input.source_actor.Y + 1));
            adjacent_tiles.Add((input.target_tiles[0].Item1, input.target_tiles[0].Item2));
            adjacent_tiles.Add((input.source_actor.X + 1, input.source_actor.Y - 1));
        }
        if (relative_x == 1 && relative_y == -1)
        {
            adjacent_tiles.Add((input.source_actor.X + 1, input.source_actor.Y));
            adjacent_tiles.Add((input.target_tiles[0].Item1, input.target_tiles[0].Item2));
            adjacent_tiles.Add((input.source_actor.X, input.source_actor.Y - 1));
        }
        if (relative_x == 0 && relative_y == -1)
        {
            adjacent_tiles.Add((input.source_actor.X + 1, input.source_actor.Y - 1));
            adjacent_tiles.Add((input.target_tiles[0].Item1, input.target_tiles[0].Item2));
            adjacent_tiles.Add((input.source_actor.X - 1, input.source_actor.Y - 1));
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

        return action;
    }

}

public class TalentAxeSpin : TalentAreaWeaponAttack
{

    public TalentAxeSpin()
    {
        name = "Axe Spin";
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/blunt_earthquake";
        cost_stamina = 3;
        cooldown = 400;
        description = "100% weapon damage to all tiles in a radius of 1";

        weapon_damage_percentage = 100.0f;
        weapon_attack_time_percentage = 100.0f;
    
        prepare_time = 50;

        prepare_message = "The <name> holds the axe firmly.";
        action_message = "The <name> spins around.";

        distance = 1;
    }
}

public class TalentAxeAdeptAxes : TalentPassiveEffects
{
    public TalentAxeAdeptAxes()
    {
        name = "Adept Axeslasher";
        type = TalentType.Passive;
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/blunt_plus";

        description = "Increases the damage of axes by 1.";

        passive_effects = new List<EffectData>();

        passive_effects.Add(new EffectAddMinWeaponDamage { amount = 1 });
        passive_effects.Add(new EffectAddMaxWeaponDamage { amount = 1 });
    }
}

public class TalentAxeAttackMovementDebuff : TalentWeaponAttack
{

    public TalentAxeAttackMovementDebuff()
    {
        name = "To the Knee";
        target = TalentTarget.Tile;
        target_range = 1;
        icon = "images/talents/blunt_slow";
        cost_stamina = 2;
        cooldown = 1000;
        description = "150% weapon damage to one tile and chance to decrease movement speed of target for 15 turns";

        weapon_damage_percentage = 150.0f;
        weapon_attack_time_percentage = 100.0f;
        effects.Add(new EffectAddMovementTime { amount = 100, damage_type = DamageType.SLASH, duration = 1500 }) ;

        prepare_time = 50;

        prepare_message = "The <name> aims for the feet.";
    }
}
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
        icon = "images/talents/axe_heavy";
        cost_stamina = 1;
        prepare_time = 0;
        recover_time = 0;
        description = "Deals 125% weapon damage to one tile";

        weapon_damage_percentage = 125.0f;
        weapon_attack_time_percentage = 100.0f;
        weapon_armor_penetration_add = 0;

        prepare_message = "<name> starts a heavy attack.";
    }
}

public class TalentAxeBattleAxeAttackHeavy : TalentAreaWeaponAttack
{
    public TalentAxeBattleAxeAttackHeavy()
    {
        name = "Heavy Battle Axe Attack";
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/axe_heavy";
        cost_stamina = 1;
        prepare_time = 0;
        recover_time = 0;
        description = "Deals 50% weapon damage to all tiles at a distance of two tiles";

        weapon_damage_percentage = 50.0f;
        weapon_attack_time_percentage = 100.0f;

        prepare_message = "The <name> grabs the battle axe with both hands.";
        action_message = "The <name> spins around.";

        distance = 2;
        include_closer_tiles = false;
    }
}

public class TalentAxeDoubleAxeAttackHeavy : TalentWeaponAttack
{
    public TalentAxeDoubleAxeAttackHeavy()
    {
        name = "Heavy Double Axe Attack";
        target = TalentTarget.Tile;
        target_range = 1;
        icon = "images/talents/axe_heavy";
        cost_stamina = 1;
        prepare_time = 0;
        recover_time = 0;
        description = "Two quick blows each dealing 75% weapon damage to one tile with 50% attack time";

        weapon_damage_percentage = 75.0f;
        weapon_attack_time_percentage = 100.0f;
        weapon_armor_penetration_add = 0;

        prepare_message = "<name> starts a heavy attack.";
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
        action.commands.Add(new WaitCommand(action.recover_time / 2 - 1));

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

        action.recover_time = action.recover_time / 2 - 1;

        return action;
    }
}

public class TalentAxePickaxeAttackHeavy : TalentWeaponAttack
{
    public TalentAxePickaxeAttackHeavy()
    {
        name = "Heavy Pickaxe Attack";
        target = TalentTarget.Tile;
        target_range = 1;
        icon = "images/talents/axe_heavy";
        cost_stamina = 1;
        prepare_time = 0;
        recover_time = 0;
        description = "Deals 100% weapon damage to one tile with +3 armor penetration";

        weapon_damage_percentage = 100.0f;
        weapon_attack_time_percentage = 100.0f;
        weapon_armor_penetration_add = 2;

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
        icon = "images/talents/axe_haft";
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
        icon = "images/talents/axe_swipe";
        cost_stamina = 2;
        prepare_time = 25;
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
        icon = "images/talents/axe_spin";
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
        icon = "images/talents/axe_plus";

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
        icon = "images/talents/axe_slow";
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

public class TalentAxeFrenzy : TalentSubstainedEffects
{
    public TalentAxeFrenzy()
    {
        name = "Frenzy Swings";
        type = TalentType.Substained;
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/axe_frenzy";
        cost_stamina = 5;
        prepare_time = 0;
        recover_time = 100;
        cooldown = 100;
        description = "Deals +1 to +3 damage per attack but loses one point of stamina each turn.";

        substained_effects = new List<EffectData>();

        substained_effects.Add(new EffectAddMaxWeaponDamage { amount = 3 });
        substained_effects.Add(new EffectAddMinWeaponDamage { amount = 1 });
        substained_effects.Add(new EffectSubstractStamina{ amount = 1, execution_time = EffectDataExecutionTime.CONTINUOUS });
    }
}

public class TalentAxeMastery : TalentPassiveEffects
{
    public TalentAxeMastery()
    {
        name = "Axe Mastery";
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/axe_mastery";
        cost_stamina = 0;
        recover_time = 0;
        cooldown = 0;
        description = "All attacks with axes deal +1 damage with +1 armor penetration.";

        passive_effects = new List<EffectData>();

        passive_effects.Add(new EffectAddWeaponPenetration { amount = 1});
        passive_effects.Add(new EffectAddMinWeaponDamage { amount = 1 });
        passive_effects.Add(new EffectAddMaxWeaponDamage { amount = 1 });
    }
}

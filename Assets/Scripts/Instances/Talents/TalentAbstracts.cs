using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TalentStandardMeleeAttack : TalentPrototype
{
    public List<(DamageType damage_type, int damage_min, int damage_max, int armor_penetration)> damage;
    public List<EffectData> effects;
    public List<Type> diseases;
    public List<Type> poisons;

    public TalentStandardMeleeAttack()
    {
        effects = new();
        diseases = new();
        poisons = new();
        damage = new();

        target = TalentTarget.Tile;
        target_range = 1;

        ai_data = new TalentAIInfo
        {
            type = TalentAIInfoType.CombatMelee
        };

        recover_time = 100;
    }
    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
        List<AttackedTileData> tiles = new List<AttackedTileData>();
        var actual_damage = new List<(DamageType, int, int)>();
        foreach (var v in damage)
            actual_damage.Add((v.damage_type, UnityEngine.Random.Range(v.damage_min, v.damage_max + 1), v.armor_penetration));
        tiles.Add(new AttackedTileData
        {
            x = input.target_tiles[0].Item1,
            y = input.target_tiles[0].Item2,
            damage_on_hit = actual_damage,
            effects_on_hit = effects,
            diseases_on_hit = diseases,
            poisons_on_hit=poisons,
        }); ;

        action.commands.Add(new AttackTilesCommand(input.source_actor, tiles, 1, true));
        
        return action;
    }
}


public abstract class TalentSubstainedEffects : TalentPrototype
{
    public List<EffectData> substained_effects;

    public TalentSubstainedEffects()
    {
        name = "Substained Effects";
        type = TalentType.Substained;
        target = TalentTarget.Self;
        target_range = 1;
        icon = "images/talents/sword_attack_heavy";
        cost_stamina = 10;
        recover_time = 50;
        description = "Substained Effects";

        substained_effects = new List<EffectData>();

        substained_effects.Add(new EffectAddStrength { amount = 10 });
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);

        if (input.source_actor.current_substained_talents_id.Contains(input.talent.id) == false)
        {
            action.commands.Add(new ActivateSubstainedTalentCommand(input.source_actor.id, input.talent.id));
            action.prepare_message = "The <name> tries to substain: " + name + ".";
            action.action_message = "The <name> is substaining " + name + ".";
        }
        else
        {
            action.commands.Add(new DeactivateSubstainedTalentCommand(input.source_actor.id, input.talent.id));
            action.prepare_message = "The <name> tries ending to substain: " + name + ".";
            action.action_message = "The <name> is no longer substaining " + name + ".";
        }

        action.prepare_time = prepare_time;

        action.recover_time = recover_time;
        return action;
    }

}

public abstract class TalentPassiveEffects : TalentPrototype
{
    public List<EffectData> passive_effects;

    public TalentPassiveEffects()
    {
        name = "Passive Effects";
        type = TalentType.Passive;
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/sword_attack_heavy";

        description = "Passive Effects";

        passive_effects = new List<EffectData>();
    }

    //Usually no action is needed with passive talents since they will be automatically activated when unlocked 
    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);

        return action;
    }

}

public class TalentActiveEffects : TalentPrototype
{
    public List<EffectData> active_effects;

    public TalentActiveEffects()
    {
        name = "Active Effects";
        type = TalentType.Active;
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/sword_attack_heavy";

        description = "Active Effects";

        active_effects = new List<EffectData>();
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);

        foreach(var e in active_effects)
            action.commands.Add(new GetEffectCommand(input.source_actor, e));

        return action;
    }
}

public abstract class TalentWeaponAttack : TalentPrototype
{
    public float weapon_damage_percentage = 100.0f;
    public float weapon_attack_time_percentage = 100.0f;
    public int weapon_armor_penetration_add = 0;
    public List <(DamageType type, int damage_min, int damage_max, int armor_penetration)> additional_damage;

    public List<EffectData> effects;

    public TalentWeaponAttack()
    {
        name = "";
        target = TalentTarget.Tile;
        target_range = 1;
        icon = "images/talents/sword_attack_standard";
        cost_stamina = 10;
        description = "Deals 100% weapon damage to one tile";

        effects = new();
    }

    public ItemData GetWeapon(TalentInputData input)
    {
        ItemData item = null;
        if (input.item == null)
        {
            //Uses main weapon hand
            PlayerData player = GameObject.Find("GameData").GetComponent<GameData>().player_data;
            item = player.GetMainWeapon();
        }
        else
        {
            item = input.item;
        }

        if (item == null || item.weapon_data == null)
        {
            Debug.LogError("Error: No suitable weapon found in talent: " + this.name);
            return null;
        }

        return item;
    }

    public List<(DamageType, int, int)> GetWeaponDamage(ItemData item, TalentInputData input)
    {
        List<(DamageType, int, int)> dealt_damage = new();

        foreach ((DamageType type, int damage_min, int damage_max, int armor_penetration) damage_per_type in item.GetWeaponDamage())
        {
            int damage_min = damage_per_type.damage_min + input.source_actor.GetCurrentAdditiveEffectAmount<EffectAddMinWeaponDamage>();
            int damage_max = damage_per_type.damage_max + input.source_actor.GetCurrentAdditiveEffectAmount<EffectAddMaxWeaponDamage>() + 1;
            dealt_damage.Add((damage_per_type.type, Mathf.RoundToInt(UnityEngine.Random.Range(damage_min, damage_max) * weapon_damage_percentage / 100.0f), damage_per_type.armor_penetration + weapon_armor_penetration_add));
        }
        
        if (additional_damage != null)
        {
            foreach ((DamageType type, int damage_min, int damage_max, int armor_penetration) damage_per_type in additional_damage)
            {
                dealt_damage.Add((damage_per_type.type, UnityEngine.Random.Range(damage_per_type.damage_min, damage_per_type.damage_max + 1), damage_per_type.armor_penetration));
            }
        }

        return dealt_damage;
    }

    public void SetStandardActionParameters(ActionData action, ItemData weapon, TalentInputData input)
    {
        action.prepare_message = prepare_message;
        action.prepare_time = prepare_time;
        if (action_message == "")
            action.action_message = "The <name> attacks with the " + weapon.GetName() + ".";
        action.recover_time = Mathf.RoundToInt(weapon.GetWeaponAttackTime() * (input.source_actor.GetAttackTime() / 100.0f) * (weapon_attack_time_percentage / 100.0f));
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
        List<AttackedTileData> tiles = new List<AttackedTileData>();

        ItemData weapon = GetWeapon(input);
        List<(DamageType, int, int)> dealt_damage = GetWeaponDamage(weapon, input);
        SetStandardActionParameters(action, weapon, input);

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

public abstract class TalentAreaWeaponAttack : TalentWeaponAttack
{
    public int distance = 1;

    public TalentAreaWeaponAttack()
    {
        name = "Area Attack";
        target = TalentTarget.Self;
        target_range = 0;
        
        weapon_damage_percentage = 100.0f;
        weapon_attack_time_percentage = 100.0f;
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
        List<AttackedTileData> tiles = new List<AttackedTileData>();

        ItemData weapon = GetWeapon(input);
        List<(DamageType, int, int)> dealt_damage = GetWeaponDamage(weapon,input);
        SetStandardActionParameters(action, weapon, input);

        for (int i = -distance; i <= distance; ++i)
        {
            for (int j = -distance; j <= distance; ++j)
            {
                if (i == 0 && j == 0) continue; // don't hurt yourself

                tiles.Add(new AttackedTileData
                {
                    x = input.source_actor.X + i,
                    y = input.source_actor.Y + j,
                    damage_on_hit = dealt_damage,
                    effects_on_hit = this.effects,
                });
            }
        }

        action.commands.Add(new AttackTilesCommand(input.source_actor, tiles, 1, false));
        action.recover_time -= 1;

        return action;
    }
}


public class TalentRadiusAllAttack : TalentPrototype
{
    public List<(DamageType damage_type, int damage_min, int damage_max, int armor_penetration)> damage;
    public List<EffectData> effects;
    public List<Type> diseases;
    public List<Type> poisons;
    public TalentRadiusAllAttack()
    {
        target = TalentTarget.Tile;
        target_range = 3;

        effects = new();
        diseases = new();
        poisons = new();
        damage = new();
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);

        var actual_damage = new List<(DamageType, int, int)>();
        foreach (var v in damage)
            actual_damage.Add((v.damage_type, UnityEngine.Random.Range(v.damage_min, v.damage_max + 1), v.armor_penetration));
   
        List<AttackedTileData> tiles = new List<AttackedTileData>();

        for (int i = - target_range; i <= target_range + input.source_actor.prototype.tile_width - 1; ++ i)
        {
            for (int j = -target_range; j <= target_range + input.source_actor.prototype.tile_height - 1; ++j)
            {
                if (i >= 0 && j >= 0 && i <= input.source_actor.prototype.tile_width -1 && j <= input.source_actor.prototype.tile_height -1)
                    continue;

                tiles.Add(
                    new AttackedTileData
                    {
                        x = input.source_actor.X + i,
                        y = input.source_actor.Y + j,
                        damage_on_hit = actual_damage,
                        effects_on_hit = effects,
                        diseases_on_hit = diseases,
                        poisons_on_hit=poisons,
                    }
                );
            }
        }
        action.commands.Add(new AttackTilesCommand(input.source_actor, tiles, 1, false));
        action.recover_time -= 1;
        
        return action;
    }
}
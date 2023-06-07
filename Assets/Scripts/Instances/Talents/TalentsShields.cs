using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TalentShieldBash : TalentPrototype
{
    public TalentShieldBash()
    {
        name = "Shield Bash";
        target = TalentTarget.Tile;
        target_range = 1;
        icon = "images/talents/shield_bash";
        cost_stamina = 2;
        recover_time = 100;
        cooldown = 800;
        description = "Attacks with the shield and deals crush damage equal to the sum of its armor values. May daze the target for 3 turns lowering to hit and dodge by 30.";
        action_message ="The <name> bashes with a shield.";
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);

        int damage = (input.item.GetPrototype().armor.armor_physical + input.item.GetPrototype().armor.armor_elemental + input.item.GetPrototype().armor.armor_magical);
        damage = damage * (100 + input.source_actor.GetCurrentAdditiveEffectAmount<EffectBashDamageRelative>()) / 100;
        List<AttackedTileData> tiles = new List<AttackedTileData>();

        int daze_duration = 300 + input.source_actor.GetCurrentAdditiveEffectAmount<EffectDazeTime>();
         tiles.Add(new AttackedTileData
        {
            x = input.target_tiles[0].Item1,
            y = input.target_tiles[0].Item2,
            damage_on_hit = new (){(DamageType.CRUSH, damage, 0)},
            effects_on_hit = new ()
            {
                new EffectAddToHit(){damage_type = DamageType.CRUSH, amount = -30, duration = daze_duration},
                new EffectAddDodge(){damage_type = DamageType.CRUSH, amount = -30, duration = daze_duration},
            },
        });
        
        action.commands.Add(new AttackTilesCommand(input.source_actor, tiles, 1, true));

        return action;
    }

}

public class TalentShieldParry : TalentPrototype
{
    public TalentShieldParry()
    {
        name = "Shield Parry";
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/shield_parry";
        cost_stamina = 2;
        recover_time = 100;
        cooldown = 300;
        description = "40% chance to parry an attack within the next turn negating the damage and leaving the source open for a double damage counterattack.";
        action_message ="The <name> prepares to parry.";
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);

        action.commands.Add(new GetEffectCommand(input.source_actor, 
        new EffectParryChance(){damage_type = DamageType.SLASH, duration = 100, amount = 40,}));

        return action;
    }

}

public class TalentShieldPassiveBlock : TalentPassiveEffects
{
    public TalentShieldPassiveBlock()
    {
        name = "Passive Block";
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/shield_1";
        cost_stamina = 0;
        recover_time = 0;
        cooldown = 0;
        description = "When attacked 20% chance to add shield armor to body armor.";

        passive_effects = new List<EffectData>();

        passive_effects.Add(new EffectPassiveBlock { amount = 20 });

    }
}

public class TalentShieldBlockMastery : TalentPassiveEffects
{
    public TalentShieldBlockMastery()
    {
        name = "Masterful Block";
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/shield_3";
        cost_stamina = 0;
        recover_time = 0;
        cooldown = 0;
        description = "Additional 20% passive block chance, 15% parry chance and 50% bash damage.";

        passive_effects = new List<EffectData>();

        passive_effects.Add(new EffectPassiveBlock { amount = 20 });
        passive_effects.Add(new EffectParryChanceBonus { amount = 15 });
        passive_effects.Add(new EffectBashDamageRelative { amount = 50 });

    }
}

public class TalentShieldPassiveBlockSkilled : TalentPassiveEffects
{
    public TalentShieldPassiveBlockSkilled()
    {
        name = "Skilled Passive Block";
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/shield_2";
        cost_stamina = 0;
        recover_time = 0;
        cooldown = 0;
        description = "When attacked additional 10% chance to add shield armor to body armor.";

        passive_effects = new List<EffectData>();

        passive_effects.Add(new EffectPassiveBlock { amount = 10 });

    }
}

public class TalentShieldActiveBlock : TalentSubstainedEffects
{
    public TalentShieldActiveBlock()
    {
        name = "Active Block";
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/shield_active_block";
        cost_stamina = 1;
        recover_time = 50;
        cooldown = 0;
        description = "When attacked adds shield armor to body armor but loses 1 stamina with each hit and increase movement time by 50.";

        substained_effects = new List<EffectData>();

        substained_effects.Add(new EffectActiveBlock { amount = 100 });
        substained_effects.Add(new EffectAddMovementTime { amount = 50 });

    }
}

public class TalentShieldAdvancedParry : TalentPassiveEffects
{
    public TalentShieldAdvancedParry()
    {
        name = "Advanced Parry";
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/shield_parry_plus";
        cost_stamina = 0;
        recover_time = 0;
        cooldown = 0;
        description = "When using parry with a shield increase chance to parry by 20%.";

        passive_effects = new List<EffectData>();

        passive_effects.Add(new EffectParryChanceBonus { amount = 20 });

    }
}

public class TalentShieldAdvancedBash : TalentPassiveEffects
{
    public TalentShieldAdvancedBash()
    {
        name = "Advanced Bash";
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/shield_bash_plus";
        cost_stamina = 0;
        recover_time = 0;
        cooldown = 0;
        description = "When using bash with a shield increase damage by 50% and increase daze time by one turn.";

        passive_effects = new List<EffectData>();

        passive_effects.Add(new EffectBashDamageRelative { amount = 50 });
        passive_effects.Add(new EffectDazeTime { amount = 100 });

    }
}

public class TalentShieldThrow: TalentPrototype
{
    public int damage_min;
    public int damage_max;
    public int range;

    public TalentShieldThrow()
    {
        name = "Eat that!";
        target = TalentTarget.Tile;
        target_range = 5;
        icon = "images/talents/shield_throw";
        cost_stamina = 2;
        prepare_time = 50;
        recover_time = 100;
        cooldown = 2000;

        description = "Throws the currently equipped shield at a target dealing four times its weight as pierce damage.";
       
        prepare_message = "The <name> takes their shield in both hands.";
        action_message = "The <name> throws the shield.";
    }

     public ItemData RemoveShield()
    {
        PlayerData player = GameObject.Find("GameData").GetComponent<GameData>().player_data;
        ItemData item = player.GetMainShield();
        foreach(var slot in player.equipment)
        {
            if (slot.item == item)
                slot.item = null;
        }
       
        if (item == null || item.shield_data == null)
        {
            return null;
        }

        return item;
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
        ItemData shield = RemoveShield();
        if (shield == null)
        {
            action.prepare_message = "The player has no shield equipped to throw.";
            action.action_message = "The player just stands there awkwardly.";
            return action;
        }                 
        ItemProjectile projectile_prototype = new ItemProjectile(0);
        projectile_prototype.CreateProjectile(shield);
        ItemProjectileData projectile = new ItemProjectileData( input.target_tiles[0].Item1, input.target_tiles[0].Item2, projectile_prototype);
        projectile.item = shield;
        projectile.path = input.target_tiles;
        projectile.is_shot_by_player = true;

        action.commands.Add(new CreateProjectileCommand(projectile));

        return action;
    }

}






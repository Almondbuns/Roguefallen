using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentHeavyArmorPhysicalArmor : TalentPassiveEffects
{
    public TalentHeavyArmorPhysicalArmor()
    {
        name = "Steady Armor";
        type = TalentType.Passive;
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/heavy_armor_physical";

        description = "Increases the physical armor of heavy armor pieces by 1";

        passive_effects = new List<EffectData>();

        passive_effects.Add(new EffectAddArmorPhysical { amount = 1 });
    }
}

public class TalentHeavyArmorElementalArmor : TalentPassiveEffects
{
    public TalentHeavyArmorElementalArmor()
    {
        name = "Hardened Armor";
        type = TalentType.Passive;
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/heavy_armor_elemental";

        description = "Increases the elemental armor of heavy armor pieces by 1";

        passive_effects = new List<EffectData>();

        passive_effects.Add(new EffectAddArmorElemental { amount = 1 });
    }
}

public class TalentHeavyArmorMagicalArmor : TalentPassiveEffects
{
    public TalentHeavyArmorMagicalArmor()
    {
        name = "Blessed Armor";
        type = TalentType.Passive;
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/heavy_armor_magical";

        description = "Increases the magical armor of heavy armor pieces by 1";

        passive_effects = new List<EffectData>();

        passive_effects.Add(new EffectAddArmorMagical { amount = 1 });
    }
}

public class TalentHeavyArmorDurabilityArmor : TalentPassiveEffects
{
    public TalentHeavyArmorDurabilityArmor()
    {
        name = "Improved Armor";
        type = TalentType.Passive;
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/heavy_armor_durability";

        description = "Increases the durability of heavy armor pieces by 50%";

        passive_effects = new List<EffectData>();

        passive_effects.Add(new EffectAddRelativeArmorDurability { amount = 50 });
    }
}

public class TalentHeavyArmorRush : TalentPrototype
{
    public TalentHeavyArmorRush()
    {
        name = "Armor Rush";
        type = TalentType.Active;
        target = TalentTarget.Tile;
        target_range = 1;
        icon = "images/talents/heavy_armor_rush";

        cost_stamina = 3;
        cooldown = 1000;

        prepare_time = 100;
        recover_time = 50;
        this.description = "Rush your target dealing crush damage equal to the sum of physical armor of all your heavy armor parts";
    } 
    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);
        List<AttackedTileData> tiles = new List<AttackedTileData>();
        var actual_damage = new List<(DamageType, int, int)>();
        
        if (input.source_actor is PlayerData == false)
            return action;

        PlayerData player_data = (PlayerData) input.source_actor;
        int damage = 0;
        foreach(var slot in player_data.equipment)
        {
            if (slot.item != null && slot.item.GetPrototype().armor != null && slot.item.GetPrototype().armor.sub_type == ArmorSubType.HEAVY)
            {
                damage += slot.item.GetArmor(ArmorType.PHYSICAL);
            }
        }
        actual_damage.Add((DamageType.CRUSH, damage, 0));
        tiles.Add(new AttackedTileData
        {
            x = input.target_tiles[0].Item1,
            y = input.target_tiles[0].Item2,
            damage_on_hit = actual_damage,
            effects_on_hit = {},
            diseases_on_hit = {},
            poisons_on_hit= {},
        });

        action.prepare_time = prepare_time;
        action.prepare_message = "The <name> secures their armor pieces.";
        action.action_message = "The <name> rushes their target.";
        action.commands.Add(new AttackTilesCommand(input.source_actor, tiles, 1, true));
        action.recover_time = recover_time;
        return action;
    }
}

public class TalentHeavyArmorSuitUp: TalentPrototype
{
    public TalentHeavyArmorSuitUp()
    {
        name = "Suit Up";
        type = TalentType.Active;
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/heavy_armor_suitup";

        cost_stamina = 5;
        cooldown = 2000;

        prepare_time = 100;
        recover_time = 100;
        this.description = "For 10 turns increase your physical armor by 3 and elemental armor by 2 but also increase your movement rate by 200";
    } 
    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);

        action.prepare_time = prepare_time;
        action.prepare_message = "The <name> suits up.";
        action.action_message = "The <name> is now heavily protected.";
        action.commands.Add(new GetEffectCommand(input.source_actor, 
        new EffectAddArmorPhysical
        {
            amount = 3,
            duration = 1000,
        }));
        action.commands.Add(new GetEffectCommand(input.source_actor, 
        new EffectAddArmorElemental
        {
            amount = 2,
            duration = 1000,
        }));
        action.commands.Add(new GetEffectCommand(input.source_actor, 
        new EffectAddMovementTime
        {
            amount = 200,
            duration = 1000,
        }));
        action.recover_time = recover_time;
        return action;
    }
}

public class TalentHeavyArmorRun : TalentSubstainedEffects
{
    public TalentHeavyArmorRun()
    {
        name = "Run for your Life";
        type = TalentType.Substained;
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/heavy_armor_run";
        cost_stamina = 1;
        prepare_time = 0;
        recover_time = 50;
        cooldown = 500;
        description = "Reduce movement time by 30% but lose one point of stamina each turn.";

        substained_effects = new List<EffectData>();

        substained_effects.Add(new EffectRemoveMovementTimeRelative { amount = 30 });
        substained_effects.Add(new EffectSubstractStamina{ amount = 1, execution_time = EffectDataExecutionTime.CONTINUOUS });
    }
}

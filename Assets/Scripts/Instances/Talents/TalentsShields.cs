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
        icon = "images/talents/sword_attack_heavy";
        cost_stamina = 3;
        recover_time = 100;
        cooldown = 800;
        description = "Attacks with the shield dealing crush damage equal to double the sum of its armor values. May daze the target for 5 turns.";
        action_message ="The <name> bashes with a shield.";
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);

        int damage = 2 * (input.item.GetPrototype().armor.armor_physical + input.item.GetPrototype().armor.armor_elemental + input.item.GetPrototype().armor.armor_magical);
        List<AttackedTileData> tiles = new List<AttackedTileData>();

         tiles.Add(new AttackedTileData
        {
            x = input.target_tiles[0].Item1,
            y = input.target_tiles[0].Item2,
            damage_on_hit = new (){(DamageType.CRUSH, damage, 0)},
            effects_on_hit = new ()
            {
                new EffectAddToHit(){damage_type = DamageType.CRUSH, amount = -50, duration = 500},
                new EffectAddDodge(){damage_type = DamageType.CRUSH, amount = -50, duration = 500},
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
        icon = "images/talents/sword_attack_heavy";
        cost_stamina = 2;
        recover_time = 150;
        cooldown = 300;
        description = "Concentrate on parrying the next physical attacks within 100 ticks against you negating the damage and leaving the source open for a counterattack.";
        action_message ="The <name> parries with a shield.";
    }

    public override ActionData CreateAction(TalentInputData input)
    {
        ActionData action = new ActionData(input.talent);

        action.commands.Add(new GetEffectCommand(input.source_actor, 
        new EffectParry(){damage_type = DamageType.SLASH, duration = 100, amount = 50,}));

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
        icon = "images/talents/sword_attack_heavy";
        cost_stamina = 0;
        recover_time = 0;
        cooldown = 0;
        description = "When attacked you have a 25% chance to add your shield armor to your body armor.";

        passive_effects = new List<EffectData>();

        passive_effects.Add(new EffectPassiveBlock { amount = 25 });

    }
}


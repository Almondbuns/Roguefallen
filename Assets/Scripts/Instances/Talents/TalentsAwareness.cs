using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TalentAwarenessTrapSpotting : TalentPassiveEffects
{
    public TalentAwarenessTrapSpotting()
    {
        name = "Trap Spotting";
        type = TalentType.Passive;
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/awareness_traps";

        description = "Increases chance to spot traps by 50%";

        passive_effects = new List<EffectData>();

        passive_effects.Add(new EffectAddRelativeTrapSpotting { amount = 50 });
    }
}

public class TalentAwarenessMonsterStats: TalentPassiveEffects
{
    public TalentAwarenessMonsterStats()
    {
        name = "Know your Enemies";
        type = TalentType.Passive;
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/awareness_1";

        description = "See hitpoints, stamina and mana of your enemies.";

        passive_effects = new List<EffectData>();

        passive_effects.Add(new EffectMonsterStats { amount = 1 });
    }
}

public class TalentAwarenessMonsterAdvancedStats: TalentPassiveEffects
{
    public TalentAwarenessMonsterAdvancedStats()
    {
        name = "Know your Enemies Better";
        type = TalentType.Passive;
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/awareness_2";

        description = "See to hit chance, dodge chance and movement rate of your enemies.";

        passive_effects = new List<EffectData>();

        passive_effects.Add(new EffectMonsterAdvancedStats { amount = 1 });
    }
}

public class TalentAwarenessMonsterArmor: TalentPassiveEffects
{
    public TalentAwarenessMonsterArmor()
    {
        name = "Know your Enemies Even Better";
        type = TalentType.Passive;
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/awareness_3";

        description = "See the body parts and armor values of your enemies.";

        passive_effects = new List<EffectData>();

        passive_effects.Add(new EffectMonsterArmor { amount = 1 });
    }
}

public class TalentAwarenessMonsterResistances: TalentPassiveEffects
{
    public TalentAwarenessMonsterResistances()
    {
        name = "Know your Enemies The Best";
        type = TalentType.Passive;
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/awareness_4";

        description = "See the resistances of your enemies.";

        passive_effects = new List<EffectData>();

        passive_effects.Add(new EffectMonsterResistances { amount = 1 });
    }
}

public class TalentAwarenessThrowingWeaponDamage: TalentPassiveEffects
{
    public TalentAwarenessThrowingWeaponDamage()
    {
        name = "If you strike, strike hard";
        type = TalentType.Passive;
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/awareness_throwing_2";

        description = "Throwing weapons deal 50% more damage.";

        passive_effects = new List<EffectData>();
        passive_effects.Add(new EffectAddThrowingWeaponDamageRelative { amount = 50 });
    }
}

public class TalentAwarenessThrowingWeaponAmount: TalentPassiveEffects
{
    public TalentAwarenessThrowingWeaponAmount()
    {
        name = "Look what I found";
        type = TalentType.Passive;
        target = TalentTarget.Self;
        target_range = 0;
        icon = "images/talents/awareness_throwing_1";

        description = "Find 50% more throwing weapons.";

        passive_effects = new List<EffectData>();

        passive_effects.Add(new EffectAddThrowingWeaponAmountRelative { amount = 50 });
    }
}


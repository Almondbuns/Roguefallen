using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSleepVenom : PoisonPrototype
{
    public PoisonSleepVenom()
    {
        name = "Sleep Venom";
        icon = "images/effects/effect_poison";

        max_duration = 2500;

        effects.Add(
            new EffectSubstractStamina
            {
                amount = 2,
                execution_time= EffectDataExecutionTime.CONTINUOUS,             
            }
        );
    }
}

public class PoisonDeathBell : PoisonPrototype
{
    public PoisonDeathBell()
    {
        name = "DeathBell";
        icon = "images/effects/effect_poison";

        max_duration = 5000;

        effects.Add(
            new EffectSubstractHitpoints
            {
                amount = 1,
                execution_time= EffectDataExecutionTime.CONTINUOUS,             
            }
        );
    }
}

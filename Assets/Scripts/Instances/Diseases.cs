using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiseaseScratchFever : DiseasePrototype
{
    public DiseaseScratchFever()
    {
        name = "Scratch Fever";
         icon = "images/effects/effect_disease";

        severeness_effects.Add(
            new List<EffectData>()
            {
                new EffectAddVitality
                {
                    amount = -5,
                    icon = "images/effects/effect_disease",   
                }
            }
        );
    }
}

public class DiseaseMusclePain : DiseasePrototype
{
    public DiseaseMusclePain()
    {
        name = "Muscle Pain";
         icon = "images/effects/effect_disease";

        severeness_effects.Add(
            new List<EffectData>()
            {
                new EffectAddStrength
                {
                    amount = -5,
                    icon = "images/effects/effect_disease",   
                },
            }
        );
    }
}


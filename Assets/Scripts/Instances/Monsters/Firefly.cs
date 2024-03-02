using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFly : ActorPrototype
{
    public FireFly(int level) : base(level)
    {
       
        name = "Firefly";
        icon = "images/npc/firefly";
        prefab_index = 18;

        monster = new MonsterPrototype
        {
            ai_prototype = new AIPrototype
            {
                personality = AIPersonality.HitAndRun,
            }
        };

        stats.health_max = 10;
        stats.stamina_max = 5;
        stats.mana_max = 0;
        stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (0, 1, 0), durability_max = 10 });
        stats.movement_time = 20;
        stats.to_hit = 5;
        stats.dodge = 15;
        stats.kill_experience = 20;

        stats.probability_resistances.SetResistance(DamageType.FIRE, DamageTypeResistances.EXTREMELY_RESISTANT);
        stats.probability_resistances.SetResistance(DamageType.ICE, DamageTypeResistances.EXTREMELY_WEAK);

        talents.Add(
            new TalentStandardMeleeAttack
            {
                name = "Fire Bite",
                description = "Elemental bite attack that deals fire damage",

                    damage = 
                {
                    (DamageType.FIRE, 2,4,0),
                },

                cost_stamina = 0,
                recover_time = 50,
                cooldown = 100,

                icon = "images/talents/fire",

                prepare_message = "The <name> opens its mouth.",
                action_message = "The <name> bites.",
            }
        );
    }
}

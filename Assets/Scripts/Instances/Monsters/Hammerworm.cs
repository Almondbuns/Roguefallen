using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammerworm : ActorPrototype
{
    public Hammerworm(int level) : base( level)
    {
        name = "Hammerworm";
        icon = "images/npc/hammerworm";
        prefab_index = 1;

        monster = new MonsterPrototype
        {
            ai_prototype = new AIPrototype
            {
                personality = AIPersonality.Normal,
            }
        };

        stats.health_max = 5;
        stats.stamina_max = 10;
        stats.mana_max = 0;
        stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 80, armor = (0, 0, 0), durability_max = 0 });
        stats.body_armor.Add(new ArmorStats { body_part = "head", percentage = 20, armor = (10, 5, 0), durability_max = 10 });
        stats.movement_time = 100;
        stats.to_hit = 5;
        stats.dodge = 5;
        stats.kill_experience = 5;

        stats.probability_resistances.SetResistance(DamageType.FIRE, DamageTypeResistances.VERY_WEAK);

        talents.Add(
            new TalentStandardMeleeAttack
            {
                name = "Hammer Head",
                description = "Physical head attack that deals crush damage",

                damage =
                {
                    (DamageType.CRUSH, 2,3,2),
                },

                cost_stamina = 0,
                prepare_time = 50,
                recover_time = 50,
                cooldown = 100,

                icon = "images/talents/bear_claw",

                prepare_message = "The <name> wiggles its head.",
                action_message = "The <name> headbuts.",
            }
        );
        talents.Add(
            new TalentMultiply
            {
                cost_stamina = 5,
                prepare_time = 500,
                recover_time = 100,
                cooldown = 9,
                cooldown_start = 900, // prevents immediate multi-multiplies
            }
        );
    }
}
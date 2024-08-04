using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormHatchling : ActorPrototype
{
    public WormHatchling(int level) : base( level)
    {        
        name = "Worm Hatchling";
        icon = "images/npc/worm_hatchling";
        prefab_index = 0;

        monster = new MonsterPrototype
        {
            ai_prototype = new AIPrototype
            {
                personality = AIPersonality.Normal,
            }
        };

        stats.health_max = 2;
        stats.stamina_max = 10;
        stats.mana_max = 0;
        stats.body_armor.Add(new ActorArmorStats { body_part = "body", percentage = 100, armor = (0, 0, 0)});           
        stats.movement_time = 100;
        stats.to_hit = 5;
        stats.dodge = 5;
        stats.kill_experience = 3;

        stats.probability_resistances.SetResistance(DamageType.FIRE, DamageTypeResistances.VERY_WEAK);

        talents.Add(
            new TalentStandardMeleeAttack
            {
                name = "Bite",
                description = "Physical attack that deals pierce damage",

                damage =
                {
                    (DamageType.PIERCE, 1,2,0 ),
                },

                cost_stamina = 0,
                recover_time = 100,
                cooldown = 100,

                icon = "images/talents/bite",

                prepare_message = "The <name> wiggles its head.",
                action_message = "The <name> bites.",
            }
        );
        talents.Add(
            new TalentMultiply
            {
                cost_stamina = 5,
                prepare_time = 500,
                recover_time = 100,
                cooldown = 1000,
                cooldown_start = 1000, // prevents immediate multi-multiplies
            }
        );
    }
}
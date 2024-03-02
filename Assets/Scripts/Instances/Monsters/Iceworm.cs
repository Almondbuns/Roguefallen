using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iceworm : ActorPrototype
{
    public Iceworm(int level) : base( level)
    {    
        name = "Iceworm";
        icon = "images/npc/iceworm";
        prefab_index = 2;

        monster = new MonsterPrototype
        {
            ai_prototype = new AIPrototype
            {
                personality = AIPersonality.Normal,
            }
        };

        stats.health_max = 10;
        stats.stamina_max = 10;
        stats.mana_max = 0;
        stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (0, 0, 0), durability_max = 0 });
        stats.movement_time = 100;
        stats.to_hit = 5;
        stats.dodge = 5;
        stats.kill_experience = 10;

        stats.probability_resistances.SetResistance(DamageType.FIRE, DamageTypeResistances.VERY_WEAK);
        stats.probability_resistances.SetResistance(DamageType.ICE, DamageTypeResistances.EXTREMELY_RESISTANT);

        talents.Add(
            new TalentStandardMeleeAttack
            {
                name = "Ice Head",
                description = "Head attack that deals ice damage",

                damage =
                {
                    (DamageType.ICE, 2,4,0) ,
                },

                cost_stamina = 0,
                prepare_time = 0,
                recover_time = 100,
                cooldown = 100,

                icon = "images/talents/bite",

                prepare_message = "The <name> wiggles its head.",
                action_message = "The <name> headbuts.",
            }
        );
        talents.Add(
            new TalentMultiply
            {
                cost_stamina = 5,
                prepare_time = 400,
                recover_time = 100,
                cooldown = 800,
                cooldown_start = 800, // prevents immediate multi-multiplies
            }
        );
    }
}
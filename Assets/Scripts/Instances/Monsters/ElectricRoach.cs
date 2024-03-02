using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricRoach : ActorPrototype
{
    public ElectricRoach(int level) : base(level)
    {   
        name = "Electric Roach";
        icon = "images/npc/electric_roach";
        prefab_index = 13;

        monster = new MonsterPrototype
        {
            ai_prototype = new AIPrototype
            {
                personality = AIPersonality.Normal,
            }
        };

        stats.health_max = 20;
        stats.stamina_max = 5;
        stats.mana_max = 0;
        stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 95, armor = (5, 5, 0), durability_max = 15 });
        stats.body_armor.Add(new ArmorStats { body_part = "antennae", percentage = 5, armor = (0, 0, 0), durability_max = 0 });
        stats.movement_time = 100;
        stats.to_hit = 5;
        stats.dodge = 5;

        stats.kill_experience = 30;

        stats.probability_resistances.SetResistance(DamageType.LIGHTNING, DamageTypeResistances.VERY_RESISTANT);

        talents.Add(
            new TalentStandardMeleeAttack
            {
                name = "Bite",
                description = "Physical melee attack that deals pierce damage",

                damage = {(DamageType.LIGHTNING, 2, 4, 0)},

                cost_stamina = 0,
                recover_time = 100,
                cooldown = 100,

                icon = "images/talents/bite",

                action_message = "The <name> sparks electricity.",
            } 
        );
    }
}

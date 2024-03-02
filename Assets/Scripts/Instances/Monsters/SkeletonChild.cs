using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonChild : ActorPrototype
{
    public SkeletonChild(int level) : base(level)
    {
        name = "Skeleton Child";
        icon = "images/npc/skeleton_child";
        prefab_index = 42;

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
        stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (1, 0, 0), durability_max = 10 });
        stats.movement_time = 50;
        stats.to_hit = 10;
        stats.dodge = 10;
        stats.kill_experience = 10;

        stats.probability_resistances.SetResistance(DamageType.PIERCE, DamageTypeResistances.VERY_RESISTANT);
        stats.probability_resistances.SetResistance(DamageType.CRUSH, DamageTypeResistances.VERY_WEAK);
        stats.probability_resistances.SetResistance(DamageType.DIVINE, DamageTypeResistances.VERY_WEAK);

        talents.Add(
            new TalentStandardMeleeAttack
            {
                name = "Hammer Attack",
                description = "Physical melee attack that deals crush damage",

                damage = {(DamageType.CRUSH, 1, 2, 0)},

                cost_stamina = 0,
                recover_time = 50,
                cooldown = 50,

                icon = "images/talents/bite",

                action_message = "The <name> attacks with a hammer.",
            } 
        );
    }

}

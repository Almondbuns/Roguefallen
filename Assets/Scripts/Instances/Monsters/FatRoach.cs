using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatRoach : ActorPrototype
{
    public FatRoach(int level) : base(level)
    {
       
        name = "Fat Roach";
        icon = "images/npc/fat_roach";
        prefab_index = 12;

        monster = new MonsterPrototype
        {
            ai_prototype = new AIPrototype
            {
                personality = AIPersonality.Normal,
            }
        };

        stats.kill_experience = 40;
        stats.health_max = 30;
        stats.stamina_max = 5;
        stats.mana_max = 0;
        stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 95, armor = (6, 6, 0), durability_max = 30 });
        stats.body_armor.Add(new ArmorStats { body_part = "antennae", percentage = 5, armor = (0, 0, 0), durability_max = 0 });
        stats.movement_time = 100;
        stats.to_hit = 5;
        stats.dodge = 5;

        talents.Add(
            new TalentStandardMeleeAttack
            {
                name = "Bite",
                description = "Physical melee attack that deals pierce damage",

                damage = {(DamageType.PIERCE, 4, 7, 0)},

                cost_stamina = 0,
                recover_time = 100,
                cooldown = 100,

                icon = "images/talents/bite",

                action_message = "The <name> bites.",
            } 
        );
    }
}

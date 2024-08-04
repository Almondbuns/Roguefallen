using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roach : ActorPrototype
{
    public Roach(int level) : base(level)
    {
       
        name = "Roach";
        icon = "images/npc/roach";
        prefab_index = 11;

        monster = new MonsterPrototype
        {
            ai_prototype = new AIPrototype
            {
                personality = AIPersonality.Normal,
            }
        };

        stats.health_max = 15;
        stats.stamina_max = 5;
        stats.mana_max = 0;
        stats.body_armor.Add(new ActorArmorStats { body_part = "body", percentage = 100, armor = (5, 5, 0)});
        stats.movement_time = 100;
        stats.to_hit = 5;
        stats.dodge = 5;
        stats.kill_experience = 20;

        talents.Add(
            new TalentStandardMeleeAttack
            {
                name = "Bite",
                description = "Physical melee attack that deals pierce damage",

                damage = {(DamageType.PIERCE, 2, 4, 0)},

                cost_stamina = 0,
                recover_time = 100,
                cooldown = 100,

                icon = "images/talents/bite",

                action_message = "The <name> bites.",
            } 
            );
    }    
}

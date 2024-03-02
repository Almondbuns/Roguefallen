using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : ActorPrototype
{
    public Fly(int level) : base(level)
    {
       
        name = "Fly";
        icon = "images/npc/fly";
        prefab_index = 17;

        monster = new MonsterPrototype
        {
            ai_prototype = new AIPrototype
            {
                personality = AIPersonality.HitAndRun,
            }
        };

        stats.health_max = 5;
        stats.stamina_max = 5;
        stats.mana_max = 0;
        stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (0, 0, 0), durability_max = 0 });
        stats.movement_time = 20;
        stats.to_hit = 5;
        stats.dodge = 15;
        stats.kill_experience = 10;

        talents.Add(
            new TalentStandardMeleeAttack
            {
                name = "Bite",
                description = "Bite attack that deals pierce damage",

                damage = 
                {
                    (DamageType.PIERCE, 1,3,0),
                },

                cost_stamina = 0,
                recover_time = 50,
                cooldown = 100,

                icon = "images/talents/bite",

                prepare_message = "The <name> opens its mouth.",
                action_message = "The <name> bites.",
            }
        );
    }       
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : ActorPrototype
{
    public Snail(int level) : base(level)
    {
       
        name = "Corrupted Snail";
        icon = "images/npc/snail";
        prefab_index = 48;

        monster = new MonsterPrototype
        {
            ai_prototype = new AIPrototype
            {
                personality = AIPersonality.Normal,
            }
        };

        stats.health_max = 5;
        stats.stamina_max = 5;
        stats.mana_max = 0;
        stats.body_armor.Add(new ActorArmorStats { body_part = "body", percentage = 100, armor = (0, 1, 0)});
        stats.movement_time = 300;
        stats.to_hit = 5;
        stats.dodge = 15;
        stats.kill_experience = 20;

        talents.Add(
            new TalentStandardMeleeAttack
            {
                name = "Bite",
                description = "Elemental bite attack that deals fire damage",

                    damage = 
                {
                    (DamageType.FIRE, 1,1,0),
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


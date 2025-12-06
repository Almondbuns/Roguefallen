using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thornling: ActorPrototype
{
    public Thornling(int level) : base(level)
    {
       
        name = "Thornling";
        icon = "images/npc/thornling";
        prefab_index = 49;

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
        stats.body_armor.Add(new ActorArmorStats { body_part = "body", percentage = 100, armor = (0, 1, 0)});
        stats.movement_time = 100;
        stats.to_hit = 5;
        stats.dodge = 5;
        stats.kill_experience = 10;

        talents.Add(
            new TalentStandardMeleeAttack
            {
                name = "Pierce",
                description = "Standard Piercing Attack",

                    damage = 
                {
                    (DamageType.PIERCE, 2,4,0),
                },

                cost_stamina = 0,
                recover_time = 100,
                cooldown = 100,

                icon = "images/talents/vampire_bite",

                prepare_message = "",
                action_message = "The <name> rams its thorns into you.",
            }
        );
    }
}


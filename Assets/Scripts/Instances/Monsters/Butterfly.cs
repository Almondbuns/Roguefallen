using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : ActorPrototype
{
    public Butterfly(int level) : base(level)
    {
       
        name = "Butterfly";
        icon = "images/npc/butterfly";
        prefab_index = 47;

        monster = new MonsterPrototype
        {
            ai_prototype = new AIPrototype
            {
                personality = AIPersonality.RandomMovement,
            }
        };

        stats.health_max = 5;
        stats.stamina_max = 5;
        stats.mana_max = 0;
        stats.body_armor.Add(new ActorArmorStats { body_part = "body", percentage = 100, armor = (0, 1, 0)});
        stats.movement_time = 75;
        stats.to_hit = 5;
        stats.dodge = 15;
        stats.kill_experience = 5;

        talents.Add(
            new TalentStandardMeleeAttack
            {
                name = "Bite",
                description = "Elemental bite attack",

                    damage = 
                {
                    (DamageType.LIGHTNING, 1,1,0),
                },

                cost_stamina = 0,
                recover_time = 100,
                cooldown = 100,

                icon = "images/talents/fire",

                prepare_message = "The <name> opens its mouth.",
                action_message = "The <name> bites.",
            }
        );
    }
}


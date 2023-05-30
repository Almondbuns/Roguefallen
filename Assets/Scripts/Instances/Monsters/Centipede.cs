using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centipede : ActorPrototype
{
    //TODO Unborrow
    public Centipede(int level) : base(level)
    {
        if (level <= 20 )
        {
            name = "Centipede";
            icon = "images/npc/centipede";
            prefab_index = 33;
            tile_width = 2;
            tile_height = 1;
            //is_hidden = true; TODO

            monster = new MonsterPrototype
            {
                ai_personality = AIPersonality.Normal,
            };

            stats.health_max = 30;
            stats.stamina_max = 5;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (0, 0, 0), durability_max = 0 });
            stats.movement_time = 100;
            stats.to_hit = 5;
            stats.dodge = 5;
            stats.kill_experience = 30;

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Bite",
                    description = "Bite attack that deals pierce damage",

                    damage = 
                    {
                        (DamageType.PIERCE, 2,4,2),
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
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysticMushroom : ActorPrototype
{ 
    public MysticMushroom(int level) : base(level)
    {
        if (level <= 100)
        {
            name = "Defense Mushroom";
            icon = "images/npc/big_mushroom";
            prefab_index = 46;
            tile_width = 1;
            tile_height = 2;

            monster = new MonsterPrototype
            {
                ai_prototype = new AIPrototype
                {
                    personality = AIPersonality.Normal,
                }
            };
            can_move = false;

            stats.health_max = 20;
            stats.stamina_max = 5;
            stats.mana_max = 0;
            stats.body_armor.Add(new ActorArmorStats { body_part = "body", percentage = 100, armor = (1, 1, 4)});
            stats.movement_time = 100;
            stats.to_hit = 5;
            stats.dodge = 0;
            stats.kill_experience = 0;

            talents.Add(
                new TalentThrowAtPlayer
                {
                    name = "Shoot Magical Projectile",
                    description = "Shoot Magical Projectile",

                    object_type = typeof(ProjectileMagicBall),

                    cost_stamina = 0,
                    prepare_time = 0,
                    recover_time = 200,
                    cooldown = 0,
                    target_range = 5,

                    icon = "images/objects/throwing_knife",
                 
                    action_message = "The <name> shoots a magical ball.",
                }
            );
        }       
    }
}

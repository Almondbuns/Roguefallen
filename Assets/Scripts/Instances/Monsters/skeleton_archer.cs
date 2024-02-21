using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcher : ActorPrototype
{
    public SkeletonArcher(int level) : base(level)
    {
      
        name = "Skeleton Archer";
        icon = "images/npc/skeleton_archer";
        prefab_index = 38;

        monster = new MonsterPrototype
        {
            ai_personality = AIPersonality.Normal
        };

        stats.health_max = 20;
        stats.stamina_max = 10;
        stats.mana_max = 0;
        stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (2, 1, 0), durability_max = 10 });
        stats.movement_time = 100;
        stats.to_hit = 10;
        stats.dodge = 10;
        stats.kill_experience = 20;

        talents.Add(
                new TalentThrowAtPlayer
                {
                    name = "Throw throwing knife",
                    description = "Throw throwing knife",

                    object_type = typeof(ProjectileArrow),

                    cost_stamina = 0,
                    prepare_time = 50,
                    recover_time = 100,
                    cooldown = 200,

                    icon = "images/objects/throwing_knife",

                    prepare_message = "The <name> prepares an arrow.",
                    action_message = "The <name> shoots with its bow.",
                } 
            );
    }
}

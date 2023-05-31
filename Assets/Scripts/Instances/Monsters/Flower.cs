using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : ActorPrototype
{ 
    public Flower(int level) : base(level)
    {
        if (level <= 0)
        {
            name = "Sun Flower";
            icon = "images/npc/flower";
            prefab_index = 35;
            tile_width = 1;
            tile_height = 2;

            monster = new MonsterPrototype
            {
                ai_personality = AIPersonality.Normal
            };
            can_move = false;

            stats.health_max = 10;
            stats.stamina_max = 5;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (1, 5, 0), durability_max = 20 });
            stats.movement_time = 100;
            stats.to_hit = 5;
            stats.dodge = 0;
            stats.kill_experience = 0;
        }
        else
        {
            name = "Poison Flower";
            icon = "images/npc/flower";
            prefab_index = 35;
            tile_width = 1;
            tile_height = 2;

            monster = new MonsterPrototype
            {
                ai_personality = AIPersonality.Normal
            };
            can_move = false;

            stats.health_max = 20;
            stats.stamina_max = 5;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (1, 5, 0), durability_max = 20 });
            stats.movement_time = 100;
            stats.to_hit = 5;
            stats.dodge = 0;
            stats.kill_experience = 20;

            can_catch_poison = false;

            talents.Add(
                new TalentRadiusAllAttack
                {
                    name = "Poison Dust",
                    description = "",
                    target_range = 3,

                    damage = {(DamageType.POISON, 5, 10, 0)},
                    poisons = new() {typeof(PoisonSleepVenom)},

                    cost_stamina = 0,
                    prepare_time = 200,
                    recover_time = 100,
                    cooldown = 1000,

                    icon = "images/talents/poison",

                    prepare_message = "The <name> opens its blossom.",
                    action_message = "The <name> breaths poison.",

                    ai_data = new TalentAIInfo
                    {
                        type = TalentAIInfoType.Special,
                        use_probability = 0.5f,
                        player_range = 8,
                    },
                }
            );
        }
       
    }
}

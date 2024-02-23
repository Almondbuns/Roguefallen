using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : ActorPrototype
{
    public Bear(int level) : base(level)
    {
        if (level <= 8)
        {
            name = "Calm Bear";
            icon = "images/npc/calm_bear";
            prefab_index = 26;
            monster = new MonsterPrototype
            {
                ai_prototype = new AIPrototype
                {
                    personality = AIPersonality.Normal,
                }
            };

            stats.health_max = 40;
            stats.stamina_max = 20;
            stats.mana_max = 0;
            stats.kill_experience = 40;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (2, 5, 0), durability_max = 30 });
            stats.movement_time = 100;
            stats.to_hit = 10;
            stats.dodge = 10;

            stats.probability_resistances.SetResistance(DamageType.FIRE, DamageTypeResistances.VERY_WEAK);

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Bear Claw",
                    description = "Physical attack that deals crush damage",

                    damage =
                    {
                        (DamageType.CRUSH, 5,10,0 ),
                    },

                    cost_stamina = 0,
                    recover_time = 100,
                    cooldown = 100,

                    icon = "images/talents/bear_claw",

                    prepare_message = "The <name> opens its mouth.",
                    action_message = "The <name> bites.",
                }
            );
        }
        else
        {
            name = "Angry Bear";
            icon = "images/npc/angry_bear";
            prefab_index = 27;
            monster = new MonsterPrototype
            {
                ai_prototype = new AIPrototype
                {
                    personality = AIPersonality.Normal,
                }
            };

            stats.health_max = 50;
            stats.stamina_max = 20;
            stats.mana_max = 0;
            stats.kill_experience = 50;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (2, 5, 0), durability_max = 50 });
            stats.movement_time = 100;
            stats.to_hit = 10;
            stats.dodge = 10;

            stats.probability_resistances.SetResistance(DamageType.FIRE, DamageTypeResistances.VERY_WEAK);

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Bear Claw",
                    description = "Physical attack that deals crush damage",

                    damage =
                    {
                        (DamageType.CRUSH, 5,10,0 ),
                    },

                    cost_stamina = 0,
                    recover_time = 100,
                    cooldown = 100,

                    icon = "images/talents/bear_claw",

                    prepare_message = "The <name> opens its mouth.",
                    action_message = "The <name> bites.",
                }
            );
        }
    }
}

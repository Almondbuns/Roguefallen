using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : ActorPrototype
{
    public Spider(int level) : base(level)
    {
        if (level <= 2)
        {
            name = "Cave Spider";
            icon = "images/npc/cave_spider";
            prefab_index = 6;
            monster = new MonsterPrototype
            {
                ai_personality = AIPersonality.Normal
            };

            stats.health_max = 10;
            stats.stamina_max = 5;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (1, 1, 1), durability_max = 10 });
            stats.movement_time = 100;
            stats.to_hit = 5;
            stats.dodge = 5;
            stats.kill_experience = 10;

            stats.probability_resistances.SetResistance(DamageType.FIRE, DamageTypeResistances.VERY_WEAK);
            stats.meter_resistances.SetResistance(DamageType.POISON, 20);

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Claw Hug",
                    description = "Physical attack that deals slash damage",

                    damage = 
                    {
                        (DamageType.SLASH, 2,3,0 ),
                    },
                    poisons = { typeof(PoisonSleepVenom) },

                    cost_stamina = 0,
                    recover_time = 100,
                    cooldown = 100,

                    icon = "images/talents/scratch",

                    prepare_message = "",
                    action_message = "The <name> hugs.",
                }
            );            
        }
        else if (level <= 4)
        {
            name = "Common Spider";
            icon = "images/npc/spider";
            prefab_index = 7;
            monster = new MonsterPrototype
            {
                ai_personality = AIPersonality.Normal
            };

            stats.health_max = 15;
            stats.stamina_max = 5;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (1, 1, 1), durability_max = 10 });
            stats.movement_time = 100;
            stats.to_hit = 5;
            stats.dodge = 5;
            stats.kill_experience = 20;

            stats.probability_resistances.SetResistance(DamageType.FIRE, DamageTypeResistances.VERY_WEAK);
            stats.meter_resistances.SetResistance(DamageType.POISON, 20);

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Claw Hug",
                    description = "Physical attack that deals slash damage",

                    damage =
                    {
                        (DamageType.SLASH, 2,4,1 ),
                        (DamageType.POISON, 1, 2, 0),
                    },
                    poisons = { typeof(PoisonSleepVenom)},

                    cost_stamina = 0,
                    recover_time = 100,
                    cooldown = 100,

                    icon = "images/talents/poison",

                    prepare_message = "",
                    action_message = "The <name> hugs.",
                }
            );
        }
        else
        {
            name = "Poison Spider";
            icon = "images/npc/poison_spider";
            prefab_index = 8;
            monster = new MonsterPrototype
            {
                ai_personality = AIPersonality.Normal
            };

            stats.health_max = 15;
            stats.stamina_max = 5;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (1, 1, 1), durability_max = 10 });
            stats.movement_time = 100;
            stats.to_hit = 5;
            stats.dodge = 5;
            stats.kill_experience = 30;

            stats.probability_resistances.SetResistance(DamageType.FIRE, DamageTypeResistances.VERY_WEAK);
            stats.meter_resistances.SetResistance(DamageType.POISON, 20);

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Claw Hug",
                    description = "Physical attack that deals slash damage",

                    damage =
                    {
                        (DamageType.SLASH, 2,4,1 ),
                        (DamageType.POISON, 2, 4, 0),
                    },
                    poisons = { typeof(PoisonDeathBell) },

                    cost_stamina = 0,
                    recover_time = 100,
                    cooldown = 100,

                    icon = "images/talents/poison",

                    prepare_message = "",
                    action_message = "The <name> hugs.",
                }
            );
        }
    }
}

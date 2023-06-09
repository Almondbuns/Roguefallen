using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : ActorPrototype
{
    public Worm(int level) : base( level)
    {
        if (level <= 3)
        {
            name = "Worm Hatchling";
            icon = "images/npc/worm_hatchling";
            prefab_index = 0;

            monster = new MonsterPrototype
            {
                ai_personality = AIPersonality.Normal
            };

            stats.health_max = 2;
            stats.stamina_max = 10;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (0, 0, 0), durability_max = 0 });           
            stats.movement_time = 100;
            stats.to_hit = 5;
            stats.dodge = 5;
            stats.kill_experience = 3;

            stats.probability_resistances.SetResistance(DamageType.FIRE, DamageTypeResistances.VERY_WEAK);

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Bite",
                    description = "Physical attack that deals pierce damage",

                    damage =
                    {
                        (DamageType.PIERCE, 1,2,0 ),
                    },

                    cost_stamina = 0,
                    recover_time = 100,
                    cooldown = 100,

                    icon = "images/talents/bite",

                    prepare_message = "The <name> wiggles its head.",
                    action_message = "The <name> bites.",
                }
            );
            talents.Add(
                new TalentMultiply
                {
                    cost_stamina = 5,
                    prepare_time = 500,
                    recover_time = 100,
                    cooldown = 1000,
                    cooldown_start = 1000, // prevents immediate multi-multiplies
                }
            );
        }
        else if (level <= 6)
        {
            name = "Hammerworm";
            icon = "images/npc/hammerworm";
            prefab_index = 1;

            monster = new MonsterPrototype
            {
                ai_personality = AIPersonality.Normal
            };

            stats.health_max = 5;
            stats.stamina_max = 10;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 80, armor = (0, 0, 0), durability_max = 0 });
            stats.body_armor.Add(new ArmorStats { body_part = "head", percentage = 20, armor = (10, 5, 0), durability_max = 10 });
            stats.movement_time = 100;
            stats.to_hit = 5;
            stats.dodge = 5;
            stats.kill_experience = 5;

            stats.probability_resistances.SetResistance(DamageType.FIRE, DamageTypeResistances.VERY_WEAK);

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Hammer Head",
                    description = "Physical head attack that deals crush damage",

                    damage =
                    {
                        (DamageType.CRUSH, 2,3,2),
                    },

                    cost_stamina = 0,
                    prepare_time = 50,
                    recover_time = 50,
                    cooldown = 100,

                    icon = "images/talents/bear_claw",

                    prepare_message = "The <name> wiggles its head.",
                    action_message = "The <name> headbuts.",
                }
            );
            talents.Add(
                new TalentMultiply
                {
                    cost_stamina = 5,
                    prepare_time = 500,
                    recover_time = 100,
                    cooldown = 9,
                    cooldown_start = 900, // prevents immediate multi-multiplies
                }
            );
        }
        else 
        {
            name = "Iceworm";
            icon = "images/npc/iceworm";
            prefab_index = 2;

            monster = new MonsterPrototype
            {
                ai_personality = AIPersonality.Normal
            };

            stats.health_max = 10;
            stats.stamina_max = 10;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (0, 0, 0), durability_max = 0 });
            stats.movement_time = 100;
            stats.to_hit = 5;
            stats.dodge = 5;
            stats.kill_experience = 10;

            stats.probability_resistances.SetResistance(DamageType.FIRE, DamageTypeResistances.VERY_WEAK);
            stats.probability_resistances.SetResistance(DamageType.ICE, DamageTypeResistances.EXTREMELY_RESISTANT);

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Ice Head",
                    description = "Head attack that deals ice damage",

                    damage =
                    {
                        (DamageType.ICE, 2,4,0) ,
                    },

                    cost_stamina = 0,
                    prepare_time = 0,
                    recover_time = 100,
                    cooldown = 100,

                    icon = "images/talents/bite",

                    prepare_message = "The <name> wiggles its head.",
                    action_message = "The <name> headbuts.",
                }
            );
            talents.Add(
                new TalentMultiply
                {
                    cost_stamina = 5,
                    prepare_time = 400,
                    recover_time = 100,
                    cooldown = 800,
                    cooldown_start = 800, // prevents immediate multi-multiplies
                }
            );
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : ActorPrototype
{

    public Bat(int level) : base(level)
    {
        if (level <= 7)
        {
            name = "Bat";
            icon = "images/npc/bat";

            monster = new MonsterPrototype
            {
                ai_prototype = new AIPrototype
                {
                    personality = AIPersonality.Normal,
                }
            };
            prefab_index = 19;

            stats.health_max = 10;
            stats.stamina_max = 5;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (0, 0, 0), durability_max = 0 });
            stats.movement_time = 30;
            stats.to_hit = 5;
            stats.dodge = 10;
            stats.kill_experience = 10;

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Bite",
                    description = "Physical bite attack that deals piercing damage",

                    damage = 
                        {
                            (DamageType.PIERCE, 1,2,1),
                        },

                    cost_stamina = 0,
                    recover_time = 50,
                    cooldown = 100,

                    icon = "images/talents/vampire_bite",

                    prepare_message = "The <name> opens its mouth.",
                    action_message = "The <name> bites.",
                }
            );

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Diseased Bite",
                    description = "Physical bite attack that deals piercing damage",

                    damage = 
                        {
                            (DamageType.DISEASE, 1,2,1),
                        },

                    diseases = { typeof(DiseaseScratchFever), typeof(DiseaseMusclePain) },
                    cost_stamina = 0,
                    recover_time = 50,
                    cooldown = 100,

                    icon = "images/talents/disease",

                    prepare_message = "The <name> opens its mouth.",
                    action_message = "The <name> bites.",
                }
            );
        }
        else
        {
            name = "Vampire Bat";
            icon = "images/npc/vampire_bat";

            monster = new MonsterPrototype
            {
                ai_prototype = new AIPrototype
                {
                    personality = AIPersonality.Normal,
                }
            };
            prefab_index = 20;

            stats.health_max = 20;
            stats.stamina_max = 5;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (0, 0, 0), durability_max = 0 });
            stats.movement_time = 30;
            stats.to_hit = 5;
            stats.dodge = 10;
            stats.kill_experience = 20;

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Bite",
                    description = "Physical bite attack that deals piercing damage",

                    damage = 
                        {
                            (DamageType.PIERCE, 2,3,1),
                        },

                    cost_stamina = 0,
                    recover_time = 50,
                    cooldown = 100,

                    icon = "images/talents/bite",

                    prepare_message = "The <name> opens its mouth.",
                    action_message = "The <name> bites.",
                }
            );
            talents.Add(
                new TalentStealLife
                {
                    name = "Vampire Bite",
                    description = "Physical bite attack that deals piercing damage and heals source",

                    cost_stamina = 1,
                    prepare_time = 50,
                    recover_time = 100,
                    cooldown = 200,

                    icon = "images/talents/vampire_bite",

                    prepare_message = "The <name> shows its fangs.",
                    action_message = "The <name> sucks life force.",
                }
            );

             talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Diseased Bite",
                    description = "Physical bite attack that deals piercing damage",

                    damage = 
                        {
                            (DamageType.DISEASE, 2,3,1),
                        },

                    diseases = { typeof(DiseaseScratchFever), typeof(DiseaseMusclePain) },

                    cost_stamina = 0,
                    recover_time = 50,
                    cooldown = 100,

                    icon = "images/talents/disease",

                    prepare_message = "The <name> opens its mouth.",
                    action_message = "The <name> bites.",
                }
            );
        }
    }
}
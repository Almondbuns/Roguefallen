using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rat : ActorPrototype
{
    public Rat(int level) : base(level)
    {
        int damage_min = 0;
        int damage_max = 0;

        monster = new MonsterPrototype
        {
            ai_personality = AIPersonality.Normal
        };        

        if (level <= 2)
        {            
            name = "Young Rat";
            icon = "images/npc/young_rat";
            prefab_index = 3;

            stats.health_max = 10;
            stats.stamina_max = 5;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 95, armor = (0, 0, 0), durability_max = 5 });
            stats.body_armor.Add(new ArmorStats { body_part = "tail", percentage = 5, armor = (0, 0, 0), durability_max = 0 });
            stats.movement_time = 50;
            stats.to_hit = 5;
            stats.dodge = 5;
            stats.kill_experience = 10;

            damage_min = 1;
            damage_max = 4;

            stats.meter_resistances.SetResistance(DamageType.DISEASE, 10);
        }
        else if (level <= 4)
        {
            name = "Rat";
            icon = "images/npc/rat";
            prefab_index = 4;

            stats.health_max = 15;
            stats.stamina_max = 5;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 95, armor = (1, 1, 0), durability_max = 10 });
            stats.body_armor.Add(new ArmorStats { body_part = "tail", percentage = 5, armor = (0, 0, 0), durability_max = 0 });
            stats.movement_time = 50;
            stats.to_hit = 10;
            stats.dodge = 10;

            stats.kill_experience = 20;

            damage_min = 2;
            damage_max = 5;

            stats.meter_resistances.SetResistance(DamageType.DISEASE, 15);

            talents.Add(
            new TalentStandardMeleeAttack
            {
                name = "Illness Bite",
                description = "Physical melee attack that deals Illness damage",

                damage =
                {
                    (DamageType.DISEASE, 1,2,0 ),                        
                },

                diseases = { typeof(DiseaseScratchFever), typeof(DiseaseMusclePain) },

                cost_stamina = 1,
                prepare_time = 50,
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
            name = "Elder Rat";
            icon = "images/npc/elder_rat";
            prefab_index = 5;

            stats.health_max = 20;
            stats.stamina_max = 5;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 95, armor = (2, 2, 0), durability_max = 10 });
            stats.body_armor.Add(new ArmorStats { body_part = "tail", percentage = 5, armor = (0, 0, 0), durability_max = 0 });
            stats.movement_time = 50;
            stats.to_hit = 10;
            stats.dodge = 10;

            stats.kill_experience = 30;

            damage_min = 3;
            damage_max = 6;

            stats.meter_resistances.SetResistance(DamageType.DISEASE, 20);

            talents.Add(
            new TalentStandardMeleeAttack
            {
                name = "Illness Bite",
                description = "Physical melee attack that deals Illness damage",

                damage =
                {
                    (DamageType.DISEASE, 2,3,0 ),
                },


                diseases = { typeof(DiseaseScratchFever), typeof(DiseaseMusclePain) },

                cost_stamina = 1,
                prepare_time = 50,
                recover_time = 50,
                cooldown = 100,

                icon = "images/talents/disease",

                prepare_message = "The <name> opens its mouth.",
                action_message = "The <name> bites.",
            }
        );
        }

        talents.Add(
            new TalentStandardMeleeAttack
            {
                name = "Bite",
                description = "Physical melee attack that deals pierce damage",

                damage =
                {
                    (DamageType.PIERCE, damage_min, damage_max,0 ),
                },

                cost_stamina = 0,
                recover_time = 100,
                cooldown = 200,

                icon = "images/talents/bite",

                prepare_message = "The <name> opens its mouth.",
                action_message = "The <name> bites.",
            }
        );
    }
}
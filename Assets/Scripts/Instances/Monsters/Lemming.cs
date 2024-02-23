using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lemming : ActorPrototype
{
    public Lemming(int level) : base(level)
    {
        if (level <= 2)
        {  
            name = "Lemming";
            icon = "images/npc/lemming";
            prefab_index = 9;

            monster = new MonsterPrototype
            {
                ai_prototype = new AIPrototype
                {
                    personality = AIPersonality.Normal,
                }
            };

            stats.health_max = 5;
            stats.stamina_max = 5;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100 });
            stats.movement_time = 50;
            stats.to_hit = 5;
            stats.dodge = 10;
            stats.kill_experience = 5;

            stats.probability_resistances.SetResistance(DamageType.SLASH, DamageTypeResistances.EXTREMELY_WEAK);

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Bite",
                    description = "Physical attack that deals pierce damage",

                    damage = 
                    {
                        (DamageType.PIERCE, 1,2,1),
                    },
                 
                    cost_stamina = 0,
                    recover_time = 50,
                    cooldown = 50,

                    icon = "images/talents/bite",

                    prepare_message = "",
                    action_message = "The <name> bites.",
                }
            );
        }
        else
        {
            name = "Exploding Lemming";
            icon = "images/npc/lemming_tnt";
            prefab_index = 10;

           monster = new MonsterPrototype
            {
                ai_prototype = new AIPrototype
                {
                    personality = AIPersonality.Normal,
                }
            };

            stats.health_max = 10;
            stats.stamina_max = 5;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100 });
            stats.movement_time = 50;
            stats.to_hit = 5;
            stats.dodge = 10;
            stats.kill_experience = 10;

            stats.probability_resistances.SetResistance(DamageType.SLASH, DamageTypeResistances.EXTREMELY_WEAK);

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Bite",
                    description = "Physical attack that deals pierce damage",

                    damage = 
                    {
                        (DamageType.PIERCE, 2,4,0),
                    },
                 
                    cost_stamina = 0,
                    recover_time = 50,
                    cooldown = 50,

                    icon = "images/talents/bite",

                    prepare_message = "",
                    action_message = "The <name> bites.",
                }
            );

            talents.Add(
                new TalentExplode
                {
                    name = "Explode",
                    description = "Explode",

                    damage = 
                    {
                        (DamageType.FIRE, 10, 10, 0),
                    },
                    damage_radius = 2,
                 
                    cost_stamina = 0,
                    prepare_time = 200,
                    recover_time = 1,
                    cooldown = 100,

                    icon = "images/talents/bomb",

                    //prepare_message = "",
                    //action_message = "The <name> bites.",
                }
            );
        }
    }
}

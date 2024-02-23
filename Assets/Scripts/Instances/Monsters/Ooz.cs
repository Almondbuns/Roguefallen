using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ooz : ActorPrototype
{
    public Ooz(int level) : base(level)
    {
        if (level <= 4)
        {
            name = "Ooz Blob";
            icon = "images/npc/ooz_blob";
            prefab_index = 23;

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
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (10,0,0), durability_max = 20 });
            stats.movement_time = 200;
            stats.to_hit = 5;
            stats.dodge = -20;
            stats.kill_experience = 15;

            stats.probability_resistances.SetResistance(DamageType.FIRE, DamageTypeResistances.VERY_WEAK);

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Dark Touch",
                    description = "Magical melee attack that deals dark damage",

                    damage =
                    {
                        (DamageType.DURABILITY, 2,4,0 ),
                        (DamageType.DARK, 1,2,0 ),                        
                    },

                    cost_stamina = 0,
                    recover_time = 100,
                    cooldown = 100,

                    icon = "images/talents/bite",

                    prepare_message = "The <name> wobbles.",
                    action_message = "The <name> punches.",
                }
            );
        }
        else if (level <= 7)
        {
            name = "Ooz Puddle";
            icon = "images/npc/ooz_puddle";
            prefab_index = 24;

            monster = new MonsterPrototype
            {
                ai_prototype = new AIPrototype
                {
                    personality = AIPersonality.Normal,
                }
            };

            stats.health_max = 20;
            stats.stamina_max = 5;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (10,0,0), durability_max = 40 });
            stats.movement_time = 200;
            stats.to_hit = 5;
            stats.dodge = -20;
            stats.kill_experience = 30;

            stats.probability_resistances.SetResistance(DamageType.FIRE, DamageTypeResistances.VERY_WEAK);

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Dark Touch",
                    description = "Magical melee attack that deals dark damage",

                    damage =
                    {
                        (DamageType.DURABILITY, 3,5,0 ),
                        (DamageType.DARK, 2,4,0 ),                        
                    },

                    cost_stamina = 0,
                    recover_time = 100,
                    cooldown = 100,

                    icon = "images/talents/bite",

                    prepare_message = "The <name> wobbles.",
                    action_message = "The <name> punches.",
                }
            );
        }
        else 
        {
            name = "Ooz Pile";
            icon = "images/npc/ooz_pile";
            prefab_index = 25;

            monster = new MonsterPrototype
            {
                ai_prototype = new AIPrototype
                {
                    personality = AIPersonality.Normal,
                }
            };

            stats.health_max = 30;
            stats.stamina_max = 5;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (10,0,0), durability_max = 60 });
            stats.movement_time = 200;
            stats.to_hit = 5;
            stats.dodge = -20;
            stats.kill_experience = 50;

            stats.probability_resistances.SetResistance(DamageType.FIRE, DamageTypeResistances.VERY_WEAK);

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Dark Touch",
                    description = "Magical melee attack that deals dark damage",

                    damage =
                    {
                        (DamageType.DURABILITY, 4,5,0 ),
                        (DamageType.DARK, 3,6,0 ),                        
                    },

                    cost_stamina = 0,
                    recover_time = 100,
                    cooldown = 100,

                    icon = "images/talents/bite",

                    prepare_message = "The <name> wobbles.",
                    action_message = "The <name> punches.",
                }
            );
        } 
    }
}

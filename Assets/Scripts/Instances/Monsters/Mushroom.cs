using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : ActorPrototype
{ 
    public Mushroom(int level) : base(level)
    {
        if (level <= 2)
        {
            name = "Red Mushroom";
            icon = "images/npc/red_mushroom";
            prefab_index = 14;

            monster = new MonsterPrototype
            {
                ai_personality = AIPersonality.Normal
            };
            can_move = false;

            stats.health_max = 10;
            stats.stamina_max = 5;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (1, 1, 1), durability_max = 10 });
            stats.movement_time = 100;
            stats.to_hit = 5;
            stats.dodge = 0;

            //stats.resistances.SetResistance(DamageType.FIRE, DamageTypeResistances.EXTREMELY_RESISTANT);

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Claw Hug",
                    description = "Physical attack that deals slash damage",

                    damage = {(DamageType.SLASH, 1, 4, 0)},

                    cost_stamina = 0,
                    recover_time = 100,
                    cooldown = 100,

                    icon = "images/talents/scratch",

                    prepare_message = "",
                    action_message = "The <name> hugs.",
                }
            );
        }
        else  if (level <= 4)
        {
            name = "Purple Mushroom";
            icon = "images/npc/purple_mushroom";
            prefab_index = 15;
            
            monster = new MonsterPrototype
            {
                ai_personality = AIPersonality.Normal
            };
            can_move = false;

            stats.health_max = 15;
            stats.stamina_max = 10;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (1, 1, 1), durability_max = 10 });
            stats.movement_time = 100;
            stats.to_hit = 5;
            stats.dodge = 0;

            stats.kill_experience = 20;

            //stats.resistances.SetResistance(DamageType.FIRE, DamageTypeResistances.EXTREMELY_RESISTANT);

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Claw Hug",
                    description = "Physical attack that deals slash damage",

                    damage = {(DamageType.SLASH, 2, 6, 0)},

                    cost_stamina = 0,
                    recover_time = 100,
                    cooldown = 100,

                    icon = "images/talents/scratch",

                    prepare_message = "",
                    action_message = "The <name> hugs.",
                }
            );

            talents.Add(
                new TalentPull
                {
                    name = "Pull",
                    description = "",

                    cost_stamina = 1,
                    prepare_time = 25,
                    recover_time = 75,
                    cooldown = 200,

                    icon = "images/talents/pull",

                    prepare_message = "The <name> shows its tongue.",
                    action_message = "The <name> pulls with its tongue.",

                    ai_data = new TalentAIInfo
                    {
                        type = TalentAIInfoType.Special,
                        player_range = 3,
                        use_probability = 1.0f,
                    },  
                }
            );
        }
        else 
        {
            name = "Yellow Mushroom";
            icon = "images/npc/yellow_mushroom";
            prefab_index = 16;
            
            monster = new MonsterPrototype
            {
                ai_personality = AIPersonality.Normal
            };
            can_move = false;

            stats.health_max = 25;
            stats.stamina_max = 10;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (2, 2, 2), durability_max = 20 });
            stats.movement_time = 100;
            stats.to_hit = 5;
            stats.dodge = 0;

            stats.kill_experience = 30;

            //stats.resistances.SetResistance(DamageType.FIRE, DamageTypeResistances.EXTREMELY_RESISTANT);

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Claw Hug",
                    description = "Physical attack that deals slash damage",

                    damage = {(DamageType.SLASH, 2, 6, 0), (DamageType.POISON, 1, 2, 0)},
                    poisons = { typeof(PoisonSleepVenom)},

                    cost_stamina = 0,
                    recover_time = 100,
                    cooldown = 100,

                    icon = "images/talents/bite",

                    prepare_message = "",
                    action_message = "The <name> hugs.",
                }
            );

             talents.Add(
                new TalentPull
                {
                    name = "Pull",
                    description = "",

                    cost_stamina = 1,
                    prepare_time = 25,
                    recover_time = 75,
                    cooldown = 200,

                    icon = "images/talents/pull",

                    prepare_message = "The <name> shows its tongue.",
                    action_message = "The <name> pulls with its tongue.",

                    ai_data = new TalentAIInfo
                    {
                        type = TalentAIInfoType.Special,
                        player_range = 4,
                        use_probability = 1.0f,
                    },  
                }
            );
        }
    }
}

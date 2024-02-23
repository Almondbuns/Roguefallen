using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flytrap : ActorPrototype
{
    public Flytrap(int level) : base(level)
    {
        if (level <= 8)
        {
            name = "Green Flytrap";
            icon = "images/npc/green_flytrap";
            prefab_index = 21;

            monster = new MonsterPrototype
            {
                ai_prototype = new AIPrototype
                {
                    personality = AIPersonality.Normal,
                }
            };
            can_move = false;

            stats.health_max = 30;
            stats.stamina_max = 10;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (1, 1, 1), durability_max = 10 });
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

                    damage = 
                    {
                        (DamageType.SLASH, 2,6,0),
                    },

                    cost_stamina = 0,
                    recover_time = 100,
                    cooldown = 200,

                    icon = "images/talents/scratch",

                    prepare_message = "",
                    action_message = "The <name> snaps.",
                }
            );

             talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Poisonous Claw Hug",
                    description = "Physical attack that deals slash damage",

                    damage = 
                    {
                        (DamageType.POISON, 1,3,0),
                    },
                    poisons = { typeof(PoisonSleepVenom) },
                    cost_stamina = 0,
                    prepare_time = 50,
                    recover_time = 100,
                    cooldown = 200,

                    icon = "images/talents/poison",

                    prepare_message = "The <name> smells strange.",
                    action_message = "The <name> spits poison.",
                }
            );

             talents.Add(
                new TalentTeleportTarget
                {
                    name = "Teleport",
                    description = "Teleport to target",

                    cost_stamina = 2,
                    prepare_time = 50,
                    recover_time = 50,
                    cooldown = 1000,

                    icon = "images/talents/teleport",

                    prepare_message = "The <name> flickers.",
                    action_message = "The <name> teleports.",
                }
            );
        }
        else
        {
            name = "Purpe Flytrap";
            icon = "images/npc/purple_flytrap";
            prefab_index = 22;

            monster = new MonsterPrototype
            {
                ai_prototype = new AIPrototype
                {
                    personality = AIPersonality.Normal,
                }
            };
            can_move = false;

            stats.health_max = 40;
            stats.stamina_max = 20;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (3, 3, 3), durability_max = 30 });
            stats.movement_time = 100;
            stats.to_hit = 10;
            stats.dodge = 0;
            stats.kill_experience = 50;

            //stats.resistances.SetResistance(DamageType.FIRE, DamageTypeResistances.EXTREMELY_RESISTANT);

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Claw Hug",
                    description = "Physical attack that deals slash damage",

                    damage = 
                    {
                        (DamageType.SLASH, 3,8,0),
                    },

                    cost_stamina = 0,
                    recover_time = 100,
                    cooldown = 200,

                    icon = "images/talents/scratch",

                    prepare_message = "",
                    action_message = "The <name> snaps.",
                }
            );

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Poisonous Claw Hug",
                    description = "Physical attack that deals slash damage",

                    damage = 
                    {
                        (DamageType.POISON, 1,3,0),
                    },
                    poisons = { typeof(PoisonSleepVenom), typeof(PoisonDeathBell) },                    

                    prepare_time = 50,
                    recover_time = 100,
                    cooldown = 200,

                    icon = "images/talents/poison",

                    prepare_message = "The <name> smells strange.",
                    action_message = "The <name> spits poison.",
                }
            );

            talents.Add(
                new TalentTeleportTarget
                {
                    name = "Teleport",
                    description = "Teleport to target",

                    cost_stamina = 2,
                    prepare_time = 50,
                    recover_time = 50,
                    cooldown = 500,

                    icon = "images/talents/teleport",

                    prepare_message = "The <name> flickers.",
                    action_message = "The <name> teleports.",
                }
            );
        }
    }
}
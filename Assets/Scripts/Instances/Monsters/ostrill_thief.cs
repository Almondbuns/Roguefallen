using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OstrillThief: ActorPrototype
{
    public OstrillThief(int level) : base(level)
    {
        if (level >= 0)
        {
            name = "Ostrill Thief";
            icon = "images/npc/ostrill_thief";
            prefab_index = 29;

           monster = new MonsterPrototype
            {
                ai_prototype = new AIPrototype
                {
                    personality = AIPersonality.Normal,
                }
            };

            stats.health_max = 15;
            stats.stamina_max = 20;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (1, 1, 1), durability_max = 10 });
            stats.movement_time = 100;
            stats.to_hit = 20;
            stats.dodge = 20;
            stats.kill_experience = 20;

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Dagger Attack",
                    description = "Physical melee attack that deals pierce damage",

                    damage = {(DamageType.PIERCE, 1, 6, 2)},

                    cost_stamina = 0,
                    recover_time = 100,
                    cooldown = 100,

                    icon = "images/talents/bite",

                    action_message = "The <name> attacks with two daggers.",
                } 
            );

            talents.Add(
                new TalentThrowAtPlayer
                {
                    name = "Throw throwing knife",
                    description = "Throw throwing knife",

                    object_type = typeof(ProjectileThrowingKnife),

                    cost_stamina = 5,
                    prepare_time = 50,
                    recover_time = 100,
                    cooldown = 1000,

                    icon = "images/objects/throwing_knife",

                    prepare_message = "The <name> raises a throwing knife.",
                    action_message = "The <name> shoots a throwing knife.",
                } 
            );

            talents.Add(
                new TalentThrowAtPlayer
                {
                    name = "Throw acid flask",
                    description = "Throw acid_flask",

                    object_type = typeof(ProjectileAcidFlask),

                    cost_stamina = 5,
                    prepare_time = 50,
                    recover_time = 100,
                    cooldown = 1000,

                    icon = "images/objects/acid_flask",

                    prepare_message = "The <name> raises an acid flask.",
                    action_message = "The <name> throws an acid flask.",
                } 
            );
        }   
    }
}

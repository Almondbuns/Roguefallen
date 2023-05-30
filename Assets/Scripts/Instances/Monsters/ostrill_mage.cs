using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OstrillMage: ActorPrototype
{
    public OstrillMage(int level) : base(level)
    {
        if (level >= 0)
        {
            name = "Ostrill Mage";
            icon = "images/npc/ostrill_mage";
            prefab_index = 30;

            monster = new MonsterPrototype
            {
                ai_personality = AIPersonality.Normal
            };

            stats.health_max = 10;
            stats.stamina_max = 10;
            stats.mana_max = 20;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (0, 0, 0), durability_max = 0 });
            stats.movement_time = 100;
            stats.to_hit = 10;
            stats.dodge = 10;
            stats.kill_experience = 30;

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Staff Punch",
                    description = "Physical melee attack that deals chrush damage",

                    damage = {(DamageType.CRUSH, 1, 4, 0)},

                    cost_stamina = 0,
                    recover_time = 100,
                    cooldown = 100,

                    icon = "images/talents/bite",

                    action_message = "The <name> attacks with a staff.",
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

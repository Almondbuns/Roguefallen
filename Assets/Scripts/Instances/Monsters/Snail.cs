using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.MaterialProperty;

public class Snail : ActorPrototype
{
    public Snail(int level) : base(level)
    {
       
        name = "Corrupted Snail";
        icon = "images/npc/snail";
        prefab_index = 48;

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
        stats.body_armor.Add(new ActorArmorStats { body_part = "body", percentage = 100, armor = (2, 2, 0)});
        stats.movement_time = 300;
        stats.to_hit = 5;
        stats.dodge = 0;
        stats.kill_experience = 10;

        talents.Add(
            new TalentStandardMeleeAttack
            {
                name = "Bite",
                description = "Bite attack",

                    damage = 
                {
                    (DamageType.PIERCE, 2,3,0),
                },

                cost_stamina = 0,
                recover_time = 50,
                cooldown = 50,

                icon = "images/talents/vampire_bite",

                prepare_message = "The <name> opens its mouth.",
                action_message = "The <name> bites.",
            }
        );
    }

    public override void OnMonsterKill(ActorData this_actor, ActorData killed_actor)
    {
        if (killed_actor == null || this_actor == null || this_actor == killed_actor)
            return;

        int snail_detection_range = 8;
        if (Math.Abs(this_actor.X - killed_actor.X) <= snail_detection_range && Math.Abs(this_actor.Y - killed_actor.Y) <= snail_detection_range)
        {
            GameLogger.Log("The " + name + " sees its friend die and rages.");
            this_actor.AddEffect(new EffectAddMovementTime { amount = -275, duration = 1000 });
        }
    }
}


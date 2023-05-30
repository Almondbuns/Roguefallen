using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OstrillWarrior : ActorPrototype
{
    public OstrillWarrior(int level) : base(level)
    {
        if (level >= 0)
        {
            name = "Ostrill Warrior";
            icon = "images/npc/ostrill_warrior";
            prefab_index = 28;

            monster = new MonsterPrototype
            {
                ai_personality = AIPersonality.Normal
            };

            stats.health_max = 20;
            stats.stamina_max = 10;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (2, 1, 0), durability_max = 10 });
            stats.movement_time = 100;
            stats.to_hit = 10;
            stats.dodge = 10;
            stats.kill_experience = 20;

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Axe Slash",
                    description = "Physical melee attack that deals slash damage",

                    damage = {(DamageType.SLASH, 2, 6, 0)},

                    cost_stamina = 0,
                    recover_time = 100,
                    cooldown = 100,

                    icon = "images/talents/bite",

                    action_message = "The <name> attacks with an axe.",
                } 
            );
        }   
    }
}

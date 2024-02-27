using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : ActorPrototype
{
    public Zombie(int level) : base(level)
    {
      
        name = "Zombie";
        icon = "images/npc/zombie";
        prefab_index = 39;

        monster = new MonsterPrototype
            {
                ai_prototype = new AIPrototype
                {
                    personality = AIPersonality.Normal,
                }
            };

        stats.health_max = 20;
        stats.stamina_max = 10;
        stats.mana_max = 0;
        stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (0, 0, 2), durability_max = 10 });
        stats.movement_time = 200;
        stats.to_hit = 10;
        stats.dodge = 10;
        stats.kill_experience = 20;

        talents.Add(
            new TalentStandardMeleeAttack
            {
                name = "Grab",
                description = "Physical melee attack that deals crush damage",

                damage = {(DamageType.CRUSH, 1, 5, 0)},

                cost_stamina = 0,
                recover_time = 100,
                cooldown = 100,

                icon = "images/talents/bite",

                action_message = "The <name> tries to grab.",
            } 
        );
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beehive : ActorPrototype
{ 
    public Beehive(int level) : base(level)
    {
        name = "Beehive";
        icon = "images/npc/bee_hive";
        prefab_index = 43;

        monster = new MonsterPrototype
        {
            ai_prototype = new AIPrototype
            {
                personality = AIPersonality.Normal,
            }
        };

        can_move = false;
        can_dodge = false;

        stats.health_max = 10;
        stats.stamina_max = 5;

        stats.body_armor.Add(new ActorArmorStats { body_part = "Body", percentage = 100, armor = (0, 0, 0) });

        talents.Add(new TalentSummon()
        {
            summon_type = typeof(Bee),
            prepare_time = 50,
            cooldown = 200,
            cost_stamina = 1,
            ai_data = new TalentAIInfo
            {
                type = TalentAIInfoType.Special,
                player_range = 2,
                use_probability = 0.9f,
            },
        });
    }
}

public class Bee : ActorPrototype
{
    public Bee(int level) : base(level)
    {
        name = "Bee";
        icon = "images/npc/bee";
        prefab_index = 44;
        stats.kill_experience = 5;

        monster = new MonsterPrototype
        {
            ai_prototype = new AIPrototype
            {
                personality = AIPersonality.Normal,
            }
        };

        stats.health_max = 3;
        stats.dodge = 10;

        stats.body_armor.Add(new ActorArmorStats { body_part = "Body", percentage = 100, armor = (0, 0, 0) });

        talents.Add(
                  new TalentStandardMeleeAttack
                  {
                      name = "Sting",
                      description = "Physical attack that deals piercing damage",

                      damage =
                          {
                            (DamageType.PIERCE, 1,1,1),
                          },

                      cost_stamina = 0,
                      recover_time = 50,
                      cooldown = 100,

                      icon = "images/talents/vampire_bite",

                      prepare_message = "The <name> stings.",
                      action_message = "The <name> stings.",
                  }
              );
    }
}

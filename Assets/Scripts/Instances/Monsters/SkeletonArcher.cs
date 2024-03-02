using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcher : ActorPrototype
{
    public SkeletonArcher(int level) : base(level)
    {
      
        name = "Skeleton Archer";
        icon = "images/npc/skeleton_archer";
        prefab_index = 38;

        monster = new MonsterPrototype
            {
                ai_prototype = new AIPrototype
                {
                    personality = AIPersonality.Distance,
                    prefered_distance = 8,
                }
            };

        stats.health_max = 10;
        stats.stamina_max = 10;
        stats.mana_max = 0;
        stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (0, 0, 0), durability_max = 10 });
        stats.movement_time = 100;
        stats.to_hit = 10;
        stats.dodge = 10;
        stats.kill_experience = 20;

        stats.probability_resistances.SetResistance(DamageType.PIERCE, DamageTypeResistances.VERY_RESISTANT);
        stats.probability_resistances.SetResistance(DamageType.CRUSH, DamageTypeResistances.VERY_WEAK);
        stats.probability_resistances.SetResistance(DamageType.DIVINE, DamageTypeResistances.VERY_WEAK);

        talents.Add(
                new TalentThrowAtPlayer
                {
                    name = "Shoot Bow",
                    description = "Shoot Bow",

                    object_type = typeof(ProjectileArrow),

                    cost_stamina = 0,
                    prepare_time = 50,
                    recover_time = 100,
                    cooldown = 200,

                    icon = "images/objects/arrow",

                    prepare_message = "The <name> prepares an arrow.",
                    action_message = "The <name> shoots its bow.",
                } 
            );
    }

    public override void OnKill(ActorData actor_data)
    {
        ActorData pile = new MonsterData(0,0, new SkeletonArcherPile(1));
        GameObject.Find("GameData").GetComponent<GameData>().current_map.Add(pile);
        pile.MoveTo(actor_data.X, actor_data.Y, true);
    }
}

public class SkeletonArcherPile : ActorPrototype
{
    public SkeletonArcherPile(int level) : base(level)
    {
        name = "Skeleton Pile";
        icon = "images/npc/skeleton_archer_pile";
        prefab_index = 41;
        can_move = false;
        can_dodge = false;

        monster = new MonsterPrototype
            {
                ai_prototype = new AIPrototype
                {
                    personality = AIPersonality.Normal,
                }
            };

        stats.health_max = 5;
        stats.stamina_max = 10;
        stats.mana_max = 0;
        stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (5, 5, 5), durability_max = 20 });
        stats.movement_time = 100;
        stats.to_hit = 10;
        stats.dodge = 10;
        stats.kill_experience = 20;

        stats.probability_resistances.SetResistance(DamageType.PIERCE, DamageTypeResistances.VERY_RESISTANT);
        stats.probability_resistances.SetResistance(DamageType.CRUSH, DamageTypeResistances.VERY_WEAK);
        stats.probability_resistances.SetResistance(DamageType.DIVINE, DamageTypeResistances.VERY_WEAK);

        talents.Add(new TalentSummon() {summon_type = typeof(SkeletonArcher)});
    }
}

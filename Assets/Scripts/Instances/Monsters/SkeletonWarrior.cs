using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonWarrior : ActorPrototype
{
    public SkeletonWarrior(int level) : base(level)
    {
        name = "Skeleton Warrior";
        icon = "images/npc/skeleton_warrior";
        prefab_index = 37;

        monster = new MonsterPrototype
            {
                ai_prototype = new AIPrototype
                {
                    personality = AIPersonality.Normal,
                }
            };

        stats.health_max = 15;
        stats.stamina_max = 10;
        stats.mana_max = 0;
        stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (2, 1, 0), durability_max = 10 });
        stats.movement_time = 100;
        stats.to_hit = 10;
        stats.dodge = 10;
        stats.kill_experience = 0;

        stats.probability_resistances.SetResistance(DamageType.PIERCE, DamageTypeResistances.VERY_RESISTANT);
        stats.probability_resistances.SetResistance(DamageType.CRUSH, DamageTypeResistances.VERY_WEAK);
        stats.probability_resistances.SetResistance(DamageType.DIVINE, DamageTypeResistances.VERY_WEAK);

        talents.Add(
            new TalentStandardMeleeAttack
            {
                name = "Sword Slash",
                description = "Physical melee attack that deals slash damage",

                damage = {(DamageType.SLASH, 1, 4, 0)},

                cost_stamina = 0,
                recover_time = 100,
                cooldown = 100,

                icon = "images/talents/bite",

                action_message = "The <name> attacks with an sword.",
            } 
        );
    }

    public override void OnKill(ActorData actor_data)
    {
        ActorData pile = new MonsterData(0,0, new SkeletonWarriorPile(1));
        GameObject.Find("GameData").GetComponent<GameData>().current_map.Add(pile);
        pile.MoveTo(actor_data.X, actor_data.Y, true);
    }
}

public class SkeletonWarriorPile : ActorPrototype
{
    public SkeletonWarriorPile(int level) : base(level)
    {
        name = "Skeleton Pile";
        icon = "images/npc/skeleton_warrior_pile";
        prefab_index = 40;
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

        talents.Add(new TalentSummon() {summon_type = typeof(SkeletonWarrior)});
    }
}
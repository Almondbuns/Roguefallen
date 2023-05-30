using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troll : ActorPrototype
{
    public Troll(int level) : base(level)
    {
        if (level >= 0)
        {
            name = "Cave Troll";
            icon = "images/npc/troll";
            prefab_index = 31;
            tile_width = 2;
            tile_height = 2;

            monster = new MonsterPrototype
            {
                ai_personality = AIPersonality.Normal
            };

            stats.health_max = 250;
            stats.stamina_max = 100;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 70, armor = (6, 4, 0), durability_max = 100 });
            stats.body_armor.Add(new ArmorStats { body_part = "head", percentage = 10, armor = (2, 2, 0), durability_max = 25 });
            stats.body_armor.Add(new ArmorStats { body_part = "feet", percentage = 10, armor = (2, 2, 0), durability_max = 25 });
            stats.body_armor.Add(new ArmorStats { body_part = "hands", percentage = 10, armor = (2, 2, 0), durability_max = 25 });
            stats.movement_time = 150;
            stats.to_hit = 15;
            stats.dodge = 15;
            stats.kill_experience = 1000;

            stats.probability_resistances.SetResistance(DamageType.FIRE, DamageTypeResistances.VERY_RESISTANT);
            stats.probability_resistances.SetResistance(DamageType.ICE, DamageTypeResistances.WEAK);

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Club Bash",
                    description = "Physical melee attack that deals crush damage",

                    damage = {(DamageType.CRUSH, 5, 10, 0)},

                    cost_stamina = 0,
                    prepare_time = 50,
                    recover_time = 100,
                    cooldown = 100,

                    icon = "images/talents/club",

                    prepare_message = "The <name> raises the club above his head.",
                    action_message = "The <name> swings the club.",
                } 
            );

            talents.Add(
                new TalentBossTrollThrowFirebomb
                {
                    name = "Exploding Charm",
                    description = "The Troll throws four exploding firebombs",

                    cost_stamina = 0,
                    prepare_time = 500,
                    recover_time = 500,
                    cooldown = 3000,

                    icon = "images/talents/bomb",

                    prepare_message = "The <name> grabs four firebombs.",
                    action_message = "The <name> throws the firebombs.",

                } 
            );

            talents.Add(
                new TalentBossTrollEarthquake
                {
                    name = "Earthquake",
                    description = "Deals area damage around the troll that can stun",

                    cost_stamina = 0,
                    prepare_time = 500,
                    recover_time = 200,
                    cooldown = 3000,

                    icon = "images/talents/earthquake",

                    prepare_message = "The <name> growls angrily.",
                    action_message = "The <name> stamps his foot on the ground.",
                } 
            );
        }   
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class OctopusTentacle : ActorPrototype
{
    public OctopusTentacle(int level) : base(level)
    {
        name = "Tentacle";
        icon = "images/npc/octopus_tentacle";

        monster = new MonsterPrototype
            {
                ai_prototype = new AIPrototype
                {
                    personality = AIPersonality.Normal,
                }
            };

        stats.health_max = 20;
        stats.stamina_max = 5;
        stats.mana_max = 0;
        stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (0, 0, 0), durability_max = 0 });
        stats.movement_time = 100;
        stats.to_hit = 5;
        stats.dodge = 5;

        //talents.Add(new TalentMultiply());
    }
}

public class Octopus : ActorPrototype
{
    public Octopus(int level) : base(level)
    {
        name = "Octopus";
        icon = "images/npc/octopus_boss";

       monster = new MonsterPrototype
            {
                ai_prototype = new AIPrototype
                {
                    personality = AIPersonality.Normal,
                }
            };

        stats.health_max = 100;
        stats.stamina_max = 50;
        stats.mana_max = 0;
        stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (100, 100, 100), durability_max = 1000 });
        stats.movement_time = 1000;
        stats.to_hit = 5;
        stats.dodge = 0;

        //talents.Add(new TalentBiteAttack(1, 3));
        //talents.Add(new TalentMultiply());
    }
}





public class LostExplorer : ActorPrototype
{
    public LostExplorer(int level) : base(level)
    {
        name = "The Lost Explorer";
        icon = "images/npc/lost_explorer";
        monster = new MonsterPrototype
            {
                ai_prototype = new AIPrototype
                {
                    personality = AIPersonality.Normal,
                }
            };

        stats.health_max = 50;
        stats.stamina_max = 20;
        stats.mana_max = 20;
        stats.body_armor.Add(new ArmorStats { body_part = "Explorer", percentage = 50, armor = (2, 2, 0), durability_max = 20 });
        stats.body_armor.Add(new ArmorStats { body_part = "Yntor", percentage = 50, armor = (0, 0, 2), durability_max = 20 });
        stats.movement_time = 100;
        stats.to_hit = 15;
        stats.dodge = 10;

        stats.probability_resistances.SetResistance(DamageType.DIVINE, DamageTypeResistances.WEAK);
        stats.probability_resistances.SetResistance(DamageType.DARK, DamageTypeResistances.RESISTANT);

        talents.Add(
            new TalentStandardMeleeAttack
            {
                name = "Claw Hug",
                description = "Physical attack that deals slash damage",

                /*damage_type = DamageType.DARK,
                damage_min = 5,
                damage_max = 5,*/

                cost_stamina = 1,
                recover_time = 100,
                cooldown = 100,

                icon = "images/talents/bite",

                prepare_message = "",
                action_message = "The <name> hugs.",
            }
        );
    }
}

public class Barkeeper : ActorPrototype
{
    public Barkeeper(int level) : base(level)
    {
        name = "Barkeeper";
        icon = "images/npc/barkeeper";
        monster = new MonsterPrototype
            {
                ai_prototype = new AIPrototype
                {
                    personality = AIPersonality.Normal,
                }
            };

        stats.health_max = 50;
        stats.stamina_max = 20;
        stats.mana_max = 20;
        stats.body_armor.Add(new ArmorStats { body_part = "Body", percentage = 100, armor = (1, 1, 1), durability_max = 20 });
        stats.movement_time = 100;
        stats.to_hit = 15;
        stats.dodge = 10;
    }
}

public class Questgiver1 : ActorPrototype
{
    public Questgiver1(int level) : base(level)
    {
        name = "Questgiver";
        icon = "images/npc/questgiver_1";
        monster = new MonsterPrototype
            {
                ai_prototype = new AIPrototype
                {
                    personality = AIPersonality.Normal,
                }
            };

        stats.health_max = 50;
        stats.stamina_max = 20;
        stats.mana_max = 20;
        stats.body_armor.Add(new ArmorStats { body_part = "Body", percentage = 100, armor = (1, 1, 1), durability_max = 20 });
        stats.movement_time = 100;
        stats.to_hit = 15;
        stats.dodge = 10;
    }
}

public class Shopkeeper : ActorPrototype
{
    public Shopkeeper(int level) : base(level)
    {
        name = "Shopkeeper";
        icon = "images/npc/questgiver_1";
        prefab_index = 32;
        
        monster = new MonsterPrototype
            {
                ai_prototype = new AIPrototype
                {
                    personality = AIPersonality.Normal,
                }
            };

        stats.health_max = 150;
        stats.stamina_max = 20;
        stats.mana_max = 20;
        stats.body_armor.Add(new ArmorStats { body_part = "Body", percentage = 100, armor = (1, 1, 1), durability_max = 20 });
        stats.movement_time = 100;
        stats.to_hit = 15;
        stats.dodge = 10;
    }
}
using System.Collections.Generic;
using System;
using System.IO;

public class PlayerStatsData
{
    public int experience = 0;
    public int level = 1;

    public int strength = 5;
    public int vitality = 5;
    public int dexterity = 5;
    public int constitution = 5;
    public int intelligence = 5;
    public int willpower = 5;

    public int perception = 5;

    public int attribute_points = 0;
    public int skill_expertise_points = 0;
    public int talent_points = 0;

    public  SkillTreeData skill_tree;


    internal void Save(BinaryWriter save)
    {
        save.Write(experience);
        save.Write(level);
        save.Write(strength);
        save.Write(vitality);
        save.Write(dexterity);
        save.Write(constitution);
        save.Write(intelligence);
        save.Write(willpower);
        save.Write(attribute_points);
        save.Write(perception);
        save.Write(skill_expertise_points);
        save.Write(talent_points);

        skill_tree.Save(save);
    }

    internal void Load(BinaryReader save)
    {
        experience = save.ReadInt32();
        level = save.ReadInt32();
        strength = save.ReadInt32();
        vitality = save.ReadInt32();
        dexterity = save.ReadInt32();
        constitution = save.ReadInt32();
        intelligence = save.ReadInt32();
        willpower = save.ReadInt32();
        attribute_points = save.ReadInt32();
        perception = save.ReadInt32();
        skill_expertise_points = save.ReadInt32();
        talent_points = save.ReadInt32();

        skill_tree.Load(save);
    }

    public PlayerStatsData(int starting_level)
    {
        attribute_points = 5 * (starting_level-1);
        skill_expertise_points = (starting_level-1);
        talent_points = (starting_level-1);
        level = starting_level;

        if (GameData.GODMODE == true)
        {
            attribute_points = 9999;
            skill_expertise_points = 9999;
            talent_points = 9999;
        }
        
            skill_tree = new()
        {
            skills = new List<SkillData>
            {
                new SkillData
                {
                    name = "Blunt Weapons",
                    icon = "images/skills/skill_blunt",
                    expertises = new List<SkillExpertiseData>
                    {
                        new SkillExpertiseData
                        {
                            level = SkillExpertiseLevel.NOVICE,
                            talents = new List<SkillTalentData>
                            {
                             

                                new SkillTalentData
                                {
                                    requirement = SkillTalentRequirement.BluntWeapon,
                                    talent = new TalentData(new TalentBluntDoubleAttack()),
                                    description = "When using a blunt weapon in your main hand deal two quick attacks in a row.",
                                },

                                new SkillTalentData
                                {
                                    requirement = SkillTalentRequirement.BluntWeapon,
                                    talent = new TalentData(new TalentBluntAccuracyBonus()),
                                    description = "When using a blunt weapon in your main hand increase your accuracy but also increase your attack time.",
                                },                         
                            }
                        },

                        new SkillExpertiseData
                        {
                            level = SkillExpertiseLevel.ADEPT,
                            talents = new List<SkillTalentData>
                            {
                                new SkillTalentData
                                {
                                    requirement = SkillTalentRequirement.BluntWeapon,
                                    talent = new TalentData(new TalentBluntAttackMovementDebuff()),
                                    description = "When using a blunt weapon in your main hand hit the feet of the target to slow it down.",
                                },

                                new SkillTalentData
                                {
                                    requirement = SkillTalentRequirement.BluntWeapon,
                                    talent = new TalentData(new TalentBluntAdeptBlunt()),
                                    description = "When using a blunt weapon in your main hand increase your minimum and maximum damage by 1.",
                                },
                            }
                        },

                        new SkillExpertiseData
                        {
                            level = SkillExpertiseLevel.EXPERT,
                            talents = new List<SkillTalentData>
                            {
                                 new SkillTalentData
                                {
                                    requirement = SkillTalentRequirement.BluntWeapon,
                                    talent = new TalentData(new TalentBluntAttackStun()),
                                    description = "When using a blunt weapon in your main hand deal a fearsome blow that will stun your target.",
                                },

                                   new SkillTalentData
                                {
                                    requirement = SkillTalentRequirement.BluntWeapon,
                                    talent = new TalentData(new TalentBluntAttackKnockback()),
                                    description = "When using a blunt weapon in your main hand attack with your weapon and knock back the enemy.",
                                },
                            }
                        },

                        new SkillExpertiseData
                        {
                            level = SkillExpertiseLevel.MASTER,
                            talents = new List<SkillTalentData>
                            {
                                 new SkillTalentData
                                {
                                    requirement = SkillTalentRequirement.BluntWeapon,
                                    talent = new TalentData(new TalentBluntEarthquake()),
                                    description = "When using a blunt weapon in your main hand create an earthquake that damages all enemies nearby.",
                                },
                            }
                        }
                    }
                },

                new SkillData
                {
                    name = "Heavy Armor",
                    icon = "images/skills/skill_armor_heavy",
                    expertises = new List<SkillExpertiseData>
                    {
                        new SkillExpertiseData
                        {
                            level = SkillExpertiseLevel.NOVICE,
                            talents = new List<SkillTalentData>
                            {                            
                                new SkillTalentData
                                {
                                    requirement = SkillTalentRequirement.HeavyArmor,
                                    talent = new TalentData(new TalentHeavyArmorRush()),
                                    description = "Rush into your target and deal damage equal to the sum of physical armor of all your heavy armor pieces.",
                                },

                                new SkillTalentData
                                {
                                    requirement = SkillTalentRequirement.HeavyArmor,
                                    talent = new TalentData(new TalentHeavyArmorRun()),
                                    description = "Run away in your heavy armor. Reduce movement time by 30% but lose one point of stamina each turn.",
                                },
                            }
                        },

                        new SkillExpertiseData
                        {
                            level = SkillExpertiseLevel.ADEPT,
                            talents = new List<SkillTalentData>
                            {         
                                new SkillTalentData
                                {
                                    requirement = SkillTalentRequirement.HeavyArmor,
                                    talent = new TalentData(new TalentHeavyArmorPhysicalArmor()),
                                    description = "When using a piece of heavy armor increase your physical armor by 1.",
                                },                   
                               
                                new SkillTalentData
                                {
                                    requirement = SkillTalentRequirement.HeavyArmor,
                                    talent = new TalentData(new TalentHeavyArmorSuitUp()),
                                    description = "For 10 turns increase your physical armor by 5 and elemental armor by 3 but also increase your movement rate by 200.",
                                },
                            }
                        },

                        new SkillExpertiseData
                        {
                            level = SkillExpertiseLevel.EXPERT,
                            talents = new List<SkillTalentData>
                            {          
                                 new SkillTalentData
                                {
                                    requirement = SkillTalentRequirement.HeavyArmor,
                                    talent = new TalentData(new TalentHeavyArmorElementalArmor()),
                                    description = "When using a piece of heavy armor increase your elemental armor by 1.",
                                },
                                                  
                                new SkillTalentData
                                {
                                    requirement = SkillTalentRequirement.HeavyArmor,
                                    talent = new TalentData(new TalentHeavyArmorMagicalArmor()),
                                    description = "When using a piece of heavy armor increase your magical armor by 1.",
                                },
                            }
                        },

                        new SkillExpertiseData
                        {
                            level = SkillExpertiseLevel.MASTER,
                            talents = new List<SkillTalentData>
                            {
                                new SkillTalentData
                                {
                                    requirement = SkillTalentRequirement.HeavyArmor,
                                    talent = new TalentData(new TalentHeavyArmorDurabilityArmor()),
                                    description = "When using a piece of heavy armor increase durability of your armor by 50%.",
                                },
                            }
                        }
                    }
                },

                new SkillData
                {
                    name = "Shields",
                    icon = "images/skills/skill_shield",
                    expertises = new List<SkillExpertiseData>
                    {
                        new SkillExpertiseData
                        {
                            level = SkillExpertiseLevel.NOVICE,
                            talents = new List<SkillTalentData>
                            {   
                                new SkillTalentData
                                {
                                    talent = new TalentData(new TalentShieldPassiveBlock()),
                                    description = "When attacked you have a 20% chance to add your shield armor to your body armor.",
                                },                                                         

                                new SkillTalentData
                                {
                                    talent = new TalentData(new TalentShieldActiveBlock()),
                                    description = "When attacked add your shield armor to your body armor but be slower and lose 1 stamina with each hit.",
                                },  
                            }
                        },

                        new SkillExpertiseData
                        {
                            level = SkillExpertiseLevel.ADEPT,
                            talents = new List<SkillTalentData>
                            {         
                                new SkillTalentData
                                {
                                    talent = new TalentData(new TalentShieldAdvancedParry()),
                                    description = "When using parry with a shield increase your chance to parry by another 20%.",
                                }, 
                                new SkillTalentData
                                {
                                    talent = new TalentData(new TalentShieldAdvancedBash()),
                                    description = "When using bash with a shield increase your damage by 50% and increase daze time by a turn.",
                                },                                                                                                             
                            }
                        },

                        new SkillExpertiseData
                        {
                            level = SkillExpertiseLevel.EXPERT,
                            talents = new List<SkillTalentData>
                            {          
                                new SkillTalentData
                                {
                                    talent = new TalentData(new TalentShieldPassiveBlockSkilled()),
                                    description = "When attacked you have an additional 10% chance to add your shield armor to your body armor.",
                                },                                                      
                                new SkillTalentData
                                {
                                    talent = new TalentData(new TalentShieldThrow()),
                                    description = "Throws the currently equipped shield at a target dealing four times its weight as pierce damage.",
                                },                                                      
                            }
                            
                        },

                        new SkillExpertiseData
                        {
                            level = SkillExpertiseLevel.MASTER,
                            talents = new List<SkillTalentData>
                            {
                                new SkillTalentData
                                {
                                    talent = new TalentData(new TalentShieldBlockMastery()),
                                    description = "Additional 20% passive block chance, 15% parry chance and 50% bash damage.",
                                },               
                            }
                        }
                    }
                },

                new SkillData
                {
                    name = "Awareness",
                    icon = "images/skills/skill_awareness",
                    expertises = new List<SkillExpertiseData>
                    {
                        new SkillExpertiseData
                        {
                            level = SkillExpertiseLevel.NOVICE,
                            talents = new List<SkillTalentData>
                            {
                                new SkillTalentData
                                {
                                    talent = new TalentData(new TalentAwarenessTrapSpotting()),
                                    description = "You have a 50% higher chance of spotting traps.",
                                },

                                new SkillTalentData
                                {
                                    talent = new TalentData(new TalentAwarenessMonsterStats()),
                                    description = "You can see the hitpoints, stamina and mana of your enemies.",
                                },
                            }
                        },

                        new SkillExpertiseData
                        {
                            level = SkillExpertiseLevel.ADEPT,
                            talents = new List<SkillTalentData>
                            {                        
                                new SkillTalentData
                                {
                                    talent = new TalentData(new TalentAwarenessThrowingWeaponAmount()),
                                    description = "Find 50% more throwing weapons.",
                                },

                                new SkillTalentData
                                {
                                    talent = new TalentData(new TalentAwarenessMonsterAdvancedStats()),
                                    description = "You can see the to hit and dodge chance and the movement rate of enemies.",
                                },
                            }
                        },

                        new SkillExpertiseData
                        {
                            level = SkillExpertiseLevel.EXPERT,
                            talents = new List<SkillTalentData>
                            {
                                 new SkillTalentData
                                {
                                    talent = new TalentData(new TalentAwarenessThrowingWeaponDamage()),
                                    description = "Throwing weapons deal 50% more damage.",
                                },

                                new SkillTalentData
                                {
                                    talent = new TalentData(new TalentAwarenessMonsterArmor()),
                                    description = "You can see the armor values of enemies.",
                                },
                            }
                        },

                        new SkillExpertiseData
                        {
                            level = SkillExpertiseLevel.MASTER,
                            talents = new List<SkillTalentData>
                            {
                                new SkillTalentData
                                {                                 
                                    talent = new TalentData(new TalentAwarenessMonsterResistances()),
                                    description = "You can see the resistances of enemies.",                                  
                                },
                            }
                        }
                    }
                },      
            }
        };
    }

    internal void Tick()
    {
        skill_tree.Tick();
    }
}
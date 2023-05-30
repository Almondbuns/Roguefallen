using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Irch : ActorPrototype
{
    public Irch(int level) : base(level)
    {
        if (level <= 20 )
        {
            name = "Irch";
            icon = "images/npc/irch";
            prefab_index = 34;
            tile_width = 1;
            tile_height = 2;

            monster = new MonsterPrototype
            {
                ai_personality = AIPersonality.Normal,
            };

            stats.health_max = 250;
            stats.stamina_max = 50;
            stats.mana_max = 0;
            stats.body_armor.Add(new ArmorStats { body_part = "body", percentage = 100, armor = (0, 0, 0), durability_max = 200 });
            stats.movement_time = 150;
            stats.to_hit = 10;
            stats.dodge = 5;
            stats.kill_experience = 500;

            stats.probability_resistances.SetResistance(DamageType.FIRE, DamageTypeResistances.VERY_WEAK);
            stats.probability_resistances.SetResistance(DamageType.DARK, DamageTypeResistances.VERY_WEAK);
            stats.probability_resistances.SetResistance(DamageType.DIVINE, DamageTypeResistances.VERY_RESISTANT);

            talents.Add(
                new TalentStandardMeleeAttack
                {
                    name = "Slash",
                    description = "Slash attack that deals divine damage",

                    damage = 
                    {
                        (DamageType.DIVINE, 2,4,0),
                    },

                    cost_stamina = 0,
                    recover_time = 100,
                    cooldown = 100,

                    icon = "images/talents/scratch",

                    prepare_message = "The <name> shows her nails.",
                    action_message = "The <name> scratches.",
                }
            );

            talents.Add(
                new TalentActiveEffects
                {
                    name = "Armor Buff",
                    type = TalentType.Active,
                    target = TalentTarget.Self,
                    target_range = 0,
                    icon = "images/talents/armor",
                    cost_stamina = 0,
                    prepare_time = 500,
                    recover_time = 100,
                    cooldown = 4000,

                    description = "A +10 armor buff to all defenses for 20 turns.",

                    prepare_message = "The <name> starts to tremble.",
                    action_message = "The skin of the <name> seems to be thicker now.",

                    ai_data = new TalentAIInfo
                    {
                        type = TalentAIInfoType.Special,
                        use_probability = 0.5f,
                        player_range = 10,
                    },

                    active_effects = new List<EffectData>(){
                        new EffectAddArmorPhysical { amount = 10, duration = 2000 },
                        new EffectAddArmorElemental { amount = 10, duration = 2000 },
                        new EffectAddArmorMagical { amount = 10, duration = 2000 }},
                }
            );

            talents.Add(
                new TalentActiveEffects
                {
                    name = "Speed Buff",
                    type = TalentType.Active,
                    target = TalentTarget.Self,
                    target_range = 0,
                    icon = "images/talents/speedup",
                    cost_stamina = 0,
                    prepare_time = 500,
                    recover_time = 100,
                    cooldown = 4000,

                    description = "A -50% movement time buff for 20 turns.",

                    prepare_message = "The <name> stamps on the ground.",
                    action_message = "The <name> suddenly moves faster.",

                    ai_data = new TalentAIInfo
                    {
                        type = TalentAIInfoType.Special,
                        use_probability = 0.2f,
                        player_range = 10,
                    },

                    active_effects = new List<EffectData>(){
                        new EffectRemoveMovementTimeRelative { amount = 50, duration = 2000 },
                    }
                }
            );

            talents.Add(
                new TalentRadiusAllAttack
                {
                    name = "Confusion Scream",
                    description = "",
                    target_range = 5,

                    damage = 
                    {
                        (DamageType.DIVINE, 0,0,0),
                    },

                    effects = {
                        new EffectInterrupt(){damage_type = DamageType.DIVINE},
                        new EffectConfusion(){damage_type = DamageType.DIVINE, duration = 1000, amount = 1},
                        },

                    cost_stamina = 10,
                    prepare_time = 500,
                    recover_time = 100,
                    cooldown = 3000,

                    icon = "images/talents/scream",

                    prepare_message = "The <name> takes a deep breath.",
                    action_message = "The <name> screams terribly.",

                    ai_data = new TalentAIInfo
                    {
                        type = TalentAIInfoType.Special,
                        use_probability = 0.5f,
                        player_range = 8,
                    },
                }
            );
        }
      
    }
}


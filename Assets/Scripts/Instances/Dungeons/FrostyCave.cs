using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostyCave : DungeonData
{
    public FrostyCave()
    {
        name = "The Frosty Cave";

        for (int level = 0; level < 5; ++level)
        {
            DungeonLevelData level_data = new DungeonLevelData
            {
                biome_index = 5,
        
                map_features =
                {                    
                    
                    //(typeof(MFCaveTreasureRoom), 0, 1), 
                    //(typeof(MFCaveOilRoom), 0, 2),                                           
                },                

                encounters =
                {
                    /*(1, new EncounterData() { type_amounts = {(typeof(Roach),1,1)}, level_min = 2, level_max = 7,}),
                    (1, new EncounterData() { type_amounts = {(typeof(Centipede),1,1)}, level_min = 3, level_max = 5,}),
                    (1, new EncounterData() { type_amounts = {(typeof(Fly),1,2)}, level_min = 1, level_max = 4,}),
                    (1, new EncounterData() { type_amounts = {(typeof(Worm),1,3)}, level_min = 1, level_max = 9,}),
                    (1, new EncounterData() { type_amounts = {(typeof(Rat),1,3)}, level_min = 1, level_max = 6,}),
                    (1, new EncounterData() { type_amounts = {(typeof(Ooz),1,1)}, level_min = 2, level_max = 10,}),
                    (1, new EncounterData() { type_amounts = {(typeof(Bear),1,1)}, level_min = 6, level_max = 10,}),
                    (1, new EncounterData() { type_amounts = {(typeof(Bat),3,5)}, level_min = 5, level_max = 10,}),
                    (1, new EncounterData() { type_amounts = {(typeof(Mushroom),1,1)}, level_min = 1, level_max = 6,}),
                    (1, new EncounterData() { type_amounts = {(typeof(Flytrap),2,4)}, level_min = 6, level_max = 10,}),
                    (1, new EncounterData() { type_amounts = {(typeof(Spider),1,1)}, level_min = 1, level_max = 6,}),
                    (1, new EncounterData() { type_amounts = {(typeof(Lemming),2,5)}, level_min = 1, level_max = 6,}),
                    (1, new EncounterData() { type_amounts = {(typeof(OstrillWarrior),1,2)}, level_min = 6, level_max = 6,}),
                    (1, new EncounterData() { type_amounts = {(typeof(OstrillWarrior),1,2),(typeof(OstrillThief),1,1)}, level_min = 7, level_max = 7,}),
                    (1, new EncounterData() { type_amounts = {(typeof(OstrillWarrior),1,2),(typeof(OstrillThief),1,2)}, level_min = 8, level_max = 8,}),
                    (1, new EncounterData() { type_amounts = {(typeof(OstrillWarrior),1,3),(typeof(OstrillThief),1,3)}, level_min = 9, level_max = 9,}),
                    (1, new EncounterData() { type_amounts = {(typeof(OstrillWarrior),2,3),(typeof(OstrillThief),1,3)}, level_min = 10, level_max = 10,}),                                
                */
                }
            };

            if (level >= 2)
                level_data.map_features.Add((typeof(MFCavePoisonFlowerRoom), 0, 1));

            if (UnityEngine.Random.value < 0.1f)
                level_data.map_features.Add((typeof(MFCaveStoreConsumables), 1,1));

            if (UnityEngine.Random.value < 0.2f)
                level_data.map_features.Add((typeof(MFCaveStorageRoom), 1, 1));

            if (UnityEngine.Random.value < 0.1f)
                level_data.map_features.Add((typeof(MFCaveStoreUsables), 1,1));

            if (UnityEngine.Random.value < 0.1f)
                level_data.map_features.Add((typeof(MFCaveSpiderRoom), 0, 1));


            if (level > 0 && level <= 6 && UnityEngine.Random.value < 0.1f)
                level_data.map_features.Add((typeof(MFCaveMonsterLair), 1, 1));

            //Add bosses
            if (level == 4)
                level_data.map_features.Add((typeof(MFCaveIrchBossRoom),1,1));
            if (level == 9)
                level_data.map_features.Add((typeof(MFCaveTrollBossRoom),1,1));

            if (level <= 4)
            {
                level_data.dimensions = (128, 64);
                level_data.difficulty_level = level + 1;
                level_data.number_of_rooms = (40, 50);
                level_data.number_of_encounters = (10, 15);
                level_data.number_of_gold_items = (5, 10);

                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemHealthPotion), prob_amount = {(1.0f, 1)}});
                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemAlmondBun), prob_amount = {(1.0f, 1)}});
                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemStaminaPotion), prob_amount = {(1.0f, 1)}});
                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemMeatHorn), prob_amount = {(1.0f, 1)}});

                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemRepairPowder), prob_amount = {(.8f, 0),(.2f, 1)}});

                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemHammer1H), prob_amount = {(.2f, 0),(.3f, 1),(.2f, 2), (.2f, 3), (.1f, 4)}});
                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemMace1H), prob_amount = {(.2f, 0),(.3f, 1),(.2f, 2), (.2f, 3), (.1f, 4)}});
                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemFlail1H), prob_amount = {(.2f, 0),(.3f, 1),(.2f, 2), (.2f, 3), (.1f, 4)}});
                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemWarHammer2H), prob_amount = {(.2f, 0),(.3f, 1),(.2f, 2), (.2f, 3), (.1f, 4)}});

                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemHandAxe1H), prob_amount = {(.2f, 0),(.3f, 1),(.2f, 2), (.2f, 3), (.1f, 4)}});
                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemDoubleAxe1H), prob_amount = {(.2f, 0),(.3f, 1),(.2f, 2), (.2f, 3), (.1f, 4)}});
                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemPickaxe1H), prob_amount = {(.2f, 0),(.3f, 1),(.2f, 2), (.2f, 3), (.1f, 4)}});
                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemBattleAxe2H), prob_amount = {(.2f, 0),(.3f, 1),(.2f, 2), (.2f, 3), (.1f, 4)}});

                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemBootsHeavy), prob_amount = {(.1f, 0),(.2f, 1),(.3f, 2), (.2f, 3), (.2f, 4)}});
                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemChestHeavy), prob_amount = {(.1f, 0),(.2f, 1),(.3f, 2), (.2f, 3), (.2f, 4)}});
                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemHandsHeavy), prob_amount = {(.1f, 0),(.2f, 1),(.3f, 2), (.2f, 3), (.2f, 4)}});
                
                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemHeadHeavy), prob_amount = {(.1f, 0),(.2f, 1),(.3f, 2), (.2f, 3), (.2f, 4)}});
                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemShieldHeavy), prob_amount = {(.1f, 0),(.2f, 1),(.3f, 2), (.2f, 3), (.2f, 4)}});
                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemShieldMedium), prob_amount = {(.1f, 0),(.2f, 1),(.3f, 2), (.2f, 3), (.2f, 4)}});

                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemRing), prob_amount = {(.70f, 0),(.25f, 1),(.05f, 2)}});
                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemAmulet), prob_amount = {(.70f, 0),(.25f, 1),(.05f, 2)}});

                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemFirebomb), prob_amount = {(.25f, 0), (.5f, 1), (.25f, 2)}});                
                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemThrowingKnife), prob_amount = {(.25f, 0), (.5f, 1), (.25f, 2)}});
                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemAcidFlask), prob_amount = {(.25f, 0), (.5f, 1), (.25f, 2)}});

                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemPoemOfReturn), prob_amount = {(.7f, 0), (.2f, 1), (.1f, 2)}});
                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemPoemOfAWalk), prob_amount = {(.7f, 0), (.2f, 1), (.1f, 2)}});
                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemPoemOfAJourney), prob_amount = {(.7f, 0), (.2f, 1), (.1f, 2)}});

                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemCamomileTea), prob_amount = {(.95f, 0), (.05f, 1)}});
                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemPeppermintTea), prob_amount = {(.95f, 0), (.05f, 1)}});
                level_data.items.Add(new ItemPlacementData(){type = typeof(ItemStrawberryTea), prob_amount = {(.95f, 0), (.05f, 1)}});

                level_data.dynamic_objects = new()
                {
                    (typeof(Crate), 5, 10),
                    (typeof(Jar), 5, 10),
                    (typeof(BrokenCrate), 5, 10),
                    (typeof(Chest), 0, 1),
                    //(typeof(BearTrap), 5, 10),
                    (typeof(IceSpikeTrap), 5, 10),
                    (typeof(IceWaterTrap), 50, 100),
                    //(typeof(SpiderWebTrap), 10, 20),
                };
            }
          
            if (level == 0)
            {
                level_data.dungeon_changes.Add
                (
                    new DungeonChangeData
                    {
                        name = "World Map Exit",
                        dungeon_change_type = typeof(MFCaveExit),
                        target_dungeon_name = "World Map",
                        target_entrance_name = "Frosty Cave Entrance",
                        target_entrance_parameter = "Up",
                    }
                );
            }

            if (level < 9)
                level_data.dungeon_changes.Add
                (
                    new DungeonChangeData
                    {
                        name = "Level " + (level + 2) + " Enter",
                        dungeon_change_type = typeof(MFCaveExit),
                        target_dungeon_name = "The Frosty Cave",
                        target_entrance_name = "Level " + (level + 2) + " Exit",
                        target_entrance_parameter = "Down",
                    }
                );

            if (level > 0)
                level_data.dungeon_changes.Add
                (
                    new DungeonChangeData
                    {
                        name = "Level " + (level + 1) + " Exit",
                        dungeon_change_type = typeof(MFCaveExit),
                        target_dungeon_name = "The Frosty Cave",
                        target_entrance_name = "Level " + (level + 1) + " Enter",
                        target_entrance_parameter = "Up",
                    }
                );

            dungeon_levels.Add(level_data);
        }
    }

    public override void Tick()
    {
        ++tick_counter;

        if (tick_counter >= 5000)
        {
            tick_counter = 0;
            GameLogger.Log("A chilly gust blows through the cave.");
            GameObject.Find("GameData").GetComponent<GameData>().player_data.TryToGainEffect(
                new EffectAddMovementTime() { damage_type = DamageType.ICE, amount = 50, duration = 2000});
        }
    }

}

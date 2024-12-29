using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RottenForest : DungeonData
{
    public RottenForest()
    {
        name = "The Rotten Forest";

        for (int level = 0; level < 5; ++level)
        {
            DungeonLevelData level_data = new DungeonLevelData
            {
                biome_index = 7,
                biome_variant = level,
                is_always_visible = true,
        
                map_features =
                {                                        
                    (typeof(MFCaveTreasureRoom), 0, 1), 
                    (typeof(MFCaveOilRoom), 0, 2),
                    (typeof(MFBeeTreasureRoom), 1, 2),
                },                

                encounters =
                {
                    (1, new EncounterData() { type_amounts = {(typeof(Beehive),1,1)}, level_min = 1, level_max = 1,}),
                }
            };

        
            level_data.map_features.Add((typeof(MFLivingForest),2,3));
            level_data.map_features.Add((typeof(MFPond), 1, 2));


            level_data.dimensions = (140, 70);
            level_data.difficulty_level = level + 1;
            level_data.number_of_rooms = (30, 60);
            level_data.number_of_encounters = (10, 10);
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
                (typeof(Chest), 0, 2),
            };        
          
            if (level == 0)
            {
                level_data.dungeon_changes.Add
                (
                    new DungeonChangeData
                    {
                        name = "World Map Exit",
                        dungeon_change_type = typeof(MFCaveExit),
                        target_dungeon_name = "World Map",
                        target_entrance_name = "Rotten Forest Entrance",
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
                        target_dungeon_name = name,
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
                        target_dungeon_name = name,
                        target_entrance_name = "Level " + (level + 1) + " Enter",
                        target_entrance_parameter = "Up",
                    }
                );

            dungeon_levels.Add(level_data);
        }
    }

    public override void Tick()
    {
    }

}

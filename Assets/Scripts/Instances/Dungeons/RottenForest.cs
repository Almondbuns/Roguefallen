using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RottenForest : DungeonData
{
    public List<List<string>> whispers;

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
                    (typeof(MFBeeTreasureRoom), 1, 2),
                    (typeof(MFMysticWellRoom), 4, 5),
                },                

                encounters =
                {
                    (1, new EncounterData() { type_amounts = {(typeof(Beehive),1,1)}, level_min = 1, level_max = 1,}),
                    (1, new EncounterData() { type_amounts = {(typeof(MysticMushroom),1,1)}, level_min = 1, level_max = 1,}),
                    (1, new EncounterData() { type_amounts = {(typeof(Butterfly),2,2)}, level_min = 1, level_max = 1,}),
                    (1, new EncounterData() { type_amounts = {(typeof(Snail),1,1)}, level_min = 1, level_max = 1,}),
                    (1, new EncounterData() { type_amounts = {(typeof(Thornling),1,1)}, level_min = 1, level_max = 1,}),
                }
            };

            //Plot Rooms

            //Signature Rooms
            level_data.map_features.Add((typeof(MFLivingForest),6,8));
            //level_data.map_features.Add((typeof(MFPond), 5, 5));

            //Gameplay Rooms




            level_data.dimensions = (140, 70);
            level_data.difficulty_level = level + 1;
            level_data.number_of_rooms = (30, 60);
            level_data.number_of_encounters = (30, 40);
            level_data.number_of_gold_items = (10, 20);

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

        whispers = new List<List<string>>()
        {
            new List<string>()
            {
                "'What a lovely day.'",
                "'Your footsteps are gentle.'",
                "'Welcome traveler.'",
                "'We wish you well.'",
                "'Follow your heart.'",
                "'Life is so exciting.'",
                "'Why not try something new?'",
                "'We will support you.'",
                "'You are a kind person.'",
                "'Everything will be alright.'",
            },
            new List<string>()
            {
                "'Are you sure you want to do this?'",              
            },
            new List<string>()
            {
                "'Are you sure you want to do this?'",
            },
            new List<string>()
            {
                "'Are you sure you want to do this?'",
            },
            new List<string>()
            {
                "'You are a complete failure.'",
            },
        };

    }

    public override void Tick()
    {
        ++tick_counter;

        if (tick_counter >= 5000)
        {
            MapData map_data = GameObject.Find("GameData").GetComponent<GameData>().current_map;
            PlayerData player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;
            DungeonLevelData level = GameObject.Find("GameData").GetComponent<GameData>().current_map_level;
            int level_index = level.dungeon_level;

            tick_counter = 0;
                        
            //Only activate if there is at least one tree near player
            for (int i = -5; i <= 5; ++ i)
            {
                for (int j = -5; j <= 5; ++j)
                {
                    int current_x = Math.Min(map_data.tiles.GetLength(0) - 1, Math.Max(0, player_data.X + i));
                    int current_y = Math.Min(map_data.tiles.GetLength(1) - 1, Math.Max(0, player_data.Y + j));
                    if (map_data.tiles[current_x, current_y].objects.Count > 0 && map_data.tiles[current_x, current_y].objects[0].name.Contains("tree"))
                    {
                        string text = whispers[level_index][UnityEngine.Random.Range(0, whispers[level_index].Count)];
                        VisualEffectText effect = new VisualEffectText();
                        effect.SetText(text, new Color(255 / 255.0f, 215 / 255.0f, 0 / 255.0f));
                        effect.ActivateOnTile(current_x, current_y);
                        GameLogger.Log("You hear something whisper: " + text);
                        return;
                    }
                }
            }
        }
    }

}

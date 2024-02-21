using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tomb : DungeonData
{
    public bool ball_needs_initialization = true;
    public long ball_id = -1;

    public int ball_corridor_index;
    public int ball_corridor_direction_x;
    public int ball_corridor_direction_y;
    public bool ball_clockwise_rolling = true;

    public Tomb()
    {
        name = "The Tomb";

        for (int level = 0; level < 5; ++level)
        {
            DungeonLevelData level_data = new DungeonLevelData
            {
                biome_index = 6,
                is_always_visible = false,
        
                map_features =
                {                    
                    
                    (typeof(MFCaveTreasureRoom), 1, 2), 
                    (typeof(MFTombSarcophagusRoom), 2,4),                                     
                },                

                encounters =
                {
                    (1, new EncounterData() { type_amounts = {(typeof(SkeletonWarrior),1,1)}, level_min = 1, level_max = 5,}),
                    (1, new EncounterData() { type_amounts = {(typeof(SkeletonArcher),1,1)}, level_min = 1, level_max = 5,}),                            

                    (1, new EncounterData() { type_amounts = {(typeof(CommonSpider),1,3), (typeof(CaveSpider),1,2), 
                        (typeof(PoisonSpider),1,1)}, level_min = 4, level_max = 4,}),

                    (1, new EncounterData() { type_amounts = {(typeof(CommonSpider),2,4), (typeof(CaveSpider),2,3), 
                        (typeof(PoisonSpider),1,2)}, level_min = 5, level_max = 5,}),

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
                    (10, new EncounterData() { type_amounts = {(typeof(Spider),1,1)}, level_min = 1, level_max = 6,}),
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
                level_data.dimensions = (80, 40);
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
                    (typeof(Jar), 20, 30),
                    (typeof(Chest), 0, 1),
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
                        target_entrance_name = "Cave Entrance",
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
        if (ball_needs_initialization == true && tick_counter == 0)
        {
            MapData current_map = GameObject.Find("GameData").GetComponent<GameData>().current_map;
            DungeonLevelData level = GameObject.Find("GameData").GetComponent<GameData>().current_map_level;
            
            
            DynamicObjectData ball = new DynamicObjectData(0,0, new TombGiantBall(1));
            current_map.Add(ball);
            ball.MoveTo(level.room_list[0].x, level.room_list[0].y, true);
            ball_needs_initialization = false;
            ball_id = ball.id;
            ball_corridor_index = 0;
            ball_corridor_direction_x = 1;
            ball_corridor_direction_y = 0;
        }
        ++this.tick_counter;

        if (tick_counter >= 60)
        {
            tick_counter = 0;

            MapData current_map = GameObject.Find("GameData").GetComponent<GameData>().current_map;
            DungeonLevelData level = GameObject.Find("GameData").GetComponent<GameData>().current_map_level;
            ActorData ball = current_map.GetActor(ball_id);

            //First move ball in the right direction

            //If ball is a the edge of one of the corridors the corridor has to be switched
            if (
                (ball_corridor_direction_x == 1 && ball.X == level.room_list[ball_corridor_index].x + level.room_list[ball_corridor_index].w)
                || (ball_corridor_direction_x == -1 && ball.X == level.room_list[ball_corridor_index].x)
                || (ball_corridor_direction_y == 1 && ball.Y == level.room_list[ball_corridor_index].y + level.room_list[ball_corridor_index].h)
                || (ball_corridor_direction_y == -1 && ball.Y == level.room_list[ball_corridor_index].y)
               )
            {
                ++ball_corridor_index;
                if (ball_corridor_index == 4)
                    ball_corridor_index = 0;
                
                if (ball_corridor_index == 0)
                {
                    ball_corridor_direction_x = 1;
                    ball_corridor_direction_y = 0;
                }
                else if (ball_corridor_index == 1)
                {
                    ball_corridor_direction_x = 0;
                    ball_corridor_direction_y = 1;
                }
                else if (ball_corridor_index == 2)
                {
                    ball_corridor_direction_x = -1;
                    ball_corridor_direction_y = 0;
                }
                else
                {
                    ball_corridor_direction_x = 0;
                    ball_corridor_direction_y = -1;
                }
            }

            ball.MoveTo(ball.X + ball_corridor_direction_x,ball.Y + ball_corridor_direction_y);

            //After movement damage all ball tiles
            current_map.DistributeDamage(ball, new AttackedTileData(){x = ball.X, y = ball.Y, damage_on_hit = {(DamageType.CRUSH,10,5)}}, true);
            current_map.DistributeDamage(ball, new AttackedTileData(){x = ball.X + 1, y = ball.Y, damage_on_hit = {(DamageType.CRUSH,10,5)}}, true);
            current_map.DistributeDamage(ball, new AttackedTileData(){x = ball.X, y = ball.Y + 1, damage_on_hit = {(DamageType.CRUSH,10,5)}}, true);
            current_map.DistributeDamage(ball, new AttackedTileData(){x = ball.X + 1, y = ball.Y + 1, damage_on_hit = {(DamageType.CRUSH,10,5)}}, true);
        }
    }

}

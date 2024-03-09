using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TombBallData
{
    public bool is_activated = false;
    public bool needs_initialization = true;
    public long id = -1;

    public int corridor_index;
    public int corridor_direction_x;
    public int corridor_direction_y;
    public bool clockwise_rolling = true;
    
}

public class Tomb : DungeonData
{
    public List<TombBallData> balls;

    public Tomb()
    {
        name = "The Tomb";

        balls = new ();

        for (int level = 0; level < 5; ++level)
        {
            balls.Add(new TombBallData());

            DungeonLevelData level_data = new DungeonLevelData
            {
                biome_index = 6,
                is_always_visible = true,
                difficulty_level = level + 3,
               
        
                map_features =
                {                    
                    
                    (typeof(MFCaveTreasureRoom), 1, 2), 
                    (typeof(MFTombSarcophagusRoom4), 0,2),   
                    (typeof(MFTombSarcophagusRoom2), 0,3), 
                    (typeof(MFTombSarcophagusRoom2Small), 0,3),
                    (typeof(MFTombSarcophagusRoom1), 0,3),
                    (typeof(MFTombJars), 0,3),
                    (typeof(MFTombPillars), 5,10),                                    
                },                

                encounters =
                {
                    (1, new EncounterData() { type_amounts = {(typeof(SkeletonWarrior),1,1)}, level_min = 1, level_max = 10,}),                    
                    (1, new EncounterData() { type_amounts = {(typeof(SkeletonArcher),1,1)}, level_min = 3, level_max = 3,}),                            
                    (1, new EncounterData() { type_amounts = {(typeof(SkeletonChild),1,1)}, level_min = 3, level_max = 3,}), 
                    (1, new EncounterData() { type_amounts = {(typeof(Zombie),1,1)}, level_min = 3, level_max = 3,}),                            

                    (1, new EncounterData() { type_amounts = {(typeof(Roach),1,1)}, level_min = 3, level_max = 3,}),
                    (1, new EncounterData() { type_amounts = {(typeof(FatRoach),1,1)}, level_min = 3, level_max = 3,}),
                    (1, new EncounterData() { type_amounts = {(typeof(ElectricRoach),1,1)}, level_min = 3, level_max = 3,}),
                    (1, new EncounterData() { type_amounts = {(typeof(Hammerworm),1,1)}, level_min = 3, level_max = 3,}),
                    (1, new EncounterData() { type_amounts = {(typeof(Fly),2,4)}, level_min = 3, level_max = 3,}),
                }
            };

            level_data.dimensions = (120, 60);
            level_data.number_of_rooms = (40, 50);
            level_data.number_of_encounters = (20, 30);
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
                (typeof(BallTrapTrigger),10,10),
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
        //TODO: Currently only works for current level
        int level_index = 0;
        if (balls[level_index].is_activated == false)
            return;

        if (balls[level_index].needs_initialization == true && tick_counter == 0)
        {
            MapData current_map = GameObject.Find("GameData").GetComponent<GameData>().current_map;
            DungeonLevelData level = GameObject.Find("GameData").GetComponent<GameData>().current_map_level;
            
            
            DynamicObjectData ball = new DynamicObjectData(0,0, new TombGiantBall(1));
            current_map.Add(ball);
            ball.MoveTo(level.room_list[0].x, level.room_list[0].y, true);
            balls[level_index].needs_initialization = false;
            balls[level_index].id = ball.id;
            balls[level_index].corridor_index = 0;
            balls[level_index].corridor_direction_x = 1;
            balls[level_index].corridor_direction_y = 0;
            balls[level_index].clockwise_rolling = false;
        }
        ++this.tick_counter;

        if (tick_counter >= 60)
        {
            tick_counter = 0;

            MapData current_map = GameObject.Find("GameData").GetComponent<GameData>().current_map;
            DungeonLevelData level = GameObject.Find("GameData").GetComponent<GameData>().current_map_level;
            ActorData ball = current_map.GetActor(balls[level_index].id);

            //First move ball in the right direction

            //If ball is a the edge of one of the corridors the corridor has to be switched
            if (
                (balls[level_index].corridor_direction_x == 1 && ball.X == level.room_list[balls[level_index].corridor_index].x + level.room_list[balls[level_index].corridor_index].w)
                || (balls[level_index].corridor_direction_x == -1 && ball.X == level.room_list[balls[level_index].corridor_index].x)
                || (balls[level_index].corridor_direction_y == 1 && ball.Y == level.room_list[balls[level_index].corridor_index].y + level.room_list[balls[level_index].corridor_index].h)
                || (balls[level_index].corridor_direction_y == -1 && ball.Y == level.room_list[balls[level_index].corridor_index].y)
               )
            {
                if (balls[level_index].clockwise_rolling == false)
                {
                    ++balls[level_index].corridor_index;
                    if (balls[level_index].corridor_index == 4)
                        balls[level_index].corridor_index = 0;
                    
                    if (balls[level_index].corridor_index == 0)
                    {
                        balls[level_index].corridor_direction_x = 1;
                        balls[level_index].corridor_direction_y = 0;
                    }
                    else if (balls[level_index].corridor_index == 1)
                    {
                        balls[level_index].corridor_direction_x = 0;
                        balls[level_index].corridor_direction_y = 1;
                    }
                    else if (balls[level_index].corridor_index == 2)
                    {
                        balls[level_index].corridor_direction_x = -1;
                        balls[level_index].corridor_direction_y = 0;
                    }
                    else
                    {
                        balls[level_index].corridor_direction_x = 0;
                        balls[level_index].corridor_direction_y = -1;
                    }
                }
                else
                {
                     --balls[level_index].corridor_index;
                    if (balls[level_index].corridor_index == -1)
                        balls[level_index].corridor_index = 3;
                    
                    if (balls[level_index].corridor_index == 0)
                    {
                        balls[level_index].corridor_direction_x = -1;
                        balls[level_index].corridor_direction_y = 0;
                    }
                    else if (balls[level_index].corridor_index == 1)
                    {
                        balls[level_index].corridor_direction_x = 0;
                        balls[level_index].corridor_direction_y = -1;
                    }
                    else if (balls[level_index].corridor_index == 2)
                    {
                        balls[level_index].corridor_direction_x = 1;
                        balls[level_index].corridor_direction_y = 0;
                    }
                    else
                    {
                        balls[level_index].corridor_direction_x = 0;
                        balls[level_index].corridor_direction_y = 1;
                    }
                }
            }

            ball.MoveTo(ball.X + balls[level_index].corridor_direction_x,ball.Y + balls[level_index].corridor_direction_y);

            //After movement damage all ball tiles
            current_map.DistributeDamage(ball, new AttackedTileData(){x = ball.X, y = ball.Y, damage_on_hit = {(DamageType.CRUSH,10,5)}}, true);
            current_map.DistributeDamage(ball, new AttackedTileData(){x = ball.X + 1, y = ball.Y, damage_on_hit = {(DamageType.CRUSH,10,5)}}, true);
            current_map.DistributeDamage(ball, new AttackedTileData(){x = ball.X, y = ball.Y + 1, damage_on_hit = {(DamageType.CRUSH,10,5)}}, true);
            current_map.DistributeDamage(ball, new AttackedTileData(){x = ball.X + 1, y = ball.Y + 1, damage_on_hit = {(DamageType.CRUSH,10,5)}}, true);
        }
    }

    public override int SendMessage(string message)
    {
        if (message == "Trigger Ball")
        {
            balls[0].is_activated = true;
            balls[0].clockwise_rolling = !balls[0].clockwise_rolling;
            balls[0].corridor_direction_x *= -1;
            balls[0].corridor_direction_y *= -1;
        }
        return -1;
    }

}

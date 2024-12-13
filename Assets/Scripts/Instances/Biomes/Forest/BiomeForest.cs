using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BiomeForest : BiomeData
{
    public BiomeForest()
    {
        name = "Forest";
        ambience_light = new Color(0.35f,0.35f, 0.65f);

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("mystic_forest_floor_1"));
        collection.Add(new MapObjectData("mystic_forest_floor_2"));
        collection.Add(new MapObjectData("mystic_forest_floor_3"));
        collection.Add(new MapObjectData("mystic_forest_floor_4"));
        collection.Add(new MapObjectData("mystic_forest_floor_5"));
        floors["floor"] = collection;

        collection = new();
        collection.Add(new MapObjectData("mystic_forest_living_tree_1") { emits_light = true, light_color = new Color((float)(184 / 255.0), (float)(55 / 255.0), (float)(234 / 255.0)), movement_blocked = true, sight_blocked = false, light_distance = 6 });
        collection.Add(new MapObjectData("mystic_forest_living_tree_2") { emits_light = true, light_color = new Color((float)(184 / 255.0), (float)(55 / 255.0), (float)(234 / 255.0)), movement_blocked = true, sight_blocked = false, light_distance = 6 });
        collection.Add(new MapObjectData("mystic_forest_living_tree_3") { emits_light = true, light_color = new Color((float)(171 / 255.0), (float)(0 / 255.0), (float)(104 / 255.0)), movement_blocked = true, sight_blocked = false, light_distance = 6 });
        collection.Add(new MapObjectData("mystic_forest_living_tree_4") { emits_light = true, light_color = new Color((float)(171 / 255.0), (float)(0 / 255.0), (float)(104 / 255.0)), movement_blocked = true, sight_blocked = false, light_distance = 6 });
        objects["living_tree"] = collection;

        collection = new();
        collection.Add(new MapObjectData("mystic_forest_dead_tree_1") { sight_blocked = false });
        objects["dead_tree"] = collection;

        collection = new();
        collection.Add(new MapObjectData("mystic_forest_mushroom_lamp_1"){ emits_light = true, light_color = new Color((float)(255/255.0),(float)(188/255.0),(float)(78/255.0)), movement_blocked = true, sight_blocked = false, light_distance = 6});
        collection.Add(new MapObjectData("mystic_forest_living_tree_1"){ emits_light = true, light_color = new Color((float)(184/255.0),(float)(55/255.0),(float)(234/255.0)), movement_blocked = true, sight_blocked = false, light_distance = 6 });
        collection.Add(new MapObjectData("mystic_forest_living_tree_3") { emits_light = true, light_color = new Color((float)(171 / 255.0), (float)(0 / 255.0), (float)(104 / 255.0)), movement_blocked = true, sight_blocked = false, light_distance = 6 });

        collection.Add(new MapObjectData("mystic_forest_flower_1"){ emits_light = true, light_color = new Color((float)(255/255.0),(float)(188/255.0),(float)(78/255.0)), movement_blocked = false, sight_blocked = false, light_distance = 2 });
        collection.Add(new MapObjectData("mystic_forest_flower_2"){ emits_light = true, light_color = new Color((float)(255/255.0),(float)(255/255.0),(float)(255/255.0)), movement_blocked = false, sight_blocked = false, light_distance = 2  });
        objects["obstacle"] = collection;
    }

    public (int x, int y, int w, int h)? AddRandomPositionRoom(MapData map, List<(int x, int y, int w, int h)> room_list, int w, int h, bool truely_random_distribution = false)
    {
        bool room_found = false;
        int number_of_tries = 0;

        int x = 0;
        int y = 0;

        //New rooms should be within the range of old rooms because no corridors are used

        while (room_found == false && number_of_tries < 10000)
        {
            ++number_of_tries;
            //Start with an existing room if possible
            if (room_list.Count >= 1)
            {
                int start_room_index = UnityEngine.Random.Range(0, room_list.Count);
                //Prefer newly created rooms (should lead to more deep construction)
                if (truely_random_distribution == false && UnityEngine.Random.value <= 0.75)
                    start_room_index = UnityEngine.Random.Range(9 * (room_list.Count / 10), room_list.Count);
                

                // The new room has to be near enough to be connected
                // Select one of four borders
                int border = UnityEngine.Random.Range(0,4);

                if (border == 0)
                {
                    //Left Border
                    x = room_list[start_room_index].x - w - 1;
                    y = room_list[start_room_index].y + room_list[start_room_index].h / 2 - h/2;  
                }
                else if (border == 1)
                {
                    //Right Border
                    x = room_list[start_room_index].x + room_list[start_room_index].w + 1;
                    y = room_list[start_room_index].y + room_list[start_room_index].h / 2 - h/2;  
                }
                else if (border == 2)
                {
                    //Upper Border
                    x = room_list[start_room_index].x + room_list[start_room_index].w / 2 - w/2;  
                    y = room_list[start_room_index].y + room_list[start_room_index].h + 1;
                }
                else
                {
                    //Lower Border
                    x = room_list[start_room_index].x + room_list[start_room_index].w / 2 - w/2;  
                    y = room_list[start_room_index].y - h - 1;
                }             
            }
            else
            {
                //If no room has been created we start in the center
                x = map.tiles.GetLength(0) / 2;
                y = map.tiles.GetLength(1) / 2;
            }

            //Make sure the new room is inside the map
                if (x < 0) continue;
                if (x > map.tiles.GetLength(0) - w) continue;

                if (y < 0) continue;
                if (y > map.tiles.GetLength(1) - h) continue;

            room_found = true;
            foreach ((int x, int y, int w, int h) room in room_list)
            {
                if (room.x + room.w < x)
                    continue;
                if (room.x > x + w)
                    continue;

                if (room.y + room.h < y)
                    continue;
                if (room.y > y + h)
                    continue;

                room_found = false;
                break;
            }

            if (room_found == true)
            {
                room_list.Add((x, y, w, h));
                return (x, y, w, h);
            }
        }

        return null;
    }

    public override MapData CreateMapLevel(int level, int max_x, int max_y, int number_of_rooms, List<(Type type, int amount_min, int amount_max)> map_features, List<DungeonChangeData> dungeon_change_data, List<(int x, int y, int w, int h)> room_list, int difficulty_level, int biome_variant)
    {
        MapData map = new MapData(max_x, max_y);

        for (int x = 0; x < map.tiles.GetLength(0); ++x)
            for (int y = 0; y < map.tiles.GetLength(1); ++y)
            {
                map.tiles[x, y].floor = floors["floor"].Random();
                // The deeper into the forest the more dead trees will show
                if (UnityEngine.Random.value < biome_variant * 0.25f)
                    map.tiles[x, y].objects.Add(objects["dead_tree"].Random());
                else
                    map.tiles[x, y].objects.Add(objects["living_tree"].Random());
            }

        //Create half of the forest structure first
        for (int i = 0; i < number_of_rooms / 2; ++i)
        {
            int w = UnityEngine.Random.Range(5,12);
            int h = UnityEngine.Random.Range(5, 12);

            (int x, int y, int w, int h)? position = AddRandomPositionRoom(map, room_list, w, h);
        }  

        foreach (DungeonChangeData dcd in dungeon_change_data)
        {
            MapFeatureData feature = (MapFeatureData)Activator.CreateInstance(dcd.dungeon_change_type, map, dcd);
            (int x, int y, int w, int h)? position = AddRandomPositionRoom(map, room_list, feature.dimensions.x + 2, feature.dimensions.y + 2);
            if (position == null)
                continue;
            feature.position.x = position.Value.x + 1;
            feature.position.y = position.Value.y + 1;

            map.features.Add(feature);            
        }

        foreach (var feature_data in map_features)
        {
            int amount = UnityEngine.Random.Range(feature_data.amount_min, feature_data.amount_max + 1);
            for (int i = 0; i < amount; ++i)
            {
                MapFeatureData feature = (MapFeatureData)Activator.CreateInstance(feature_data.type, map);
                (int x, int y, int w, int h)? position = AddRandomPositionRoom(map, room_list, feature.dimensions.x + 2, feature.dimensions.y + 2, true);
                if (position == null)
                    continue;
                feature.position.x = position.Value.x + 1;
                feature.position.y = position.Value.y + 1;

                feature.difficulty_level = difficulty_level;
                map.features.Add(feature);
            }
        }

        //Create the other half of the forest structure last
        for (int i = 0; i < number_of_rooms / 2; ++i)
        {
            int w = UnityEngine.Random.Range(5, 12);
            int h = UnityEngine.Random.Range(5, 12);

            (int x, int y, int w, int h)? position = AddRandomPositionRoom(map, room_list, w, h);
        }

        foreach (var room in room_list)
        {
            CreateRoom(map, room);
        }

        foreach (var feature in map.features)
        {
            feature.Generate();
        }

        return map;
    }
  
    public void CreateRoom(MapData map, (int x, int y, int w, int h) position)
    {
        int radius = Mathf.Max(position.w/2 + 2,position.h/2 + 2);

        for (int x = position.x - 10; x < position.x + position.w + 10; ++x)
            for (int y = position.y - 10; y < position.y + position.h + 10; ++y)
            {
                if (x < 0 || x >= map.w || y < 0 || y >= map.h)
                    continue;

                float distance = Mathf.Sqrt((x - position.x - position.w /2) * (x - position.x - position.w /2) + (y - position.y - position.h /2) * (y - position.y - position.h /2));
                if (distance < radius)
                {
                    map.tiles[x, y].objects.Clear();
                    if (UnityEngine.Random.value < 0.95)
                    {
                    }
                    else
                    {
                        map.tiles[x, y].objects.Add(objects["obstacle"].Random());
                    }
                }
            }
    }
}

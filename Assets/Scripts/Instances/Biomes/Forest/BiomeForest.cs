using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BiomeForest : BiomeData
{
    public BiomeForest()
    {
        name = "Forest";
        connectivity_probability = 0.67f;
        ambience_light = new Color(0.75f,0.75f, 0.75f);

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("forest_floor_1"));
        floors["floor"] = collection;

        collection = new();
        collection.Add(new MapObjectData("cave_mushroom_blue") { emits_light = true, light_color = new Color(0.0f,0.0f,0.9f), movement_blocked = false, sight_blocked = false });
        objects["light_1"] = collection;

        collection = new();
        collection.Add(new MapObjectData("cave_mushroom_orange") { emits_light = true, light_color = new Color(0.9f,0.6f,0.3f), movement_blocked = false, sight_blocked = false }) ;
        objects["light_2"] = collection;

        collection = new();
        collection.Add(new MapObjectData("forest_tree_1"));
        objects["wall"] = collection;

        collection = new();
        collection.Add(new MapObjectData("grassland_obstacle_1"));
        collection.Add(new MapObjectData("grassland_obstacle_3"));
        objects["obstacle"] = collection;
    }

    public (int x, int y, int w, int h)? AddRandomPositionRoom(MapData map, List<(int x, int y, int w, int h)> room_list, int w, int h)
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

                // The new room has to be near enough to be connected
                // Select one of four borders
                int border = UnityEngine.Random.Range(0,4);

                if (border == 0)
                {
                    //Left Border
                    x = room_list[start_room_index].x - w - 2;
                    y = UnityEngine.Random.Range(room_list[start_room_index].y - h - 2, room_list[start_room_index].y + 2);
                }
                else if (border == 1)
                {
                    //Right Border
                    x = room_list[start_room_index].x + room_list[start_room_index].w + 2;
                    y = UnityEngine.Random.Range(room_list[start_room_index].y - h - 2, room_list[start_room_index].y + 2);
                }
                else if (border == 2)
                {
                    //Upper Border
                    x = UnityEngine.Random.Range(room_list[start_room_index].x - w - 2, room_list[start_room_index].x + 2);                    
                    y = room_list[start_room_index].y + room_list[start_room_index].w + 2;
                }
                else
                {
                    //Lower Border
                    x = UnityEngine.Random.Range(room_list[start_room_index].x - w - 2, room_list[start_room_index].x + 2);                    
                    y = room_list[start_room_index].y - h - 2;
                }

                //Make sure the new room is inside the map
                if (x < 0) x = 0;
                if (x > map.tiles.GetLength(0) - w) x = map.tiles.GetLength(0) - w;

                if (y < 0) y = 0;
                if (y > map.tiles.GetLength(1) - h) y = map.tiles.GetLength(1) - h;
            }
            else
            {
                //If no room has been created yet we can start anywhere
                x = UnityEngine.Random.Range(0, map.tiles.GetLength(0) - w);
                y = UnityEngine.Random.Range(0, map.tiles.GetLength(1) - h);
            }

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

    public override MapData CreateMapLevel(int level, int max_x, int max_y, int number_of_rooms, List<(Type type, int amount_min, int amount_max)> map_features, List<DungeonChangeData> dungeon_change_data, List<(int x, int y, int w, int h)> room_list, int difficulty_level)
    {
        MapData map = new MapData(max_x, max_y);

        for (int x = 0; x < map.tiles.GetLength(0); ++x)
            for (int y = 0; y < map.tiles.GetLength(1); ++y)
            {
                map.tiles[x, y].floor = floors["floor"].Random();
                map.tiles[x, y].objects.Add(objects["wall"].Random());
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
                (int x, int y, int w, int h)? position = AddRandomPositionRoom(map, room_list, feature.dimensions.x + 2, feature.dimensions.y + 2);
                if (position == null)
                    continue;
                feature.position.x = position.Value.x + 1;
                feature.position.y = position.Value.y + 1;

                feature.difficulty_level = difficulty_level;
                map.features.Add(feature);
            }
        }

        for (int i = 0; i < number_of_rooms; ++i)
        {
            int w = UnityEngine.Random.Range(10, 30);
            int h = UnityEngine.Random.Range(10, 30);
         
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
        int radius = Mathf.Max(position.w/2 + 4,position.h/2 + 4);

        for (int x = position.x - 10; x < position.x + position.w + 10; ++x)
            for (int y = position.y - 10; y < position.y + position.h + 10; ++y)
            {
                if (x < 0 || x >= map.w || y < 0 || y >= map.h)
                    continue;

                float distance = Mathf.Sqrt((x - position.x - position.w /2) * (x - position.x - position.w /2) + (y - position.y - position.h /2) * (y - position.y - position.h /2));
                if (distance < radius)
                {
                    map.tiles[x, y].objects.Clear();
                    if (UnityEngine.Random.value < 0.9)
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

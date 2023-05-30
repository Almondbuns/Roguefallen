using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BiomeCave : BiomeData
{
    public BiomeCave()
    {
        name = "Cave";
        connectivity_probability = 0.67f;
        ambience_light = new Color(0.25f,0.25f, 0.25f);

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("cave_floor_1"));
        collection.Add(new MapObjectData("cave_floor_2"));
        collection.Add(new MapObjectData("cave_floor_3"));
        floors["floor"] = collection;

        collection = new();
        collection.Add(new MapObjectData("cave_mushroom_blue") { emits_light = true, light_color = new Color(0.0f,0.0f,0.9f), movement_blocked = false, sight_blocked = false });
        objects["light_blue"] = collection;

        collection = new();
        collection.Add(new MapObjectData("cave_mushroom_orange") { emits_light = true, light_color = new Color(0.9f,0.6f,0.3f), movement_blocked = false, sight_blocked = false }) ;
        objects["light_orange"] = collection;

        collection = new();
        collection.Add(new MapObjectData("cave_wall_1"));
        collection.Add(new MapObjectData("cave_wall_2"));
        collection.Add(new MapObjectData("cave_wall_3"));
        collection.Add(new MapObjectData("cave_wall_4"));
        objects["wall"] = collection;

        collection = new();
        collection.Add(new MapObjectData("cave_stones"));
        collection.Add(new MapObjectData("cave_stones_2"));
        collection.Add(new MapObjectData("cave_stones_3") { sight_blocked = false });
        collection.Add(new MapObjectData("cave_stones_4") { sight_blocked = false });
        objects["stone"] = collection;

        room_list = new();
    }

    public (int x, int y, int w, int h)? AddRandomPositionRoom(MapData map, int w, int h)
    {
        bool room_found = false;
        int number_of_tries = 0;

        int x = 0;
        int y = 0;

        while (room_found == false && number_of_tries < 1000)
        {
            ++number_of_tries;
            x = UnityEngine.Random.Range(0, map.tiles.GetLength(0) - w);
            y = UnityEngine.Random.Range(0, map.tiles.GetLength(1) - h);

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

    public override MapData CreateMapLevel(int level, int max_x, int max_y, int number_of_rooms, List<(Type type, int amount_min, int amount_max)> map_features, List<DungeonChangeData> dungeon_change_data)
    {
        MapData map = new MapData(max_x, max_y);
        room_list = new();
       

        for (int x = 0; x < map.tiles.GetLength(0); ++x)
            for (int y = 0; y < map.tiles.GetLength(1); ++y)
            {
                map.tiles[x, y].floor = floors["floor"].Random();
                map.tiles[x, y].objects.Add(objects["wall"].Random());
            }

        foreach (DungeonChangeData dcd in dungeon_change_data)
        {
            MapFeatureData feature = (MapFeatureData)Activator.CreateInstance(dcd.dungeon_change_type, map, dcd);
            (int x, int y, int w, int h)? position = AddRandomPositionRoom(map, feature.dimensions.x + 2, feature.dimensions.y + 2);
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
                (int x, int y, int w, int h)? position = AddRandomPositionRoom(map, feature.dimensions.x + 2, feature.dimensions.y + 2);
                if (position == null)
                    continue;
                feature.position.x = position.Value.x + 1;
                feature.position.y = position.Value.y + 1;

                feature.difficulty_level = level + 1;
                map.features.Add(feature);
            }
        }

        for (int i = 0; i < number_of_rooms; ++i)
        {
            int w = UnityEngine.Random.Range(10, 30);
            int h = UnityEngine.Random.Range(10, 30);
            // 10% big rooms
            if (i <= number_of_rooms / 10)
            {
                w = UnityEngine.Random.Range(30, 60);
                h = UnityEngine.Random.Range(30, 60);
            }
            
         
            (int x, int y, int w, int h)? position = AddRandomPositionRoom(map, w, h);
        }

        //Create Corridors first
        foreach (var room in room_list)
        {
            (int x, int y, int w, int h)? closest_room = null;
            double min_distance = -1;
            //Connect with closest room
            foreach (var room2 in room_list)
            {
                double room_distance = Mathf.Abs(room2.x + room2.w / 2 - (room.x + room.w / 2)) + Mathf.Abs(room2.y + room2.h / 2 - (room.y + room.h / 2));

                if (room_distance == 0) continue; // same room!

                if (closest_room.HasValue == false || room_distance < min_distance)
                {
                    closest_room = room2;
                    min_distance = room_distance;
                    continue;
                }
            }

            CreateCorridor(map, room, closest_room.Value);
        }

        //Second Pass: Make sure the first room is connected to all other rooms
        foreach (var room in room_list)
        {
            CreateCorridor(map, room_list[0], room);
        }

        //Third Pass: Create Corridor if path between two rooms is faaar longer than direct connection
        double factor = 4;
        int counter = 0;
        foreach (var room1 in room_list)
        {
            if (counter == room_list.Count - 1)
                break;

            for (int i = counter + 1; i < room_list.Count; ++i)
            {
                if (room1.x == room_list[i].x && room1.y == room_list[i].y)
                    continue;

                double direct_distance = Mathf.Sqrt(Mathf.Pow(room1.x + room1.w/2 - room_list[i].x - room_list[i].w/2, 2) + Mathf.Pow(room1.y + room1.h/2 - room_list[i].y - room_list[i].h/2, 2));
                Path path = Algorithms.AStar(map, (room1.x + room1.w / 2, room1.y + room1.h / 2), (room_list[i].x + room_list[i].w / 2, room_list[i].y + room_list[i].h / 2), false, false);
                if (path == null || path.path.Count == 0)
                    continue; //should not happen

                if (factor * direct_distance < path.path.Count)
                    CreateDirectCorridor(map, room1, room_list[i]);
            }
            ++counter;
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

    public void CreateDirectCorridor(MapData map, (int x, int y, int w, int h) room1, (int x, int y, int w, int h) room2)
    {
        List<(int x, int y)> path = Algorithms.LineofSight((room1.x + room1.w/2, room1.y + room1.h/2), (room2.x + room2.w/2, room2.y + room2.h/2));
        int circle_radius = 2;

        foreach ((int x, int y) tile in path)
        {
            for (int x = Mathf.Max(tile.x - circle_radius, 0); x < Mathf.Min(tile.x + circle_radius + 1, map.tiles.GetLength(0)); ++x)
            {
                for (int y = Mathf.Max(tile.y - circle_radius, 0); y < Mathf.Min(tile.y + circle_radius + 1, map.tiles.GetLength(1)); ++y)
                {
                    if (Mathf.Pow(tile.x - x, 2) + Mathf.Pow(tile.y - y, 2) < circle_radius * circle_radius)
                    {
                        map.tiles[x, y].objects.Clear();
                    }
                }
            }
        }
    }

    public void CreateCorridor(MapData map, (int x, int y, int w, int h) room1, (int x, int y, int w, int h) room2)
    {
        Path path = Algorithms.AStar(map, (room1.x + room1.w/2, room1.y + room1.h/2), (room2.x + room2.w/2, room2.y + room2.h/2), false, true);
        int circle_radius = 2;

        foreach (var tile in path.path)
        {
            if (tile.cost < 1000000)
                circle_radius = UnityEngine.Random.Range(0,3);
            else
                circle_radius = 2;

            for (int x = Mathf.Max(tile.x - circle_radius,0); x < Mathf.Min(tile.x + circle_radius + 1, map.tiles.GetLength(0)); ++x)
            {
                for (int y = Mathf.Max(tile.y - circle_radius, 0); y < Mathf.Min(tile.y + circle_radius + 1, map.tiles.GetLength(1)); ++y)
                {
                    if (Mathf.Pow(tile.x-x,2) + Mathf.Pow(tile.y - y, 2) < circle_radius * circle_radius)
                    {
                        map.tiles[x, y].objects.Clear();
                    }
                }
            }
        }
    }

    public void CreateRoom(MapData map, (int x, int y, int w, int h) position)
    {
        List<(int x, int y)> accepted_start_positions = new();

        for (int i = 0; i < UnityEngine.Random.Range(5, 11); ++i)
        {
            int x_center = 0;
            int y_center = 0;
            if (i == 0)
            {
                x_center = position.x + position.w / 2;
                y_center = position.y + position.h / 2;
            }
            else 
            {
                int random_position = UnityEngine.Random.Range(0, accepted_start_positions.Count);
                x_center = accepted_start_positions[random_position].x;
                y_center = accepted_start_positions[random_position].y;
            }

            int radius = UnityEngine.Random.Range(3, Mathf.Max(position.w, position.h) / 4);

            for (int x = position.x; x < position.x + position.w; ++x)
                for (int y = position.y; y < position.y + position.h; ++y)
                {
                    float distance = Mathf.Sqrt((x - x_center) * (x - x_center) + (y - y_center) * (y - y_center));
                    if (distance < radius)
                    {
                        map.tiles[x, y].objects.Clear();
                        if (UnityEngine.Random.value < 0.9)
                        {
                            if (distance >= radius - 2)
                            accepted_start_positions.Add((x, y));
                        }
                        else
                        {
                            map.tiles[x, y].objects.Add(objects["stone"].Random());
                        }
                    }
                }
        }

        bool is_blue_room = UnityEngine.Random.value < 0.5f;

        for (int z = 0; z < UnityEngine.Random.Range(2, accepted_start_positions.Count / 25); ++z)
        {
            (int x, int y) r_pos = accepted_start_positions[UnityEngine.Random.Range(0, accepted_start_positions.Count)];
            if (is_blue_room == true)
                map.tiles[r_pos.x, r_pos.y].objects.Add(objects["light_blue"].Random());
            else
                map.tiles[r_pos.x, r_pos.y].objects.Add(objects["light_orange"].Random());
        }
    }
}

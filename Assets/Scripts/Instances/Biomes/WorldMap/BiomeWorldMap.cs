using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class BiomeWorldMap : BiomeData
{
    public BiomeWorldMap()
    {
        name = "WorldMap";
        connectivity_probability = 0.67f;
        ambience_light = new Color(1f,1f,1f);

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("cave_floor_1"));
        collection.Add(new MapObjectData("cave_floor_2"));
        collection.Add(new MapObjectData("cave_floor_3"));
        floors["grass"] = collection;
        
        collection = new();
        collection.Add(new MapObjectData("water_1"));
        collection.Add(new MapObjectData("water_2"));
        collection.Add(new MapObjectData("water_3"));
        collection.Add(new MapObjectData("water_4"));
        floors["water"] = collection;

        collection = new();
        collection.Add(new MapObjectData("woods_1"));
        floors["woods"] = collection;

        collection = new();
        collection.Add(new MapObjectData("mountain_obstacle_1"));
        floors["mountain"] = collection;

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

        //Create a primitive height based island
        int number_of_peaks = UnityEngine.Random.Range(5,20);
        List<(int x, int y, int height, int radius)> peaks = new();
        for (int i = 0; i < number_of_peaks; ++ i)
        {
            peaks.Add((UnityEngine.Random.Range(8, map.tiles.GetLength(0) - 8),UnityEngine.Random.Range(8, map.tiles.GetLength(1) - 8), UnityEngine.Random.Range(100, 500), UnityEngine.Random.Range(5, 20)));
        }

        List<double> heights = new List<double>(map.tiles.GetLength(0) * map.tiles.GetLength(1));
        for (int i = 0; i < map.tiles.GetLength(0) * map.tiles.GetLength(1); ++ i)
            heights.Add(0);

        for (int i = 0; i < number_of_peaks; ++ i)
        {
            for (int x = peaks[i].x - peaks[i].radius; x <= peaks[i].x + peaks[i].radius; ++ x)
            {
                for (int y = peaks[i].y - peaks[i].radius; y <= peaks[i].y + peaks[i].radius; ++ y)
                {
                    if (x < 0 || y < 0 || x >= map.tiles.GetLength(0) || y >= map.tiles.GetLength(1))
                        continue;

                    int distance_sqr = (x - peaks[i].x) * (x - peaks[i].x) + (y - peaks[i].y) * (y - peaks[i].y) ;

                    if (distance_sqr > peaks[i].radius * peaks[i].radius)
                        continue;

                    heights[x + map.tiles.GetLength(0) * y] += peaks[i].height / (distance_sqr+1);
                }
            }
        }
        List<double> sorted_heights = new (map.tiles.GetLength(0) * map.tiles.GetLength(1));
        for (int i = 0; i < heights.Count; ++ i)
        {
            sorted_heights.Add(heights[i]); 
            Debug.Log(heights[i]);
        }       

        sorted_heights.Sort();

        double min_land_height = sorted_heights[((map.tiles.GetLength(0) * map.tiles.GetLength(1)) * 3) / 10]; 
        double min_woods_height = sorted_heights[((map.tiles.GetLength(0) * map.tiles.GetLength(1)) * 8) / 10]; 
        double min_mountain_height = sorted_heights[((map.tiles.GetLength(0) * map.tiles.GetLength(1)) * 9) / 10]; 

        for (int x = 0; x < map.tiles.GetLength(0); ++x)
            for (int y = 0; y < map.tiles.GetLength(1); ++y)
            {
                if (heights[x + map.tiles.GetLength(0) * y] >= min_mountain_height)
                    map.tiles[x, y].floor = floors["mountain"].Random();
                else if (heights[x + map.tiles.GetLength(0) * y] >= min_woods_height)
                    map.tiles[x, y].floor = floors["woods"].Random();
                else if (heights[x + map.tiles.GetLength(0) * y] >= min_land_height)
                    map.tiles[x, y].floor = floors["grass"].Random();
                else
                    map.tiles[x, y].floor = floors["water"].Random();
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

        foreach (var feature in map.features)
        {
            feature.Generate();
        }

        return map;
    }
}

public class MFStandardDungeonEntrance : MFChangeDungeon
{
    public MFStandardDungeonEntrance(MapData map) : base(map)
    {
       
    }

    public MFStandardDungeonEntrance(MapData map, DungeonChangeData dcd) : base(map, dcd)
    {
        dimensions = (1,1);

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData(dcd.dungeon_change_image));
        objects["entrance"] = collection;
    }

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);
       
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);
    }

    public override void Generate()
    {
        map.tiles[position.x, position.y].objects.Clear();
        map.tiles[position.x , position.y].objects.Add(objects["entrance"].Random());

        enter_tiles.Add((position.x, position.y));
        exit_tile = (position.x + dimensions.x / 2, position.y);
    }
}

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
        ambience_light = new Color(.7f,.7f,.7f);

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("grass_1"));
        floors["grass"] = collection;
        
        collection = new();
        collection.Add(new MapObjectData("ocean_1"));
        floors["water"] = collection;

        collection = new();
        collection.Add(new MapObjectData("hill_1"));
        floors["hill"] = collection;

        collection = new();
        collection.Add(new MapObjectData("mountain_1"){movement_blocked = true});
        floors["mountain"] = collection;
        objects["mountain"] = collection;

        collection = new();
        collection.Add(new MapObjectData("beach_1"));
        floors["beach"] = collection;

        collection = new();
        collection.Add(new MapObjectData("forest_1"){movement_blocked = false});
        objects["forest"] = collection;

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

        double min_beach_height = sorted_heights[((map.tiles.GetLength(0) * map.tiles.GetLength(1)) * 3) / 10]; 
        double min_land_height = sorted_heights[((map.tiles.GetLength(0) * map.tiles.GetLength(1)) * 4) / 10]; 
        double min_woods_height = sorted_heights[((map.tiles.GetLength(0) * map.tiles.GetLength(1)) * 8) / 10]; 
        double min_mountain_height = sorted_heights[((map.tiles.GetLength(0) * map.tiles.GetLength(1)) * 9) / 10]; 

        for (int x = 0; x < map.tiles.GetLength(0); ++x)
            for (int y = 0; y < map.tiles.GetLength(1); ++y)
            {
                if (heights[x + map.tiles.GetLength(0) * y] >= min_mountain_height)
                {
                    map.tiles[x, y].floor = floors["mountain"].Random();
                    map.tiles[x, y].objects.Add(objects["mountain"].Random());
                }
                else if (heights[x + map.tiles.GetLength(0) * y] >= min_woods_height)
                    map.tiles[x, y].floor = floors["hill"].Random();
                else if (heights[x + map.tiles.GetLength(0) * y] >= min_land_height)
                    map.tiles[x, y].floor = floors["grass"].Random();
                else if (heights[x + map.tiles.GetLength(0) * y] >= min_beach_height)
                    map.tiles[x, y].floor = floors["beach"].Random();
                else
                    map.tiles[x, y].floor = floors["water"].Random();
            }

        int number_of_forests = UnityEngine.Random.Range(10,20);
        for (int i = 0; i < number_of_forests; ++i)
        {
            int size_of_forest = UnityEngine.Random.Range(7,25);

            int number_of_tries = 0;
            bool found = false;
            int x = 0; int y = 0;
            while (found == false && number_of_tries < 1000)
            {
                x = UnityEngine.Random.Range(0, map.tiles.GetLength(0));
                y = UnityEngine.Random.Range(0, map.tiles.GetLength(1));

                if (map.tiles[x,y].floor.name.Contains("grass") == true || map.tiles[x,y].floor.name.Contains("hill") == true)
                {
                    found = true;
                }
                ++number_of_tries;
            }

            if (found == false)
                continue;

            map.tiles[x,y].objects.Add(objects["forest"].Random());
            List<(int x, int y)> forest_tiles = new();
            forest_tiles.Add((x,y));

            for (int j = 0; j < size_of_forest; ++ j)
            {
                number_of_tries = 0;
                found = false;
                x = 0; y = 0;
                while (found == false && number_of_tries < 1000)
                {
                    x = UnityEngine.Random.Range(-1, 2);
                    y = UnityEngine.Random.Range(-1, 2);
                    (int x, int y) start_position = forest_tiles[UnityEngine.Random.Range(0, forest_tiles.Count)];

                    if (start_position.x + x < 0 || start_position.x + x >= map.tiles.GetLength(0) 
                    || start_position.y + y < 0 || start_position.y + y >= map.tiles.GetLength(1) )
                        continue;

                    if (map.tiles[start_position.x + x,start_position.y + y].objects.Count == 0
                        && (map.tiles[start_position.x + x,start_position.y + y].floor.name.Contains("grass") == true 
                            || map.tiles[start_position.x + x,start_position.y + y].floor.name.Contains("hill") == true))
                    {
                        found = true;
                        map.tiles[start_position.x + x,start_position.y + y].objects.Add(objects["forest"].Random());
                        forest_tiles.Add((start_position.x + x,start_position.y + y));
                    }
                    ++number_of_tries;
                }
            }
            
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class BiomeWorldMap : BiomeData
{
    MapData map;
    int max_x;
    int max_y;

    List<(int x, int y)> mountain_tiles;
    List<(int x, int y)> hill_tiles;
    List<(int x, int y)> grass_tiles;
    List<(int x, int y)> beach_tiles;
    List<(int x, int y)> ocean_tiles;

    public BiomeWorldMap()
    {
        name = "WorldMap";
        connectivity_probability = 0.67f;
        ambience_light = new Color(.7f,.7f,.7f);

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("grass_1"));
        collection.Add(new MapObjectData("grass_2"));
        collection.Add(new MapObjectData("grass_3"));
        collection.Add(new MapObjectData("grass_4"));
        floors["grass"] = collection;
        
        collection = new();
        collection.Add(new MapObjectData("ocean_1"));
        floors["water"] = collection;

        collection = new();
        collection.Add(new MapObjectData("ocean_dark_1"));
        floors["water_deep"] = collection;

        collection = new();
        collection.Add(new MapObjectData("hill_1"));
        collection.Add(new MapObjectData("hill_2"));
        collection.Add(new MapObjectData("hill_3"));
        collection.Add(new MapObjectData("hill_4"));
        floors["hill"] = collection;

        collection = new();
        collection.Add(new MapObjectData("mountain_1"){movement_blocked = true});
        collection.Add(new MapObjectData("mountain_2"){movement_blocked = true});
        collection.Add(new MapObjectData("mountain_3"){movement_blocked = true});
        collection.Add(new MapObjectData("mountain_4"){movement_blocked = true});
        floors["mountain"] = collection;
        objects["mountain"] = collection;

        collection = new();
        collection.Add(new MapObjectData("beach_1"));
        collection.Add(new MapObjectData("beach_2"));
        floors["beach"] = collection;

        collection = new();
        collection.Add(new MapObjectData("forest_1"){movement_blocked = false});
        collection.Add(new MapObjectData("forest_2"){movement_blocked = false});
        collection.Add(new MapObjectData("forest_3"){movement_blocked = false});
        collection.Add(new MapObjectData("forest_4"){movement_blocked = false});
        objects["forest"] = collection;

        room_list = new();
    }

    public (int x, int y, int w, int h)? AddRandomPositionRoom(MapData map, int w, int h, Type dungeon_type = null)
    {
        bool room_found = false;
        int number_of_tries = 0;

        int x = 0;
        int y = 0;

        while (room_found == false && number_of_tries < 1000)
        {
            ++number_of_tries;
            if (dungeon_type == null)
            {
                x = UnityEngine.Random.Range(0, map.tiles.GetLength(0) - w);
                y = UnityEngine.Random.Range(0, map.tiles.GetLength(1) - h);
            }
            else
            {
                if (dungeon_type == typeof(BiomeVillage))
                {
                    (int x, int y) rand = grass_tiles[UnityEngine.Random.Range(0, grass_tiles.Count)];
                    x = rand.x;
                    y = rand.y;
                }
                else if (dungeon_type == typeof(BiomeCave))
                {
                    (int x, int y) rand = hill_tiles[UnityEngine.Random.Range(0, hill_tiles.Count)];
                    x = rand.x;
                    y = rand.y;
                }
                else if (dungeon_type == typeof(BiomeSewers))
                {
                    (int x, int y) rand = grass_tiles[UnityEngine.Random.Range(0, grass_tiles.Count)];
                    x = rand.x;
                    y = rand.y;
                }
                else if (dungeon_type == typeof(BiomeCastle))
                {
                    (int x, int y) rand = mountain_tiles[UnityEngine.Random.Range(0, mountain_tiles.Count)];
                    x = rand.x;
                    y = rand.y;
                }

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

    public void CreateTerrain()
    {
        mountain_tiles = new();
        hill_tiles = new();
        grass_tiles = new();
        beach_tiles = new();
        ocean_tiles = new();


        //Create a primitive height based island
        int number_of_peaks = UnityEngine.Random.Range(max_x*max_y / (5*32), max_x*max_y / (2*32));
        List<(int x, int y, int height, int radius)> peaks = new();
        for (int i = 0; i < number_of_peaks; ++ i)
        {
            peaks.Add((UnityEngine.Random.Range(8, max_x - 8),UnityEngine.Random.Range(8, max_y - 8), UnityEngine.Random.Range(-2500, 5000), UnityEngine.Random.Range(max_x*max_y / (10*32), max_x*max_y / (2*32))));
        }

        List<double> heights = new List<double>(max_x * max_y);
        for (int i = 0; i < max_x * max_y; ++ i)
            heights.Add(0);

        for (int i = 0; i < number_of_peaks; ++ i)
        {
            for (int x = peaks[i].x - peaks[i].radius; x <= peaks[i].x + peaks[i].radius; ++ x)
            {
                for (int y = peaks[i].y - peaks[i].radius; y <= peaks[i].y + peaks[i].radius; ++ y)
                {
                    if (x < 0 || y < 0 || x >= max_x || y >= max_y)
                        continue;

                    int distance_sqr = (x - peaks[i].x) * (x - peaks[i].x) + (y - peaks[i].y) * (y - peaks[i].y) ;

                    if (distance_sqr > peaks[i].radius * peaks[i].radius)
                        continue;

                    heights[x + max_x * y] += peaks[i].height / (double)((distance_sqr+1));
                }
            }
        }
        List<double> sorted_heights = new (max_x * max_y);
        for (int i = 0; i < heights.Count; ++ i)
        {
            sorted_heights.Add(heights[i]); 
        }       

        sorted_heights.Sort();

        double min_beach_height = sorted_heights[((max_x * max_y) * 3) / 10]; 
        double min_grass_height = sorted_heights[((max_x * max_y) * 4) / 10]; 
        double min_hill_height = sorted_heights[((max_x * max_y) * 8) / 10]; 
        double min_mountain_height = sorted_heights[((max_x * max_y) * 9) / 10]; 

        for (int x = 0; x < max_x; ++x)
        {
            for (int y = 0; y < max_y; ++y)
            {
                if (heights[x + max_x * y] >= min_mountain_height)
                {
                    map.tiles[x, y].floor = floors["mountain"].Random();
                    map.tiles[x, y].objects.Add(objects["mountain"].Random());
                    mountain_tiles.Add((x,y));
                }
                else if (heights[x + max_x * y] >= min_hill_height)
                {
                    map.tiles[x, y].floor = floors["hill"].Random();
                    hill_tiles.Add((x,y));
                }
                else if (heights[x + max_x * y] >= min_grass_height)
                {
                    map.tiles[x, y].floor = floors["grass"].Random();
                    grass_tiles.Add((x,y));
                }
                else if (heights[x + max_x * y] >= min_beach_height)
                {
                    map.tiles[x, y].floor = floors["beach"].Random();
                    beach_tiles.Add((x,y));
                }
                else 
                {
                    map.tiles[x, y].floor = floors["water"].Random();
                    beach_tiles.Add((x,y));
                }
            }
        }
    }

    void CreateForests()
    {
        int number_of_forests = UnityEngine.Random.Range(max_x*max_y / (5*32), max_x*max_y / (3*32));
        for (int i = 0; i < number_of_forests; ++i)
        {
            int size_of_forest = UnityEngine.Random.Range(10,20);

            int number_of_tries = 0;
            bool found = false;
            int x = 0; int y = 0;
            while (found == false && number_of_tries < 1000)
            {
                x = UnityEngine.Random.Range(0, max_x);
                y = UnityEngine.Random.Range(0, max_y);

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

                    if (start_position.x + x < 0 || start_position.x + x >= max_x 
                    || start_position.y + y < 0 || start_position.y + y >= max_y )
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
    }

    void DistributeDungeons(List<DungeonChangeData> dungeon_change_data)
    {
        foreach (DungeonChangeData dcd in dungeon_change_data)
        {
            GameData game_data = GameObject.Find("GameData").GetComponent<GameData>();
            DungeonData dungeon = game_data.dungeons.Find(x => x.name == dcd.target_dungeon_name);
            if (dungeon == null)
            {
                Debug.LogError("Error. Could not find dungeon with name: " + dcd.target_dungeon_name + " in world map creation.");
                return;
            }

            MapFeatureData feature = (MapFeatureData)Activator.CreateInstance(dcd.dungeon_change_type, map, dcd);
            (int x, int y, int w, int h)? position = AddRandomPositionRoom(map, feature.dimensions.x, feature.dimensions.y, game_data.biomes[dungeon.GetMapLevelData(0).biome_index].GetType());
            if (position == null)
                continue;
            feature.position.x = position.Value.x;
            feature.position.y = position.Value.y;

            map.features.Add(feature);            
        }
    }

    public override MapData CreateMapLevel(int level, int max_x, int max_y, int number_of_rooms, List<(Type type, int amount_min, int amount_max)> map_features, List<DungeonChangeData> dungeon_change_data)
    {
        map = new MapData(max_x, max_y);
        this.max_x = max_x;
        this.max_y = max_y;

        room_list = new();

        CreateTerrain();
        CreateForests();

        DistributeDungeons(dungeon_change_data);

     

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
    string icon = "";
    public MFStandardDungeonEntrance(MapData map) : base(map)
    {
       
    }

    public MFStandardDungeonEntrance(MapData map, DungeonChangeData dcd) : base(map, dcd)
    {
        dimensions = (1,1);
        this.icon = dcd.dungeon_change_image;
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
        map.Add(new DynamicObjectData(position.x, position.y, new DungeonEntrance(this.difficulty_level, icon)));

        enter_tiles.Add((position.x, position.y));
        exit_tile = (position.x + dimensions.x / 2, position.y);
    }
}

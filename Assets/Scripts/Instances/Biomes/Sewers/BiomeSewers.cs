using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class BiomeSewers : BiomeData
{
    public BiomeSewers()
    {
        name = "Sewers";
        connectivity_probability = 0.5f;

        MapObjectCollection collection = new();
        collection.Add(new MapObject("sewers_water_1"));
        collection.Add(new MapObject("sewers_water_2"));
        collection.Add(new MapObject("sewers_water_3"));
        floors["water"] = collection;

        collection = new();
        collection.Add(new MapObject("sewers_earth_1"));
        collection.Add(new MapObject("sewers_earth_2"));
        collection.Add(new MapObject("sewers_earth_3"));
        floors["earth"] = collection;

        collection = new();
        collection.Add(new MapObject("stone_mosaic_1"));
        collection.Add(new MapObject("stone_mosaic_2"));
        collection.Add(new MapObject("stone_mosaic_3"));
        floors["stones"] = collection;

        collection = new();
        collection.Add(new MapObject("sewers_wall"));
        objects["wall"] = collection;
    }

    public override MapData[,] CreateMapLevel(int level, int max_x, int max_y, List<DungeonChangeData> region_changes, string[,] placement_maps)
    {
        MapData[,] maps = new MapData[max_x, max_y];

        for (int x = 0; x < max_x; ++x)
        {
            for (int y = 0; y < max_y; ++y)
            {
                maps[x, y] = new MapData();
            }
        }

        for (int x = 0; x < max_x - 1; ++x)
        {
            for (int y = 0; y < max_y - 1; ++y)
            {
                if (UnityEngine.Random.value <= connectivity_probability)
                {
                    maps[x, y].exit_right = true;
                    maps[x + 1, y].exit_left = true;
                }
                if (UnityEngine.Random.value <= connectivity_probability)
                {
                    maps[x, y].exit_up = true;
                    maps[x, y + 1].exit_down = true;
                }
            }
        }

        GuaranteeMapLevelConnectivity(maps);

        for (int x = 0; x < max_x; ++x)
        {
            for (int y = 0; y < max_y; ++y)
            {
                CreateMap(x, y, maps[x, y], placement_maps[x,y]);

                if (placement_maps[x, y].Contains("Region Change"))
                {
                    foreach (DungeonChangeData region_change in region_changes)
                    {
                        if (region_change.z_src == level)
                        {
                            maps[x, y].enter_region = region_change;
                            maps[x, y].tiles[7, 7].objects.Add(
                                new MapObject
                                {
                                    name = "Region Change",
                                    texture_name = "entrance_sewers",
                                }
                            );
                        }
                    }
                }

                if (placement_maps[x, y].Contains("Level Up"))
                {

                    maps[x, y].tiles[7, 8].objects.Add(
                        new MapObject
                        {
                            name = "Level Up",
                            texture_name = "entrance_sewers",
                        }
                    );
                }

                if (placement_maps[x, y].Contains("Level Down"))
                {

                    maps[x, y].tiles[8, 7].objects.Add(
                        new MapObject
                        {
                            name = "Level Down",
                            texture_name = "entrance_sewers",
                        }
                    );
                }
            }
        }

        return maps;
    }

    public void CreateMap(int map_x, int map_y, MapData map, string feature)
    {
        float room_probability = 0.5f;

        if (map.exit_up == true)
        {
            for (int y = 7; y < map.tiles.GetLength(1); ++y)
            {
                map.tiles[7, y].floor = floors["water"].Random();
                map.tiles[8, y].floor = floors["water"].Random();
                map.tiles[6, y].floor = floors["stones"].Random();
                map.tiles[9, y].floor = floors["stones"].Random();
            }
        }
        else
        {
            float random = UnityEngine.Random.value;
            if (feature == "" && random < room_probability)
            {
                map.tiles[6, 9].floor = floors["earth"].Random();
                map.tiles[9, 9].floor = floors["earth"].Random();
                map.tiles[6, 10].floor = floors["earth"].Random();
                map.tiles[9, 10].floor = floors["earth"].Random();
                map.tiles[6, 11].floor = floors["earth"].Random();
                map.tiles[9, 11].floor = floors["earth"].Random();

                for (int x = 3; x < 13; ++x)
                {
                    for (int y = 12; y <= 15; ++y)
                    {
                        map.tiles[x, y].floor = floors["earth"].Random();
                    }
                }
            }
        }

        if (map.exit_down == true)
        {
            for (int y = 8; y >= 0; --y)
            {
                map.tiles[7, y].floor = floors["water"].Random();
                map.tiles[8, y].floor = floors["water"].Random();
                if (map.tiles[6, y].floor == null)
                    map.tiles[6, y].floor = floors["stones"].Random();
                if (map.tiles[9, y].floor == null)
                    map.tiles[9, y].floor = floors["stones"].Random();

            }
        }
        else
        {
            float random = UnityEngine.Random.value;
            if (feature == "" && random < room_probability)
            {
                map.tiles[6, 6].floor = floors["earth"].Random();
                map.tiles[9, 6].floor = floors["earth"].Random();
                map.tiles[6, 5].floor = floors["earth"].Random();
                map.tiles[9, 5].floor = floors["earth"].Random();
                map.tiles[6, 4].floor = floors["earth"].Random();
                map.tiles[9, 4].floor = floors["earth"].Random();

                for (int x = 3; x < 13; ++x)
                {
                    for (int y = 3; y >= 0; --y)
                    {
                        map.tiles[x, y].floor = floors["earth"].Random();
                    }
                }
            }
        }

        if (map.exit_left == true)
        {
            for (int x = 8; x >= 0; --x)
            {
                if (map.tiles[x, 6].floor == null) map.tiles[x, 6].floor = floors["stones"].Random();
                map.tiles[x, 7].floor = floors["water"].Random();
                map.tiles[x, 8].floor = floors["water"].Random();
                if (map.tiles[x, 9].floor == null) map.tiles[x, 9].floor = floors["stones"].Random();

            }
        }
        else
        {
            float random = UnityEngine.Random.value;
            if (feature == "" && random < room_probability)
            {
                map.tiles[6, 6].floor = floors["earth"].Random();
                map.tiles[6, 9].floor = floors["earth"].Random();
                map.tiles[5, 6].floor = floors["earth"].Random();
                map.tiles[5, 9].floor = floors["earth"].Random();
                map.tiles[4, 6].floor = floors["earth"].Random();
                map.tiles[4, 9].floor = floors["earth"].Random();

                for (int y = 3; y < 13; ++y)
                {
                    for (int x = 3; x >= 0; --x)
                    {
                        map.tiles[x, y].floor = floors["earth"].Random();
                    }
                }
            }
        }

        if (map.exit_right == true)
        {
            for (int x = 7; x < map.tiles.GetLength(0); ++x)
            {
                if (map.tiles[x, 6].floor == null) map.tiles[x, 6].floor = floors["stones"].Random();
                map.tiles[x, 7].floor = floors["water"].Random();
                map.tiles[x, 8].floor = floors["water"].Random();
                if (map.tiles[x, 9].floor == null) map.tiles[x, 9].floor = floors["stones"].Random();

            }
        }
        else
        {
            float random = UnityEngine.Random.value;
            if (feature == "" && random < room_probability)
            {
                map.tiles[9, 6].floor = floors["earth"].Random();
                map.tiles[9, 9].floor = floors["earth"].Random();
                map.tiles[10, 6].floor = floors["earth"].Random();
                map.tiles[10, 9].floor = floors["earth"].Random();
                map.tiles[11, 6].floor = floors["earth"].Random();
                map.tiles[11, 9].floor = floors["earth"].Random();

                for (int y = 3; y < 13; ++y)
                {
                    for (int x = 12; x <= 15; ++x)
                    {
                        map.tiles[x, y].floor = floors["earth"].Random();
                    }
                }
            }
        }

        //All unused tiles are walls
        for (int x = 0; x < map.tiles.GetLength(0); ++x)
        {
            for (int y = 0; y < map.tiles.GetLength(1); ++y)
            {
                if (map.tiles[x, y].floor == null)
                {
                    map.tiles[x, y].floor = floors["earth"].Random();
                    map.tiles[x, y].objects.Add(objects["wall"].Random());
                }
            }
        }
    }
}

*/
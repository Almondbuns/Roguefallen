using System.Collections.Generic;
using UnityEngine;
using System;

public class BiomeVillage : BiomeData
{
    public BiomeVillage()
    {
        name = "Village";
        ambience_light = Color.white;

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("grassland_background_1"));
        collection.Add(new MapObjectData("grassland_background_2"));
        collection.Add(new MapObjectData("grassland_background_3"));
        collection.Add(new MapObjectData("grassland_background_4"));
        floors["grass"] = collection;

        collection = new();
        collection.Add(new MapObjectData("grassland_obstacle_1"));
        collection.Add(new MapObjectData("grassland_obstacle_3"));
        objects["obstacle"] = collection;

        collection = new();
        collection.Add(new MapObjectData("grassland_obstacle_2"));
        objects["tree"] = collection;

        room_list = new();
    }

    public override MapData CreateMapLevel(int level, int max_x, int max_y, int number_of_rooms, List<(Type type, int amount_min, int amount_max)> map_features, List<DungeonChangeData> dungeon_change_data)
    {
        MapData map = new MapData(max_x, max_y);
        room_list = new();

        for (int x = 0; x < max_x; ++x)
        {
            for (int y = 0; y < max_y; ++y)
            {
                map.tiles[x, y].floor = floors["grass"].Random();
                if (UnityEngine.Random.value <= 0.05f)
                    map.tiles[x, y].objects.Add(objects["obstacle"].Random());

                double tree_probability = 6 * Mathf.Pow(Mathf.Sqrt(Mathf.Pow(x - max_x / 2, 2) + Mathf.Pow(y - max_y / 2, 2)) / (max_x),4);
                if (UnityEngine.Random.value <= tree_probability)
                    map.tiles[x, y].objects.Add(objects["tree"].Random());
            }
        }

        foreach (var dcd in dungeon_change_data)
        {
            MapFeatureData feature = (MapFeatureData)Activator.CreateInstance(dcd.dungeon_change_type, map, dcd);
            (int x, int y, int w, int h)? position = AddRandomPositionRoom(map, feature.dimensions.x, feature.dimensions.y);
            if (position == null)
                continue;
            feature.position.x = position.Value.x;
            feature.position.y = position.Value.y;

            map.features.Add(feature);
            feature.Generate();
            ClearBorders(map, position.Value);            
        }

        foreach (var feature_type in map_features)
        {
            int amount = UnityEngine.Random.Range(feature_type.amount_min, feature_type.amount_max + 1);
            for (int i = 0; i < amount; ++i)
            {
                MapFeatureData feature = (MapFeatureData)Activator.CreateInstance(feature_type.type, map);
                (int x, int y, int w, int h)? position = AddRandomPositionRoom(map, feature.dimensions.x, feature.dimensions.y);
                if (position == null)
                    continue;
                feature.position.x = position.Value.x;
                feature.position.y = position.Value.y;
                feature.difficulty_level = level;

                map.features.Add(feature);
                feature.Generate();
                ClearBorders(map, position.Value);
            }
        }

        return map;
    }

    private void ClearBorders(MapData map, (int x, int y, int w, int h) position)
    {
        for (int i = position.x-1; i < position.x + position.w +1; ++i)
        {
            map.tiles[i, position.y - 1].objects.Clear();
            map.tiles[i, position.y + position.h].objects.Clear();
        }

        for (int j = position.y-1; j < position.y + position.h +1; ++j)
        {
            map.tiles[position.x - 1,j].objects.Clear();
            map.tiles[position.x + position.w,j].objects.Clear();
        }
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
            x = UnityEngine.Random.Range(1, map.tiles.GetLength(0) - w - 1);
            y = UnityEngine.Random.Range(1, map.tiles.GetLength(1) - h - 1);

            room_found = true;
            foreach ((int x, int y, int w, int h) room in room_list)
            {
                if (room.x + room.w < x - 1)
                    continue;
                if (room.x > x + w + 1)
                    continue;

                if (room.y + room.h < y - 1)
                    continue;
                if (room.y > y + h + 1)
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
}
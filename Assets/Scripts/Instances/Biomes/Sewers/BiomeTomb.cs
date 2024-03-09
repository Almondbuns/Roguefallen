using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class BiomeTomb : BiomeSewers
{
    public BiomeTomb()
    {
        name = "Tomb";
        connectivity_probability = 0.5f;
        ambience_light = new Color(0.45f, 0.45f, 0.45f);

        has_water = false;
        has_complex_corridors = false;
        has_clutter = false;
        size_large_corridors = 2;
        size_small_corridors = 1;
        room_w_min = 5;
        room_w_max = 11;
        room_h_min = 5;
        room_h_max = 11;

        MapObjectCollectionData collection = new();
        collection = new();
        collection.Add(new MapObjectData("tomb_sand_1"));
        collection.Add(new MapObjectData("tomb_sand_2"));
        collection.Add(new MapObjectData("tomb_sand_3"));
        collection.Add(new MapObjectData("tomb_sand_4"));
        collection.Add(new MapObjectData("tomb_sand_5"));
        floors["small_corridor"] = collection;

        collection = new();
        collection.Add(new MapObjectData("tomb_chamber_floor_1"));
        collection.Add(new MapObjectData("tomb_chamber_floor_2"));
        collection.Add(new MapObjectData("tomb_chamber_floor_3"));
        collection.Add(new MapObjectData("tomb_chamber_floor_4"));
        floors["room_floor"] = collection;

        collection = new();
        collection.Add(new MapObjectData("tomb_stones_1"));
        collection.Add(new MapObjectData("tomb_stones_2"));
        collection.Add(new MapObjectData("tomb_stones_3"));
        collection.Add(new MapObjectData("tomb_stones_4"));
        floors["large_corridor"] = collection;

        collection = new();
        collection.Add(new MapObjectData("tomb_wall_1"));
        collection.Add(new MapObjectData("tomb_wall_2"));
        collection.Add(new MapObjectData("tomb_wall_3"));
        collection.Add(new MapObjectData("tomb_wall_4"));
        objects["wall"] = collection;

        collection = new();
        collection.Add(new MapObjectData("tomb_candles_1") { emits_light = true, light_color = new Color(1.0f,1.0f,1.0f), movement_blocked = false, sight_blocked = false });
        collection.Add(new MapObjectData("tomb_candles_2") { emits_light = true, light_color = new Color(1.0f,1.0f,1.0f), movement_blocked = false, sight_blocked = false });
        objects["light"] = collection;

        collection = new();
        collection.Add(new MapObjectData("tomb_alt_wall_1"));
        objects["alt_wall"] = collection;

        /*collection = new();
        collection.Add(new MapObjectData("hay") {sight_blocked = false, movement_blocked = false});
        collection.Add(new MapObjectData("bricks") {sight_blocked = false, movement_blocked = false});
        objects["clutter"] = collection;*/
    }

    public override void CreateRoom(MapData map, (int x, int y, int w, int h) position)
    {
        base.CreateRoom(map,position);

        if (UnityEngine.Random.Range(0,3) == 0)
        {
            map.tiles[position.x, position.y].objects.Add(objects["alt_wall"].Random());
            map.tiles[position.x + position.w - 1, position.y].objects.Add(objects["alt_wall"].Random());
            map.tiles[position.x, position.y + position.h - 1].objects.Add(objects["alt_wall"].Random());
            map.tiles[position.x + position.w - 1, position.y + position.h - 1].objects.Add(objects["alt_wall"].Random());
        }

        if (UnityEngine.Random.Range(0,3) == 0)
        {
            for (int i = position.x+1; i < position.x + position.w - 1; ++i)
                map.tiles[i, position.y + position.h / 2].objects.Add(objects["alt_wall"].Random());
        }

        if (UnityEngine.Random.Range(0,3) == 0)
        {
            for (int i = position.y+1; i < position.y + position.h - 1; ++i)
                map.tiles[position.x + position.w/2, i].objects.Add(objects["alt_wall"].Random());
        }
        
    }

}

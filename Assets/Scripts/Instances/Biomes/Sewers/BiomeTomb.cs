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
        size_large_corridors = 2;
        size_small_corridors = 1;

        MapObjectCollectionData collection = new();
        collection = new();
        collection.Add(new MapObjectData("tomb_earth_1"));
        floors["small_corridor"] = collection;

        collection = new();
        collection.Add(new MapObjectData("tomb_chamber_floor"));
        floors["room_floor"] = collection;

        collection = new();
        collection.Add(new MapObjectData("tomb_stone_chamber"));
        floors["large_corridor"] = collection;

        collection = new();
        collection.Add(new MapObjectData("tomb_wall"));
        objects["wall"] = collection;

        collection = new();
        collection.Add(new MapObjectData("sewer_flower_1") { emits_light = true, light_color = new Color(1.0f,1.0f,1.0f), movement_blocked = false, sight_blocked = false });
        objects["light"] = collection;

        collection = new();
        collection.Add(new MapObjectData("hay") {sight_blocked = false, movement_blocked = false});
        collection.Add(new MapObjectData("bricks") {sight_blocked = false, movement_blocked = false});
        objects["clutter"] = collection;
    }

}

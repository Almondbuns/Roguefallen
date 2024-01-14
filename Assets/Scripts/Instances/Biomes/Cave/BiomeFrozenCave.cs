using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BiomeFrozenCave : BiomeCave
{
    public BiomeFrozenCave()
    {
        name = "Frozen Cave";
        ambience_light = new Color(0.25f,0.25f, 0.25f);

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("ice_cave_floor_1"));
        collection.Add(new MapObjectData("ice_cave_floor_2"));
        collection.Add(new MapObjectData("ice_cave_floor_3"));
        floors["floor"] = collection;

       
        collection = new();
        collection.Add(new MapObjectData("ice_cave_wall_1"));
        collection.Add(new MapObjectData("ice_cave_wall_2"));
        collection.Add(new MapObjectData("ice_cave_wall_3"));
        collection.Add(new MapObjectData("ice_cave_wall_4"));
        objects["wall"] = collection;

        collection = new();
        collection.Add(new MapObjectData("ice_cave_crystal_5") { emits_light = true, light_color = new Color(1.0f,1.0f,1.0f)});
        collection.Add(new MapObjectData("ice_cave_crystal_6") { emits_light = true, light_color = new Color(1.0f,0.22f,0.6f)});
        collection.Add(new MapObjectData("ice_cave_sta_1") { sight_blocked = false });
        collection.Add(new MapObjectData("ice_cave_sta_1") { sight_blocked = false });
        collection.Add(new MapObjectData("ice_cave_sta_2") { sight_blocked = false });
        collection.Add(new MapObjectData("ice_cave_sta_2") { sight_blocked = false });
        objects["obstacle"] = collection;

        collection = new();
        collection.Add(new MapObjectData("ice_cave_crystal_3") { emits_light = true, light_color = new Color(1.0f,1.0f,1.0f), movement_blocked = false, sight_blocked = false });
        collection.Add(new MapObjectData("ice_cave_crystal_4") { emits_light = true, light_color = new Color(1.0f,1.0f,1.0f), movement_blocked = false, sight_blocked = false });
        objects["light_1"] = collection;

        collection = new();
        collection.Add(new MapObjectData("ice_cave_crystal_1") { emits_light = true, light_color = new Color(1.0f,0.22f,0.6f), movement_blocked = false, sight_blocked = false }) ;
        collection.Add(new MapObjectData("ice_cave_crystal_2") { emits_light = true, light_color = new Color(1.0f,0.22f,0.6f), movement_blocked = false, sight_blocked = false }) ;
        objects["light_2"] = collection;
    }
}


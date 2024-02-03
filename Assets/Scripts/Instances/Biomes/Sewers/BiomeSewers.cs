using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


public class BiomeSewers : BiomeData
{
    public bool has_water = true;
    public bool has_complex_corridors = true;
    public int size_large_corridors = 3;
    public int size_small_corridors = 2;

    public BiomeSewers()
    {
        name = "Sewers";
        connectivity_probability = 0.5f;
        ambience_light = new Color(0.45f, 0.45f, 0.45f);

        MapObjectCollectionData collection = new();
        collection = new();
        collection.Add(new MapObjectData("sewers_water_1"));
        collection.Add(new MapObjectData("sewers_water_2"));
        collection.Add(new MapObjectData("sewers_water_3"));
        floors["water"] = collection;

        collection = new();
        collection.Add(new MapObjectData("sewers_earth_1"));
        collection.Add(new MapObjectData("sewers_earth_2"));
        collection.Add(new MapObjectData("sewers_earth_3"));
        floors["small_corridor"] = collection;

        collection = new();
        collection.Add(new MapObjectData("cellar_1"));
        collection.Add(new MapObjectData("cellar_2"));
        collection.Add(new MapObjectData("cellar_3"));
        floors["room_floor"] = collection;

        collection = new();
        collection.Add(new MapObjectData("stone_mosaic_1"));
        collection.Add(new MapObjectData("stone_mosaic_2"));
        collection.Add(new MapObjectData("stone_mosaic_3"));
        floors["large_corridor"] = collection;

        collection = new();
        collection.Add(new MapObjectData("temple_wall"));
        objects["wall"] = collection;

        collection = new();
        collection.Add(new MapObjectData("sewer_flower_1") { emits_light = true, light_color = new Color(0.0f,0.8f,1.0f), movement_blocked = false, sight_blocked = false });
        objects["light"] = collection;

        collection = new();
        collection.Add(new MapObjectData("cupboard") {});
        collection.Add(new MapObjectData("wine_shelf") {});
        collection.Add(new MapObjectData("tavern_table") {sight_blocked = false,});
        collection.Add(new MapObjectData("tavern_chair_L") {sight_blocked = false,});
        collection.Add(new MapObjectData("tavern_chair_R") {sight_blocked = false,});
        collection.Add(new MapObjectData("hay") {sight_blocked = false, movement_blocked = false});
        collection.Add(new MapObjectData("bricks") {sight_blocked = false, movement_blocked = false});
        objects["clutter"] = collection;

        collection = new();
        collection.Add(new MapObjectData("barn_background_1"));
        collection.Add(new MapObjectData("barn_background_2"));
        collection.Add(new MapObjectData("barn_background_3"));
        collection.Add(new MapObjectData("barn_background_4"));
        floors["wooden_bridge"] = collection;
    }

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);

        save.Write(has_water);
        save.Write(has_complex_corridors);
        save.Write(size_large_corridors);
        save.Write(size_small_corridors);
    }

    internal override void Load(BinaryReader save)
    {
        has_water = save.ReadBoolean();
        has_complex_corridors = save.ReadBoolean();
        size_large_corridors = save.ReadInt32();
        size_small_corridors = save.ReadInt32();
    }

    public (int x, int y, int w, int h)? AddRandomPositionRoom(MapData map, int w, int h, bool needs_connection = true)
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
                //Find nearest room already connected
                int min_distance = -1;
                (int,int,int,int) nearest_room = (0,0,0,0);
                foreach (var v in room_list)
                {
                    int distance = (int) (Mathf.Pow(x+w-v.x-v.w,2) + Mathf.Pow(y+h-v.y-v.h,2));
                    if (distance < min_distance || min_distance == -1)
                    {
                        min_distance = distance;
                        nearest_room = v;
                    }
                }
                int water_size = 0;
                if (has_water == true)
                    water_size = 1;
                CreateCorridor(map, (x,y,w,h), nearest_room, water_size);
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
        {
            for (int y = 0; y < map.tiles.GetLength(1); ++y)
            {
                map.tiles[x, y].floor = floors["small_corridor"].Random();
                map.tiles[x, y].objects.Add(objects["wall"].Random());
            }
        }

        //Start with canal system

        int number_of_canals = 0;
        if (has_complex_corridors == true)
        {
            (int x,int y)[] random_positions = new (int,int)[6];
            for (int i = 0; i < 6; ++ i)
            {
                random_positions[i] = (UnityEngine.Random.Range((i%3) * (max_x/3) + 10,((i%3)+1) * (max_x/3) - 10), UnityEngine.Random.Range((i/3) * (max_y/2) + 10,((i/3)+1) * (max_y/2) - 10));
            }
            int water_size = 0;
            if (has_water == true)
                water_size = 2;

            CreateSewerSystemAbstract(map, random_positions[0].x, random_positions[0].y,random_positions[1].x, random_positions[1].y,water_size);
            CreateSewerSystemAbstract(map, random_positions[1].x, random_positions[1].y,random_positions[2].x, random_positions[2].y,water_size);
            CreateSewerSystemAbstract(map, random_positions[2].x, random_positions[2].y,random_positions[5].x, random_positions[5].y,water_size);
            CreateSewerSystemAbstract(map, random_positions[3].x, random_positions[3].y,random_positions[0].x, random_positions[0].y,water_size);
            CreateSewerSystemAbstract(map, random_positions[4].x, random_positions[4].y,random_positions[3].x, random_positions[3].y,water_size);
            CreateSewerSystemAbstract(map, random_positions[5].x, random_positions[5].y,random_positions[4].x, random_positions[4].y,water_size);
            CreateSewerSystemAbstract(map, random_positions[1].x, random_positions[1].y,random_positions[4].x, random_positions[4].y,water_size);

            number_of_canals = 7;
        }
        else
        {
            (int x,int y)[] random_positions = new (int,int)[2];
            
            random_positions[0] = (UnityEngine.Random.Range(10, max_x / 2 - 10), UnityEngine.Random.Range(10, max_y / 2 - 10));
            random_positions[1] = (UnityEngine.Random.Range(max_x / 2 + 10, max_x - 10), UnityEngine.Random.Range(max_y / 2 + 10, max_y - 10));

            //Only horizontal or vertical corridors allowed
            room_list.Add((Mathf.Min(random_positions[0].x,random_positions[1].x) , random_positions[0].y, 
                Mathf.Abs(random_positions[0].x - random_positions[1].x),size_large_corridors));
            room_list.Add((random_positions[1].x, Mathf.Min(random_positions[0].y,random_positions[1].y), 
                size_large_corridors,Mathf.Abs(random_positions[0].y - random_positions[1].y)));            
            room_list.Add((Mathf.Min(random_positions[0].x,random_positions[1].x) , random_positions[1].y, 
                Mathf.Abs(random_positions[0].x - random_positions[1].x),size_large_corridors));
            room_list.Add((random_positions[0].x, Mathf.Min(random_positions[0].y,random_positions[1].y), 
                size_large_corridors,Mathf.Abs(random_positions[0].y - random_positions[1].y)));            
            
            number_of_canals = 2;
        }
        //Now grow map and attach every new room - connectivity guaranteed
        
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

        //First create and any room connections then draw the canal system on top
        for (int i = 0; i < number_of_rooms; ++i)
        {
            int w = UnityEngine.Random.Range(6, 10);
            int h = UnityEngine.Random.Range(6, 10);
         
            (int x, int y, int w, int h)? position = AddRandomPositionRoom(map, w, h);
        }

        for (int i = 0; i < 2 * number_of_canals; ++i)
        {
            var corridor = room_list[i];
            if (corridor.w == size_large_corridors)
                CreateVerticalCorridor(map,corridor.x,corridor.y,corridor.y+corridor.h,0,2,true);
            else
                CreateHorizontalCorridor(map,corridor.x,corridor.y,corridor.x+corridor.w,0,2,true);
        }

        for (int i = 2 * number_of_canals; i < room_list.Count; ++ i)
        {
            CreateRoom(map, room_list[i]);
        }
        
        foreach (var feature in map.features)
        {
            feature.Generate();
        }

        return map;
    }

    public void CreateHorizontalCorridor(MapData map, int x1, int y, int x2, int size_water, int size_border, bool canal_system = true)
    {
        int size_all = size_border + size_water;
        string texture = "";
        if (canal_system == true)
            texture = "large_corridor";
        else
            texture = "small_corridor";

        /*for (int j = 0; j < size_all; ++ j)
        {
            if (map.tiles[Mathf.Min(x1,x2)-1, y + j - size_all/2].objects.Count > 0)
            {
                map.tiles[Mathf.Min(x1,x2)-1, y + j - size_all/2].floor = floors[texture].Random();
                map.tiles[Mathf.Min(x1,x2)-1, y + j - size_all/2].objects.Clear();
            }
            if (map.tiles[Mathf.Max(x1,x2)+1, y + j - size_all/2].objects.Count > 0)
            {
                map.tiles[Mathf.Max(x1,x2)+1, y + j - size_all/2].floor = floors[texture].Random();
                map.tiles[Mathf.Max(x1,x2)+1, y + j - size_all/2].objects.Clear();
            }
        }*/

        for (int i = Mathf.Min(x1,x2); i <= Mathf.Max(x1,x2) + size_all - 1; ++i)
        {                        
            for (int j = 0; j < size_all; ++ j)
            {
                map.tiles[i, y + j].objects.Clear();
                map.tiles[i, y + j].floor = floors[texture].Random();
            }

            for (int j = size_border/2; j < size_border / 2 + size_water; ++ j)
                map.tiles[i, y + j - size_all/2].floor = floors["water"].Random();
            
        }

        if (canal_system == true && Mathf.Abs(x1-x2) > 6)
        {
            int random_position = UnityEngine.Random.Range(Mathf.Min(x1,x2) + 2, Mathf.Min(x1,x2) + Mathf.Abs(x1-x2) -2);
            for (int j = size_border; j < size_border + size_water; ++ j)
                map.tiles[random_position, y + j - size_all/2].floor = floors["wooden_bridge"].Random();
        }

    }

    public void CreateVerticalCorridor(MapData map, int x, int y1, int y2, int size_water, int size_border, bool canal_system = true)
    {
        int size_all = size_border + size_water;
        string texture = "";
        if (canal_system == true)
            texture = "large_corridor";
        else
            texture = "small_corridor";

        /*for (int j = 0; j < size_all; ++ j)
        {
            if (map.tiles[x + j - size_all/2, Mathf.Min(y1,y2)-1].objects.Count > 0)
            {
                map.tiles[x + j - size_all/2, Mathf.Min(y1,y2)-1].floor = floors[texture].Random();
                map.tiles[x + j - size_all/2, Mathf.Min(y1,y2)-1].objects.Clear();
            }
            if (map.tiles[x + j - size_all/2, Mathf.Max(y1,y2)+1].objects.Count > 0)
            {
                map.tiles[x + j - size_all/2, Mathf.Max(y1,y2)+1].floor = floors[texture].Random();
                map.tiles[x + j - size_all/2, Mathf.Max(y1,y2)+1].objects.Clear();
            }
        }*/

        for (int i = Mathf.Min(y1,y2); i <= Mathf.Max(y1,y2)  + size_all - 1; ++i)
        {
             for (int j = 0; j < size_all; ++ j)
             {
                map.tiles[x + j, i].objects.Clear();
                map.tiles[x + j, i].floor = floors[texture].Random();
             }
            
            for (int j = size_border/2; j < size_border / 2 + size_water; ++ j)
                map.tiles[x + j - size_all/2, i].floor = floors["water"].Random();           
        }

        if (canal_system == true && Mathf.Abs(y1-y2) > 6)        {
            int random_position = UnityEngine.Random.Range(Mathf.Min(y1,y2) + 2, Mathf.Min(y1,y2) + Mathf.Abs(y1-y2) -2);
            for (int j = size_border; j < size_border + size_water; ++ j)
                map.tiles[x + j - size_all/2, random_position].floor = floors["wooden_bridge"].Random();
        }
    }

    public void CreateCorridor(MapData map, (int x, int y, int w, int h) room1, (int x, int y, int w, int h) room2, int size_water = 2)
    {
        //Only horizontal or vertical corridors allowed
        int rand = UnityEngine.Random.Range(0,2);
        if (rand == 0) //horizontal first
        {
            CreateHorizontalCorridor(map, room1.x + room1.w/2, room1.y + room1.h/2, room2.x + room2.w/2, size_water, size_small_corridors, false);
            CreateVerticalCorridor(map, room2.x + room2.w/2, room1.y + room1.h/2, room2.y + room2.h/2, size_water, size_small_corridors, false);
        }
        else
        {
            CreateVerticalCorridor(map, room1.x + room1.w/2, room1.y + room1.h/2, room2.y + room2.h/2, size_water, size_small_corridors, false);
            CreateHorizontalCorridor(map, room1.x + room1.w/2, room2.y + room2.h/2, room2.x + room2.w/2, size_water, size_small_corridors, false);            
        }       
    }

    public void CreateSewerSystemAbstract(MapData map, int x1, int y1, int x2, int y2, int size_water = 2)
    {
        //Only horizontal or vertical corridors allowed
        int rand = UnityEngine.Random.Range(0,2);
        if (rand == 0) //horizontal first
        {
            room_list.Add((Mathf.Min(x1,x2) , y1,
                Mathf.Abs(x1 - x2),size_large_corridors));
            room_list.Add((x2, Mathf.Min(y1,y2),
                size_large_corridors,Mathf.Abs(y1 - y2)));
        }
        else
        {
            room_list.Add((Mathf.Min(x1,x2) , y2,
                Mathf.Abs(x1 - x2),size_large_corridors));
            room_list.Add((x1, Mathf.Min(y1,y2),
                size_large_corridors,Mathf.Abs(y1 - y2)));            
        }       
    }

    public void CreateRoom(MapData map, (int x, int y, int w, int h) position)
    {
        int floor_style = UnityEngine.Random.Range(0,1);

        for (int x = position.x; x < position.x + position.w; ++x)
        {
            for (int y = position.y; y < position.y + position.h; ++ y)
            {
                if(map.tiles[x,y].floor.name.Contains("water") == false)
                {
                    if (floor_style == 0)
                        map.tiles[x,y].floor = floors["room_floor"].Random();
                }
                map.tiles[x,y].objects.Clear();
            }
        }

        int number_of_clutter = position.w * position.h / 10;
        for (int i = 0; i < UnityEngine.Random.Range(number_of_clutter,2*number_of_clutter); ++i)
        {
            int x = UnityEngine.Random.Range(position.x, position.x + position.w);
            int y = UnityEngine.Random.Range(position.y, position.y + position.h);

            if(map.tiles[x,y].floor.name.Contains("water") == false)
            {
                map.tiles[x,y].objects.Clear();
                map.tiles[x,y].objects.Add(objects["clutter"].Random());
            }
        }

        for (int i = 0; i < UnityEngine.Random.Range(1,4); ++i)
        {
            int x = UnityEngine.Random.Range(position.x, position.x + position.w);
            int y = UnityEngine.Random.Range(position.y, position.y + position.h);

            if(map.tiles[x,y].floor.name.Contains("water") == false)
            {
                map.tiles[x,y].objects.Clear();
                map.tiles[x,y].objects.Add(objects["light"].Random());
            }
        }
    }
}

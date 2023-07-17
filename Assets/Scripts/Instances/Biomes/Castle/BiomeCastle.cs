using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class BiomeCastle : BiomeData
{
    public BiomeCastle()
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
        floors["floor"] = collection;

        collection = new();
        collection.Add(new MapObjectData("cellar_1"));
        collection.Add(new MapObjectData("cellar_2"));
        collection.Add(new MapObjectData("cellar_3"));
        floors["cellar_1"] = collection;

        collection = new();
        collection.Add(new MapObjectData("stone_mosaic_1"));
        collection.Add(new MapObjectData("stone_mosaic_2"));
        collection.Add(new MapObjectData("stone_mosaic_3"));
        floors["stone"] = collection;

        collection = new();
        collection.Add(new MapObjectData("temple_wall"));
        objects["wall"] = collection;

        collection = new();
        collection.Add(new MapObjectData("sewer_flower_1") { emits_light = true, light_color = new Color(0.0f,0.8f,1.0f), movement_blocked = false, sight_blocked = false });
        objects["flower"] = collection;

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
                CreateCorridor(map, (x,y,w,h), nearest_room, 1);
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
                map.tiles[x, y].floor = floors["floor"].Random();
                map.tiles[x, y].objects.Add(objects["wall"].Random());
            }
        }

        //Start with sewer canal system
        (int x,int y)[] random_positions = new (int,int)[6];
        for (int i = 0; i < 6; ++ i)
        {
            random_positions[i] = (UnityEngine.Random.Range((i%3) * (max_x/3) + 10,((i%3)+1) * (max_x/3) - 10), UnityEngine.Random.Range((i/3) * (max_y/2) + 10,((i/3)+1) * (max_y/2) - 10));
            Debug.Log(random_positions[i]);
        }
        CreateSewerSystem(map, (random_positions[0].x, random_positions[0].y,1,1),(random_positions[1].x, random_positions[1].y,1,1),2);
        CreateSewerSystem(map, (random_positions[1].x, random_positions[1].y,1,1),(random_positions[2].x, random_positions[2].y,1,1),2);
        CreateSewerSystem(map, (random_positions[2].x, random_positions[2].y,1,1),(random_positions[5].x, random_positions[5].y,1,1),2);
        CreateSewerSystem(map, (random_positions[3].x, random_positions[3].y,1,1),(random_positions[0].x, random_positions[0].y,1,1),2);
        CreateSewerSystem(map, (random_positions[4].x, random_positions[4].y,1,1),(random_positions[3].x, random_positions[3].y,1,1),2);
        CreateSewerSystem(map, (random_positions[5].x, random_positions[5].y,1,1),(random_positions[4].x, random_positions[4].y,1,1),2);
        CreateSewerSystem(map, (random_positions[1].x, random_positions[1].y,1,1),(random_positions[4].x, random_positions[4].y,1,1),2);

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

        for (int i = 0; i < number_of_rooms; ++i)
        {
            int w = UnityEngine.Random.Range(6, 10);
            int h = UnityEngine.Random.Range(6, 10);
         
            (int x, int y, int w, int h)? position = AddRandomPositionRoom(map, w, h);
        }

        for (int i = 14; i < room_list.Count; ++ i)
        {
            CreateRoom(map, room_list[i]);
        }
        
        foreach (var feature in map.features)
        {
            feature.Generate();
        }

        return map;
    }

    /*public void CreateDirectCorridor(MapData map, (int x, int y, int w, int h) room1, (int x, int y, int w, int h) room2, Path path)
    {
        //Debug.Log("test");
        bool currently_connected = true;
        int start_x = 0, start_y = 0;
        int old_cost = 0;
        foreach ((int x, int y, int cumulated_cost) tile in path.path)
        {
            if (tile.cumulated_cost - old_cost> 999 && currently_connected == true)
            {
                currently_connected = false;
                start_x = tile.x;
                start_y = tile.y;                
            }
            if (tile.cumulated_cost - old_cost <= 999 && currently_connected == false)
            {                
                currently_connected = true;
                CreateCorridor(map, (start_x, start_y, 1, 1), (tile.x, tile.y, 1,1), 0);
                //Debug.Log((start_x, start_y, 1, 1));
                //Debug.Log((tile.x, tile.y, 1,1));
            }
            old_cost = tile.cumulated_cost;
        }
    }*/

    public void CreateHorizontalCorridor(MapData map, int x1, int y, int x2, int size_water, bool canal_system = true)
    {
        int size_border = 1;
        int size_all = size_border + size_water + size_border;
        string texture = "";
        if (canal_system == true)
            texture = "stone";
        else
            texture = "floor";

        for (int j = 0; j < size_all; ++ j)
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
        }

        for (int i = Mathf.Min(x1,x2); i <= Mathf.Max(x1,x2); ++i)
        {
            for (int j = 0; j < size_border; ++ j)
            {
                if (map.tiles[i, y + j - size_all/2].objects.Count > 0)
                    map.tiles[i, y + j - size_all/2].floor = floors[texture].Random();
            }
            for (int j = size_border; j < size_border + size_water; ++ j)
                map.tiles[i, y + j - size_all/2].floor = floors["water"].Random();
            for (int j = size_border + size_water; j < size_all; ++ j)
            {
                if (map.tiles[i, y + j - size_all/2].objects.Count > 0)
                map.tiles[i, y + j - size_all/2].floor = floors[texture].Random();
            }
            for (int j = 0; j < size_all; ++ j)
                map.tiles[i, y + j - size_all/2].objects.Clear();
        }

        if (canal_system == true && Mathf.Abs(x1-x2) > 6)
        {
            int random_position = UnityEngine.Random.Range(Mathf.Min(x1,x2) + 2, Mathf.Min(x1,x2) + Mathf.Abs(x1-x2) -2);
            for (int j = size_border; j < size_border + size_water; ++ j)
                map.tiles[random_position, y + j - size_all/2].floor = floors["wooden_bridge"].Random();
        }

    }

    public void CreateVerticalCorridor(MapData map, int x, int y1, int y2, int size_water, bool canal_system = true)
    {
        int size_border = 1;
        int size_all = size_border + size_water + size_border;
        string texture = "";
        if (canal_system == true)
            texture = "stone";
        else
            texture = "floor";

        for (int j = 0; j < size_all; ++ j)
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
        }

        for (int i = Mathf.Min(y1,y2); i <= Mathf.Max(y1,y2); ++i)
        {
            for (int j = 0; j < size_border; ++ j)
            {
                if (map.tiles[x + j - size_all/2, i].objects.Count > 0)
                    map.tiles[x + j - size_all/2, i].floor = floors[texture].Random();
            }
            for (int j = size_border; j < size_border + size_water; ++ j)
                map.tiles[x + j - size_all/2, i].floor = floors["water"].Random();
            for (int j = size_border + size_water; j < size_all; ++ j)
            {
                if (map.tiles[x + j - size_all/2, i].objects.Count > 0)
                    map.tiles[x + j - size_all/2, i].floor = floors[texture].Random();
            }
            for (int j = 0; j < size_all; ++ j)
                map.tiles[x + j - size_all/2, i].objects.Clear();
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
            CreateHorizontalCorridor(map, room1.x + room1.w/2, room1.y + room1.h/2, room2.x + room2.w/2, size_water, false);
            CreateVerticalCorridor(map, room2.x + room2.w/2, room1.y + room1.h/2, room2.y + room2.h/2, size_water, false);
        }
        else
        {
            CreateVerticalCorridor(map, room1.x + room1.w/2, room1.y + room1.h/2, room2.y + room2.h/2, size_water, false);
            CreateHorizontalCorridor(map, room1.x + room1.w/2, room2.y + room2.h/2, room2.x + room2.w/2, size_water, false);            
        }       
    }

    public void CreateSewerSystem(MapData map, (int x, int y, int w, int h) room1, (int x, int y, int w, int h) room2, int size_water = 2)
    {
        //Only horizontal or vertical corridors allowed
        int rand = UnityEngine.Random.Range(0,2);
        if (rand == 0) //horizontal first
        {
            CreateHorizontalCorridor(map, room1.x + room1.w/2, room1.y + room1.h/2, room2.x + room2.w/2, size_water);
            room_list.Add((Mathf.Min(room1.x + room1.w/2,room2.x + room2.w/2) , room1.y+ room1.h/2-2,
                Mathf.Abs(room1.x + room1.w/2-(room2.x + room2.w/2)),4));
            CreateVerticalCorridor(map, room2.x + room2.w/2, room1.y + room1.h/2, room2.y + room2.h/2, size_water);
            room_list.Add((room2.x + room2.w/2 -2 , Mathf.Min(room1.y+ room1.h/2,room2.y+ room2.h/2),
                4,Mathf.Abs(room1.y + room1.h/2-(room2.y + room2.h/2))));
        }
        else
        {
            CreateVerticalCorridor(map, room1.x + room1.w/2, room1.y + room1.h/2, room2.y + room2.h/2, size_water);
            room_list.Add((room1.x + room1.w/2 -2 , Mathf.Min(room1.y+ room1.h/2,room2.y+ room2.h/2),
                4,Mathf.Abs(room1.y + room1.h/2-(room2.y + room2.h/2))));
            CreateHorizontalCorridor(map, room1.x + room1.w/2, room2.y + room2.h/2, room2.x + room2.w/2, size_water);
            room_list.Add((Mathf.Min(room1.x + room1.w/2,room2.x + room2.w/2) , room2.y+ room2.h/2-2,
                Mathf.Abs(room1.x + room1.w/2-(room2.x + room2.w/2)),4));            
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
                        map.tiles[x,y].floor = floors["cellar_1"].Random();
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
                map.tiles[x,y].objects.Add(objects["flower"].Random());
            }
        }
    }
}

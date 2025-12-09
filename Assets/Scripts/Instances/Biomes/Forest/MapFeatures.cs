using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MFBeeTreasureRoom : MapFeatureData
{
    public MFBeeTreasureRoom(MapData map) : base(map)
    {
        dimensions = (6,6);

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("mystic_forest_living_tree_1") { emits_light = true, light_color = new Color((float)(184 / 255.0), (float)(55 / 255.0), (float)(234 / 255.0)), movement_blocked = true, sight_blocked = false, light_distance = 6 });
        collection.Add(new MapObjectData("mystic_forest_living_tree_2") { emits_light = true, light_color = new Color((float)(184 / 255.0), (float)(55 / 255.0), (float)(234 / 255.0)), movement_blocked = true, sight_blocked = false, light_distance = 6 });
        collection.Add(new MapObjectData("mystic_forest_living_tree_3") { emits_light = true, light_color = new Color((float)(171 / 255.0), (float)(0 / 255.0), (float)(104 / 255.0)), movement_blocked = true, sight_blocked = false, light_distance = 6 });
        collection.Add(new MapObjectData("mystic_forest_living_tree_4") { emits_light = true, light_color = new Color((float)(171 / 255.0), (float)(0 / 255.0), (float)(104 / 255.0)), movement_blocked = true, sight_blocked = false, light_distance = 6 });

        objects["tree"] = collection;
    }

    public override void Generate()
    {
        for (int x = position.x; x < position.x + dimensions.x; ++x)
        {
            for (int y = position.y; y < position.y + dimensions.y; ++y)
            {
                map.tiles[x, y].objects.Clear();
            }
        }

        map.tiles[position.x+1, position.y+3].objects.Add(objects["tree"].Random());
        map.tiles[position.x+5, position.y+3].objects.Add(objects["tree"].Random());
        map.Add(new MonsterData(position.x + 2, position.y + 3, new Beehive(difficulty_level)));
        map.Add(new MonsterData(position.x + 4, position.y + 3, new Beehive(difficulty_level)));

        DynamicObjectData s = new DynamicObjectData(0, 0, new Chest(difficulty_level));
        s.MoveTo(position.x + 3, position.y + 3);
        map.Add(s);

        ActorData vendor = new MonsterData(position.x + 3, position.y + 4, new UnicornUlrich(10));
        map.Add(vendor);
    }
}
public class MFLivingForest : MapFeatureData
{
    public MFLivingForest(MapData map) : base(map)
    {
        dimensions = (UnityEngine.Random.Range(10,21), UnityEngine.Random.Range(10, 21));

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("mystic_forest_living_tree_1") { emits_light = true, light_color = new Color((float)(184 / 255.0), (float)(55 / 255.0), (float)(234 / 255.0)), movement_blocked = true, sight_blocked = false, light_distance = 6 });
        collection.Add(new MapObjectData("mystic_forest_living_tree_2") { emits_light = true, light_color = new Color((float)(184 / 255.0), (float)(55 / 255.0), (float)(234 / 255.0)), movement_blocked = true, sight_blocked = false, light_distance = 6 });
        collection.Add(new MapObjectData("mystic_forest_living_tree_3") { emits_light = true, light_color = new Color((float)(171 / 255.0), (float)(0 / 255.0), (float)(104 / 255.0)), movement_blocked = true, sight_blocked = false, light_distance = 6 });
        collection.Add(new MapObjectData("mystic_forest_living_tree_4") { emits_light = true, light_color = new Color((float)(171 / 255.0), (float)(0 / 255.0), (float)(104 / 255.0)), movement_blocked = true, sight_blocked = false, light_distance = 6 });

        objects["tree"] = collection;
    }

    public override void Generate()
    {
        for (int x = position.x; x < position.x + dimensions.x; ++x)
        {
            for (int y = position.y; y < position.y + dimensions.y; ++y)
            {
                map.tiles[x, y].objects.Clear();

                if ((x+y) % 2 == 0 && UnityEngine.Random.Range(0,100) < 50)
                    map.tiles[x,y].objects.Add(objects["tree"].Random());
            }
        }
    }
}

public class MFPond : MapFeatureData
{
    public MFPond(MapData map) : base(map)
    {
        dimensions = (UnityEngine.Random.Range(15, 25), UnityEngine.Random.Range(8, 15));

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("mystic_forest_water_1") { movement_blocked = true, sight_blocked = false, emits_light = true, light_color = new Color((float)(33 / 255.0), (float)(171 / 255.0), (float)(203 / 255.0)), light_distance = 1 });
        collection.Add(new MapObjectData("mystic_forest_water_2") { movement_blocked = true, sight_blocked = false });
        collection.Add(new MapObjectData("mystic_forest_water_3") { movement_blocked = true, sight_blocked = false });
        collection.Add(new MapObjectData("mystic_forest_water_2") { movement_blocked = true, sight_blocked = false });
        collection.Add(new MapObjectData("mystic_forest_water_3") { movement_blocked = true, sight_blocked = false });
        collection.Add(new MapObjectData("mystic_forest_water_2") { movement_blocked = true, sight_blocked = false });
        collection.Add(new MapObjectData("mystic_forest_water_3") { movement_blocked = true, sight_blocked = false });
        collection.Add(new MapObjectData("mystic_forest_water_5") { movement_blocked = true, sight_blocked = false, emits_light = true, light_color = new Color((float)(255 / 255.0), (float)(188 / 255.0), (float)(78 / 255.0)), light_distance = 2 });
        collection.Add(new MapObjectData("mystic_forest_water_6") { movement_blocked = true, sight_blocked = false, emits_light = true, light_color = new Color((float)(33 / 255.0), (float)(171 / 255.0), (float)(203 / 255.0)), light_distance = 1 });

        objects["water"] = collection;

        collection = new();
        collection.Add(new MapObjectData("mystic_forest_water_border_n_1") { movement_blocked = true, sight_blocked = false });
        objects["water_border_n"] = collection;

        collection = new();
        collection.Add(new MapObjectData("mystic_forest_water_border_w_1") { movement_blocked = true, sight_blocked = false });
        objects["water_border_w"] = collection;

        collection = new();
        collection.Add(new MapObjectData("mystic_forest_water_border_nw_1") { movement_blocked = true, sight_blocked = false });
        objects["water_border_nw"] = collection;

        collection = new();
        collection.Add(new MapObjectData("mystic_forest_water_border_sw_1") { movement_blocked = true, sight_blocked = false });
        objects["water_border_sw"] = collection;

        collection = new();
        collection.Add(new MapObjectData("mystic_forest_water_border_ne_1") { movement_blocked = true, sight_blocked = false });
        objects["water_border_ne"] = collection;

        collection = new();
        collection.Add(new MapObjectData("mystic_forest_water_border_e_1") { movement_blocked = true, sight_blocked = false });
        objects["water_border_e"] = collection;

        collection = new();
        collection.Add(new MapObjectData("mystic_forest_water_border_s_1") { movement_blocked = true, sight_blocked = false });
        objects["water_border_s"] = collection;

        collection = new();
        collection.Add(new MapObjectData("mystic_forest_water_border_se_1") { movement_blocked = true, sight_blocked = false });
        objects["water_border_se"] = collection;

        collection = new();
        collection.Add(new MapObjectData("mystic_forest_flower_1") { emits_light = true, light_color = new Color((float)(255 / 255.0), (float)(188 / 255.0), (float)(78 / 255.0)), movement_blocked = false, sight_blocked = false, light_distance = 2 });
        objects["flower"] = collection;
    }

    public override void Generate()
    {
        //Create a rect pond
        for (int x = position.x; x < position.x + dimensions.x; ++x)
        {
            for (int y = position.y; y < position.y + dimensions.y; ++y)
            {
                map.tiles[x, y].objects.Clear();

                if (x == position.x || x == position.x + dimensions.x - 1 || y == position.y || y == position.y + dimensions.y - 1)
                {
                    if (UnityEngine.Random.value <= 0.1f)
                        map.tiles[x, y].objects.Add(objects["flower"].Random());
                }
                else
                {
                    map.tiles[x, y].objects.Add(objects["water"].Random());
                }
            }
        }

        //Delete some edges to make it look more natural
        int del_w = UnityEngine.Random.Range(2, dimensions.x / 2-1);
        int del_h = UnityEngine.Random.Range(2, dimensions.y / 2-1);

        for (int x = position.x; x < position.x + del_w; ++x)
        {
            for (int y = position.y; y < position.y + del_h; ++y)
            {
                map.tiles[x, y].objects.Clear();
            }
        }

        del_w = UnityEngine.Random.Range(2, dimensions.x / 2 - 1);
        del_h = UnityEngine.Random.Range(2, dimensions.y / 2 - 1);

        for (int x = position.x + dimensions.x - del_w; x < position.x + dimensions.x; ++x)
        {
            for (int y = position.y; y < position.y + del_h; ++y)
            {
                map.tiles[x, y].objects.Clear();
            }
        }

        del_w = UnityEngine.Random.Range(2, dimensions.x / 2 - 1);
        del_h = UnityEngine.Random.Range(2, dimensions.y / 2 - 1);

        for (int x = position.x; x < position.x + del_w; ++x)
        {
            for (int y = position.y + dimensions.y - del_h; y < position.y + dimensions.y; ++y)
            {
                map.tiles[x, y].objects.Clear();
            }
        }

        del_w = UnityEngine.Random.Range(2, dimensions.x / 2);
        del_h = UnityEngine.Random.Range(2, dimensions.y / 2);

        for (int x = position.x + dimensions.x - del_w; x < position.x + dimensions.x; ++x)
        {
            for (int y = position.y + dimensions.y - del_h; y < position.y + dimensions.y; ++y)
            {
                map.tiles[x, y].objects.Clear();
            }
        }

        //Now replace the border tiles with border textures
       
        for (int x = position.x; x < position.x + dimensions.x; ++x)
        {
            for (int y = position.y; y < position.y + dimensions.y; ++y)
            {
                int border_x = 0;
                int border_y = 0;
                if (map.tiles[x, y].objects.Count > 0 && map.tiles[x, y].objects[0].name.Contains("water"))
                {
                    if (x <= 0 || map.tiles[x-1, y].objects.Count == 0 || map.tiles[x-1, y].objects[0].name.Contains("water") == false)
                    {
                        //Left Border
                        border_x = -1;
                    }
                    else
                    if (x >= map.w - 1 || map.tiles[x + 1, y].objects.Count == 0 || map.tiles[x + 1, y].objects[0].name.Contains("water") == false)
                    {
                        //Right Border
                        border_x = 1;
                    }

                    if (y <= 0 || map.tiles[x, y-1].objects.Count == 0 || map.tiles[x, y-1].objects[0].name.Contains("water") == false)
                    {
                        //Upper Border
                        border_y = 1;
                    }
                    else
                    if (y >= map.h - 1 || map.tiles[x, y+1].objects.Count == 0 || map.tiles[x, y+1].objects[0].name.Contains("water") == false)
                    {
                        //Lower Border
                        border_y = -1;
                    }

                    //Select the right tile graphic
                    if (border_x == -1 && border_y == -1)
                    {
                        map.tiles[x, y].objects.Clear();
                        map.tiles[x, y].objects.Add(objects["water_border_nw"].Random());
                    }
                    else if (border_x == 1 && border_y == -1)
                    {
                        map.tiles[x, y].objects.Clear();
                        map.tiles[x, y].objects.Add(objects["water_border_ne"].Random());
                    }
                    else if (border_x == -1 && border_y == 1)
                    {
                        map.tiles[x, y].objects.Clear();
                        map.tiles[x, y].objects.Add(objects["water_border_sw"].Random());
                    }
                    else if (border_x == -1 && border_y == 0)
                    {
                        map.tiles[x, y].objects.Clear();
                        map.tiles[x, y].objects.Add(objects["water_border_w"].Random());
                    }
                    else if (border_x == 0 && border_y == -1)
                    {
                        map.tiles[x, y].objects.Clear();
                        map.tiles[x, y].objects.Add(objects["water_border_n"].Random());
                    }
                    else if (border_x == 1 && border_y == 1)
                    {
                        map.tiles[x, y].objects.Clear();
                        map.tiles[x, y].objects.Add(objects["water_border_se"].Random());
                    }
                    else if (border_x == 0 && border_y == 1)
                    {
                        map.tiles[x, y].objects.Clear();
                        map.tiles[x, y].objects.Add(objects["water_border_s"].Random());
                    }
                    else if (border_x == 1 && border_y == 0)
                    {
                        map.tiles[x, y].objects.Clear();
                        map.tiles[x, y].objects.Add(objects["water_border_e"].Random());
                    }
                }
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MFLivingForest : MapFeatureData
{
    public MFLivingForest(MapData map) : base(map)
    {
        distribute_general_actors = false;
        distribute_general_items = false;

        dimensions = (UnityEngine.Random.Range(5,15), UnityEngine.Random.Range(5, 15));

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("mystic_forest_living_tree_1") { emits_light = true, light_color = new Color((float)(184 / 255.0), (float)(55 / 255.0), (float)(234 / 255.0)), movement_blocked = true, sight_blocked = false, light_distance = 6 });

        objects["tree"] = collection;
    }

    public override void Generate()
    {
        for (int x = position.x; x < position.x + dimensions.x; ++x)
        {
            for (int y = position.y; y < position.y + dimensions.y; ++y)
            {
                map.tiles[x, y].objects.Clear();

                if ((x+y) % 2 == 0)
                    map.tiles[x,y].objects.Add(objects["tree"].Random());
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class MFOctopusBoss : MapFeatureData
{
    ActorData octopus;
    List<ActorData> tentacles;

    public MFOctopusBoss(MapData map) : base(map)
    {
        distribute_general_actors = false;
        distribute_general_items = false;

        tentacles = new();

        MapObjectCollection collection = new();
        collection.Add(new MapObject("sewers_water_1"));
        collection.Add(new MapObject("sewers_water_2"));
        collection.Add(new MapObject("sewers_water_3"));
        floors["water"] = collection;

        collection = new();
        collection.Add(new MapObject("stone_mosaic_1"));
        collection.Add(new MapObject("stone_mosaic_2"));
        collection.Add(new MapObject("stone_mosaic_3"));
        floors["stones"] = collection;
    }

    public override void Generate()

    {
        position = (1, 1, 13, 13);

        for (int x = 1; x < 15; ++x)
        {
            for (int y = 1; y < 15; ++y)
            {
                map.tiles[x, y].objects.Clear();

                if (x == 1 || x == 14 || y == 1 || y == 14)
                    map.tiles[x, y].floor = floors["stones"].Random();
                else
                    map.tiles[x, y].floor = floors["water"].Random();

            }
        }


        tentacles.Add(new OctopusTentacle(3, 12, 1));
        tentacles.Add(new OctopusTentacle(12, 12, 1));
        tentacles.Add(new OctopusTentacle(12, 3, 1));
        tentacles.Add(new OctopusTentacle(3, 3, 1));
        tentacles.Add(new OctopusTentacle(7, 12, 1));
        tentacles.Add(new OctopusTentacle(12, 7, 1));
        tentacles.Add(new OctopusTentacle(7, 3, 1));
        tentacles.Add(new OctopusTentacle(3, 7, 1));

        foreach (ActorData tentacle in tentacles)
            map.Add(tentacle);

        octopus = new Octopus(8, 8, 5);
        map.Add(octopus);
    }
}

public class MFSewersArena : MapFeatureData
{
    public MFSewersArena(MapData map) : base(map)
    {
        MapObjectCollection collection = new();
        collection.Add(new MapObject("stone_mosaic_1"));
        collection.Add(new MapObject("stone_mosaic_2"));
        collection.Add(new MapObject("stone_mosaic_3"));
        floors["stones"] = collection;

        collection = new();
        collection.Add(new MapObject("sewers_wall"));
        objects["wall"] = collection;
    }

    public override void Generate()
    {
        position = (2, 2, 11, 11);

        for (int x = 2; x < 14; ++x)
        {
            for (int y = 2; y < 14; ++y)
            {
                map.tiles[x, y].objects.Clear();

                map.tiles[x, y].floor = floors["stones"].Random();
            }
        }

        map.tiles[4, 4].objects.Add(objects["wall"].Random());
        map.tiles[4, 5].objects.Add(objects["wall"].Random());
        map.tiles[5, 4].objects.Add(objects["wall"].Random());
        map.tiles[5, 5].objects.Add(objects["wall"].Random());

        map.tiles[10, 4].objects.Add(objects["wall"].Random());
        map.tiles[10, 5].objects.Add(objects["wall"].Random());
        map.tiles[11, 4].objects.Add(objects["wall"].Random());
        map.tiles[11, 5].objects.Add(objects["wall"].Random());

        map.tiles[4, 10].objects.Add(objects["wall"].Random());
        map.tiles[4, 11].objects.Add(objects["wall"].Random());
        map.tiles[5, 10].objects.Add(objects["wall"].Random());
        map.tiles[5, 11].objects.Add(objects["wall"].Random());

        map.tiles[10, 10].objects.Add(objects["wall"].Random());
        map.tiles[10, 11].objects.Add(objects["wall"].Random());
        map.tiles[11, 10].objects.Add(objects["wall"].Random());
        map.tiles[11, 11].objects.Add(objects["wall"].Random());
    }
}

public class MFSewersMonsterLair: MapFeatureData
{
    public MFSewersMonsterLair(MapData map) : base(map)
    {
        distribute_general_actors = false;
        distribute_general_items = false;

        MapObjectCollection collection = new();
        collection.Add(new MapObject("stone_mosaic_1"));
        collection.Add(new MapObject("stone_mosaic_2"));
        collection.Add(new MapObject("stone_mosaic_3"));
        floors["stones"] = collection;

        collection = new();
        collection.Add(new MapObject("sewers_earth_1"));
        collection.Add(new MapObject("sewers_earth_2"));
        collection.Add(new MapObject("sewers_earth_3"));
        floors["earth"] = collection;

        collection = new();
        collection.Add(new MapObject("sewers_wall"));
        objects["wall"] = collection;

        collection = new();
        collection.Add(new MapObject("bones", false));
        objects["bones"] = collection;
    }

    public override void Generate()
    {
        position = (5, 5, 5, 5);

        for (int x = 5; x < 11; ++x)
        {
            for (int y = 5; y < 11; ++y)
            {
                map.tiles[x, y].objects.Clear();

                map.tiles[x, y].floor = floors["earth"].Random();
            }
        }

        map.tiles[6, 6].objects.Add(objects["bones"].Random());
        map.tiles[6, 9].objects.Add(objects["bones"].Random());
        map.tiles[9, 6].objects.Add(objects["bones"].Random());
        map.tiles[9, 9].objects.Add(objects["bones"].Random());

        map.Add(new Bear(7, 7, 5));

        map.Add(new ItemBootsHeavy(6, 7, 2));
        map.Add(new ItemChestHeavy(6, 8, 2));
        map.Add(new ItemHealthPotion(7, 7, 2));
        map.Add(new ItemMeatHorn(7, 8, 2));
        map.Add(new ItemAxe1H(8, 7, 2));
        map.Add(new ItemSword2H(8, 8, 2));
        map.Add(new ItemSpear1H(9, 7, 2));
        map.Add(new ItemHealthPotion(9, 8, 2));


    }
}
*/
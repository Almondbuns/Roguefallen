using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VisualEffect
{
    None,
    Hit,
    Fire,
    Teleport,
}

public class Map : MonoBehaviour
{
    public GameData game_data;
    public MapData map_data;

    public Texture2D atlas_texture;
    public Texture2D light_texture;
    public Texture2D visibility_texture;

    public GameObject actor_prefab;
    public GameObject octopus_tentacle_prefab;
    public GameObject octopus_prefab;
    public GameObject barkeeper_prefab;
    public GameObject questgiver1_prefab;
    public GameObject projectile_prefab;
    public GameObject dynamic_object_prefab;
    public GameObject player_prefab;
    public GameObject item_prefab;
    public GameObject tile_selector_prefab;

    public GameObject lost_explorer_prefab;

    public GameObject visual_effect_hit_prefab;
    public GameObject visual_effect_fire_prefab;
    public GameObject visual_effect_teleport_prefab;

    public List<Actor> actors;
    public List<Item> items;

    Dictionary<string, (int u, int v)> texture_lookup;

    void Awake()
    {
        //actors = new List<Actor>();
        //items = new List<Item>();
    }

    // Start is called before the first frame update
    void Start()
    {
        name = "Map";
        game_data = GameObject.Find("GameData").GetComponent<GameData>();
        map_data = game_data.current_map;
        map_data.CreateActor += CreateActor;
        map_data.CreateItem += CreateItem;
        map_data.RefreshVisibility += RefreshVisibility;

        CreateAtlasTexture();
        CreateLightTexture();
        CreateVisibilityTexture();
        CreateMesh();

        //Creating Items
        foreach (ItemData item_data in map_data.items)
        {
            CreateItem(item_data);
        }

        //Creating Actors
        foreach (ActorData actor_data in map_data.actors)
        {
            CreateActor(actor_data);
        }

        RefreshVisibility();
    }

    public void CreateItem(ItemData item_data)
    {
        GameObject item = GameObject.Instantiate(item_prefab, transform);
        item.GetComponent<Item>().item_data = item_data;
        items.Add(item.GetComponent<Item>());
    }

    public void CreateLightTexture()
    {
        light_texture = new Texture2D(map_data.tiles.GetLength(0), map_data.tiles.GetLength(1));
        light_texture.filterMode = FilterMode.Bilinear;

        RefreshLightTexture();
    }

    public void RefreshLightTexture()
    {
        PlayerData player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;

        for (int y = 0; y < map_data.tiles.GetLength(1); y++)
        {
            for (int x = 0; x < map_data.tiles.GetLength(0); x++)
            {
                light_texture.SetPixel(x, y, map_data.tiles[x, y].light);
            }
        }

        int player_light = 2;
        for (int x = player_data.x - player_light; x <= player_data.x + player_light; ++x)
        {
            for (int y = player_data.y - player_light; y <= player_data.y + player_light; ++y)
            {
                if (x < 0 || y < 0 || x >= map_data.tiles.GetLength(0) || y >= map_data.tiles.GetLength(1))
                    continue;

                List<(int x, int y)> path = Algorithms.LineofSight((player_data.x, player_data.y), (x, y));

                bool blocked = false;
                int counter = 0;
                while (blocked == false && counter < path.Count - 1) //ignore target tile
                {
                    foreach (MapObjectData o in map_data.tiles[path[counter].x, path[counter].y].objects)
                    {
                        if (o.movement_blocked == true)
                        {
                            blocked = true;
                        }
                    }
                    ++counter;
                }

                if (blocked == false)
                {
                    Color tile_color = map_data.tiles[x, y].light;
                    light_texture.SetPixel(x, y, new Color(Mathf.Max(tile_color.r,0.9f), Mathf.Max(tile_color.g, 0.6f), Mathf.Max(tile_color.b, 0.3f)));
                }
            }
        }

        light_texture.Apply();
    }

    public void CreateVisibilityTexture()
    {
        visibility_texture = new Texture2D(map_data.tiles.GetLength(0), map_data.tiles.GetLength(1));
        visibility_texture.filterMode = FilterMode.Point;

        RefreshVisibility();
    }

    public void RefreshVisibility()
    {
        for (int y = 0; y < map_data.tiles.GetLength(1); y++)
        {
            for (int x = 0; x < map_data.tiles.GetLength(0); x++)
            {
                switch (map_data.tiles[x, y].visibility)
                {
                    case Visibility.None:
                        visibility_texture.SetPixel(x, y, new Color(0, 0, 0));
                        break;
                    case Visibility.Once:
                        visibility_texture.SetPixel(x, y, new Color(0.5f, 0.5f, 0.5f));
                        break;
                    case Visibility.Active:
                        visibility_texture.SetPixel(x, y, new Color(1, 1, 1));
                        break;
                }

            }
        }

        visibility_texture.Apply();
        RefreshLightTexture();

        foreach(Item item in items)
        {
            item.UpdateVisibility(map_data);
        }

        foreach (Actor actor in actors)
        {
            actor.UpdateVisibility(map_data);
        }
    }

    public void CreateAtlasTexture()
    {
        atlas_texture = new Texture2D(2048, 2048);
        atlas_texture.filterMode = FilterMode.Point;
        
        texture_lookup = new();

         // Drawing Map with Textures
         for (int y = 0; y < map_data.tiles.GetLength(1); y++)
         {
             for (int x = 0; x < map_data.tiles.GetLength(0); x++) 
             {
                 if (map_data.tiles[x,y].floor != null)
                 {
                    CollectTexture(map_data.tiles[x, y].floor.texture_name);  
                 }

                 foreach(MapObjectData o in map_data.tiles[x,y].objects)
                 {
                    CollectTexture(o.texture_name);
                 }
             }
         }

         atlas_texture.Apply();
    }

    private void CollectTexture(string name)
    {
        if (texture_lookup.ContainsKey(name) == false)
        {
            int x = texture_lookup.Count % 64;
            int y = texture_lookup.Count / 64;
            Texture2D texture = Resources.Load<Texture2D>("images/tiles/" + name);
            PasteSprite(x, y, texture);
            texture_lookup[name] = (x, y);
        }
    }

    public void CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        Vector3[] vertices = new Vector3[4 * map_data.tiles.GetLength(0) * map_data.tiles.GetLength(1)];
        long counter = 0;
        for (int x = 0; x < map_data.tiles.GetLength(0); ++x)
        {
            for (int y = 0; y < map_data.tiles.GetLength(1); ++y)
            {
                vertices[counter] = new Vector3(x, y, 0);
                vertices[counter+1] = new Vector3(x+1, y, 0);
                vertices[counter+2] = new Vector3(x, y+1, 0);
                vertices[counter+3] = new Vector3(x+1, y+1, 0);
                counter += 4;
            }
        }
        mesh.SetVertices(vertices);

        //Floor
        Vector2[] uv = new Vector2[4 * map_data.tiles.GetLength(0) * map_data.tiles.GetLength(1)];
        counter = 0;
        for (int x = 0; x < map_data.tiles.GetLength(0); ++x)
        {
            for (int y = 0; y < map_data.tiles.GetLength(1); ++y)
            {
                (int u, int v) atlas_position = texture_lookup[map_data.tiles[x, y].floor.texture_name];
                uv[counter] = new Vector2(atlas_position.u / 64.0f, atlas_position.v / 64.0f);
                uv[counter+1] = new Vector2((atlas_position.u+1) / 64.0f, atlas_position.v / 64.0f);
                uv[counter+2] = new Vector2(atlas_position.u / 64.0f, (atlas_position.v+1) / 64.0f);
                uv[counter+3] = new Vector2((atlas_position.u+1) / 64.0f, (atlas_position.v + 1) / 64.0f);
                counter +=4;
            }
        }
        mesh.SetUVs(0, uv);

        //Objects
        Vector2[] uv2 = new Vector2[4 * map_data.tiles.GetLength(0) * map_data.tiles.GetLength(1)];
        counter = 0;
        for (int x = 0; x < map_data.tiles.GetLength(0); ++x)
        {
            for (int y = 0; y < map_data.tiles.GetLength(1); ++y)
            {
                (int u, int v) atlas_position = texture_lookup[map_data.tiles[x, y].floor.texture_name];
                uv2[counter] = new Vector2(atlas_position.u / 64.0f, atlas_position.v / 64.0f);
                uv2[counter + 1] = new Vector2((atlas_position.u + 1) / 64.0f, atlas_position.v / 64.0f);
                uv2[counter + 2] = new Vector2(atlas_position.u / 64.0f, (atlas_position.v + 1) / 64.0f);
                uv2[counter + 3] = new Vector2((atlas_position.u + 1) / 64.0f, (atlas_position.v + 1) / 64.0f);

                foreach (MapObjectData o in map_data.tiles[x, y].objects)
                {
                    atlas_position = texture_lookup[o.texture_name];
                    uv2[counter] = new Vector2(atlas_position.u / 64.0f, atlas_position.v / 64.0f);
                    uv2[counter + 1] = new Vector2((atlas_position.u + 1) / 64.0f, atlas_position.v / 64.0f);
                    uv2[counter + 2] = new Vector2(atlas_position.u / 64.0f, (atlas_position.v + 1) / 64.0f);
                    uv2[counter + 3] = new Vector2((atlas_position.u + 1) / 64.0f, (atlas_position.v + 1) / 64.0f);
                }
                counter += 4;
            }
        }
        mesh.SetUVs(1, uv2);

        //Light
        Vector2[] uv3 = new Vector2[4*map_data.tiles.GetLength(0) * map_data.tiles.GetLength(1)];
        counter = 0;
        for (int x = 0; x < map_data.tiles.GetLength(0); ++x)
        {
            for (int y = 0; y < map_data.tiles.GetLength(1); ++y)
            {
                uv3[counter] = new Vector2(x / (float) map_data.tiles.GetLength(0), y / (float)map_data.tiles.GetLength(1));
                uv3[counter + 1] = new Vector2((x+1) / (float)map_data.tiles.GetLength(0), y / (float)map_data.tiles.GetLength(1));
                uv3[counter + 2] = new Vector2(x / (float)map_data.tiles.GetLength(0), (y+1) / (float)map_data.tiles.GetLength(1));
                uv3[counter + 3] = new Vector2((x+1) / (float)map_data.tiles.GetLength(0), (y+1) / (float)map_data.tiles.GetLength(1));

                counter += 4;
            }
        }
        mesh.SetUVs(2, uv3);

        //Visibility
        Vector2[] uv4 = new Vector2[4 * map_data.tiles.GetLength(0) * map_data.tiles.GetLength(1)];
        counter = 0;
        for (int x = 0; x < map_data.tiles.GetLength(0); ++x)
        {
            for (int y = 0; y < map_data.tiles.GetLength(1); ++y)
            {
                uv4[counter] = new Vector2(x / (float)map_data.tiles.GetLength(0), y / (float)map_data.tiles.GetLength(1));
                uv4[counter + 1] = new Vector2((x + 1) / (float)map_data.tiles.GetLength(0), y / (float)map_data.tiles.GetLength(1));
                uv4[counter + 2] = new Vector2(x / (float)map_data.tiles.GetLength(0), (y + 1) / (float)map_data.tiles.GetLength(1));
                uv4[counter + 3] = new Vector2((x + 1) / (float)map_data.tiles.GetLength(0), (y + 1) / (float)map_data.tiles.GetLength(1));

                counter += 4;
            }
        }
        mesh.SetUVs(3, uv3);

        int[] triangles = new int[6 * map_data.tiles.GetLength(0) * map_data.tiles.GetLength(1)];
        counter = 0;
        int x_max = map_data.tiles.GetLength(0);
        int y_max = map_data.tiles.GetLength(1);
        for (int x = 0; x < x_max; ++x)
        {
            for (int y = 0; y < y_max; ++y)
            {
                triangles[counter] = 4 * y_max * x + 4 * y + 0;
                triangles[counter+1] = 4 * y_max * x + 4 * y + 2;
                triangles[counter+2] = 4 * y_max * x + 4 * y + 1;
                triangles[counter+3] = 4 * y_max * x + 4 * y + 1;
                triangles[counter+4] = 4 * y_max * x + 4 * y + 2;
                triangles[counter+5] = 4 * y_max * x + 4 * y + 3;
                counter += 6;
            }
        }
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material.mainTexture = atlas_texture;
        GetComponent<MeshRenderer>().material.SetTexture("_DetailTex", atlas_texture);
        GetComponent<MeshRenderer>().material.SetTexture("_LightTex", light_texture);
        GetComponent<MeshRenderer>().material.SetTexture("_VisibilityTex", visibility_texture);
        GetComponent<MeshRenderer>().ResetBounds();
    }

    internal void DestroyMap()
    {
        GameObject.Find("Camera").transform.SetParent(GameObject.Find("GameData").transform);
        GameObject.Find("Camera").transform.localPosition = new Vector3(0, 0, -10);
        Destroy(gameObject);
    }

    public void AddVisualEffectToTile(VisualEffect effect, (int x, int y) tile)
    {
        if (tile.x < 0 || tile.y < 0 || tile.x >= map_data.tiles.GetLength(0) || tile.y >= map_data.tiles.GetLength(1)) return;

        if (effect == VisualEffect.Hit)
        {
            GameObject visual_effect = GameObject.Instantiate(visual_effect_hit_prefab, transform);
            visual_effect.transform.localPosition = new Vector3(tile.x + 0.5f, tile.y + 0.5f, -1);
            Destroy(visual_effect, 5);
        }
        
        if (effect == VisualEffect.Fire)
        {
            GameObject visual_effect = GameObject.Instantiate(visual_effect_fire_prefab, transform);
            visual_effect.transform.localPosition = new Vector3(tile.x + 0.5f, tile.y + 0.5f, -1);
            Destroy(visual_effect, 5);
        }

        if (effect == VisualEffect.Teleport)
        {
            GameObject visual_effect = GameObject.Instantiate(visual_effect_teleport_prefab, transform);
            visual_effect.transform.localPosition = new Vector3(tile.x + 0.5f, tile.y + 0.5f, -1);
            Destroy(visual_effect, 5);
        }
    }

    internal void Refresh()
    {
      
    }

    public void CreateActor(ActorData actor_data)
    {
        GameObject actor;
        if (actor_data.prototype.prefab_index != -1)
            actor = GameObject.Instantiate(GameObject.Find("PrefabFactory").GetComponent<PrefabFactory>().prefabs[actor_data.prototype.prefab_index], transform);
        else if (actor_data is PlayerData)
            actor = GameObject.Instantiate(player_prefab, transform);
        else if (actor_data.prototype is OctopusTentacle)
            actor = GameObject.Instantiate(octopus_tentacle_prefab, transform);
        else if (actor_data.prototype is Octopus)
            actor = GameObject.Instantiate(octopus_prefab, transform);
        else if (actor_data.prototype is Barkeeper)
            actor = GameObject.Instantiate(barkeeper_prefab, transform);
        else if (actor_data.prototype is Questgiver1)
            actor = GameObject.Instantiate(questgiver1_prefab, transform);
        else if (actor_data.prototype is LostExplorer)
            actor = GameObject.Instantiate(lost_explorer_prefab, transform);
        else if (actor_data is ProjectileData)
            actor = GameObject.Instantiate(projectile_prefab, transform);
        else if (actor_data is DynamicObjectData)
            actor = GameObject.Instantiate(dynamic_object_prefab, transform);
        else
            actor = GameObject.Instantiate(actor_prefab, transform);

        actor.GetComponent<Actor>().Create(actor_data);

        actor_data.HandleKill += Kill;
        actor_data.HandleUnhide += Unhide;

        actors.Add(actor.GetComponent<Actor>());
    }

    void PasteSprite(int x, int y, Texture2D source)
    {
       for (int target_y = y * 32; target_y < y * 32 + source.height; target_y++)
        {
            for (int target_x = x * 32; target_x < x * 32 + source.width; target_x++) //Goes through each pixel
            {
                Color color = source.GetPixel(target_x - x * 32, target_y - y * 32);
      
                if (source.GetPixel(target_x - x * 32, target_y - y * 32).a == 1)
                    atlas_texture.SetPixel(target_x, target_y, color);
            }
        }
    }

    public void Kill(ActorData actor_data)
    {
        if (actor_data is PlayerData)
            return;
            
        Actor destroyed_actor = null;
        foreach(Actor actor in actors)
        {
            if (actor.actor_data == actor_data)
            {
                actor.HandleKill();                
                destroyed_actor = actor;
            }
        }
        if (destroyed_actor)
        {
            actors.Remove(destroyed_actor);
            destroyed_actor.actor_data.HandleKill -= Kill;
            destroyed_actor.actor_data.HandleUnhide -= Unhide;
        }
    }

    void Unhide(ActorData actor_data)
    {
        //Needs to be called in Map class because actor script is deactivated!

        GameLogger.Log("The Player spots a " + actor_data.prototype.name.ToLower() + ".");
          
        actors.Find(x => x.actor_data == actor_data).UpdateVisibility(GameObject.Find("GameData").GetComponent<GameData>().current_map);
    }

    public void Kill(ItemData item_data)
    {
        //Currently has to be manually called when an item has been removed from the map
        Item destroyed_item = null;
        foreach (Item item in items)
        {
            if (item.item_data == item_data)
            {
                Destroy(item.gameObject);
                destroyed_item = item;
            }
        }
        if (destroyed_item)
        {
            items.Remove(destroyed_item);
        }
    }

    void OnDestroy()
    {
        map_data.CreateActor -= CreateActor;
        map_data.CreateItem -= CreateItem;

        foreach (Actor actor in actors)
        {
            actor.actor_data.HandleKill -= Kill;
        }
    }

    public void SpeedUpActorVisualActions()
    {
        foreach (Actor actor in actors)
        {
            actor.SpeedUpVisualActions();
        }
    }
}

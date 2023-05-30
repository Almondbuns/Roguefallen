using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

class AStarNode
{
    public int x;
    public int y;

    public AStarNode previous_node;

    public int cost_current;
    public int cost_guessed;

    public int cost_unique;
}

public class Path
{
    public List<(int x, int y, int cost)> path;
    int current_target;

    public Path()
    {
        path = new ();
    }

    public (int x, int y, int cost)? GetCurrentTarget()
    {
        if (current_target >= path.Count)
        return null;

        return path[current_target];
    }

    public void SetNextTarget()
    {
        ++current_target;
    }
}

class Algorithms
{
    public static Path AStar(MapData map_data, (int x, int y) start, (int x, int y) end, bool ignore_blocked_target = false, bool use_blocked_tiles_with_cost = false, ActorData actor = null)
    {
        var open_list = new Dictionary<int,AStarNode>();
        var closed_list = new Dictionary<int, AStarNode>();

        open_list.Add(start.x + start.y * map_data.tiles.GetLength(0), new AStarNode
        {
            x = start.x,
            y = start.y,
            previous_node = null,
            cost_current = 0,
            cost_guessed = 100 * Mathf.Max(Mathf.Abs(end.x - start.x), Mathf.Abs(end.y - start.y))
        });

        do
        {
            AStarNode current_node = null;
            int cost_min = int.MaxValue;

            //Select node from open_list with mininum cost
            foreach(KeyValuePair<int,AStarNode> kvp in open_list)
            {
                if (kvp.Value.cost_current + kvp.Value.cost_guessed < cost_min)
                {
                    cost_min = kvp.Value.cost_current + kvp.Value.cost_guessed;
                    current_node = kvp.Value;
                }
            }
            open_list.Remove(current_node.x + current_node.y * map_data.tiles.GetLength(0));

            if (current_node.x == end.x && current_node.y == end.y)
            {
                //We are finished. Create Path.
                Path path = new Path();
                AStarNode traverse_node = current_node;
                while (traverse_node.previous_node != null)
                {
                    path.path.Add((traverse_node.x, traverse_node.y, traverse_node.cost_current));
                    traverse_node = traverse_node.previous_node;
                }
                // Path should start with origin
                path.path.Reverse();

                return path;
            }

            closed_list.Add(current_node.x + current_node.y * map_data.tiles.GetLength(0), current_node);

            for (int i = -1; i <= 1; ++i)
            {
                for (int j = -1; j <= 1; ++j)
                {
                    if (i == 0 && j == 0) continue;
                    if (current_node.x + i < 0 || current_node.x + i >= map_data.tiles.GetLength(0)) continue;
                    if (current_node.y + j < 0 || current_node.y + j >= map_data.tiles.GetLength(1)) continue;

                    int successor_x = current_node.x + i;
                    int successor_y = current_node.y + j;

                    int transition_cost = 0;

                    bool move_accessable = true;
                    if (actor == null || (actor.prototype.tile_width == 1 && actor.prototype.tile_height == 1))
                    {
                        move_accessable = map_data.IsAccessableTile(successor_x, successor_y, true);
                    }
                    else
                    {
                        for (int w = successor_x; w <= successor_x + actor.prototype.tile_width - 1; ++w)
                        {
                            for (int h = successor_y; h <= successor_y + actor.prototype.tile_height - 1; ++h)
                            {
                                bool result = map_data.IsAccessableTile(w, h, true, actor);
                                if (result == false)
                                {
                                    move_accessable = false;
                                }
                            }
                        }
                    }

                    if (move_accessable == false)
                    {
                        if (ignore_blocked_target == false && successor_x == end.x && successor_y == end.y)
                        {

                        }
                        else
                        {
                            if (use_blocked_tiles_with_cost == true)
                            {
                                transition_cost = 1000000;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }

                    if (closed_list.ContainsKey(successor_x + successor_y * map_data.tiles.GetLength(0)))
                        continue;

                    if (i == 0 || j == 0)
                        transition_cost += 100;
                    else
                        transition_cost += 110; // avoid diagonal "dances" because movement costs are the same

                    int calculated_cost = current_node.cost_current + transition_cost;

                    AStarNode successor_node;
                    bool successor_found = true;
                    try
                    {
                        successor_node = open_list[successor_x + successor_y * map_data.tiles.GetLength(0)];
                    }
                    catch (KeyNotFoundException)
                    {
                        successor_found = false;
                        successor_node = new AStarNode
                        {
                            x = successor_x,
                            y = successor_y,
                        };
                    }

                    if (successor_found && 
                        calculated_cost > open_list[successor_x + successor_y * map_data.tiles.GetLength(0)].cost_current)
                        continue;

                    successor_node.previous_node = current_node;
                    successor_node.cost_current = calculated_cost;
                    successor_node.cost_guessed = 
                        100 * Mathf.Max(Mathf.Abs(end.x - successor_node.x), Mathf.Abs(end.y - successor_node.y));
                    if (successor_found == false)
                    {
                        open_list.Add(successor_x + successor_y * map_data.tiles.GetLength(0), successor_node);
                    }
                }
            }
        }
        while (open_list.Count > 0);

        return null;
    }

    /*
    public static Path AStar(MapData[,] maps, (int x, int y) start, (int x, int y) end)
    {
        var open_list = new Dictionary<int,AStarNode>();
        var closed_list = new Dictionary<int, AStarNode>();

        open_list.Add(start.x + start.y * maps.GetLength(0), new AStarNode
        {
            x = start.x,
            y = start.y,
            previous_node = null,
            cost_current = 0,
            cost_guessed = Mathf.Abs(end.x - start.x) + Mathf.Abs(end.y - start.y)
        });

        do
        {
            AStarNode current_node = null;
            int cost_min = int.MaxValue;

            //Select node from open_list with mininum cost
            foreach(KeyValuePair<int,AStarNode> kvp in open_list)
            {
                if (kvp.Value.cost_current + kvp.Value.cost_guessed < cost_min)
                {
                    cost_min = kvp.Value.cost_current + kvp.Value.cost_guessed;
                    current_node = kvp.Value;
                }
            }
            open_list.Remove(current_node.x + current_node.y * maps.GetLength(0));

            if (current_node.x == end.x && current_node.y == end.y)
            {
                //We are finished. Create Path.
                Path path = new Path();
                AStarNode traverse_node = current_node;
                while (traverse_node.previous_node != null)
                {
                    path.path.Add((traverse_node.x, traverse_node.y, traverse_node.cost_unique));
                    traverse_node = traverse_node.previous_node;
                }
                // Path should start with origin
                path.path.Reverse();

                return path;
            }

            closed_list.Add(current_node.x + current_node.y * maps.GetLength(0), current_node);

            for (int i = -1; i <= 1; ++i)
            {
                for (int j = -1; j <= 1; ++j)
                {
                    if (i == 0 && j == 0) continue;
                    if (i != 0 && j != 0) continue; // no diagonal connections
        
                    if (current_node.x + i < 0 || current_node.x + i >= maps.GetLength(0)) continue;
                    if (current_node.y + j < 0 || current_node.y + j >= maps.GetLength(1)) continue;

                    int successor_x = current_node.x + i;
                    int successor_y = current_node.y + j;

                    if (closed_list.ContainsKey(successor_x + successor_y * maps.GetLength(0)))
                        continue;

                    int transition_cost;
                   
                    transition_cost = 1;
                    int max_cost = maps.GetLength(0) * maps.GetLength(1) + 1;

                    if (i == -1 && j == 0 && maps[current_node.x, current_node.y].exit_left == false)
                        transition_cost = max_cost;

                    if (i == 1 && j == 0 && maps[current_node.x, current_node.y].exit_right == false)
                        transition_cost = max_cost;

                    if (i == 0 && j == -1 && maps[current_node.x, current_node.y].exit_down == false)
                        transition_cost = max_cost;

                    if (i == 0 && j == 1 && maps[current_node.x, current_node.y].exit_up == false)
                        transition_cost = max_cost;

                    int calculated_cost = current_node.cost_current + transition_cost;

                    AStarNode successor_node;
                    bool successor_found = true;
                    try
                    {
                        successor_node = open_list[successor_x + successor_y * maps.GetLength(0)];
                    }
                    catch (KeyNotFoundException)
                    {
                        successor_found = false;
                        successor_node = new AStarNode
                        {
                            x = successor_x,
                            y = successor_y,
                        };
                    }

                    if (successor_found && 
                        calculated_cost > open_list[successor_x + successor_y * maps.GetLength(0)].cost_current)
                        continue;

                    successor_node.previous_node = current_node;
                    successor_node.cost_current = calculated_cost;
                    successor_node.cost_guessed = 
                        Mathf.Abs(end.x - successor_node.x) + Mathf.Abs(end.y - successor_node.y);
                    successor_node.cost_unique = transition_cost;

                    if (successor_found == false)
                    {
                        open_list.Add(successor_x + successor_y * maps.GetLength(0), successor_node);
                    }
                }
            }
        }
        while (open_list.Count > 0);

        return null;
    }
    */

    public static List<(int x, int y)> LineofSight((int x, int y) src, (int x, int y) target)
    {
        List<(int x, int y)> path = new();

        if (Mathf.Abs(target.x - src.x) >= Mathf.Abs(target.y - src.y))
        {
            float m = (target.y - src.y) / (float)(target.x - src.x);
            double epsilon = Mathf.Abs(m) - 1;

            int j = src.y;
            if (target.x >= src.x)
            {
                for (int i = src.x; i < target.x; ++i)
                {
                    path.Add((i, j));
                    if (epsilon >= 0)
                    {
                        if (target.y >= src.y)
                            j += 1;
                        else
                            j -= 1;

                        epsilon -= 1.0;
                    }
                    epsilon += Mathf.Abs(m);
                }
            }
            else
            {
                for (int i = src.x; i > target.x; --i)
                {
                    path.Add((i, j));
                    if (epsilon >= 0)
                    {
                        if (target.y >= src.y)
                            j += 1;
                        else
                            j -= 1;
                        epsilon -= 1.0;
                    }
                    epsilon += Mathf.Abs(m);
                }
            }
        }
        else
        {
            float m = (target.x - src.x) / (float)(target.y - src.y);
            double epsilon = Mathf.Abs(m) - 1;

            int i = src.x;
            if (target.y >= src.y)
            {
                for (int j = src.y; j < target.y; ++j)
                {
                    path.Add((i, j));
                    if (epsilon >= 0)
                    {
                        if (target.x >= src.x)
                            i += 1;
                        else
                            i -= 1;

                        epsilon -= 1.0;
                    }
                    epsilon += Mathf.Abs(m);
                }
            }
            else
            {
                for (int j = src.y; j > target.y; --j)
                {
                    path.Add((i, j));
                    if (epsilon >= 0)
                    {
                        if (target.x >= src.x)
                            i += 1;
                        else
                            i -= 1;
                        epsilon -= 1.0;
                    }
                    epsilon += Mathf.Abs(m);
                }
            }
        }

        path.Add(target);

        return path;
    }
}

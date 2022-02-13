using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SearchTree 
{
    public Vector3 initial;
    public Vector3 goal;
    public CellMatrix domain;
    //public List<SearchNode> open_nodes;
    public LinkedList<SearchNode> open_nodes;
    public SearchNode solution;
    public int nodes_explored;
    public int max_depth = 2000;
    public Dictionary<Vector3, int> positions_done;
    private List<SearchNode> new_node_list;
    public SearchNode best_node; 
    public bool search_complete;
    public List<SearchNode> dead_end_node_list;

    public SearchTree(Vector3 initial, Vector3 goal, CellMatrix domain) {
        this.initial = initial;
        this.goal = goal;
        this.domain = domain;
        Cell initial_cell = domain.GetCell((int)initial.x, (int)initial.y);
        SearchNode root = new SearchNode(initial, null, 0, 0, Heuristic(initial_cell), new Move(initial_cell));
        open_nodes = new LinkedList<SearchNode>();
        open_nodes.AddFirst(root);
        solution = null;
        this.nodes_explored = 0;
        best_node = root;
        search_complete = false;
        this.positions_done = new Dictionary<Vector3, int>();
        positions_done.Add(initial, 0);
        new_node_list = new List<SearchNode>();
        dead_end_node_list = new List<SearchNode>();
    }

    public IEnumerator search() {
        UnityEngine.Debug.Log("start search");
        Stopwatch stopwatch = new Stopwatch();
        float frame_time = 0f;
        bool new_frame = true;
        int frame_count = 0;
        while (open_nodes.Count > 0 && solution is null) {
            if (new_frame) {
                stopwatch.Restart();
                frame_count++;
                frame_time = Time.deltaTime;
                new_frame = false;
            }
            SearchNode node = open_nodes.First.Value;
            open_nodes.RemoveFirst();
            nodes_explored++;
            if (node.heuristic < best_node.heuristic) {
                best_node = node;
            }
            //Debug.Log($"STATS {nodes_explored},{open_nodes.Count},{node.depth},{node.heuristic}");
            if (node.playerPosition == goal) {
                UnityEngine.Debug.Log(frame_count);
                solution = node;
                search_complete = true;
                yield return null;
                break;
            }
            DefineNewNodes(node);
            if (new_node_list.Count > 0)
            {
                new_node_list.Sort((n1, n2) => n1.heuristic.CompareTo(n2.heuristic));
                //open_nodes.AddRange(new_node_list);
                SortNodes();
            }
            else {
                node.is_dead_end = true;
                dead_end_node_list.Add(node);
            }
            //UnityEngine.Debug.Log(stopwatch.ElapsedMilliseconds);
            if (stopwatch.ElapsedMilliseconds > (frame_time/2)*1000) {
                UnityEngine.Debug.Log(frame_time);
                UnityEngine.Debug.Log(stopwatch.ElapsedMilliseconds);
                new_frame = true;
                yield return null;
            }
        }

        UnityEngine.Debug.Log("end search");
        search_complete = true;
        yield return null;
    }

    public void SortNodes()
    {
        var linked_node = open_nodes.First;
        int i = 0;
        while (!(linked_node is null) && i < new_node_list.Count) {
            if (new_node_list[i].heuristic - linked_node.Value.heuristic <= 0) {
                open_nodes.AddBefore(linked_node, new_node_list[i]);
                i++;
            }
            linked_node = linked_node.Next;
        }

        // Add remaining nodes 
        for (int j = i; j < new_node_list.Count; j++) {
            open_nodes.AddLast(new_node_list[j]);
        }
    }

    public List<Command> GetCommandSolution() {
        List<Command> ret = new List<Command>();
        if (solution is null) {
            return ret;
        }
        foreach (SearchNode node in solution.GetParents()) {
            ret.Add(node.action);
        }
        return ret;
    }

    public void DefineNewNodes(SearchNode node) {
        new_node_list.Clear();
        Cell original_cell = domain.GetCell((int)node.playerPosition.x, (int)node.playerPosition.y);
        Cell cell1 = domain.GetCell((int)node.playerPosition.x + 1, (int)node.playerPosition.y);
        Cell cell2 = domain.GetCell((int)node.playerPosition.x - 1, (int)node.playerPosition.y);
        Cell cell3 = domain.GetCell((int)node.playerPosition.x, (int)node.playerPosition.y + 1);
        Cell cell4 = domain.GetCell((int)node.playerPosition.x, (int)node.playerPosition.y - 1);
        CheckAndAddNode(node, original_cell, cell1);
        CheckAndAddNode(node, original_cell, cell2);
        CheckAndAddNode(node, original_cell, cell3);
        CheckAndAddNode(node, original_cell, cell4);
    }

    public void CheckAndAddNode(SearchNode node, Cell original_cell, Cell cell) {
        if (!(cell is null) &&
            !(node.PositionInParent(cell.getVisualHeightPosition())) &&
            CanWalkToCell(original_cell, cell) &&
            CheckPosition(node, cell.getVisualHeightPosition()))
        {
            new_node_list.Add(new SearchNode(cell.getVisualHeightPosition(), node, node.depth + 1, node.cost + 1, Heuristic(cell), new Move(cell)));
        }

    }

    public bool CheckPosition(SearchNode node, Vector3 position) {
        int old_depth = -1;
        if (!positions_done.TryGetValue(position, out old_depth)) {
            positions_done.Add(position, node.depth + 1);
            return true;
        }
        if (old_depth >= node.depth+1) {
            positions_done[position] = node.depth + 1;
            return true;
        }
        return false;
    }

    public static bool CanWalkToCell(Cell src, Cell dest)
    {
        if (!(src is null) && !(dest is null) && src.IsWalkable() && dest.IsWalkable())
        {
            if (src is GroundCell)
            {
                if (dest is GroundCell)
                {
                    if (src.getHeight() != dest.getHeight())
                    {
                        return false;
                    }
                    Vector2 srcPos = src.getVisualPosition();
                    Vector2 destPos = dest.getVisualPosition();
                    int result = (int)(Math.Abs(srcPos.x - destPos.x) + Math.Abs(srcPos.y - destPos.y));
                    return result == 1;
                }
                else if (dest is RampCell)
                {
                    Vector3 srcHeightPos = src.getVisualHeightPosition();
                    foreach (Vector3 pos in dest.getPossiblePositions())
                    {
                        if (pos == srcHeightPos)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            else if (src is RampCell)
            {
                Vector3 destHeightPos = dest.getVisualHeightPosition();
                foreach (Vector3 pos in src.getPossiblePositions())
                {
                    if (pos == destHeightPos)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        return false;
    }

    public double Heuristic(Cell cell) {
        return Vector3.Distance(cell.getVisualHeightPosition(), this.goal);
    }

    /* Test
    Cell cell1 = new GroundCell(0, 0, 0);
    Cell cell2 = new GroundCell(0, 1, 0);
    Cell cell3 = new GroundCell(1, 1, 0);
    Cell cell4 = new GroundCell(1, 3, 1);
    Cell cell5 = new RampCell(1, 2, 1, "Up");
    Debug.Log(CanWalkToCell(cell1, cell2)); // True
    Debug.Log(CanWalkToCell(cell1, cell3)); // False
    Debug.Log(CanWalkToCell(cell1, cell4)); // False
    Debug.Log(CanWalkToCell(cell1, cell5)); // True
    Debug.Log(CanWalkToCell(cell5, cell4)); // True
    */

}

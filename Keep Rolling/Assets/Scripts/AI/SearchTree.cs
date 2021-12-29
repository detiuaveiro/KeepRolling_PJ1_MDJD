using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SearchTree 
{
    public Vector3 initial;
    public Vector3 goal;
    public CellMatrix domain;
    public List<SearchNode> open_nodes;
    public SearchNode solution;
    public int nodes_explored;

    public SearchTree(Vector3 initial, Vector3 goal, CellMatrix domain) {
        this.initial = initial;
        this.goal = goal;
        this.domain = domain;
        SearchNode root = new SearchNode(initial, null, 0, 0, 0, new Move(domain.GetCell((int)initial.x,(int)initial.y)));
        Debug.Log(root);
        open_nodes = new List<SearchNode> { root };
        solution = null;
        this.nodes_explored = 0;
    }

    public IEnumerator search() {
        while (open_nodes.Count > 0 && solution is null) {
            SearchNode node = open_nodes[0];
            open_nodes.RemoveAt(0);
            nodes_explored++;
            Debug.Log($"STATS {nodes_explored},{open_nodes.Count},{node.depth},{node.heuristic}");
            if (node.playerPosition == goal) {
                solution = node;
                yield return null;
            }
            List<SearchNode> new_nodes = DefineNewNodes(node);
            open_nodes.AddRange(new_nodes);
            open_nodes.Sort((n1,n2) => (n1.heuristic+n1.cost).CompareTo(n2.heuristic+n2.cost));
            yield return null;
        }
        Debug.Log("end");
        yield return null;
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

    public List<SearchNode> DefineNewNodes(SearchNode node) {
        List<SearchNode> nodeList = new List<SearchNode>();
        Cell original_cell = domain.GetCell((int)node.playerPosition.x, (int)node.playerPosition.y);
        Debug.Log($"BANANA {original_cell}");
        Cell cell1 = domain.GetCell((int)node.playerPosition.x + 1, (int)node.playerPosition.y);
        Cell cell2 = domain.GetCell((int)node.playerPosition.x - 1, (int)node.playerPosition.y);
        Cell cell3 = domain.GetCell((int)node.playerPosition.x, (int)node.playerPosition.y + 1);
        Cell cell4 = domain.GetCell((int)node.playerPosition.x, (int)node.playerPosition.y - 1);
        /*
        Debug.Log($"cell1 {original_cell}");
        Debug.Log($"cell2 {original_cell}");
        Debug.Log($"cell3 {original_cell}");
        Debug.Log($"cell4 {original_cell}");
        */
        List<Cell> cellList = new List<Cell>() {cell1, cell2, cell3, cell4};
        foreach (Cell cell in cellList) {
            //if (node.depth + 1 < 40 && CanWalkToCell(cell, original_cell)) {
            if (node.depth+1 < 20 && CanWalkToCell(original_cell, cell) && !(node.PositionInParent(cell.getVisualHeightPosition()))) {
                Debug.Log($"AQUI {cell}");
                SearchNode new_node = new SearchNode(cell.getVisualHeightPosition(), node, node.depth + 1, node.cost + 1, Heuristic(cell), new Move(cell));
                //TODO: check if it is repeated
                nodeList.Add(new_node);
            }
        }
        return nodeList;
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
                    Debug.Log($"ESTOU AQUI {src},{dest}");
                    Vector3 srcHeightPos = src.getVisualHeightPosition();
                    foreach (Vector3 pos in dest.getPossiblePositions())
                    {
                        if (pos == srcHeightPos)
                        {
                            Debug.Log($"ESTOU AQUI {true}");
                            return true;
                        }
                    }
                    Debug.Log($"ESTOU AQUI {false}");
                    return false;
                }
            }
            else if (src is RampCell)
            {
                Debug.Log($"ESTOU AQUI2 {src},{dest}");
                Vector3 destHeightPos = dest.getVisualHeightPosition();
                foreach (Vector3 pos in src.getPossiblePositions())
                {
                    if (pos == destHeightPos)
                    {
                        Debug.Log($"ESTOU AQUI2 {true}");
                        return true;
                    }
                }
                Debug.Log($"ESTOU AQUI2 {false}");
                return false;
            }
        }
        return false;
    }

    public float Heuristic(Cell cell) {
        Vector3 position = cell.getVisualHeightPosition();
        return Vector3.Distance(position, this.goal);
    }

    private void test() {
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
    }

}

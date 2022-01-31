using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SearchNode
{
    public Vector3 playerPosition;
    public SearchNode parent;
    public int depth;
    public int cost;
    public double heuristic;
    public Command action;
    public bool is_dead_end;

    public SearchNode(Vector3 playerPosition, SearchNode parent, int depth, int cost, double heuristic, Command action) {
        this.playerPosition = playerPosition;
        this.parent = parent;
        this.depth = depth;
        this.cost = cost;
        this.heuristic = heuristic;
        this.action = action;
        this.is_dead_end = false;
    }

    public List<SearchNode> GetParents() {
        if (parent is null) {
            return new List<SearchNode> {this};
        }
        List<SearchNode> ret = parent.GetParents();
        ret.Add(this);
        return ret;
    }

    public bool PositionInParent(Vector3 position) {
        SearchNode node = this;
        while (!(node.parent is null)) {
            if (node.parent.playerPosition == position)
                return true;
            node = node.parent;
        }
        return false;
        /*
        if (parent is null) 
            return false;

        if (parent.playerPosition == position)
            return true;

        return parent.PositionInParent(position);
        */
    }

    public List<Command> GetCommandList()
    {
        List<Command> ret = new List<Command>();
        foreach (SearchNode node in GetParents())
        {
            ret.Add(node.action);
        }
        return ret;
    }

    public static int GetCommonPoint(SearchNode node1, SearchNode node2)
    {
        List<SearchNode> node1_parents = node1.GetParents();
        List<SearchNode> node2_parents = node2.GetParents();
        for (int i = 0; i < Math.Min(node1_parents.Count, node2_parents.Count); i++)
        {
            if (node1_parents[i].playerPosition != node2_parents[i].playerPosition)
            {
                return node1_parents[i].depth - 1;
            }
        }
        return node1.depth <= node2.depth ? node1.depth : node2.depth;
    }

    public override string ToString()
    {
        return $"SearchNode position:{playerPosition} depth:{depth} parent:{parent} ";
    }

}

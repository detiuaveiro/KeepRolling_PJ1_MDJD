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

    public override string ToString()
    {
        return $"SearchNode position:{playerPosition} depth:{depth} parent:{parent} ";
    }

}

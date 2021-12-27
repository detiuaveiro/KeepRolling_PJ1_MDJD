using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchNode
{
    public Vector3 playerPosition;
    public SearchNode parent;
    public int depth;
    public int cost;
    public float heuristic;
    public Command action;

    public SearchNode(Vector3 playerPosition, SearchNode parent, int depth, int cost, float heuristic, Command action) {
        this.playerPosition = playerPosition;
        this.parent = parent;
        this.depth = depth;
        this.cost = cost;
        this.heuristic = heuristic;
        this.action = action;
    }

    public List<SearchNode> GetParents() {
        if (parent is null) {
            return new List<SearchNode> {this};
        }
        List<SearchNode> ret = GetParents();
        ret.Add(this);
        return ret;
    }
}

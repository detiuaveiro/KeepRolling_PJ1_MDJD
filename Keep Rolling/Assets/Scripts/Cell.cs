using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private int x;
    private int y;
    private int height;
    private bool walkable;

    public Cell(int x, int y, int height, bool walkable) {
        this.x = x;
        this.y = y;
        this.height = height;
        this.walkable = walkable;
    }

    public bool IsWalkable() {
        return this.walkable;
    }

    public void OnEnter() { 
    }

    public void OnLeave() {
    }
}

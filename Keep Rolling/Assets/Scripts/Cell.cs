using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    protected int x { get; }
    protected int y { get; }
    protected int height { get; }
    protected bool walkable;
    protected List<Tuple<int,int,int>> possible_positions;

    public Cell(int x, int y, int height, bool walkable) {
        this.x = x;
        this.y = y;
        this.height = height;
        this.walkable = walkable;
        possible_positions = new List<Tuple<int, int,int>>();
    }

    public bool IsWalkable() {
        return this.walkable;
    }

    public bool IsNextPositionValid(int x, int y, int height) {
        return possible_positions.Contains(Tuple.Create(x, y, height));
    }

    public virtual bool CanPlaceOnTop(PieceType pieceType) {
        return false;
    }

    public void OnEnter() { 
    }

    public void OnLeave() {
    }

    public override string ToString()
    {
        return $"Cell x:{x} y:{y}, height:{height}";
    }

    public Vector2 getPosition()
    {
        return new Vector2(x, y);
    }

    public int getX() {
        return x;
    }
    public int getY()
    {
        return y;
    }
    public int getHeight()
    {
        return height;
    }

}

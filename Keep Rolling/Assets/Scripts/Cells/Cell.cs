using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    protected int x { get; }
    protected int y { get; }
    protected int visual_x { get; }
    protected int visual_y { get; }
    protected int height { get; }
    protected bool walkable;
    protected Vector3 visualHeightPosition;

    public Cell(int x, int y, int height, bool walkable) {
        this.x = x;
        this.y = y;
        this.height = height;
        this.walkable = walkable;
        this.visual_x = x - height;
        this.visual_y = y - height;
        this.visualHeightPosition = new Vector3(visual_x, visual_y, height);

    }

    public bool IsWalkable() {
        return this.walkable;
    }

    public virtual bool CanPlaceOnTop(PieceType pieceType) {
        return false;
    }

    public override string ToString()
    {
        return $"Cell x:{x} y:{y}, height:{height}";
    }

    public virtual List<Vector3> getPossiblePositions() {
        return new List<Vector3>();
    }

    public Vector2 getPosition()
    {
        return new Vector2(x, y);
    }

    public Vector2 getVisualPosition()
    {
        return new Vector2(visual_x, visual_y);
    }

    public Vector3 getVisualHeightPosition() {
        return visualHeightPosition;
    }

    public int getX()
    {
        return x;
    }

    public int getY()
    {
        return y;
    }

    public int getVisualX()
    {
        return visual_x;
    }

    public int getVisualY()
    {
        return visual_y;
    }

    public int getHeight()
    {
        return height;
    }

}

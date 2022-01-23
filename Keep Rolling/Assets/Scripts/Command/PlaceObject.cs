using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceObject : Command
{
    private Sprite sprite;
    private Cell cell;
    private int height;
    private Direction direction;
    private Piece piece;
    private TileBase oldTile;
    private Cell oldCell;

    public PlaceObject(Sprite sprite, Cell cell, int height, Direction direction, Piece piece)
    {
        this.sprite = sprite;
        this.cell = cell;
        this.height = height;
        this.direction = direction;
        this.piece = piece;
    }
    public override void Execute()
    {
        IsometricRuleTile newTile = new IsometricRuleTile();
        Cell new_cell;
        switch (this.piece.type)
        {
            case (PieceType.Ramp):
                {
                    newTile.m_DefaultSprite = sprite;
                    oldTile = MapManager.instance.tilemaps[cell.getHeight() + height + 1].GetTile(new Vector3Int(cell.getX() + height, cell.getY() + height, 0));
                    MapManager.instance.tilemaps[cell.getHeight() + height + 1].SetTile(new Vector3Int(cell.getX() + height, cell.getY() + height, 0), newTile);
                    new_cell = new RampCell(cell.getX() + 1 + height, cell.getY() + 1 + height, cell.getHeight() + height + 1, direction);
                    MapManager.instance.cell_matrix.AddCell(new_cell);
                    break;
                }
            case (PieceType.FixGround):
                {
                    newTile = piece.tile;
                    oldTile = MapManager.instance.tilemaps[cell.getHeight() + height].GetTile(new Vector3Int(cell.getVisualX() + height, cell.getVisualY() + height, 0));
                    MapManager.instance.tilemaps[cell.getHeight() + height].SetTile(new Vector3Int(cell.getVisualX() + height, cell.getVisualY() + height, 0), newTile);
                    new_cell = new GroundCell(cell.getX() + height, cell.getY() + height, cell.getHeight() + height);
                    MapManager.instance.cell_matrix.AddCell(new_cell);
                    break;
                }
        }

    }

    public override void Undo()
    {
        switch (this.piece.type)
        {
            case (PieceType.Ramp):
                {
                    MapManager.instance.tilemaps[cell.getHeight() + height + 1].SetTile(new Vector3Int(cell.getX() + height, cell.getY() + height, 0), oldTile);
                    break;
                }
            case (PieceType.FixGround):
                {
                    MapManager.instance.tilemaps[cell.getHeight() + height].SetTile(new Vector3Int(cell.getVisualX() + height, cell.getVisualY() + height, 0), oldTile);
                    break;
                }
        }
        MapManager.instance.cell_matrix.AddCell(cell);
        LevelManager.instance.RefoundPiece(piece);
    }
}
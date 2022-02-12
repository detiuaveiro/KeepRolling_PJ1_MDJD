using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObstacleSelected : MonoBehaviour
{
    public Tile tile;
    public Grid grid;
    private Cell lastSnappedCell;
    public Piece piece;
    private SpriteRenderer spr;
    private int currentSprite = 0;
    private Sprite[] sprites;
    private void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        spr = this.gameObject.GetComponent<SpriteRenderer>();
        sprites = piece.sprites;
    }

    private void ChangeSprite()
    {
        currentSprite = (currentSprite + 1) % sprites.Length;
        spr.sprite = sprites[currentSprite];
    }

    void Update()
    {
        if (Input.GetButtonDown("Rotate Objects"))
        {
            ChangeSprite();
        }
        if (Input.GetButtonDown("Deselect Object"))
        {
            Destroy(this.gameObject);
        }
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);

        lastSnappedCell = null;
        var result = MapManager.instance.GetLastSnappedCell(transform.position, piece);
        lastSnappedCell = result.Item1;
        int offset = result.Item2;

        if (lastSnappedCell != null)
        {
            int x = lastSnappedCell.getVisualX() + offset;
            int y = lastSnappedCell.getVisualY() + offset;
            Vector3 place = Vector3.zero;
            switch (piece.type) {
                case PieceType.Ramp:
                    if (lastSnappedCell.getHeight() + 1 >= MapManager.instance.tilemaps.Count) {
                        return;
                    }
                    place = MapManager.instance.tilemaps[lastSnappedCell.getHeight() + 1].GetCellCenterWorld(new Vector3Int(x, y, 0));
                    break;
                case PieceType.FixGround:
                    place = MapManager.instance.tilemaps[lastSnappedCell.getHeight()].GetCellCenterWorld(new Vector3Int(x, y, 0));
                    break;
            }


            transform.position = place;
            if (MapManager.instance.CanPlaceTile(lastSnappedCell, piece, offset))
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.2f, 1, 0.2f, 0.75f);
            } else
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.2f, 0.2f, 0.75f);
            }
            if (Input.GetButton("Fire1"))
            {
                if (MapManager.instance.PlaceTile(lastSnappedCell, sprites[currentSprite], offset, piece, (Direction)currentSprite)) {
                    Destroy(this.gameObject);
                }
            }
        } else
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.75f);
        }


    }
}

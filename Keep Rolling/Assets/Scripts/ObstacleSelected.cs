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
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);

        lastSnappedCell = null;
        var result = MapManager.instance.GetLastSnappedCell(transform.position, piece);
        lastSnappedCell = result.Item1;
        int offset = result.Item2;

        if (lastSnappedCell != null)
        {
            int x = lastSnappedCell.getX() + offset;
            int y = lastSnappedCell.getY() + offset;
            Vector3 place = MapManager.instance.tilemaps[0].GetCellCenterWorld(new Vector3Int(x,y,0));
            
            transform.position = place;
            if (MapManager.instance.CanPlaceTile(lastSnappedCell, piece,offset))
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.2f, 1, 0.2f, 0.75f);
            } else
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.2f, 0.2f, 0.75f);
            }
            if (Input.GetButton("Fire1"))
            {
                if (MapManager.instance.PlaceTile(lastSnappedCell, sprites[currentSprite], offset,piece)) {
                    Destroy(this.gameObject);
                }
            }
        } else
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.75f);
        }

        if (Input.GetButtonDown("Rotate Objects"))
        {
            ChangeSprite();
        }
    }
}

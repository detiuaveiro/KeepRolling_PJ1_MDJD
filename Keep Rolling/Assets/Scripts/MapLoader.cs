using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public class MapLoader : MonoBehaviour
{
    [System.Serializable]
    private class DefaultTileAssociation
    {
        public string tile;
        public string defaultTile;
    }
    [System.Serializable]
    private class DefaultTileAssociations
    {
        public DefaultTileAssociation[] associations;
    }

    [System.Serializable]
    private class TileColor
    {
        public float R;
        public float G;
        public float B;

        public bool Equals(TileColor other)
        {
            if (this.G.Equals(other.G) && this.B.Equals(other.B))
            {
                return true;
            }
            return false;
        }
    }
    [System.Serializable]
    private class Tile
    {
        public TileColor color;
        public string tileName;
        public string cellType;
    }
    [System.Serializable]
    private class Tiles
    {
        public Tile[] tiles;
    }

    private static Tile getTileFromColor(Color32 color, Tiles tilesData)
    {
        TileColor newColor = new TileColor()
        {
            R = color.r,
            G = color.g,
            B = color.b
        };
        
        for (int i=0;i< tilesData.tiles.Length; i++)
        {
            if (tilesData.tiles[i].color.Equals(newColor))
            {
                return tilesData.tiles[i];
            }
        }
        return null;
    }
    /**
     * Função calcular nivel tilemap = R / 10
     * 
     *
     */
    public static List<Cell> loadLevel(int level,List<Tilemap> tilemaps,Tilemap baseTileMap)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Tiles/tileAssociaton");
        Tiles tilesData = JsonUtility.FromJson<Tiles>(jsonFile.text);
        List<Cell> cellList = new List<Cell>();

        TextAsset jsonFileDefaultAssociation = Resources.Load<TextAsset>("Tiles/tileDefaultGroundTile");
        DefaultTileAssociations defaulTtilesData = JsonUtility.FromJson<DefaultTileAssociations>(jsonFileDefaultAssociation.text);
        Debug.Log("loading level " + level);
        Texture2D image = (Texture2D)Resources.Load("Levels/Level"+ level);
        for (int i = 0; i < image.width; i++)
        {
            for (int j = 0; j < image.height; j++)
            {
                Color32 pixel = image.GetPixel(i, j);
                Tile tile = getTileFromColor(pixel, tilesData);
                if (tile != null)
                {
                    float division = pixel.r / 10.0f;
                    int height = (int)division;
                    if (pixel.g == 14)
                    {
                        GameObject dinamico = Resources.Load<GameObject>("Tiles/" + tile.tileName);
                        Vector3 position = tilemaps[height].CellToWorld(new Vector3Int(j, image.width - i + 1, 0));
                        Instantiate(dinamico, position, new Quaternion(0f,0f,0f,0f));
                        if (tilemaps[height] != null)
                        {
                            for (int k = 0; k < defaulTtilesData.associations.Length; k++)
                            {
                                if (defaulTtilesData.associations[k].tile.Equals(tile.tileName))
                                {
                                    IsometricRuleTile defaultTile = Resources.Load<IsometricRuleTile>("Tiles/" + defaulTtilesData.associations[k].defaultTile);
                                    tilemaps[height - 1].SetTile(new Vector3Int(j, image.width - i + 1, 0), defaultTile);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        IsometricRuleTile newTile = Resources.Load<IsometricRuleTile>("Tiles/" + tile.tileName);
                        if (newTile != null)
                        {
                            if (tilemaps[height] != null)
                            {
                                tilemaps[height].SetTile(new Vector3Int(j, image.width - i + 1, 0), newTile);
                                Cell cell = CellFactory.CreateCellFromCellName(j + height, image.width - i + 1 + height, height, tile.cellType);
                                if (!(cell is null))
                                    cellList.Add(cell);
                                for (int k = 0; k < defaulTtilesData.associations.Length; k++)
                                {
                                    if (defaulTtilesData.associations[k].tile.Equals(tile.tileName))
                                    {
                                        IsometricRuleTile defaultTile = Resources.Load<IsometricRuleTile>("Tiles/" + defaulTtilesData.associations[k].defaultTile);
                                        tilemaps[height - 1].SetTile(new Vector3Int(j, image.width - i + 1, 0), defaultTile);
                                        break;
                                    }
                                }

                                int special = (int)((division - Math.Truncate(division)) * 10.0f);
                                if (special == 1)
                                {
                                    MapManager.instance.startPosition = new Vector3Int(j, image.width - i + 1, height);
                                }
                                else if (special == 2)
                                {
                                    MapManager.instance.endPosition = new Vector3Int(j, image.width - i + 1, height);
                                }
                            }
                        }
                    }
                }
            }
        }
        IsometricRuleTile baseTile = Resources.Load<IsometricRuleTile>("Tiles/baseTile");
        for (int i = 0; i < image.width; i++)
        {
            for (int j = 0; j < image.height; j++)
            {
                baseTileMap.SetTile(new Vector3Int(j, image.width - i + 1, 0), baseTile);
            }
        }
        return cellList;
    }
}

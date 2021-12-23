using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;

public class MapLoader 
{
    private static string defaultGroundTile = "asfaltoTile";

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
    }
    [System.Serializable]
    private class Tiles
    {
        public Tile[] tiles;
    }

    private static string getTileFromColor(Color32 color, Tiles tilesData)
    {
        TileColor newColor = new()
        {
            R = color.r,
            G = color.g,
            B = color.b
        };
        
        for (int i=0;i< tilesData.tiles.Length; i++)
        {
            if (tilesData.tiles[i].color.Equals(newColor))
            {
                return tilesData.tiles[i].tileName;
            }
        }
        return null;
    }
    /**
     * Função calcular nivel tilemap = R / 10
     * 
     *
     */
    public static void loadLevel(List<Tilemap> tilemaps)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Tiles/tileAssociaton");
        Tiles tilesData = JsonUtility.FromJson<Tiles>(jsonFile.text);

        //IsometricRuleTile defaultGroundTile = Resources.Load<IsometricRuleTile>("Tiles/" + MapLoader.defaultGroundTile);

        Texture2D image = (Texture2D)Resources.Load("Levels/Level1");
        for (int i = 0; i < image.width; i++)
        {
            for (int j = 0; j < image.height; j++)
            {
                Color32 pixel = image.GetPixel(i, j);
                string tile = getTileFromColor(pixel, tilesData);
                if (tile != null)
                {
                    IsometricRuleTile newTile = Resources.Load<IsometricRuleTile>("Tiles/" + tile);
                    if (newTile != null)
                    {
                        float division = pixel.r / 10.0f;
                        int height = (int)division;
                        if (tilemaps[height] != null)
                        {
                            tilemaps[height].SetTile(new Vector3Int( j  , image.width - i +1, 0), newTile);
                            /*for (int k =0; k < height; k++) {
                                tilemaps[k].SetTile(new Vector3Int(j + k, image.width - i +k , 0), defaultGroundTile);
                            }*/
                            int special = (int)((division - Math.Truncate(division)) * 10.0f);
                            if (special == 1)
                            {
                                MapManager.instance.startPosition = new Vector3Int(j, image.width - i + 1, height);
                            } else if (special == 2)
                            {
                                MapManager.instance.endPosition = new Vector3Int(j, image.width - i + 1, height);
                            }
                        }
                    }
                }
            }
        }
    }
}

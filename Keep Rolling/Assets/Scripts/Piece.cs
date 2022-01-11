using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Piece : MonoBehaviour
{
    public PieceType type;
    public GameObject prefab;
    public IsometricRuleTile tile;
    public Sprite[] sprites;
    public int price;
    public Text label;

    private void Start()
    {
        label.text = price + "$";
    }
}

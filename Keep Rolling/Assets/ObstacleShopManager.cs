using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleShopManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void obstacleCliked(Piece obstacle)
    {
        GameObject newTile = Instantiate(obstacle.prefab, this.transform);
        newTile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1,0.75f);
        newTile.GetComponent<ObstacleSelected>().type = obstacle.type;
    }
}

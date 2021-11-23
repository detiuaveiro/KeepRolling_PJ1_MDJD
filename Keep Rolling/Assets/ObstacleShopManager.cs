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

    public void obstacleCliked(Obstacle obstacle)
    {
        Instantiate(obstacle.prefab, this.transform);
    }
}

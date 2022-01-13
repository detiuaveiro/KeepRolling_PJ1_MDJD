using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleShopManager : MonoBehaviour
{
    public static ObstacleShopManager instance;
    private bool canSelectObstacle = true;
    private GameObject piece;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableSelectingObstacle() {
        canSelectObstacle = false;
        Destroy(piece);
    }

    public void EnableSelectingObstacle()
    {
        canSelectObstacle = true;
    }


    public void obstacleCliked(Piece obstacle)
    {
        if (canSelectObstacle) { 
            if (!(piece is null))
            {
                Destroy(piece);
            }
            piece = Instantiate(obstacle.prefab, this.transform);
            piece.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1,0.75f);
            ObstacleSelected obS = piece.GetComponent<ObstacleSelected>();
            obS.piece = obstacle;
        }
    }
}

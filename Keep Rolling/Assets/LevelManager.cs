using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public Text balanceLabel;
    SearchTree searchTree;
    bool solving = false;
    private int currentBalance;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        instance = this;
    }

    void Update()
    {
        if (solving && searchTree.search_complete) {
            // level complete with success
            if (searchTree.solution != null)
            {
                Debug.Log(searchTree.solution);
                Debug.Log(searchTree.nodes_explored);
                Debug.Log(searchTree.open_nodes.Count);

                foreach (Command move in searchTree.GetCommandSolution())
                {
                    Debug.Log(((Move)move).destination);
                    ChairMovementController.instance.commandQueue.Enqueue((Move)move);
                    // se calhar enviar um último command para saber quando chegou ao fim do nível?
                }
            } // level failed 
            else {
                solving = false;
                Debug.Log("Lost");
                // se calhar enviar comando ?
                // enable control again
            }
        }
    }

    [System.Serializable]
    private class LevelConfig
    {
        public int totalBalance;
    }

    private LevelConfig levelConfig;
    void Start()
    {
        TextAsset jsonLevelConfig = Resources.Load<TextAsset>("Levels/Level" + GameManager.instance.GetCurrentLevel()+"Config");
        levelConfig = JsonUtility.FromJson<LevelConfig>(jsonLevelConfig.text);

        currentBalance = levelConfig.totalBalance;
        UpdateBalance();
    }

    public bool BuyPiece(Piece piece)
    {
        if (currentBalance >= piece.price)
        {
            currentBalance -= piece.price;
            UpdateBalance();
            return true;
        }
        return false;
    }

    public int GetCurrentBalance()
    {
        return currentBalance;
    }

    private void UpdateBalance()
    {
        balanceLabel.text = currentBalance + "$";
    }

    public void StartSimulation() {
        // disable control
        StartCoroutine(SolveLevel());
    }

    private IEnumerator SolveLevel()
    {
        Vector3Int startPosition = MapManager.instance.startPosition;
        Vector3Int endPosition = MapManager.instance.endPosition;
        Debug.Log(startPosition);
        Debug.Log(endPosition);
        ChairMovementController.instance.transform.position = MapManager.instance.tilemaps[startPosition.z].CellToWorld(startPosition);
        searchTree = new SearchTree(startPosition, endPosition, MapManager.instance.cell_matrix);
        solving = true;
        StartCoroutine(searchTree.search());
        yield return null;
    }
}

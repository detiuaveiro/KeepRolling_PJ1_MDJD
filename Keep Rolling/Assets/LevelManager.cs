using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public Text balanceLabel;
    public SearchTree searchTree;
    bool solving = false;
    private int currentBalance;

    public GameObject finalScreen;
    public Text finalScreenLabel;
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
                }
                solving = false;
            } // level failed 
            else {
                solving = false;
                Debug.Log("Lost");

                searchTree.dead_end_node_list.Sort((n1, n2) => n1.heuristic.CompareTo(n2.heuristic));
                SearchNode node1 = searchTree.dead_end_node_list[0];
                var node1_comm_list = node1.GetCommandList();
                foreach (Command move in node1_comm_list)
                {
                    Debug.Log(((Move)move).destination);
                    ChairMovementController.instance.commandQueue.Enqueue((Move)move);
                }
                if (searchTree.dead_end_node_list.Count > 1) { 
                    SearchNode node2 = searchTree.dead_end_node_list[1];
                    int common_point = SearchNode.GetCommonPoint(node1, node2);
                    for (int i = node1_comm_list.Count - 1; i >= common_point; i--) {
                        ChairMovementController.instance.commandQueue.Enqueue((Move)node1_comm_list[i]);
                    }
                    var node2_comm_list = node2.GetCommandList();
                    for (int j = common_point + 1; j < node2_comm_list.Count; j++) {
                        ChairMovementController.instance.commandQueue.Enqueue((Move)node2_comm_list[j]);
                    }
                }


                // enable control again
                ObstacleShopManager.instance.EnableSelectingObstacle();
            }
        }
    }

    public void OnMovementComplete() {
        // won the level
        if (LevelCompleteWithSuccess())
        {
            float score = GenerateScore();
            finalScreenLabel.text = "Level Completed\nScore: " + score;
            finalScreen.SetActive(true);
            //Debug.Log("ganhou:"+ score);
        } // lost the level
        else
        {
            finalScreenLabel.text = "Level Failed";
            finalScreen.SetActive(true);
            //Debug.Log("perdeu");
        }
    }

    public void Retry()
    {
        finalScreen.SetActive(false);
        var startPosition = MapManager.instance.startPosition;
        ChairMovementController.instance.transform.position = MapManager.instance.tilemaps[startPosition.z].CellToWorld(startPosition);
        ChairMovementController.instance.heightLevel = startPosition.z;
        ChairMovementController.instance.RestartMovement();
    }

    public bool LevelCompleteWithSuccess() {
        return !(searchTree.solution is null);
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

    public void RefoundPiece(Piece piece)
    {
        currentBalance += piece.price;
        UpdateBalance();
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
        ObstacleShopManager.instance.DisableSelectingObstacle();
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

    private float GenerateScore()
    {
        List<Command> solution = searchTree.GetCommandSolution();
        float score = ((levelConfig.totalBalance/(levelConfig.totalBalance - currentBalance)) * 100f) + ((searchTree.max_depth - solution.Count) * 300);
        return score;
    }
}

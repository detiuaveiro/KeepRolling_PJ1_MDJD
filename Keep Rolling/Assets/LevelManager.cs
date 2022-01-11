using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public Text balanceLabel;
    private int currentBalance;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        instance = this;
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
}

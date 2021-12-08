using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCreation : MonoBehaviour
{
    [Header("Character Creation Defaults")]
    [SerializeField]
    int[] statValues = new int[(int)PlayerStatType.STATCOUNT];

    [SerializeField]
    int[] statIncrements = new int[(int)PlayerStatType.STATCOUNT];

    [SerializeField]
    int pointsAvailable = 0;
    int pointsUsed = 0;

    [Header("UI Elements")]
    [SerializeField]
    Text[] StatDisplayText = new Text[(int)PlayerStatType.STATCOUNT];

    [SerializeField]
    Text AvailablePointsText;

    List<int>[] modifiers = new List<int>[6];

    private void Start()
    {
        for (int i = 0; i < (int)PlayerStatType.STATCOUNT; i++)
        {
            modifiers[i] = new List<int>();
            statValues[i] = GameStatics._statMinValues[i];
            statIncrements[i] = GameStatics._statIncrementValues[i];
        }
        if (PlayerStats._fileLoaded) LoadFromPlayer();
        else Debug.Log("Loaded Editor default player values");
        DisplayValues();
    }

    private void DisplayValues()
    {
        for (int i = 0; i < (int)PlayerStatType.STATCOUNT; i++)
        {
            int skillModifier = 0;
            foreach (int mod in modifiers[i]) { 
                skillModifier += mod; 
            }
            StatDisplayText[i].text = (statValues[i] + skillModifier).ToString();
        }
        AvailablePointsText.text = pointsAvailable.ToString();
    }

    public void IncrementStat(int type)
    {
        if (pointsAvailable > 0)
        {
            pointsAvailable--;
            pointsUsed++;
            statValues[type]+=(statIncrements[type]);
            if (type == (int)PlayerStatType.DataIntegrity)
            {
                PlayerStats._currentPlayerIntegrity += statIncrements[type];
            }
            DisplayValues();
        }
    }

    public void LoadFromPlayer()
    {
        for (int i = 0; i < (int)PlayerStatType.STATCOUNT; i++)
        {
            statValues[i] = PlayerStats._currentPlayerStats[i];
        }
        pointsAvailable = PlayerStats._playerUpgradePoints;
        Debug.Log($"Loaded from {PlayerStats._currentSaveFileName}");
    }

    public void Save()
    {
        int i = 0;
        foreach (int skill in statValues)
        {
            PlayerStats._currentPlayerStats[i++] = skill;
        }
        PlayerStats._playerUpgradePoints = pointsAvailable;
        Debug.Log("PlayerStats updated");
    }

    public void StartGame(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
}

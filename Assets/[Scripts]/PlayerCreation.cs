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
        }
        if (PlayerStats._loaded) LoadFromPlayer();
        else Debug.Log("Loading Editor Defaults");
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
            modifiers[type].Add(statIncrements[type]);
            DisplayValues();
        }
    }

    public void LoadFromPlayer()
    {
        for (int i = 0; i < (int)PlayerStatType.STATCOUNT; i++)
        {
            statValues[i] = PlayerStats._stats[i];
            foreach(int mod in PlayerStats._modifiers[i]) 
                statValues[i] += mod;
        }
        Debug.Log($"Loaded from {PlayerStats._fileName}");
    }

    public void Save()
    {
        FlushPlayerModifiers();
        int i = 0;
        foreach(int skill in statValues)
        {
            foreach (int mod in modifiers[i]) PlayerStats._modifiers[i].Add(mod);
            PlayerStats._stats[i++] = skill;
        }
        Debug.Log("PlayerStats updated");
    }

    public void StartGame(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
   
    public void Revert()
    {
        pointsAvailable += pointsUsed;
        pointsUsed = 0;
        for (int i = 0; i < modifiers.Length; i++)
        {
            modifiers[i].Clear();
        }
        DisplayValues();
    }

    private void FlushPlayerModifiers()
    {
        for (int i = 0; i < (int)PlayerStatType.STATCOUNT; i++)
        {
            if (PlayerStats._modifiers[i].Count > 0)
            {
                PlayerStats._modifiers[i].Clear();
            }
        }
    }
}

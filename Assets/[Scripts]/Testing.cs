using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class Testing : MonoBehaviour
{
    [SerializeField]
    Text StatDisplay;

    private string saveFileName;

    public void SetFileName(string name)
    {
        saveFileName = name;
    }

    private void Start()
    {
        //Initializes the lists inside the static class
        for (int i = 0; i < (int)PlayerStatType.STATCOUNT; i++)
        {
            PlayerStats._modifiers[i] = new List<int>();
        }
    }

    public void GenerateRandomUserData()
    {
        FlushPlayerData();
        for (int i = 0; i < (int)PlayerStatType.STATCOUNT; i++ )
        {
            PlayerStats._stats[i] = Random.Range(0, 100);
            int randomMods = Random.Range(0, 5);
            for (int j = 0; j < randomMods; j++)
            {
                PlayerStats._modifiers[i].Add(Random.Range(-10, 10));
            }
        }
        PlayerStats._loaded = true;
    }

    public void DisplayUserData()
    {
        string output = "";
        if (GameStatics._fileLoaded)
        {
            for (int i = 0; i < (int)PlayerStatType.STATCOUNT; i++)
            {
                if (i == 0)
                {
                    output += ((PlayerStatType)i).ToString() + $": {GameStatics._currentPlayerIntegrity}/{GameStatics._currentPlayerStats[i]}\n";
                }
                else output += ((PlayerStatType)i).ToString() + $": {GameStatics._currentPlayerStats[i]}\n";
            }
            output += $"Points Available: {GameStatics._playerUpgradePoints}\n" +
                $"Difficulty: {GameStatics._enemyDifficulty}\n\n" +
                $"Saved as: {GameStatics._currentSaveFileName}";
        }
        StatDisplay.text = output;
    }

    public void FlushPlayerData()
    {
        for (int i = 0; i < (int)PlayerStatType.STATCOUNT; i++)
        {
            GameStatics._currentPlayerStats[i] = GameStatics._statMinValues[i];
        }
        GameStatics._currentPlayerIntegrity = GameStatics._currentPlayerStats[0];
        GameStatics._playerUpgradePoints = 0;
        GameStatics._enemyDifficulty = 0;
        GameStatics._fileLoaded = false;
        GameStatics._currentSaveFileName = "";
    }

    /// <summary>
    /// save file format - csv, anything in {} represents parameter:
    /// {ActiveDataIntegrity}, {DataIntegrity}, {BandwidthCap}, {ConnectionSpeed}, {Backups}, {SystemKnowledge}, {ExfilChance}, {PointsAvailable}, {EnemyLevel}
    /// </summary>
    public void WritePlayerData()
    {
        StreamWriter sw = new StreamWriter(Application.dataPath + Path.DirectorySeparatorChar + $"{saveFileName}.txt");
        string line = $"{GameStatics._currentPlayerIntegrity},";
        foreach (int stat in GameStatics._currentPlayerStats)
        {
            line += $"{stat},";
        }
        line += $"{GameStatics._playerUpgradePoints},{GameStatics._enemyDifficulty}";
        sw.WriteLine(line);
        sw.Close();
        GameStatics._fileLoaded = true;
        GameStatics._currentSaveFileName = saveFileName;
        Debug.Log($"Save file registered: {saveFileName}");
    }

    public void ReadPlayerData()
    {
        try
        {
            using (StreamReader sr = new StreamReader(Application.dataPath + Path.DirectorySeparatorChar + $"{saveFileName}.txt"))
            {
                FlushPlayerData();
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] csv = line.Split(',');
                    GameStatics._currentPlayerIntegrity = int.Parse(csv[0]);
                    for (int i = 0; i < (int)PlayerStatType.STATCOUNT; i++)
                    {
                        GameStatics._currentPlayerStats[i] = int.Parse(csv[i+1]);
                    }
                    GameStatics._playerUpgradePoints = int.Parse(csv[7]);
                    GameStatics._enemyDifficulty = int.Parse(csv[8]);
                }
                sr.Close();
                GameStatics._fileLoaded = true;
                GameStatics._currentSaveFileName = saveFileName;
                Debug.Log($"Save file registered: {saveFileName}");
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            GenerateRandomUserData();
        }
    }

    public void LoadCreationScene()
    {
        SceneManager.LoadScene("Scenes/PlayerCreation");
    }

    public void LoadCombatScene()
    {
        SceneManager.LoadScene("Scenes/CombatScene");
    }
}

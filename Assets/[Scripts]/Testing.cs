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
    }

    public void GenerateRandomUserData()
    {
        FlushPlayerData();
        for (int i = 0; i < (int)PlayerStatType.STATCOUNT; i++ )
        {
            PlayerStats._currentPlayerStats[i] = Random.Range(0, 100);
            int randomMods = Random.Range(0, 5);
        }
        PlayerStats._fileLoaded = true;
    }

    public void DisplayUserData()
    {
        string output = "";
        if (PlayerStats._fileLoaded)
        {
            for (int i = 0; i < (int)PlayerStatType.STATCOUNT; i++)
            {
                if (i == 0)
                {
                    output += ((PlayerStatType)i).ToString() + $": {PlayerStats._currentPlayerIntegrity}/{PlayerStats._currentPlayerStats[i]}\n";
                }
                else output += ((PlayerStatType)i).ToString() + $": {PlayerStats._currentPlayerStats[i]}\n";
            }
            output += $"Points Available: {PlayerStats._playerUpgradePoints}\n" +
                $"Difficulty: {PlayerStats._enemyDifficulty}\n\n" +
                $"Saved as: {PlayerStats._currentSaveFileName}";
        }
        StatDisplay.text = output;
    }

    public void FlushPlayerData()
    {
        for (int i = 0; i < (int)PlayerStatType.STATCOUNT; i++)
        {
            PlayerStats._currentPlayerStats[i] = GameStatics._statMinValues[i];
        }
        PlayerStats._currentPlayerIntegrity = PlayerStats._currentPlayerStats[0];
        PlayerStats._playerUpgradePoints = 0;
        PlayerStats._enemyDifficulty = 0;
        PlayerStats._fileLoaded = false;
        PlayerStats._currentSaveFileName = "";
    }

    /// <summary>
    /// save file format - csv, anything in {} represents parameter:
    /// {ActiveDataIntegrity}, {DataIntegrity}, {BandwidthCap}, {ConnectionSpeed}, {Backups}, {SystemKnowledge}, {ExfilChance}, {PointsAvailable}, {EnemyLevel}, {CurrentIntegrity}
    /// 
    /// </summary>
    public void SavePlayerData()
    {

        StreamWriter sw = new StreamWriter(Application.dataPath + Path.DirectorySeparatorChar + $"{PlayerStats._currentSaveFileName}.txt");
        string line = "";
        line += $"{PlayerStats._currentPlayerIntegrity},{PlayerStats._currentPlayerStats[0]},{PlayerStats._currentPlayerStats[1]},{PlayerStats._currentPlayerStats[2]},{PlayerStats._currentPlayerStats[3]},{PlayerStats._currentPlayerStats[4]},{PlayerStats._currentPlayerStats[5]},{PlayerStats._playerUpgradePoints},{PlayerStats._enemyDifficulty}";
        sw.WriteLine(line);
        sw.Close();

    }

    public void WritePlayerData()
    {
        StreamWriter sw = new StreamWriter(Application.dataPath + Path.DirectorySeparatorChar + $"{saveFileName}.txt");
        string line = $"{PlayerStats._currentPlayerIntegrity},";
        foreach (int stat in PlayerStats._currentPlayerStats)
        {
            line += $"{stat},";
        }
        line += $"{PlayerStats._playerUpgradePoints},{PlayerStats._enemyDifficulty}";
        sw.WriteLine(line);
        sw.Close();
        PlayerStats._fileLoaded = true;
        PlayerStats._currentSaveFileName = saveFileName;
        PlayerStats._playerUpgradePoints = 5;
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
                    PlayerStats._currentPlayerIntegrity = int.Parse(csv[0]);
                    PlayerStats._currentPlayerStats[0] = int.Parse(csv[1]);
                    PlayerStats._currentPlayerStats[1] = int.Parse(csv[2]);
                    PlayerStats._currentPlayerStats[2] = int.Parse(csv[3]);
                    PlayerStats._currentPlayerStats[3] = int.Parse(csv[4]);
                    PlayerStats._currentPlayerStats[4] = int.Parse(csv[5]);
                    PlayerStats._currentPlayerStats[5] = int.Parse(csv[6]);
                    PlayerStats._playerUpgradePoints = int.Parse(csv[7]);
                    PlayerStats._enemyDifficulty = int.Parse(csv[8]);
                }
                sr.Close();
                PlayerStats._fileLoaded = true;
                PlayerStats._currentSaveFileName = saveFileName;
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

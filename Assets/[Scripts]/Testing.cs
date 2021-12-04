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
                PlayerStats._modifiers[i].Add(Random.Range(0, 10));
            }
        }
        PlayerStats._loaded = true;
    }


    public void DisplayUserData()
    {
        string output = "";
        if (PlayerStats._loaded)
        {
            for (int i = 0; i < (int)PlayerStatType.STATCOUNT; i++)
            {
                output += ((PlayerStatType)i).ToString() + $": {PlayerStats._stats[i]}";
                int modifierCount = PlayerStats._modifiers[i].Count;
                if (modifierCount > 0)
                {
                    foreach (int mod in PlayerStats._modifiers[i])
                    {
                        output += $" + {mod}";
                    }
                }
                output += "\n";
            }
            output += $"Saved as: {PlayerStats._fileName}";
        }
        StatDisplay.text = output;
    }

    public void FlushPlayerData()
    {
        for (int i = 0; i < (int)PlayerStatType.STATCOUNT; i++)
        {
            PlayerStats._stats[i] = 0;
            if (PlayerStats._modifiers[i].Count > 0)
            {
                PlayerStats._modifiers[i].Clear();
            }
        }
        PlayerStats._loaded = false;
        PlayerStats._fileName = "";
    }

    public void WritePlayerData()
    {
        StreamWriter sw = new StreamWriter(Application.dataPath + Path.DirectorySeparatorChar + $"{saveFileName}.txt");
        for (int i = 0; i < (int)PlayerStatType.STATCOUNT; i++)
        {
            int modifierCount = PlayerStats._modifiers[i].Count;
            string line = $"{PlayerStats._stats[i]},{PlayerStats._modifiers[i].Count}";
            if (modifierCount > 0)
            {
                foreach (int mod in PlayerStats._modifiers[i])
                {
                    line += $",{mod}";
                }
            }
            sw.WriteLine(line);
        }
        sw.Close();
        PlayerStats._loaded = true;
        PlayerStats._fileName = saveFileName;
    }

    public void ReadPlayerData()
    {
        try
        {
            using (StreamReader sr = new StreamReader(Application.dataPath + Path.DirectorySeparatorChar + $"{saveFileName}.txt"))
            {
                FlushPlayerData();
                string line;
                int currentModifiedStat = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] csv = line.Split(',');
                    PlayerStats._stats[currentModifiedStat] = int.Parse(csv[0]);
                    for (int i = 0; i < int.Parse(csv[1]); i++)
                    {
                        PlayerStats._modifiers[currentModifiedStat].Add(int.Parse(csv[2 + i]));
                    }
                    currentModifiedStat++;
                }
                sr.Close();
                PlayerStats._loaded = true;
                PlayerStats._fileName = saveFileName;
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    public void LoadCreationScene()
    {
        SceneManager.LoadScene("PlayerCreation");
    }
}

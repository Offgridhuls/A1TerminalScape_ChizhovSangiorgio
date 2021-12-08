using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CombatModule : MonoBehaviour
{
    [SerializeField]
    Text playerStatsDisplay;

    [SerializeField]
    Text enemyStatsDisplay;

    [SerializeField]
    Text battleStatusDisplay;

    [SerializeField]
    public GameObject playerAbilitiesPanel;

    public PlayerCombat player;
    public Enemy enemy;

    bool playerTurn, combatTerminated = false;
    void Start()
    {
        player.statsDisplay = playerStatsDisplay;
        player.combatModule = this;
        player.LoadStats();
        player.QueueAbilities();

        enemy.statsDisplay = enemyStatsDisplay;
        enemy.GenerateNew(PlayerStats._enemyDifficulty);
        enemy.combatModule = this;
        enemy.QueueAbilities();

        playerAbilitiesPanel.SetActive(false);
        playerTurn = (player.RollInitiative() >= enemy.RollInitiative());
        Invoke("BeginTurn", 1.5f);
    }

    public void OnPlayerAbilityButtonPressed(int abilityIndex)
    {
        player.AttemptPlayAbility((PlayerSkills)abilityIndex);
    }

    void BeginTurn()
    {
        playerAbilitiesPanel.SetActive(playerTurn);
        if (playerTurn)
        {
            battleStatusDisplay.text = "Player Turn";
            player.OnBeginTurn();
        }
        else
        {
            battleStatusDisplay.text = "Enemy Turn";
            enemy.OnBeginTurn();
            ProcessEnemyTurn();
        }
    }

    public void ConcludePlayerTurn()
    {
        if (!playerTurn) return;
        playerTurn = false;
        BeginTurn();
    }

    void ProcessEnemyTurn()
    {
        //begin enemy ability chain
        StartCoroutine(OnEnemyAttemtAbilities());
    }

    IEnumerator OnEnemyAttemtAbilities()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("Enemy selecting ability");
        int skillToPlay = Random.Range(0, (int)EnemySkills.SKILLCOUNT);
        if (enemy.AttemptPlayAbility((EnemySkills)skillToPlay)) 
        {
            StartCoroutine(OnEnemyAttemtAbilities());
        }
        else
        {
            playerTurn = true;
            BeginTurn();
        }
    }

    IEnumerator OnBattleOver(bool playerWin)
    {
        yield return new WaitForSeconds(1.5f);
        if (playerWin)
        {
            player.OnPlayerWin();
            PlayerStats._playerUpgradePoints++;
            PlayerStats._enemyDifficulty++;
            //TODO: save statics to file

            ReturnToCreationScene();
        }
        else
        {
            //TODO: delete player save
            ReturnToMainMenu();
        }
    }

    public void OnPlayerDeath()
    {
        BattleNotify("Player had been defeated");
        playerAbilitiesPanel.SetActive(false);
        combatTerminated = true;
        StartCoroutine(OnBattleOver(false));
    }

    public void OnEnemyDeath()
    {
        BattleNotify("Breach successful, upgrade point acquired");
        playerAbilitiesPanel.SetActive(false);
        combatTerminated = true;
        StartCoroutine(OnBattleOver(true));
    }

    public void OnPlayerExfilSuccess()
    {
        BattleNotify("Player Escape successful");
        playerAbilitiesPanel.SetActive(false);
        Invoke("ReturnToGameScene", 1.5f);
    }

    public void OnZeroDaySuccess()
    {
        BattleNotify("Zero Day successful");
        playerAbilitiesPanel.SetActive(false);
        StartCoroutine(OnBattleOver(true));
    }


    //UI Functions
    public void BattleNotify(string msg)
    {
        battleStatusDisplay.text = msg;
    }
    public void EnemyActionNotify(int abilityPlayed, bool addsModifiers, int statModified, int modValue)
    {
        string outMsg = $"Enemy plays {((EnemySkills)abilityPlayed).ToString()}\n";
        if (addsModifiers) outMsg += $"{modValue} {((CombatModifiers)statModified).ToString()} added";
        else outMsg += $"{((PlayerStatType)statModified).ToString()} decreased by {modValue}";
        battleStatusDisplay.text = outMsg;
    }

    //scene functions
    void ReturnToGameScene()
    {
        SceneManager.LoadScene("Scenes/GameScene");
    }

    void ReturnToCreationScene()
    {
        SceneManager.LoadScene("Scenes/PlayerCreation");
    }

    void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Scenes/SaveSelect");
    }
}

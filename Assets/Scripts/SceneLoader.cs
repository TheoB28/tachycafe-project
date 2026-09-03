using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    EnemyData[] Enemies;
    public bool Incombat;


    CombatHandler combatHandler;
    PlayerDataHandler playerDataHandler;

    void Start()
    {
        int SceneLoaderCount = FindObjectsOfType<SceneLoader>().Length;
        if (SceneLoaderCount > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        playerDataHandler = FindFirstObjectByType<PlayerDataHandler>();
    }

    private void Update()
    {
        //finds the combatHandler if in combat
        if(!combatHandler && SceneManager.GetActiveScene().name == "CombatScene")
        {
            combatHandler = FindObjectOfType<CombatHandler>();
            combatHandler.SetupEnemies(Enemies);
        }
    }

    public void LoadCombatScene(EnemyData[] enemies)
    {
        SceneManager.LoadScene("CombatScene");
        Incombat = true;
        Enemies = enemies;
    }

    public void LoadOverworld()
    {
        SceneManager.LoadScene("Overworld");
        Incombat = false;
        combatHandler = null;
    }
}

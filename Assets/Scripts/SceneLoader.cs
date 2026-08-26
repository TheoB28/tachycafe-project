using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    EnemyData[] Enemies;

    CombatHandler combatHandler;

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
    }

    private void Update()
    {
        if(!combatHandler && SceneManager.GetActiveScene().name == "CombatScene")
        {
            combatHandler = FindObjectOfType<CombatHandler>();
            combatHandler.SetupEnemies(Enemies);
        }
    }

    public void LoadCombatScene(EnemyData[] enemies)
    {
        SceneManager.LoadScene("CombatScene");
        Enemies = enemies;
    }

    public void LoadOverworld()
    {
        SceneManager.LoadScene("Overworld");
        combatHandler = null;
    }
}

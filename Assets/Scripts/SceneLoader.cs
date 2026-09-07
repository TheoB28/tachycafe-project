using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    EnemyData[] Enemies;
    [SerializeField] PlayerDataHandler playerDataHandler;
    public bool Incombat;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadCombatScene(EnemyData[] enemies)
    {
        Enemies = enemies;
        SceneManager.LoadScene("CombatScene");

    }

    public void LoadOverworld()
    {
        SceneManager.LoadScene("Overworld");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "CombatScene":
                Incombat = true;
                playerDataHandler.ActivateCombat();
                combatHandler = FindObjectOfType<CombatHandler>();
                combatHandler.SetupEnemies(Enemies);
                break;
            case "Overworld":
                Incombat = false;
                combatHandler = null;
                playerDataHandler.DeactivateCombat();
                break;
        }
    }
}

using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    EnemyData[] Enemies;
    [SerializeField] PlayerDataHandler playerDataHandler;
    public bool Incombat, newOScene;

    Vector2 newPlayerPos;

    CombatHandler combatHandler;
    PlayerOverworld playerOverworld;
    CinemachineBrain MainCamera;
    CinemachineCamera Camera;


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
        playerOverworld = FindAnyObjectByType<PlayerOverworld>();
        MainCamera = FindAnyObjectByType<CinemachineBrain>();
        Camera = FindAnyObjectByType<CinemachineCamera>();
    }

    public void LoadCombatScene(EnemyData[] enemies)
    {
        Enemies = enemies;
        SceneManager.LoadScene("CombatScene");

    }

    public void LoadOverworld()
    {
        playerDataHandler?.UpdateData();
        SceneManager.LoadScene("Overworld");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!playerDataHandler) { playerDataHandler = FindAnyObjectByType<PlayerDataHandler>(); }

        if (playerOverworld)
        {
            SceneManager.MoveGameObjectToScene(playerOverworld.gameObject, SceneManager.GetActiveScene());
            playerOverworld.transform.position = newPlayerPos;
            SceneManager.MoveGameObjectToScene(MainCamera.gameObject, SceneManager.GetActiveScene());
            SceneManager.MoveGameObjectToScene(Camera.gameObject, SceneManager.GetActiveScene());
        }


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
            case "MainMenu":
                FindAnyObjectByType(typeof(MainMenu)).GetComponent<MainMenu>().Setup(this);
                break;
        }
    }

    public void LoadScene(string sceneName, Vector2 pos, Color color)
    {
        newPlayerPos = pos;
        newOScene = true;
        DontDestroyOnLoad(playerOverworld);
        DontDestroyOnLoad(MainCamera);
        MainCamera.GetComponent<Camera>().backgroundColor = color;
        DontDestroyOnLoad(Camera);
        SceneManager.LoadScene(sceneName);
    }
}

using UnityEngine;

public class CombatTrigger : MonoBehaviour
{

    [SerializeField] EnemyData[] enemies;

    SceneLoader sceneLoader;
    private void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            sceneLoader.LoadCombatScene(enemies);
        }
    }
}

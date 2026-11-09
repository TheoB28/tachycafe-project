using UnityEngine;

public class SceneDoor : MonoBehaviour
{

    [SerializeField] string SceneName;
    [SerializeField] Vector2 posToLoad;

    SceneLoader loader;

    private void Start()
    {
        loader = FindAnyObjectByType<SceneLoader>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            loader.LoadScene(SceneName, posToLoad);
        }
    }


}

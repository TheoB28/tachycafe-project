using UnityEngine;

public class SceneDoor : MonoBehaviour
{

    [SerializeField] string SceneName;
    [SerializeField] Vector2 posToLoad;
    [SerializeField] Color color = new Color(230, 144, 78);

    SceneLoader loader;

    private void Start()
    {
        loader = FindAnyObjectByType<SceneLoader>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            loader.LoadScene(SceneName, posToLoad, color);
        }
    }


}

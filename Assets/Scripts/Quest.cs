using UnityEngine;

[CreateAssetMenu(fileName = "Quests", menuName = "Scriptable Objects/Quests")]
public class Quest : ScriptableObject
{
    public enum QuestType { fetch}
    [SerializeField] public string Name;
    [SerializeField] public string Description;
    public bool Completed;
    [SerializeField] public QuestType type;

    [Header("Fetch quest")]
    [SerializeField] public QuestItem item;
    public bool HasItem;

    public void Activate()
    {
        item.gameObject.SetActive(true);
    }
    
}

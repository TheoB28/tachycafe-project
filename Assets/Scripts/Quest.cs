using UnityEngine;

[CreateAssetMenu(fileName = "Quests", menuName = "Scriptable Objects/Quests")]
public class Quest : ScriptableObject
{
    public enum QuestType { fetch}
    [SerializeField] public string Name;
    [SerializeField] public string Description;
    public bool Completed;
    [SerializeField] public QuestType type;
    [SerializeField] public int XPReward;

    [Header("Fetch quest")]
    [SerializeField] public QuestItem item;
    [SerializeField] Vector3 position;
    public bool HasItem;

    public void Activate()
    {
        Instantiate(item, position, Quaternion.identity);
    }
    
}

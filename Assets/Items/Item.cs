using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    [SerializeField] string Name;
    [SerializeField] string Description;
    [SerializeField] int cost;
    [SerializeField] int sellValue;

}

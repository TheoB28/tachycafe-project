using UnityEngine;

public class ObservableObject : MonoBehaviour
{

    [SerializeField] string Description;
    [SerializeField] string DysDescript;

    public string Descript(bool dysphoria)
    {
        if (dysphoria)
        {
            return DysDescript;
        }
        else
        {
            return Description;
        }
    }


}

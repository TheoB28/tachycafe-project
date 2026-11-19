using System.Collections;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] float force;
    [SerializeField] Vector2 forceRando;
    float damageTic, curNum; 
    int fullDam;

    TextMeshProUGUI textMeshPro;

    public void bounce(int damage)
    {
        fullDam = damage;
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-forceRando.x, forceRando.x), Random.Range(0.5f * forceRando.y, forceRando.y)) * force, ForceMode2D.Impulse);
        GetComponent<Rigidbody2D>().AddTorque(10 * force * Random.Range(-10, 10));
        StartCoroutine(numberUp());
        Invoke("destroy", 5);
    }

    IEnumerator numberUp()
    {
        damageTic = (float) fullDam / 100;
        while (curNum < fullDam)
        {
            curNum += damageTic;
            textMeshPro.text = ((int) curNum).ToString();
            yield return new WaitForSeconds(0.001f);
            if(curNum >= fullDam)
            {
                textMeshPro.text = fullDam.ToString();
            }
        }
        
    }

    void destroy()
    {
        Destroy(gameObject);
    }
}

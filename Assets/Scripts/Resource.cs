using UnityEngine;

public class Resource : MonoBehaviour
{
    public void TakeDamage(int damage)
    {
        Debug.Log("Ресурс получил урон: " + damage);
    }
}

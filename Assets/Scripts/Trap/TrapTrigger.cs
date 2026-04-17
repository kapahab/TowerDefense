using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TrapTrigger : MonoBehaviour
{
    private ITrapEffect[] trapEffects;

    void Start()
    {
        trapEffects = GetComponents<ITrapEffect>();

        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy entered trap trigger");
            foreach (var effect in trapEffects)
            {
                effect.ApplyEffect(other.gameObject);
                Debug.Log("effect applied from trap");
            }

            // Optional: If the trap is single-use, destroy it here
            // Destroy(gameObject);
        }
        else
        {
            Debug.Log("Non-enemy entered trap trigger: " + other.name);
        }
    }
}
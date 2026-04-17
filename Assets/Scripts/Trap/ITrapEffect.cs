using UnityEngine;

// Any script that implements this interface can be used as a trap effect
public interface ITrapEffect
{
    void ApplyEffect(GameObject enemy);
}
using Health;
using UnityEngine;

public class Target: MonoBehaviour, Damageable {
    // float Damageable.MaxHP { get ; set ; }
    // float Damageable.MaxEnergy { get ; set ; }
    // float Damageable.MaxStamina { get ; set ; }
    float Damageable.Defense { get ; set ; }
    float Damageable.Attack { get ; set ; }
    float Damageable.CurrentHP { get ; set ; }
    float Damageable.CurrentEnergy { get ; set ; }
    float Damageable.CurrentStamina { get ; set ; }

    void Damageable.DealDamage(Damageable opponent) {
        
    }

    void Damageable.ReceiveDamage(float damage, GameObject opponent) {
        Debug.Log("Damage --- " + damage);
    }

    private void Update() {
        var dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.position += dir * 20 * Time.deltaTime;
    }
}
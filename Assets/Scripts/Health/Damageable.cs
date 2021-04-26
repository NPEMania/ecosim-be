using UnityEngine;

namespace Health {

    public interface Damageable {

        // float MaxHP { get; set; }
        // float MaxEnergy { get; set; }
        // float MaxStamina { get; set; }
        float Defense { get; set; } // varies from 0 to 100
        float Attack { get; set; } // varies from 0 to 100


        float CurrentHP { get; set; }
        float CurrentEnergy { get; set; }
        float CurrentStamina { get; set; }

        void ReceiveDamage(float damage);
        // hp = hp - attack * 100 / (100 + defense)

        void DealDamage(Damageable opponent);
    }
}
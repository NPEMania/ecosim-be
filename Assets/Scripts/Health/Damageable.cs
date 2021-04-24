using UnityEngine;

namespace Health {

    public abstract class Damageable {

        protected float maxHP;
        protected float maxEnergy;
        protected float maxStamina;
        protected float defense; // varies from 0 to 100
        protected float attack; // varies from 0 to 100


        protected float currentHP;
        protected float currentEnergy;
        protected float currentStamina;

        public void ReceiveDamage(float damage) {
            currentHP = currentHP - 100 * damage / (100 + defense);
        }

        public void DealDamage(Damageable opponent) {
            opponent.ReceiveDamage(this.attack);
        }
    }
}
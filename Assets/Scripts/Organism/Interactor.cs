using System.Collections;
using System.Collections.Generic;
using Health;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Organism {

    // This class will solely use interaction i.e. Attack, reproduce
    public class Interactor: MonoBehaviour {

        private GameObject target;

        private IBrain brain;

        private Coroutine attackCoroutine;

        
        void Start() {
            brain = GetComponent<IBrain>();
        }

        public void TargetInRange(GameObject target) {
            this.target = target;
        }

        public void TargetOutOfRange() {
            this.target = null;
        }

        private void Update() {
            switch (brain.OrgState) {
                case OrganismState.ATTACKING_FOOD: {
                    if (target != null) {
                        if ((transform.position - target.transform.position).sqrMagnitude < 20) {
                            // Attack logic working
                            if (attackCoroutine == null) attackCoroutine = StartCoroutine(DealDamage());
                        } else {
                            if (attackCoroutine != null) StopCoroutine(attackCoroutine);
                            brain.OnTargetLeftAttackRange(target);
                        }
                    }
                    break;
                }
            }
        }

        IEnumerator DealDamage() {
            while (true) {
                if (target != null) {
                    Debug.Log("Attacking");
                    target.GetComponent<Damageable>().ReceiveDamage(5f);
                    yield return new WaitForSeconds(2f);
                }
            }
        }
    }
}

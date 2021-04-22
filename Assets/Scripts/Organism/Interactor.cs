using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Organism {

    // This class will solely use interaction i.e. Attack, reproduce
    public class Interactor: MonoBehaviour {

        private GameObject target;

        private IBrain brain;

        
        void Start() {
            brain = GetComponent<IBrain>();
        }

        private void OnTriggerStay(Collider other) {
            if (other.gameObject.tag == "test") {
                if (target != other.gameObject) {
                    brain.OnHuntTargetAcquired(other.gameObject);
                }
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.gameObject == target) {
                brain.OnHuntTargetLeft(other.gameObject);
            }
        }

        public void TargetInRange(GameObject target) {
            this.target = target;
        }

        public void TargetOutOfRange() {
            this.target = null;
        }

        private void Update() {
            switch(brain.OrgState) {
                case OrganismState.ATTACKING_FOOD: {
                    if (target != null) {
                        if ((transform.position - target.transform.position).sqrMagnitude > 16) {
                            brain.OnTargetLeftAttackRange(target);
                        } else {
                            Debug.Log("Attacking Now");
                        }
                    } else {
                        Debug.Log("Target is null, but state is still attacking");
                    }
                    break;
                }
            }
        }
    }
}

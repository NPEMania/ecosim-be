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
                        if ((transform.position - target.transform.position).sqrMagnitude < 16) {
                            // Attack logic working
                        } else {
                            brain.OnTargetLeftAttackRange(target);
                        }
                    }
                    break;
                }
            }
        }
    }
}

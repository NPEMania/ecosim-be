using Health;
using UnityEngine;

namespace Organism {

    // This class will solely use interaction i.e. Attack, reproduce
    public class Interactor: MonoBehaviour {
        public GameObject prefab;
        private GameObject target;

        private IBrain brain;
        private Damageable damageable;
        private float attackRange=4f;
        private float attack=5f;
        private float rotSpeed = 14f;
        private float attackGap;
        private float timeSinceLastAttacked;
        private Rigidbody body;

        public void SetupGene(Gene gene) {
            this.attackRange = gene.attackRange;
            this.attack = gene.attack;
            this.attackGap = gene.attackGap;
            this.timeSinceLastAttacked = attackGap;
        }

        void Start() {
            brain = GetComponent<IBrain>();
            damageable=GetComponent<Damageable>();
            body = GetComponent<Rigidbody>();
        }

        public void TargetInRange(GameObject target) {
            this.target = target;
        }

        public void TargetOutOfRange() {
            this.target = null;
        }

        private void Update() {
            switch (brain.OrgState) {
                case OrganismState.ATTACKING: {
                    Debug.Log("target is null " + (target == null));
                    if (target != null) {
                        if ((transform.position - target.transform.position).sqrMagnitude < (attackRange*attackRange)) {
                            // Attack logic working
                            LookIn((target.transform.position - transform.position).normalized);
                            if (timeSinceLastAttacked >= attackGap) {
                                target.GetComponent<Damageable>().ReceiveDamage(attack, gameObject);
                                timeSinceLastAttacked = 0f;
                            }
                            timeSinceLastAttacked += Time.deltaTime;
                        } else {
                            timeSinceLastAttacked = attackGap;
                            //if (attackCoroutine != null) StopCoroutine(attackCoroutine);
                            brain.OnTargetLeftAttackRange(target);
                        }
                    } else {
                        brain.OrgState = OrganismState.IDLE;
                    }
                    break;
                }
                case OrganismState.FITNESS_CHECK: {
                    break;
                }
            }
        }

        private void LookIn(Vector3 dir) {
            if (dir.sqrMagnitude > 0f) {
                var rotation = Quaternion.LookRotation(dir, transform.up);
                body.MoveRotation(Quaternion.Lerp(body.rotation, rotation, Time.deltaTime * rotSpeed));
                // Debug.Log(brain.Velocity.sqrMagnitude + " ---- " + body.velocity.sqrMagnitude);
            }
        }
    }
}

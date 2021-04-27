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
        private float attackGap;
        private float timeSinceLastAttacked = 0f;

        private Coroutine attackCoroutine;

        public void SetupGene(Gene gene) {
            this.attackRange = gene.attackRange;
            this.attack = gene.attack;
            this.attackGap = gene.attackGap;
        }

        void Start() {
            brain = GetComponent<IBrain>();
            damageable=GetComponent<Damageable>();
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
                    if (target != null) {
                        if ((transform.position - target.transform.position).sqrMagnitude < (attackRange*attackRange)) {
                            // Attack logic working
                            //if (attackCoroutine == null) attackCoroutine = StartCoroutine(DealDamage());
                            //Destroy(target);
                            timeSinceLastAttacked += Time.deltaTime;
                            if (timeSinceLastAttacked > attackGap) {
                                target.GetComponent<Damageable>().ReceiveDamage(attack, gameObject);
                                timeSinceLastAttacked = 0f;
                            }
                        } else {
                            //if (attackCoroutine != null) StopCoroutine(attackCoroutine);
                            brain.OnTargetLeftAttackRange(target);
                        }
                    } else {
                        Debug.Log("got here");
                        brain.OrgState = OrganismState.IDLE;
                    }
                    break;
                }
                case OrganismState.FITNESS_CHECK: {
                    break;
                }
            }
        }
    }
}

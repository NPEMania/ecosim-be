using System.Dynamic;
using System.Runtime.InteropServices;
using Health;
using UnityEditor;
using UnityEngine;

namespace Organism {

    public class PlantBrain : MonoBehaviour, IBrain, Damageable {

        public GameObject prefab;
        private float currentHP;
        private Gene gene;
        private OrganismState state;
        private GameObject target;
        public Gene SelfGene { get { return gene; }  set {}  }
        public OrganismState OrgState { get { return state; }  set {}  }
        public Vector3 Velocity { get { return Vector3.zero; }  set {}  }
        public float Defense { get { return gene.defense; }  set {}  }
        public float Attack { get { return 0; }  set {}  }
        public float CurrentHP { get { return currentHP; }
            set {
                currentHP = value;
                if (currentHP <= 0) {
                    if (target != null) {
                        // this means someone killed us
                        target.GetComponent<IBrain>().RegisterKill(gene);
                    }
                    Destroy(this.gameObject);
                }
                if (currentHP > gene.maxHP) currentHP = gene.maxHP;
            }
        }
        public float CurrentEnergy { get { return 0; }  set {}  }
        public float CurrentStamina { get { return 0; }  set {}  }

        public void DealDamage(Damageable opponent){
            
        }

        public void ReceiveDamage(float damage, GameObject opponent) {
            target = opponent;
            var effectiveDamage = damage * 100 / (100 + gene.defense);
            CurrentHP = CurrentHP - effectiveDamage;
        }

        public void OnHuntTargetAcquired(GameObject target) {}

        public void OnHuntTargetLeft(GameObject target) {}

        public void OnMateAcquired(GameObject mate) {}

        public void OnMateInRange(GameObject mate) {}

        public void OnMateLeft(){}

        public void OnMateLeftRange(GameObject mate) {}

        public void OnStateChanged(OrganismState newState) {}

        public void OnTargetInAttackRange(GameObject target) {}

        public void OnTargetLeftAttackRange(GameObject target) {}

        public void ReceiveMateRequest(GameObject otherMate) {}

        public void ReceiveMateResponse(bool accepted, GameObject otherMate) {}

        public void RegisterKill(Gene gene) {}

        public void SetupGenes(Gene gene) {}

        private void Start() {
            gene = SampleGenes.plantGene;
            SetupGene(gene);
        }

        private void SetupGene(Gene gene) {
            currentHP = gene.maxHP;
        }

        private void Update() {
            
        }

        public static IBrain Create(Gene gene, GameObject prefab, Vector3 position, Quaternion rotation) {
            GameObject org = Instantiate(prefab, position, rotation);
            IBrain brain = org.GetComponent<IBrain>();
            brain.SetupGenes(gene);
            return brain;
        }
    }
}
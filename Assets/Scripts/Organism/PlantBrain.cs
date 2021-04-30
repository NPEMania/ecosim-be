using System.Dynamic;
using System.Runtime.InteropServices;
using Health;
using UnityEditor;
using UnityEngine;

namespace Organism {

    public class PlantBrain : MonoBehaviour, IBrain, Damageable {

        public GameObject prefab;
        private DayNightCycle cycle;
        private Environment environment;
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

        private float currentEnergy;
        public float CurrentEnergy { get { return currentEnergy; }
            set {
                currentEnergy = value; 
            }
        }
        public float CurrentStamina { get { return 0; }  set {}  }

        private float urge = 0;
        private float timeSinceAlive = 0f;
        private float creationTime;

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
            cycle = FindObjectOfType<DayNightCycle>();
            environment = FindObjectOfType<Environment>();
            timeSinceAlive = 0f;
            transform.localScale = new Vector3(gene.scale, gene.scale, gene.scale);
        }

        private void SetupGene(Gene gene) {
            currentHP = gene.maxHP;
        }

        private void Update() {
            timeSinceAlive += Time.deltaTime;
            //Debug.Log(gameObject.name + " " + timeSinceAlive + " " + gene.lifespan);
            if (timeSinceAlive > gene.lifespan) {
                Debug.Log(gameObject.name + " Destroying");
                Destroy(gameObject);
            }
            if (cycle != null) {
                if (cycle.TimeOfDay > 6.5 && cycle.TimeOfDay < 18.75) {
                    urge += Time.deltaTime * gene.urgeRate;
                    if (urge > 100) {
                        Debug.Log("Spawning new plant");
                        environment.SpawnPlant(transform.position, gene);
                        urge = 0;
                    }
                } else {
                    urge -= Time.deltaTime * gene.urgeRate;
                    if (urge < 0) {
                        urge = 0;
                    }
                }
            }
        }

        public static IBrain Create(Gene gene, GameObject prefab, Vector3 position, Quaternion rotation) {
            GameObject org = Instantiate(prefab, position, rotation);
            IBrain brain = org.GetComponent<IBrain>();
            brain.SetupGenes(gene);
            return brain;
        }
    }
}
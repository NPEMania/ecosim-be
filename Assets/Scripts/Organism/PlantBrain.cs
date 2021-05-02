using System.Dynamic;
using System.Runtime.InteropServices;
using Health;
using UnityEditor;
using UnityEngine;

namespace Organism {

    public class PlantBrain : MonoBehaviour, IBrain, Damageable {

        public GameObject prefab;
        private DayNightCycle cycle;
        private int offSpring=0;
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
        private float timeSinceAlive = 0f;//hours
       public float timeSinceAliveDays  {
             get {
            //    return ((timeSinceAlive/(24f*cycle.dayEquivalentInMinutes)));//notsure
               
               return ((timeSinceAlive/(24f)));//notsure
            }
            set {}
        } 
        
        private float creationTime;

        public void DealDamage(Damageable opponent){
            
        }

        public void ReceiveDamage(float damage, GameObject opponent) {
            Debug.Log("Plant received Damage");
            target = opponent;
            var effectiveDamage = damage * 100 / (100 + gene.defense);
            CurrentHP = CurrentHP - damage * 2;
            //opponent.GetComponent<IBrain>().RegisterKill(gene);
            //Destroy(this.gameObject)
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

        public void SetGeneration(int gen) {}

        public int GetGeneration() { return 0; }

        private void Update() {
            timeSinceAlive += Time.deltaTime * cycle.dayRate;
            //Debug.Log(gameObject.name + " " + timeSinceAlive + " " + gene.lifespan+" .."+timeSinceAliveDays);
            
            if (timeSinceAlive > gene.lifespan) {
                //Debug.Log(gameObject.name + " Destroying");
                Destroy(gameObject);
            }
            if (cycle != null) {
                if (cycle.TimeOfDay > 6.5 && cycle.TimeOfDay < 18.75) {
                    urge += Time.deltaTime * gene.urgeRate;
                     if (timeSinceAlive > (gene.lifespan*0.5f)&&offSpring<1) {
                        //Debug.Log("Spawning new plant");
                        offSpring++;
                        environment.SpawnPlant(transform.position, gene);
                    }
                    if (timeSinceAlive > (gene.lifespan*0.7f)&&offSpring<2) {
                        //Debug.Log("Spawning new plant");
                        offSpring++;
                        environment.SpawnPlant(transform.position, gene);
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
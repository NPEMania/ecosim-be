using UnityEngine;
using Organism;
using System.Collections;
using System;
using UnityEditorInternal;
using Health;
using UnityEngine.Networking.Types;

namespace Organism {

    class FSMBrain: MonoBehaviour, IBrain, Damageable {

        public String id;
        public int geneId;
        private Gene objGene;
        private float currentHP;
        private OrganismState state;
        private OrganismState lastState;
        private Interactor interactor;
        private MovementController controller;
        private GameObject target;

        private TriggerDetector triggerDetector;

        private float stamina = 100f;
        private float maxStamina = 100f;

        private float maxHP = 100f;
        private float cooldown = 4f;

        private Coroutine staminaRegen;

        public void SetupGenes(Gene gene) {
            this.maxStamina=gene.maxStamina;
            this.stamina = maxStamina;
            this.maxHP=gene.maxHP;
            this.currentHP=maxHP;
        }
       

        public OrganismState OrgState {
            get { return state; }
            set {
                if (state == value) return;
                state = value;
                OnStateChanged(state);
            }
        }

        private Vector3 velocity;
        public Vector3 Velocity {
            get { return velocity; }
            set { velocity = value; }
        }

        // float Damageable.MaxHP { get ; set ; }
        // float Damageable.MaxEnergy { get ; set ; }
        // float Damageable.MaxStamina { get ; set ; }
        public float Defense { get { return objGene.defense; } set {} }
        public float Attack { get { return objGene.attack; } set {} }
        public float CurrentHP { get { return currentHP; }
            set { 
                currentHP=value;
                if (currentHP <= 0) {
                    Destroy(this.gameObject);
                }
            }
        }
        public float CurrentEnergy { get { return 0; } set {} }
        public float CurrentStamina { get { return stamina; } set {stamina = value; } }

        public void DealDamage(Damageable opponent) {
            
        }

        public void ReceiveDamage(float damage) {
            /*if(currentHP<=0) {
                Debug.Log(id + " Deaddddd --- " + damage);
                Destroy(this.gameObject);
            } else {
                currentHP=(currentHP-damage);
                Debug.Log(id + " Damage --- " + currentHP);
            }*/
            CurrentHP = CurrentHP - damage * 100 / (100 + objGene.defense);
            Debug.Log(id + " Damage --- " + currentHP);
        }

        private void Start() {
            objGene=SampleGenes.geneArray[geneId];
            SetupGenes(objGene);
            interactor = GetComponent<Interactor>();
            interactor.SetupGene(objGene);
            controller = GetComponent<MovementController>();
            controller.SetupGene(objGene);
            triggerDetector=GetComponent<TriggerDetector>();
            triggerDetector.SetupGene(objGene);
        }

        public void OnStateChanged(OrganismState state) {
            
        }

        public void OnHuntTargetAcquired(GameObject target) {
            Debug.Log(id + " Target Got");
            OrgState = OrganismState.CHASING_FOOD;
            controller.UpdateTarget(target);
        }

        public void OnHuntTargetLeft(GameObject target) {
            Debug.Log(id + " Target Left");
            OrgState = OrganismState.IDLE;
            controller.UpdateTarget(null);
        }

        public void OnMateAcquired(GameObject target) {
            
        }

        public void OnTargetInAttackRange(GameObject target) {
            if (this.target != target) this.target = target;
            OrgState = OrganismState.ATTACKING_FOOD;
            interactor.TargetInRange(target);
        }

        public void OnTargetLeftAttackRange(GameObject target) {
            interactor.TargetOutOfRange();
            OrgState = OrganismState.CHASING_FOOD;
            controller.UpdateTarget(target);
        }

        private void Update() {
            switch (OrgState) {
                case OrganismState.CHASING_FOOD: {
                    if (velocity.sqrMagnitude > (maxStamina-1)) {
                        Debug.Log("Stamina Depleting");
                        stamina -= Time.deltaTime * 10f;
                    } else {
                        if (stamina < maxStamina) {
                            stamina += Time.deltaTime * 10f;
                        }
                    }
                    if (stamina < 0) {
                        Debug.Log("Stamina is less than zero");
                        state = OrganismState.REST;
                    } else if (stamina > (maxStamina/2)) {
                        // Should Help restore last state
                    }
                    break;
                }
            }
        }

        private void ChangeAndCacheState(OrganismState newState) {
            if (state != newState) {
                lastState = state;
                state = newState;
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
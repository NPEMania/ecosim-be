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

        private Rigidbody body;

         public void SetupGeneNInitStats(Gene gene) {
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
        float Damageable.Defense { get ; set ; }
        float Damageable.Attack { get ; set ; }
        float Damageable.CurrentHP { get {return currentHP;} set{ currentHP=value;} }
        float Damageable.CurrentEnergy { get ; set ; }
        float Damageable.CurrentStamina { get ; set ; }

        void Damageable.DealDamage(Damageable opponent) {
            
        }

        void Damageable.ReceiveDamage(float damage) {
            if(currentHP<=0)
            {
            Debug.Log(id + " Deaddddd --- " + damage);
                  //Destroy(gameObject);
            }else{
            currentHP=(currentHP-damage);
            Debug.Log(id + " Damage --- " + currentHP);
        }
        }

        private void Start() {
            objGene=SampleGenes.geneArray[geneId];
            SetupGeneNInitStats(objGene);
            interactor = GetComponent<Interactor>();
            interactor.SetupGene(objGene);
            controller = GetComponent<MovementController>();
            controller.SetupGene(objGene);
            triggerDetector=GetComponent<TriggerDetector>();
            triggerDetector.SetupGene(objGene);
            body = GetComponent<Rigidbody>();
            
            
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
    }
}
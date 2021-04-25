using UnityEngine;
using Organism;
using System.Collections;
using System;
using UnityEditorInternal;
using Health;

namespace Organism {

    class FSMBrain: MonoBehaviour, IBrain, Damageable {

        public String id;

        private OrganismState state;
        private OrganismState lastState;
        private Interactor interactor;
        private MovementController controller;
        private GameObject target;

        private float stamina = 100f;

        private float cooldown = 4f;

        private Coroutine staminaRegen;

        private Rigidbody body;

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

        float Damageable.MaxHP { get ; set ; }
        float Damageable.MaxEnergy { get ; set ; }
        float Damageable.MaxStamina { get ; set ; }
        float Damageable.Defense { get ; set ; }
        float Damageable.Attack { get ; set ; }
        float Damageable.CurrentHP { get ; set ; }
        float Damageable.CurrentEnergy { get ; set ; }
        float Damageable.CurrentStamina { get ; set ; }

        void Damageable.DealDamage(Damageable opponent) {
            
        }

        void Damageable.ReceiveDamage(float damage) {
            Debug.Log(id + " Damage --- " + damage);
        }

        private void Start() {
            interactor = GetComponent<Interactor>();
            controller = GetComponent<MovementController>();
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
                    if (velocity.sqrMagnitude > 99f) {
                        Debug.Log("Stamina Depleting");
                        stamina -= Time.deltaTime * 10f;
                    } else {
                        if (stamina < 100f) {
                            stamina += Time.deltaTime * 10f;
                        }
                    }
                    if (stamina < 0) {
                        Debug.Log("Stamina is less than zero");
                        state = OrganismState.REST;
                    } else if (stamina > 50f) {
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
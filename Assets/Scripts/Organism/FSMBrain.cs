using UnityEngine;
using Organism;
using System.Collections;
using System;

namespace Organism {

    class FSMBrain: MonoBehaviour, IBrain {

        private OrganismState state;
        private Interactor interactor;
        private MovementController controller;
        private GameObject target;

        private float stamina = 100f;

        private bool cooldown = false;

        private Coroutine staminaRegen;

        public OrganismState OrgState {
            get { return state; }
            set {
                if (state == value) return;
                state = value;
                OnStateChanged(state);
            }
        }

        private void Start() {
            interactor = GetComponent<Interactor>();
            controller = GetComponent<MovementController>();
        }

        public void OnStateChanged(OrganismState state) {
            
        }

        public void OnHuntTargetAcquired(GameObject target) {
            Debug.Log("Target Got");
            OrgState = OrganismState.CHASING_FOOD;
            controller.UpdateTarget(target);
        }

        public void OnHuntTargetLeft(GameObject target) {
            Debug.Log("Target Left");
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
                    if (stamina > 0.1f && !cooldown) {
                        stamina -= Time.deltaTime * 6f;
                        Debug.Log(stamina);
                    }
                    if (stamina <= 0.1f) {
                        cooldown = true;
                        OrgState = OrganismState.REST;
                        controller.UpdateTarget(null);
                        stamina += Time.deltaTime * 2f; // Here stamina regen rate can be used
                        if (stamina >= 100f) {
                            stamina = 100f;
                            cooldown = false;
                            OrgState = OrganismState.IDLE;
                        }
                    }
                    break;
                }
            }
        }
    }
}
using UnityEngine;
using System;
using Health;
using JetBrains.Annotations;

namespace Organism {

    class FSMBrain: MonoBehaviour, IBrain, Damageable {

        public String id;
        public int geneId;
        public Gene gene;
        private OrganismState state;
        private OrganismState lastState;
        private Interactor interactor;
        private MovementController controller;
        private GameObject target;

        private TriggerDetector triggerDetector;

        private float stamina;
        private float currentHP;
        private float energy;
        private float staminaRate = 10f;
        private float urge = 0f;
        private float timeSinceLastHit = 0f;
        private float timeSinceAlive = 0f;
        private int encounters = 0;
        public int killSuccess = 0;
        private Environment environment;

        public float WinRate {
            get {
                if (encounters == 0) return 0;
                else return killSuccess / encounters;
            }
            set {}
        }

        public void SetupGenes(Gene gene) {
            this.stamina = gene.maxStamina;
            this.currentHP = gene.maxHP;
            this.energy = gene.maxEnergy;
        }

        public Gene SelfGene {
            get { return gene; }
            set {}
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
        public float Defense { get { return gene.defense; } set {} }
        public float Attack { get { return gene.attack; } set {} }
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
        public float CurrentEnergy { get { return energy; } 
            set {
                energy = value;
                if (energy < 0) {
                    energy = gene.maxEnergy;
                    CurrentHP = CurrentHP - 1f; // Not scaling
                }
                if (energy > gene.maxEnergy) energy = gene.maxEnergy;
            }
        }
        public float CurrentStamina { get { return stamina; }
            set {
                stamina = value;
                if (stamina < 0) {
                    if (OrgState == OrganismState.EVADING || OrgState == OrganismState.CHASING_FOOD) {
                        stamina = gene.maxStamina;
                        CurrentEnergy = CurrentEnergy - 1f; // Not scaling
                    } else {
                        stamina = 0;
                        OrgState = OrganismState.REST;
                    }
                }
                if (stamina > gene.maxStamina) stamina = gene.maxStamina;
            } 
        }

        public void DealDamage(Damageable opponent) {
            
        }

        public void ReceiveDamage(float damage, GameObject opponent) {
            target = opponent;
            var effectiveDamage = damage * 100 / (100 + gene.defense);
            CurrentHP = CurrentHP - effectiveDamage;
            //Debug.Log(id + " Damage --- " + currentHP);
            timeSinceLastHit = timeSinceAlive;
            
            var other = opponent.GetComponent<Damageable>();
            if (CurrentHP / gene.maxHP > 0.75f) {
                // Compare kills (win percent)
                // TODO: predator prey in gene can be used
                if (WinRate < opponent.GetComponent<FSMBrain>().WinRate) {
                    OrgState = OrganismState.EVADING;
                    controller.UpdateTarget(opponent);
                } else {
                    OnHuntTargetAcquired(opponent);
                }
            } else {
                OrgState = OrganismState.EVADING;
                controller.StartEvasion(opponent);
            }
        }

        public void RegisterKill(Gene killedGene) {
            if (killedGene.organismType == OrganismType.ANIMAL) {
                ++killSuccess;
            }
            //CurrentHP += killedGene.scale * 10;
            if (UtilityMethods.IsEdible(gene, killedGene)) {
                CurrentHP += killedGene.maxHP * 0.9f;
                CurrentEnergy += killedGene.maxEnergy * 0.9f;
            }
            //Have to choose next state
        }

        private void Start() {
            gene = SampleGenes.geneArray[geneId];
            SetupGenes(gene);
            interactor = GetComponent<Interactor>();
            interactor.SetupGene(gene);
            controller = GetComponent<MovementController>();
            controller.SetupGene(gene);
            triggerDetector=GetComponent<TriggerDetector>();
            triggerDetector.SetupGene(gene);
            OrgState = OrganismState.SEEKING_FOOD;
            transform.localScale = new Vector3(gene.scale, gene.scale, gene.scale);
            environment = FindObjectOfType<Environment>();
        }

        public void OnStateChanged(OrganismState state) {
            
        }

        public void OnHuntTargetAcquired(GameObject target) {
            encounters++;
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
            OrgState = OrganismState.CHASING_MATE;
            controller.UpdateTarget(target);
        }

        public void OnTargetInAttackRange(GameObject target) {
            // if (this.target != target) this.target = target;
            OrgState = OrganismState.ATTACKING;
            interactor.TargetInRange(target);
        }

        public void OnTargetLeftAttackRange(GameObject target) {
            interactor.TargetOutOfRange();
            OrgState = OrganismState.CHASING_FOOD;
            controller.UpdateTarget(target);
        }

        public void OnMateLeft() {

        }

        private void Update() {

            // TODO: Deplete Energy on attacking
            UpdateStats();
            DetermineAction();
        }

        private void UpdateStats() {
            timeSinceAlive += Time.deltaTime;
            if (OrgState != OrganismState.REST) {
                CurrentEnergy = CurrentEnergy - Time.deltaTime;
            }
            if (velocity.sqrMagnitude >= (gene.sprintSpeed * gene.sprintSpeed)) {
                CurrentStamina = CurrentStamina - Time.deltaTime * staminaRate;
            } else {
                // When not running, stamina regens over time
                CurrentStamina += Time.deltaTime * staminaRate;
            }
            //Reset urge to zero after mating;
            urge = urge + Time.deltaTime;
            if (urge > 100f) {
                urge = 100f;
            }
            Debug.Log(gene.species + " " + gameObject.name + " " + OrgState + " " + CurrentHP + " " + CurrentEnergy + " " + CurrentStamina + " " + urge + " " + gene.gender + " " + WinRate);
        }

        private void DetermineAction() {
            if (OrgState != OrganismState.EVADING 
                || OrgState != OrganismState.CHASING_FOOD
                || OrgState != OrganismState.CHASING_MATE
                || OrgState != OrganismState.ATTACKING
                || OrgState != OrganismState.FITNESS_CHECK) {
                if (CurrentHP < gene.maxHP / 2 || CurrentEnergy < gene.maxEnergy / 2) {
                    OrgState = OrganismState.SEEKING_FOOD;
                } else if (urge == 100f && (CurrentHP / gene.maxHP) > 0.75f && (CurrentEnergy / gene.maxEnergy) > 0.75f) {
                    OrgState = OrganismState.SEARCHING_MATE;
                }   
            } else if (OrgState == OrganismState.EVADING) {
                Debug.Log(gene.species + " " + gameObject.name +  " evade brain block");
                //TODO: Use some way to determine if this is safe
                // Maybe timeout after last received damage (Health was depleted long ago)
                // Maybe chaser exiting collider
                if (timeSinceAlive - timeSinceLastHit > gene.evadeCooldown) {
                    // Now probably it is safe, so can go to idle
                    Debug.Log(gene.species + " " + gameObject.name +  " is ending evade");
                    OrgState = OrganismState.IDLE;
                    controller.UpdateTarget(null);
                }
            } else if (OrgState == OrganismState.REST) {
                // Check day and night and rest accordingly
                if (CurrentStamina / gene.maxStamina > 0.75f) {
                    OrgState = OrganismState.IDLE;
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

        public void OnMateInRange(GameObject mate) {
            OrgState = OrganismState.FITNESS_CHECK;
            if (SelfGene.gender == Gender.MALE) {
                mate.GetComponent<IBrain>().ReceiveMateRequest(this.gameObject);
            }
        }

        public void ReceiveMateRequest(GameObject otherMate) {
            // Request is received by female, so otherMate is male
            OrgState = OrganismState.FITNESS_CHECK;
            if (CurrentEnergy / gene.maxEnergy >= 0.75f) {
                var male = otherMate.GetComponent<IBrain>();
                if (male.SelfGene.gender == Gender.MALE) {
                    // Check genes compatibility etc
                    // Randomly reject
                    // Reject based on advantage in genes
                    // Or check successful hunts or evasions if greater than self
                    // Or randomly check advantage over gene
                    // kundli system
                    Debug.Log("Starting a baby");
                    CurrentEnergy = CurrentEnergy - (CurrentEnergy / 2);
                    male.ReceiveMateResponse(true, this.gameObject);
                    var babyGene = Gene.combine(SelfGene, male.SelfGene, environment.mutation);
                    Create(babyGene, interactor.prefab, transform.position + new Vector3(2, 0, 2), transform.rotation);
                    urge = 0;
                }
            } else {
                otherMate.GetComponent<IBrain>().ReceiveMateResponse(false, this.gameObject);
                OrgState = OrganismState.IDLE;
            }
        }

        public void ReceiveMateResponse(bool accepted, GameObject otherMate) {
            // Other should be female
            if (accepted) {
                CurrentEnergy = CurrentEnergy - (CurrentEnergy / 4);
                urge = 0;
            } else {
                OrgState = OrganismState.IDLE;
            }
        }

        public void OnMateLeftRange(GameObject mate) {
            
        }
    }
}
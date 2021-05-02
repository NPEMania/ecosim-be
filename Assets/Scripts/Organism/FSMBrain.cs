using UnityEngine;
using System;
using Health;
using JetBrains.Annotations;

namespace Organism {

    public class FSMBrain: MonoBehaviour, IBrain, Damageable {

        public String id;
        
        public int gen;
        public Gene gene;
        private OrganismState state;
        private OrganismState lastState;
        private Interactor interactor;
        private MovementController controller;
        private GameObject target;

        private TriggerDetector triggerDetector;
        private DayNightCycle cycle;

        private float stamina;
        private float currentHP;
        private float energy;
        private float staminaRate = 10f;
        private float urge = 0f;
        private float timeSinceLastHit = 0f;
        public float timeSinceAlive = 0f;   //hours lived
        
        public float timeSinceAliveDays  {
            get {
            //    return ((timeSinceAlive/(24f*cycle.dayEquivalentInMinutes)));//notsure
               
               return ((timeSinceAlive/(24f)));//notsure
            }
            set {}
        }  //hours lived
        public int encounters = 0;
        public int killSuccess = 0;
        public int evasions = 0;
        public int childrenCount = 0;
        public String causeOfDeath;
        private Environment environment;
        private float depletionRatio = 0.36f;

        public float WinRate {
            get {
                if (encounters == 0) return 0;
                else return killSuccess / encounters;
            }
            set {}
        }

        public void SetupGenes(Gene gene) {
            // Debug.Log("gene is null ");
            this.gene = gene;
            this.stamina = gene.maxStamina;
            this.currentHP = gene.maxHP;
            this.energy = gene.maxEnergy;
            transform.localScale = new Vector3(gene.scale, gene.scale, gene.scale);
            interactor.SetupGene(gene);
            controller.SetupGene(gene);
            triggerDetector.SetupGene(gene);
        }

        public void SetGeneration(int gen) {
            this.gen = gen;
        }

        public int GetGeneration() {
            return gen;
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
                        causeOfDeath = "killed";
                        // this means someone killed us
                        target.GetComponent<IBrain>().RegisterKill(gene);
                    } else {
                        causeOfDeath = "health-depleted";
                    }
                    environment.RegisterDeath(new OrganismExportData(gene, this));
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
                    CurrentHP = CurrentHP - (depletionRatio * gene.maxHP); // scaling
                }
                if (energy > gene.maxEnergy) energy = gene.maxEnergy;
            }
        }

        //1e -> 10s, xe -> maxs

        public float CurrentStamina { get { return stamina; }
            set {
                stamina = value;
                if (stamina < 0) {
                    if (OrgState == OrganismState.EVADING || OrgState == OrganismState.CHASING_FOOD) {
                        stamina = gene.maxStamina;
                        CurrentEnergy = CurrentEnergy - (depletionRatio * gene.maxEnergy); // scaling
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

        private void Awake() {
            interactor = GetComponent<Interactor>();
            
            controller = GetComponent<MovementController>();
            
            triggerDetector=GetComponent<TriggerDetector>();
            
            OrgState = OrganismState.IDLE;
            environment = FindObjectOfType<Environment>();
            cycle = FindObjectOfType<DayNightCycle>();
            interactor.SetupCycle(cycle);
            controller.SetupCycle(cycle);
            Debug.Log("llllllllllllllllll"+gene.species + " " + gameObject.name + " " + OrgState + " " + CurrentHP + " " + CurrentEnergy + " " + CurrentStamina + " " + urge + " " + gene.gender + " " + WinRate);
      
        }
      
        public void OnStateChanged(OrganismState state) {
            
        }

        public void OnHuntTargetAcquired(GameObject target) {
            if (target.GetComponent<IBrain>().SelfGene.organismType == OrganismType.ANIMAL) {
                encounters++;
            }
            // Debug.Log(id + " Target Got");
            OrgState = OrganismState.CHASING_FOOD;
            controller.UpdateTarget(target);
        }

        public void OnHuntTargetLeft(GameObject target) {
            // Debug.Log(id + " Target Left");
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
            timeSinceAlive += Time.deltaTime * cycle.dayRate;
         
            if (timeSinceAlive > gene.lifespan) {
                causeOfDeath = "lifespan";
                environment.RegisterDeath(new OrganismExportData(gene, this));
                Destroy(this.gameObject);
            }
            if (OrgState != OrganismState.REST) {
                CurrentEnergy = CurrentEnergy - Time.deltaTime * gene.scale;
            }
            if (velocity.sqrMagnitude >= (gene.sprintSpeed * gene.sprintSpeed)) {
                CurrentStamina = CurrentStamina - Time.deltaTime * staminaRate * gene.scale;
            } else {
                // When not running, stamina regens over time
                CurrentStamina += Time.deltaTime * staminaRate ;
            }
            //Reset urge to zero after mating;
            urge = urge + Time.deltaTime * gene.urgeRate;
            if (urge > 100f) {
                urge = 100f;
            }
            // Debug.Log(gene.species + " " + gameObject.name + " " + OrgState + " " + CurrentHP + " " + CurrentEnergy + " " + CurrentStamina + " " + urge + " " + gene.gender + " " + WinRate);
        }

        private void DetermineAction() {
            if (cycle.TimeOfDay > 6.5f && cycle.TimeOfDay < 18.5f) {
                //   Debug.Log("Day");
                 if (OrgState != OrganismState.EVADING 
                || OrgState != OrganismState.CHASING_FOOD
                || OrgState != OrganismState.CHASING_MATE
                || OrgState != OrganismState.ATTACKING
                || OrgState != OrganismState.FITNESS_CHECK) {
                    //    Debug.Log("First if");
                if (CurrentHP < gene.maxHP / 2 || CurrentEnergy < gene.maxEnergy / 2) {
                    //   Debug.Log("First if inner 1");
                    OrgState = OrganismState.SEEKING_FOOD;
                } else if (urge == 100f && (CurrentHP / gene.maxHP) > 0.75f && (CurrentEnergy / gene.maxEnergy) > 0.75f) {
                    //    Debug.Log("First if inner 2");
                    OrgState = OrganismState.SEARCHING_MATE;
                }else{
                 OrgState = OrganismState.IDLE;
                    
                }   
            } else if (OrgState == OrganismState.EVADING) {
                Debug.Log(gene.species + " " + gameObject.name +  " evade brain block");
                //TODO: Use some way to determine if this is safe
                // Maybe timeout after last received damage (Health was depleted long ago)
                // Maybe chaser exiting collider
                if (timeSinceAlive - timeSinceLastHit > gene.evadeCooldown) {
                    // Now probably it is safe, so can go to idle
                    ++evasions;
                    // Debug.Log(gene.species + " " + gameObject.name +  " is ending evade");
                    OrgState = OrganismState.IDLE;
                    controller.UpdateTarget(null);
                }
            } else if (OrgState == OrganismState.REST) {
                // Check day and night and rest accordingly
                //    Debug.Log("Enter here "+stamina);
                if (CurrentStamina / gene.maxStamina > 0.75f) {
                //    Debug.Log("Entered here hahah");
                    OrgState = OrganismState.IDLE;
                }
            }}else{
                    OrgState = OrganismState.REST;
                
            }
            Debug.Log(cycle.TimeOfDay);
        }

        private void ChangeAndCacheState(OrganismState newState) {
            if (state != newState) {
                lastState = state;
                state = newState;
            }
        }

        public static IBrain Create(Gene gene, GameObject prefab, Vector3 position, Quaternion rotation, int gen,Color colorBg) {
            GameObject org = Instantiate(prefab, position, rotation);
            IBrain brain = org.GetComponent<IBrain>();
            org.GetComponentInChildren<Renderer>().material.color = colorBg;
            brain.SetupGenes(gene);
            brain.SetGeneration(gen);
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
                    // Debug.Log("Starting a baby");
                    CurrentEnergy = CurrentEnergy - (CurrentEnergy / 2);
                    male.ReceiveMateResponse(true, this.gameObject);
                    var babyGene = Gene.combine(SelfGene, male.SelfGene, environment.mutation);
                    var nextGen = ((gen > male.GetGeneration()) ? gen : male.GetGeneration()) + 1;
                    Create(babyGene, interactor.prefab, transform.position + new Vector3(2, 0, 2), transform.rotation, nextGen,GetComponentInChildren<Renderer>().material.color);
                    childrenCount ++;
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
                childrenCount ++;
            } else {
                OrgState = OrganismState.IDLE;
            }
        }

        public void OnMateLeftRange(GameObject mate) {
            
        }

        private void OnDestroy() {
            Debug.Log(gene.species + " --- " + causeOfDeath + " <-");
        }
    }
}
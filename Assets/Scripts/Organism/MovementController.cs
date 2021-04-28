using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Organism {

    // This class will solely control ONLY Movement (walking running stopping etc)
    public class MovementController: MonoBehaviour {

        private IBrain brain;
        private Rigidbody body;
        public float wandering = 0.3f;
        public float steering = 1f;

        private float range = 10f;
        private float angle = 120f;
        private float rotSpeed=7f;

        private float walkSpeed;
        private float sprintSpeed;
        private float attackRange=4f;

        private Vector3 desiredDir;

        private GameObject target = null;

        private float timeSinceToppled = 0f;

        private void Start() {
            body = GetComponent<Rigidbody>();
            brain = GetComponent<IBrain>();
        }

        public void UpdateTarget(GameObject target) {
            this.target = target;
        }

        public void SetupGene(Gene gene) {
            this.walkSpeed = gene.walkSpeed;
            this.sprintSpeed = gene.sprintSpeed;
            this.range=gene.range;
            this.attackRange=gene.attackRange;
        }

        private void FixedUpdate() {
            
            if (Vector3.Angle(transform.up, Vector3.up) > 60f) {
                timeSinceToppled += Time.deltaTime;
                if (timeSinceToppled >= 2f) {
                    UnTopple();
                    timeSinceToppled = 0f;
                }
            } else if (IsMoving()) {
                body.angularVelocity = Vector3.zero;
                body.velocity = Vector3.zero;
            }

            //desiredDir = (desiredDir + GetLevelledDir() * wandering + transform.forward * steering).normalized;
            //MoveTo(desiredDir.normalized, 5, 7);

            switch (brain.OrgState) {
                case OrganismState.SEARCHING_MATE:
                case OrganismState.SEEKING_FOOD:
                case OrganismState.IDLE: {
                    desiredDir = (desiredDir + GetLevelledDir() * wandering + transform.forward * steering).normalized;
                    MoveTo(desiredDir.normalized, walkSpeed, rotSpeed);
                    break;
                }
                case OrganismState.CHASING_FOOD: {
                    if (target != null) {
                        desiredDir = (target.transform.position - transform.position);
                        desiredDir.y = 0f;
                        if (desiredDir.sqrMagnitude > (range * range * 4)) {
                            brain.OnHuntTargetLeft(target);
                        } else if (desiredDir.sqrMagnitude > (attackRange*attackRange)) {
                            MoveTo(desiredDir.normalized, sprintSpeed, rotSpeed);
                        } else {
                            // Here the body has reached the attack range
                            body.velocity = Vector3.zero;
                            body.angularVelocity = Vector3.zero;
                            brain.Velocity = Vector3.zero;
                            brain.OnTargetInAttackRange(target);
                        }
                    }
                    break;
                }
                case OrganismState.CHASING_MATE: {
                    if (target != null) {
                        desiredDir = (target.transform.position - transform.position);
                        desiredDir.y = 0f;
                        if (desiredDir.sqrMagnitude > (attackRange*attackRange)) {
                            MoveTo(desiredDir.normalized, walkSpeed, rotSpeed);
                        } else {
                            // Here the body has reached the attack range
                            body.velocity = Vector3.zero;
                            body.angularVelocity = Vector3.zero;
                            brain.Velocity = Vector3.zero;
                            brain.OnMateInRange(target);
                        }
                    }
                    break;
                }
                case OrganismState.REST: {
                    Debug.Log("REST State");
                    body.velocity = Vector3.zero;
                    body.angularVelocity = Vector3.zero;
                    break;
                }
                case OrganismState.EVADING: {
                    desiredDir = (desiredDir + GetLevelledDir() * wandering + transform.forward * steering).normalized;
                    MoveTo(desiredDir, sprintSpeed, rotSpeed);
                    break;
                }
            }
        }

        public void StartEvasion(GameObject target) {
            desiredDir = Quaternion.AngleAxis(120, transform.up) * desiredDir;
            body.MoveRotation(Quaternion.LookRotation(desiredDir));
        }

        private void UnTopple() {
            transform.up = Vector3.up;
            body.angularVelocity = Vector3.zero;
            body.velocity = Vector3.zero;
        }

        private void MoveTo(Vector3 dir, float moveSpeed, float rotSpeed) {
            if (dir.sqrMagnitude > 0f) {
                var rotation = Quaternion.LookRotation(dir, transform.up);
                body.MoveRotation(Quaternion.Lerp(body.rotation, rotation, Time.deltaTime * rotSpeed));
                brain.Velocity = dir * moveSpeed;
                body.MovePosition(transform.position + brain.Velocity * Time.deltaTime);
                // Debug.Log(brain.Velocity.sqrMagnitude + " ---- " + body.velocity.sqrMagnitude);
            }
        }

        private Vector3 GetLevelledDir() {
            Vector2 random = Random.insideUnitCircle.normalized;
            Vector3 pos = new Vector3(random.x, 0, random.y);
            return pos;
        }

        private bool IsMoving() {
            return body.angularVelocity.sqrMagnitude > 0.01f || body.velocity.sqrMagnitude > 0.01f;
        }

        private void OnCollisionStay(Collision other) {
            if (other.gameObject.tag == "walls") {
                desiredDir = other.GetContact(0).normal;
                MoveTo(desiredDir.normalized, walkSpeed, rotSpeed);
            }
        }
    }
}
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

            

            switch (brain.OrgState) {
                case OrganismState.IDLE: {
                    desiredDir = (desiredDir + GetLevelledDir() * wandering + transform.forward * steering).normalized;
                    MoveTo(desiredDir.normalized, 5, 7);
                    break;
                }
                case OrganismState.CHASING_FOOD: {
                    if (target != null) {
                        desiredDir = (target.transform.position - transform.position);
                        desiredDir.y = 0f;
                        if (desiredDir.sqrMagnitude > 16) {
                            MoveTo(desiredDir.normalized, 10, 7);
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
                case OrganismState.REST: {
                    Debug.Log("REST State");
                    body.velocity = Vector3.zero;
                    body.angularVelocity = Vector3.zero;
                    break;
                }
            }
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
                MoveTo(desiredDir.normalized, 5f, 7f);
            }
        }
    }
}
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
            desiredDir = transform.forward;
            GetComponent<SphereCollider>().radius = range;
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
            } else if (target == null && !IsMoving()) {
                UnTopple();
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
                var velocity = dir * moveSpeed;
                body.MovePosition(transform.position + velocity * Time.deltaTime);
            }
        }

        private Vector3 GetLevelledDir() {
            Vector2 random = Random.insideUnitCircle;
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

        private void OnDrawGizmos() {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * range);
            var leftDir = - Mathf.Sin(angle * Mathf.Deg2Rad / 2f) * transform.right + Mathf.Cos(angle * Mathf.Deg2Rad / 2f) * transform.forward;
            var rightDir = Mathf.Sin(angle * Mathf.Deg2Rad / 2f) * transform.right + Mathf.Cos(angle * Mathf.Deg2Rad / 2f) * transform.forward;
            Gizmos.DrawLine(transform.position, transform.position + leftDir * range);
            Gizmos.DrawLine(transform.position, transform.position + rightDir * range);
        }
    }
}
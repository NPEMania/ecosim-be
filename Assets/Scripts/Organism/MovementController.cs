using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Organism {
    public class MovementController: MonoBehaviour {

        private Rigidbody body;
        private float health;
        public float wandering = 4f;
        public float steering = 1f;

        private State state = State.IDLE;

        private Vector3 position;
        private Vector3 velocity;
        private Vector3 desiredDir;

        private void Start() {
            body = GetComponent<Rigidbody>();
            desiredDir = transform.forward;
        }

        void Update() {
            
        }

        private void FixedUpdate() {
            desiredDir = (desiredDir + GetLevelledDir() * wandering + transform.forward * steering).normalized;
            MoveTo(desiredDir, 5, 7);
        }

        private void MoveWithControls() {
            var dir = new Vector3(Input.GetAxis("Horizontal"), 0 , Input.GetAxis("Vertical")).normalized;
            if (dir.sqrMagnitude > 0f) {
                var rotation = Quaternion.LookRotation(dir, transform.up);
                body.MoveRotation(Quaternion.Lerp(body.rotation, rotation, Time.deltaTime * 3f));
                var velocity = dir * 3;
                body.MovePosition(transform.position + velocity * Time.deltaTime);
            }
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
    }

    enum State {
        IDLE, CHASING, ATTACKING
    }
}
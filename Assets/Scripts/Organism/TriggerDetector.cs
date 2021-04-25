using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Organism {

    public class TriggerDetector: MonoBehaviour {

        private float range = 10f;
        private float angle = 120f;

        private GameObject target;
        private IBrain brain;

        private void Start() {
            brain = GetComponent<IBrain>();
            GetComponent<SphereCollider>().radius = range; // collider.radius = range / scale;
        }

        private void OnTriggerStay(Collider other) {
            if (other.gameObject.tag == "mover") {
                if (other.gameObject != target) {
                    target = other.gameObject;
                    brain.OnHuntTargetAcquired(other.gameObject);
                }
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.gameObject == target) {
                Debug.Log("Trigger Detector");
                brain.OnHuntTargetLeft(other.gameObject);
                target = null;
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
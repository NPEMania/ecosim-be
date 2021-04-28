using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Organism {

    public class TriggerDetector: MonoBehaviour {

        private float range = 10f;
        private float angle = 120f;
        private float scale = 1f;

        private GameObject target;
        private GameObject mate;
        private IBrain brain;

        private void Awake() {
            brain = GetComponent<IBrain>();
            GetComponent<SphereCollider>().radius = (range/scale); // collider.radius = range / scale;
        }

        private void OnTriggerStay(Collider other) {
            if (other.gameObject.tag == "mover") {
                var otherBrain = other.gameObject.GetComponent<IBrain>();
                if (brain.OrgState == OrganismState.SEEKING_FOOD) {
                    // Allow to scan for food targets
                    // Add angle check
                    // Check if gene is not same
                    if (brain.SelfGene.species != otherBrain.SelfGene.species) {
                        brain.OnHuntTargetAcquired(other.gameObject);
                    }
                } else if (brain.OrgState == OrganismState.SEARCHING_MATE) {
                    // Allow to scan mates
                
                    if (otherBrain.SelfGene.species == brain.SelfGene.species
                        && otherBrain.SelfGene.gender != brain.SelfGene.gender
                        && otherBrain.OrgState == OrganismState.SEARCHING_MATE) {
                        brain.OnMateAcquired(other.gameObject);
                    }
                }
            }
            /*if (other.gameObject.tag == "mover") {
                if (other.gameObject != target) {
                    target = other.gameObject;
                    brain.OnHuntTargetAcquired(other.gameObject);
                }
            }*/
        }
        public void SetupGene(Gene gene) {
            this.range=gene.range;
            this.scale=gene.scale;
            GetComponent<SphereCollider>().radius = (range/scale);
        }

        private void OnTriggerExit(Collider other) {
            if (brain.OrgState == OrganismState.SEEKING_FOOD) {
                // Allow to scan for food targets
            } else if (brain.OrgState == OrganismState.SEARCHING_MATE) {
                // Allow to scan mates
            }
            /*if (other.gameObject == target) {
                Debug.Log("Trigger Detector");
                brain.OnHuntTargetLeft(other.gameObject);
                target = null;
            }*/
        }

        
        private void OnDrawGizmos() {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * range / scale);
            var leftDir = - Mathf.Sin(angle * Mathf.Deg2Rad / 2f) * transform.right + Mathf.Cos(angle * Mathf.Deg2Rad / 2f) * transform.forward;
            var rightDir = Mathf.Sin(angle * Mathf.Deg2Rad / 2f) * transform.right + Mathf.Cos(angle * Mathf.Deg2Rad / 2f) * transform.forward;
            Gizmos.DrawLine(transform.position, transform.position + leftDir * range / scale);
            Gizmos.DrawLine(transform.position, transform.position + rightDir * range / scale);
        }
    }

    
    enum SearchMode {
        SEARCH_PREY, SEARCH_MATE
    }
}
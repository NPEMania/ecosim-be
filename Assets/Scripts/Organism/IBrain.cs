using UnityEngine;

namespace Organism {
    
    public interface IBrain {
        OrganismState OrgState {
            get;
            set;
        }

        Vector3 Velocity {
            get; set;
        }

        void SetupGenes(Gene gene);

        void OnStateChanged(OrganismState newState);

        void OnHuntTargetAcquired(GameObject target);

        void OnHuntTargetLeft(GameObject target);

        void OnMateAcquired(GameObject mate);

        void OnTargetInAttackRange(GameObject target);

        void OnTargetLeftAttackRange(GameObject target);
    }
}
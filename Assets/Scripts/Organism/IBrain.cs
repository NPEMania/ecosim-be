using UnityEngine;

namespace Organism {
    
    public interface IBrain {
        OrganismState OrgState {
            get;
            set;
        }

        void OnStateChanged(OrganismState newState);

        void OnHuntTargetAcquired(GameObject target);

        void OnHuntTargetLeft(GameObject target);

        void OnMateAcquired(GameObject mate);

        void OnTargetInAttackRange(GameObject target);

        void OnTargetLeftAttackRange(GameObject target);
    }
}
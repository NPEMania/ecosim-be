using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Organism {
    public class Gene {

        public readonly float range;
        public readonly float angle;
        public readonly float sprintSpeed;
        public readonly float walkSpeed;
        public readonly float maxHP;
        public readonly float maxEnergy;
        public readonly float maxStamina;
        public readonly float attack;
        public readonly float attackGap; // In seconds;
        public readonly float damage;
        public readonly float defense;
    }
}

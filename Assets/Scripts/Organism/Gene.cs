using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Organism {
    public class Gene {
        public readonly String species;
        public readonly String[] preys; // insert species
        public readonly String[] predators; // insert species
        public readonly Gender gender;
        public readonly float range; // collider.radius = range / scale;
        public readonly float angle;
        public readonly float sprintSpeed;
        public readonly float walkSpeed;
        public readonly float maxHP;
        public readonly float maxEnergy;
        public readonly float maxStamina;
        public readonly float attack;
        public readonly float attackGap; // In seconds;
        public readonly float attackRange;
        public readonly float defense;
        public readonly float scale;
        public readonly float urgeRate;
        public readonly float evadeCooldown; // In seconds;

        public float[] ToArray() {
            return new float[] {
                range, angle, sprintSpeed, walkSpeed, 
                maxHP, maxEnergy, maxStamina, attack,
                attackGap, attackRange, defense, scale,
                urgeRate
            };
        }

        public static Gene FromArray(string species, float[] array) {
            return new Gene(species, array);
        }

        public Gene(string species, float[] a) {
            int random = (new System.Random()).Next(0, 2);
            if (random == 0) gender = Gender.MALE;
            else gender = Gender.FEMALE;
            this.species = species;
            range = a[0];
            angle = a[1];
            sprintSpeed = a[2];
            walkSpeed = a[3];
            maxHP = a[4];
            maxEnergy = a[5];
            maxStamina = a[6];
            attack = a[7];
            attackGap = a[8];
            attackRange = a[9];
            defense = a[10];
            scale = a[11];
            urgeRate = a[12];
        }

        public Gene(string species, Gender gender, float[] a) {
            this.gender = gender;
            this.species = species;
            range = a[0];
            angle = a[1];
            sprintSpeed = a[2];
            walkSpeed = a[3];
            maxHP = a[4];
            maxEnergy = a[5];
            maxStamina = a[6];
            attack = a[7];
            attackGap = a[8];
            attackRange = a[9];
            defense = a[10];
            scale = a[11];
            urgeRate = a[12];
        }
    }
}

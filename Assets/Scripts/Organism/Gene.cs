using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;
using Organism;
using JetBrains.Annotations;

namespace Organism {
    [Serializable]
    public class Gene {
        public readonly String species;
        public readonly OrganismType organismType;
        public readonly DietType dietType;
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
        public readonly float lifespan;

        public float[] ToArray() {
            return new float[] {
                range, angle, sprintSpeed, walkSpeed, 
                maxHP, maxEnergy, maxStamina, attack,
                attackGap, attackRange, defense, scale,
                urgeRate, evadeCooldown, lifespan
            };
        }

        public static Gene FromArray(string species, OrganismType organismType, DietType diet, float[] array) {
            return new Gene(species, organismType, diet, array);
        }

        public Gene(string species, OrganismType organismType, DietType diet, float[] a) {
            int random = (new System.Random()).Next(0, 2);
            if (random == 0) gender = Gender.MALE;
            else gender = Gender.FEMALE;
            this.species = species;
            this.organismType = organismType;
            this.dietType = diet;
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
            evadeCooldown = a[13];
            lifespan = a[14];
        }

        public Gene(string species, Gender gender, OrganismType organismType, DietType diet,  float[] a) {
            this.gender = gender;
            this.species = species;
            this.organismType = organismType;
            this.dietType = diet;
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
            evadeCooldown = a[13];
            lifespan = a[14];
        }

        // mutation between 0 to 99
        public static Gene combine(Gene one, Gene two, float mutation) {
            float[] a = one.ToArray();
            float[] b = two.ToArray();
            float[] c = new float[a.Length];
            for (int i = 0; i < c.Length; ++i) {
                c[i] = (a[i] + b[i]) / 2 * (1 + UnityEngine.Random.Range(-mutation, mutation) / 100);
            }
            return new Gene(one.species, one.organismType, one.dietType, c);
        }

        public Gene mutate(float mutation) {
            float[] a = ToArray();
            float[] b = new float[a.Length];
            for (int i = 0; i < a.Length; ++i) {
                b[i] = a[i] * (1 + UnityEngine.Random.Range(-mutation, mutation) / 100);
            }
            return new Gene(species, organismType, dietType, b);
        }

        public Gene(GeneInput input) {
            int random = (new System.Random()).Next(0, 2);
            if (random == 0) gender = Gender.MALE;
            else gender = Gender.FEMALE;
            this.species = input.species;
            //Debug.Log("from gene input " + input.species);
            this.organismType = input.organismType;
            this.dietType = input.dietType;
            range = input.range;
            angle = input.angle;
            sprintSpeed = input.sprintSpeed;
            walkSpeed = input.walkSpeed;
            maxHP = input.maxHP;
            maxEnergy = input.maxEnergy;
            maxStamina = input.maxStamina;
            attack = input.attack;
            attackGap = input.attackGap;
            attackRange = input.attackRange;
            defense = input.defense;
            scale = input.scale;
            urgeRate = input.urgeRate;
            evadeCooldown = input.evadeCooldown;
            lifespan = input.lifespan;
        }
    }

    [System.Serializable]
    public class GeneInput {
        public String species;
        public OrganismType organismType;
        public DietType dietType;
        public float range; // collider.radius = range / scale;
        public float angle;
        public float sprintSpeed;
        public float walkSpeed;
        public float maxHP;
        public float maxEnergy;
        public float maxStamina;
        public float attack;
        public float attackGap; // In seconds;
        public float attackRange;
        public float defense;
        public float scale;
        public float urgeRate;
        public float evadeCooldown; // In seconds;
        public float lifespan;

        public override string ToString() {
            return "Gene Input: " + this.species + " blabla " + this.attackRange;
        }
    }

    [System.Serializable]
    public class GeneCollection {
        public GeneInput[] genes; 
    }
     [System.Serializable]
    public class GeneCountCollection {
        public int[] genesCount; 
    }
    
}

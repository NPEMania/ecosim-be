using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using Organism;
using UnityEngine;

[Serializable]
public class OrganismExportData {
    public int generation;
    public   String species;
    public   String organismType;
    public   String dietType;
    public   String gender;
    public   float range; // collider.radius = range / scale;
    public   float angle;
    public   float sprintSpeed;
    public   float walkSpeed;
    public   float maxHP;
    public   float maxEnergy;
    public   float maxStamina;
    public   float attack;
    public   float attackGap; // In seconds;
    public   float attackRange;
    public   float defense;
    public   float scale;
    public   float urgeRate;
    public   float evadeCooldown; // In seconds;
    public   float lifespan;

    public  int encounters;
    public int kills;
    public int evasions;
    public float timeSinceAlive;
    public int children;
    public String causeOfDeath;

    public OrganismExportData(Gene gene, FSMBrain brain) {
        generation = brain.GetGeneration();
        encounters = brain.encounters;
        kills = brain.killSuccess;
        evasions = brain.evasions;
        timeSinceAlive = brain.timeSinceAlive;
        causeOfDeath = brain.causeOfDeath;
        this.gender = gene.gender.ToString();
        this.species = gene.species;
        this.organismType = gene.organismType.ToString();
        this.dietType = gene.dietType.ToString();
        range = gene.range;
        angle = gene.angle;
        sprintSpeed = gene.sprintSpeed;
        walkSpeed = gene.walkSpeed;
        maxHP = gene.maxHP;
        maxEnergy = gene.maxEnergy;
        maxStamina = gene.maxStamina;
        attack = gene.attack;
        attackGap = gene.attackGap;
        attackRange = gene.attackRange;
        defense = gene.defense;
        scale = gene.scale;
        urgeRate = gene.urgeRate;
        evadeCooldown = gene.evadeCooldown;
        lifespan = gene.lifespan;
    }
}

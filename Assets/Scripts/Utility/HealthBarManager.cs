using System.Collections;
using System.Collections.Generic;
using Health;
using Organism;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour {

    public IBrain brain;
    public Damageable self;
    public Text state;
    public Text species;
    public Slider hp;
    public Slider energy;
    public Slider stamina;
    public Camera sceneCamera;
    public Canvas canvas;
    void Start() {
        brain = GetComponent<IBrain>();
        species.text = brain.SelfGene.species;
        self = GetComponent<Damageable>();
        sceneCamera = Camera.main;
    }

    void Update() {
        state.text = brain.OrgState.ToString();
        species.text = brain.SelfGene.species;
        hp.value = self.CurrentHP / brain.SelfGene.maxHP;
        energy.value = self.CurrentEnergy / brain.SelfGene.maxEnergy;
        stamina.value = self.CurrentStamina / brain.SelfGene.maxStamina;
        canvas.transform.LookAt(sceneCamera.transform.position);
    }
}

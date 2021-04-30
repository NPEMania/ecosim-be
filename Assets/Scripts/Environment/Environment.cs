using Organism;
using UnityEditor;
using UnityEngine;

public class Environment : MonoBehaviour {

    public GameObject plantPrefab;

    private void Start() {
        //SpawnPlant(Vector3.zero, SampleGenes.plantGene);
    }

    public void SpawnPlant(Vector3 position, Gene gene) {
        Vector3 spawnPosition = position + UtilityMethods.OnUnitCircle() * 10;
        PlantBrain.Create(gene, plantPrefab, spawnPosition, Quaternion.identity);
    }
}

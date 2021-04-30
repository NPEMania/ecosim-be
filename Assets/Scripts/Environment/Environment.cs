using Organism;
using UnityEditor;
using UnityEngine;

public class Environment : MonoBehaviour {

    public GameObject plantPrefab;

    public void SpawnPlant(Vector3 position, Gene gene) {
        Vector3 spawnPosition = position + UtilityMethods.OnUnitCircle() * 10;
        PlantBrain.Create(gene, plantPrefab, position, Quaternion.identity);
    }
}

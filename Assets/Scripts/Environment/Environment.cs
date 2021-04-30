using System;
using System.IO;
using Organism;
using UnityEngine;

public class Environment : MonoBehaviour {

    public GameObject plantPrefab;
    public GameObject animalPrefab;
    [Range(0, 20)] public float mutation;
    private StreamWriter writer;
    public TextAsset jsonInput;
    private String outPath = "/outputData.txt";

    private void Start() {
        //SpawnPlant(Vector3.zero, SampleGenes.plantGene);
        Debug.Log(jsonInput.text);
        GeneCollection collection = JsonUtility.FromJson<GeneCollection>(jsonInput.text);
        SpawnInitialAnimals(collection.genes);
        outPath = Application.dataPath + outPath;
        writer = new StreamWriter(File.Open(outPath, FileMode.Create));
        writer.WriteLine("[");
    }

    private void SpawnInitialAnimals(GeneInput[] geneInputs) {
        foreach (GeneInput g in geneInputs) {
            Debug.Log(JsonUtility.ToJson(g));
            Gene gene = new Gene(g);
            float x = UnityEngine.Random.Range(-10f, 10f);
            float z = UnityEngine.Random.Range(-10f, 10f);
            float y = gene.scale;
            FSMBrain.Create(gene, animalPrefab, new Vector3(x, y, z), Quaternion.identity, 0);
        }
    }

    public void SpawnPlant(Vector3 position, Gene gene) {
        Vector3 spawnPosition = position + UtilityMethods.OnUnitCircle() * 10;
        PlantBrain.Create(gene, plantPrefab, spawnPosition, Quaternion.identity);
    }

    public void RegisterDeath(OrganismExportData data) {
        writer.WriteLine(JsonUtility.ToJson(data) + ",");
    }

    private void OnApplicationQuit() {
        writer.WriteLine("]");
        writer.Close();
    }
}

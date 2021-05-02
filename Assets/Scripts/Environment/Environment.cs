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
    
    public TextAsset jsonInputCount;
    private String outPath = "/outputData.txt";

    private void Start() {
        //SpawnPlant(Vector3.zero, SampleGenes.plantGene);
        //Debug.Log(jsonInput.text);
        GeneCollection collection = JsonUtility.FromJson<GeneCollection>(jsonInput.text);
        GeneCountCollection collectionCount = JsonUtility.FromJson<GeneCountCollection>(jsonInputCount.text);
       
        SpawnInitialAnimals(collection.genes,collectionCount.genesCount);
        outPath = Application.dataPath + outPath;
        writer = new StreamWriter(File.Open(outPath, FileMode.Create));
        writer.WriteLine("[");
    }

    private void SpawnInitialAnimals(GeneInput[] geneInputs,int[] genesCount) {
        foreach (GeneInput g in geneInputs) {
            //Debug.Log(JsonUtility.ToJson(g));
            Gene gene = new Gene(g);
            Color background = new Color(
                 UnityEngine.Random.Range(0f, 1f), 
                 UnityEngine.Random.Range(0f, 1f), 
                 UnityEngine.Random.Range(0f, 1f)
                 );
            //Debug.Log("Gene from gene input: " + gene.species + " --- " + gene.maxHP + " --- " + gene.scale);
            foreach(int count in genesCount)
            {
                
                for(int i=0;i<count;i++)
                {
                    float x = UnityEngine.Random.Range(-200f, 200f );
                    float z = UnityEngine.Random.Range(-200f, 200f );
                    float y = gene.scale;
                    Vector3 vec=new Vector3(x,y,z);
                    FSMBrain.Create(gene, animalPrefab, vec, Quaternion.LookRotation(Vector3.zero-vec,Vector3.up), 0,background);
                    
                }
            }
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

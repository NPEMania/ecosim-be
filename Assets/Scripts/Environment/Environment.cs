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
       SpawnPlantsInVicinity(8);
        outPath = Application.dataPath + outPath;
        writer = new StreamWriter(File.Open(outPath, FileMode.Create));
        writer.WriteLine("[");
    }

    private void SpawnInitialAnimals(GeneInput[] geneInputs,int[] genesCount) {
        int j=0;
        foreach (GeneInput g in geneInputs) {
            //Debug.Log(JsonUtility.ToJson(g));
            
           
            Color background = new Color(
                UnityEngine.Random.Range(0f, 1f), 
                UnityEngine.Random.Range(0f, 1f), 
                UnityEngine.Random.Range(0f, 1f)
            );
            //Debug.Log("Gene from gene input: " + gene.species + " --- " + gene.maxHP + " --- " + gene.scale);
             
            for (int i=0;i<genesCount[j];i++) {
                Gene gene = new Gene(g);
                Debug.Log("kkkk  "+gene.gender);
                if(i%2==0) {
                    gene.gender=Gender.MALE;
                }else{
                    gene.gender=Gender.FEMALE;
                }
                Debug.Log("kkkk  "+gene.gender);
                float x = UnityEngine.Random.Range(-50f, 50f );
                float z = UnityEngine.Random.Range(-50f, 50f );
                float y = gene.scale;
                Vector3 vec=new Vector3(x,y,z);
                Vector3 center = new Vector3(0, y, 0);
                FSMBrain.Create(gene, animalPrefab, vec, Quaternion.LookRotation(center - vec, Vector3.up), 0,background);
            }
                j++;
        }
    }

    public void SpawnPlantsInVicinity(int count){
        for(int i=0;i<count;i++){
            float x = UnityEngine.Random.Range(-100f, 100f );
            float z = UnityEngine.Random.Range(-100f, 100f );
            float y = 0;
            Vector3 vec=new Vector3(x,y,z);
            // Vector3 spawnPosition = vec + UtilityMethods.OnUnitCircle() * 10;
            PlantBrain.Create(SampleGenes.plantGene, plantPrefab, vec, Quaternion.identity);
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

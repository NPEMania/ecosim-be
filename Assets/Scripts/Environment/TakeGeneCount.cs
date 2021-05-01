using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeGeneCount : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject textTemplate=transform.GetChild(0).gameObject;
        GameObject g;
        for(int i=0;i<10;i++){
            g=Instantiate(textTemplate,transform);
        }
        Destroy(textTemplate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

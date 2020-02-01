using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patientController : MonoBehaviour
{
    public int combinationSize;
    private string[] combination;
    private string[] possibleKeys = { "A", "B", "X", "Y", "U", "D", "L", "R"};
    
    // Start is called before the first frame update
    void Start()
    {
               
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void init() {
        combination = new string[combinationSize];
        for (int i = 0; i < combinationSize; ++i)
        {
            int choice = Random.Range(0, possibleKeys.Length);
            combination[i] = possibleKeys[choice];
            //Debug.Log(combination[i]);
        }
    }
    public string[] getCombination() {
        return this.combination;
    }
}

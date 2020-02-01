using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class hudController : MonoBehaviour
{
    public GameObject combinationPanel;
    private Image[] combinationElements;
    private Transform currentInstanceTransform;

    public Sprite[] btnSprites;
    public Sprite[] btnSpritesPressed;
    public Sprite[] btnSpritesFailed;

    private string[] currentCombinationNames;
    /*
     0: ButtonA
     1: ButtonB
     2: ButtonX
     3: ButtonY
     4: DpadUp
     5: DpadDown
     6: DpadLeft
     7: DpadRight
     */
     // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void setCombinationButton(int index, string mode) {
        Sprite[] currentBtnSprites = btnSprites;
        switch (mode)
        {
            case "pressed":
                currentBtnSprites = btnSpritesPressed;
                break;
            case "failed":
                currentBtnSprites = btnSpritesFailed;
                break;
            case "normal":
                break;
            default:
                break;
        }
        switch (currentCombinationNames[index])
        {
            case "A":
                combinationElements[index].GetComponent<Image>().sprite = currentBtnSprites[0];
                break;
            case "B":
                combinationElements[index].GetComponent<Image>().sprite = currentBtnSprites[1];
                break;
            case "X":
                combinationElements[index].GetComponent<Image>().sprite = currentBtnSprites[2];
                break;
            case "Y":
                combinationElements[index].GetComponent<Image>().sprite = currentBtnSprites[3];
                break;
            case "U":
                combinationElements[index].GetComponent<Image>().sprite = currentBtnSprites[4];
                break;
            case "D":
                combinationElements[index].GetComponent<Image>().sprite = currentBtnSprites[5];
                break;
            case "L":
                combinationElements[index].GetComponent<Image>().sprite = currentBtnSprites[6];
                break;
            case "R":
                combinationElements[index].GetComponent<Image>().sprite = currentBtnSprites[7];
                break;
            default:
                combinationElements[index].GetComponent<Image>().sprite = currentBtnSprites[0];
                break;
        }
    }

    public void setCombination(string[] combination, Canvas parent) {
        combinationElements = new Image[combination.Length];
        currentCombinationNames = combination;
        for (int i = 0; i < combinationElements.Length; ++i)
        {
            Destroy(combinationElements[i]);
        }
        GameObject combination_panel = parent.transform.Find("combination_panel").gameObject;
        foreach (Transform child in combination_panel.transform ) {
                Destroy(child.gameObject);
        }
        RectTransform rt = combination_panel.GetComponent<RectTransform>();
        currentInstanceTransform = parent.transform;
         
       // currentInstanceTransform.GetComponent<HorizontalLayoutGroup>().transform.= Vector3.one;

        for (int i = 0;i< currentCombinationNames.Length;++i) {
            combinationElements[i] = new GameObject().AddComponent<Image>(); //Instantiate(btnImage, currentInstanceTransform.FindChild("combination_panel"));
            combinationElements[i].transform.SetParent(combination_panel.transform);
            setCombinationButton(i, "normal");
            combinationElements[i].transform.localScale = Vector3.one * 1f;
        }
    }

    public void setTimer(int time,Transform parent) {
        Text timerText =  parent.Find("timer").GetComponent<Text>();
        timerText.text = "Time (Seconds): " + time.ToString();
    }
}

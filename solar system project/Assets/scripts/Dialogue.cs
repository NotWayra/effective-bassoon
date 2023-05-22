using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public TextMeshProUGUI labelTextComponent;
    public string[] lines;
    public float textSpeed;
    public int idx;
    public bool done = false;
    public GameObject yes, no;
    public int[] choiceLines;
    public int choiceIdx;
    public int[] skipLines;
    public int skipIdx;
    public YesNoPair[] skip;
    public bool yesActive, noActive;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
        yes.SetActive(false);
        no.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (textComponent.text == lines[idx] && idx != choiceLines[choiceIdx]) {
                NextLine();
            } else {
                StopAllCoroutines();
                textComponent.text = lines[idx];
                
            }
        }
    }
    public void StartDialogue() {
        idx = 0;
        choiceIdx = 0;
        skipIdx = 0;
        StartCoroutine(TypeLine());
    }
    IEnumerator TypeLine() {
        //type out each character 1 by 1
        foreach(char c in lines[idx].ToCharArray()) {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
    void NextLine() {
        // Debug.Log(idx);
        // for (int i = 0; i < choiceLines.Length; i++) {
        //     Debug.Log(choiceLines[i]);
        // }
        Debug.Log(choiceLines[choiceIdx]);
        if (idx < lines.Length - 1) {
            idx++;
            if (yesActive || noActive) {
                if(yesActive && idx > skip[skipIdx].yesE) {
                    idx = skip[skipIdx].noE + 1;
                    yesActive = false;
                    noActive = false;
                    skipIdx++;
                } else if (noActive && idx > skip[skipIdx].noE) {
                    yesActive =false;
                    noActive = false;
                    skipIdx++;
                }
            }
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
            yes.SetActive(idx == choiceLines[choiceIdx]);
            no.SetActive(idx == choiceLines[choiceIdx]);
        } else {
            lines = null;
            textComponent.text = "";
            gameObject.SetActive(false);  
            done = true;
        }
    }
    void PreivousLine() {
        if (idx > 1) {
            idx -= 1;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
            if (idx == choiceLines[choiceIdx]) {
                yes.SetActive(true);
                no.SetActive(true);
            } else {
                yes.SetActive(false);
                no.SetActive(false);
            }
        } else {
            lines = null;
            textComponent.text = "";
            gameObject.SetActive(false);  
            done = true;
        }
    }
    public void yesPress() {
        
        
            idx = skip[skipIdx].yesS;
            noActive = false;
            yesActive = true;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
            yes.SetActive(idx == choiceLines[choiceIdx]);
            no.SetActive(idx == choiceLines[choiceIdx]);
            choiceIdx += choiceIdx < choiceLines.Length - 1? 1: 0;

    }
    public void noPress() {
        
        
            idx = skip[skipIdx].noS;
            noActive = true;
            yesActive = false;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
            yes.SetActive(idx == choiceLines[choiceIdx]);
            no.SetActive(idx == choiceLines[choiceIdx]);
            choiceIdx += choiceIdx < choiceLines.Length - 1? 1: 0;

    }


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    // [SerializeField] private string InteractText;
    [SerializeField] private string[] dialogues;
    [SerializeField] private int[] choices;
    



    public GameObject dialogue;
    private Dialogue dialogueComponent;

    public bool isInteracting = false;
    public int[] skipLines;
    [SerializeField] public YesNoPair[] skip;
  
    void start(){
        dialogueComponent = dialogue.GetComponent<Dialogue>();
    }

    void Update(){
        try{
            if(dialogueComponent.done){
                isInteracting = false;
            }
        }catch{

        }
        
    }
    public void Interact(){
        
        if(isInteracting == false){
            isInteracting = true;
            Debug.Log("Interact!");
            dialogue.SetActive(true);
            dialogueComponent = dialogue.GetComponent<Dialogue>();
            dialogueComponent.lines = dialogues;
            dialogueComponent.choiceLines = choices;
            dialogueComponent.skip = skip;
            dialogueComponent.StartDialogue();
            if (dialogueComponent.done) {
                dialogueComponent.done = false;
            }
        } else{
            Debug.Log("Cannot Interact");
        }
            
    }



    public Transform GetTransform(){
        return transform;
    }
}

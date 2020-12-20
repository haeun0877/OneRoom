using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] DialogueEvent dialogue;

    public Dialogue[] GetDialogues()
    {
        DialogueEvent t_DialogueEvent = new DialogueEvent();
        t_DialogueEvent.dialogues = DatabaseManager.instance.GetDialogues((int)dialogue.line.x, (int)dialogue.line.y);


        for (int i = 0; i < dialogue.dialogues.Length; i++)
        {
            t_DialogueEvent.dialogues[i].tf_Target = dialogue.dialogues[i].tf_Target;
            t_DialogueEvent.dialogues[i].cameraType = dialogue.dialogues[i].cameraType;
        }

        dialogue.dialogues = t_DialogueEvent.dialogues;
         return dialogue.dialogues;
    }

}

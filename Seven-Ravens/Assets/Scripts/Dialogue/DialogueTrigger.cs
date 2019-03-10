using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField]
        private Dialogue dialogue;

        public void TriggerDialogue()
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        }

    }
}
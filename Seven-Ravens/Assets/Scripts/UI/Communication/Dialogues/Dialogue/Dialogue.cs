using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rokemon
{
    [System.Serializable]
    public class Dialogue
    {   
        public string speakerTag;

        public bool moveSpeaker;

        public bool displayName = true;
        public bool displayContinue = true;
        
        public bool displayDialogue = true; 

        public bool enableSpeakerVisibility;

        public int[] moveSentenceIndexes;

        public string speakerName;

        [TextArea(3, 10)]
        public string[] sentences;

    }
}
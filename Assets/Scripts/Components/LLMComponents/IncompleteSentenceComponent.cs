using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using UnityEngine;

namespace EducationalGame.Component
{
    public class IncompleteSentenceComponent : IComponent
    {
        public string incompleteSentence;       // This is the Incomplete sentence

        public List<string> corpus;             // Hints
        public string fullSentence;             // The Complete Sentence
        public string currentInput;             // Record user input
        public int missingWordCount;

        public string answer;                   // Missing words
        

        public void InitComponent()
        {
            
        }
    }
}

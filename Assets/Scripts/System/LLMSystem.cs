using System.Collections;
using System.Collections.Generic;
using EducationalGame.Component;
using EducationalGame.Core;
using TMPro;
using UnityEngine;

namespace EducationalGame
{
    public class LLMSystem : ISystem
    {
        private static LLMPuzzle puzzle;
        private static bool requireCheck;
        public void Init()
        {
            SystemManager.interactSystem.OnLLMInputChanged += RequireCheck;
            foreach(LLMPuzzle puzzleInstance in Constants.Game.llmPuzzles)
            {
                puzzleInstance.OnEnableTrigger += UpdatePuzzle;
                puzzleInstance.OnEnableTrigger += DisplayCorpus;
            }
        }

        public void Process()
        {
            
        }

        public void Update()
        {
            if (puzzle == null) return;
            if (!requireCheck) return;

            IncompleteSentenceComponent senC = EntityManager.Instance.GetComponent<IncompleteSentenceComponent>(puzzle.SentenceEntity.ID);

            if (senC.currentInput == senC.answer)
            {
                // Solved a puzzle
                Debug.Log("Solved a puzzle");
                puzzle.SolvePuzzle();
            }

            requireCheck = false;
        }

        private void RequireCheck(LLMPuzzle p) => requireCheck = true;


        private void SetPuzzleNull()
        {
            if (puzzle == null) return;
            puzzle.OnDisableTrigger -= SetPuzzleNull;
            puzzle = null;
        }

        private void UpdatePuzzle()
        {
            puzzle = Constants.Game.GetTriggerPuzzle() as LLMPuzzle;
            if (puzzle == null) return;
            puzzle.OnDisableTrigger += SetPuzzleNull;
            puzzle.OnSolvePuzzle += SetPuzzleNull;
        }

        private void DisplayCorpus()
        {
            if (puzzle == null) return;
            TMP_Text tmp = puzzle.corpusField;
            tmp.gameObject.SetActive(true);
        }
    }
}


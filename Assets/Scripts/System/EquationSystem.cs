using System.Collections;
using System.Collections.Generic;
using EducationalGame.Component;
using EducationalGame.Core;
using TMPro;
using UnityEngine;

namespace EducationalGame
{
    public class EquationSystem : ISystem
    {
        private bool requireCheck;
        // private bool requireBinaryCheck;
        // private bool requireEquationCheck;

        /// <summary>
        /// Record the triggered puzzle
        /// </summary>
        private static EquationPuzzle puzzle;

        public void Init()
        {
            SystemManager.interactSystem.OnBinaryChanged += RequireCheck;
            foreach(EquationPuzzle puzzleInstance in Constants.Game.equationPuzzles)
            {
                puzzleInstance.OnEnableTrigger += UpdatePuzzle;
            }
        }

        public void Process()
        {
            
        }

        public void Update()
        {
            if (puzzle == null) return;
            if (!requireCheck) return;


            if (puzzle.isBinaryPuzzle)
            {
                InteractSystem.GetSumAndCarryEntity(puzzle, out NumberSwitch sumEntity, out NumberSwitch carryEntity);

                bool sumIsCorrect = false;
                if (sumEntity != null)
                {
                    NumberSwitchComponent sumC = EntityManager.Instance.GetComponent<NumberSwitchComponent>(sumEntity);
                    sumIsCorrect = sumC.CurrentBinary == sumC.TargetBinary;
                }
                bool carryIsCorrect = false;
                if (carryEntity != null)
                {
                    NumberSwitchComponent carryC = EntityManager.Instance.GetComponent<NumberSwitchComponent>(carryEntity);
                    carryIsCorrect = carryC.CurrentBinary == carryC.TargetBinary;
                }

                if (sumIsCorrect && carryIsCorrect)
                {
                    // TODO: Animation/Effect
                    DisplaySuccess();
                    puzzle.SolvePuzzle();
                }
            }
            else
            {
                
            }

            requireCheck = false;

        }

        private void RequireCheck(EquationPuzzle puzzle) => requireCheck = true;
        // private void BinaryCheck(EquationPuzzle puzzle) => requireBinaryCheck = true;
        // private void EquationCheck(EquationPuzzle puzzle) => requireEquationCheck = true;

        private void SetPuzzleNull()
        {
            if (puzzle == null) return;
            puzzle.OnDisableTrigger -= SetPuzzleNull;
            puzzle = null;
        }

        private void UpdatePuzzle()
        {
            puzzle = Constants.Game.GetTriggerPuzzle() as EquationPuzzle;
            if (puzzle == null) return;
            puzzle.OnDisableTrigger += SetPuzzleNull;
            puzzle.OnSolvePuzzle += SetPuzzleNull;
        }


        private void DisplaySuccess()
        {
            if (puzzle == null) return;
            TextMeshPro tmp = puzzle.text;
            tmp.gameObject.SetActive(true);
            tmp.text = "Solved a Puzzle!";
        }
    }

}

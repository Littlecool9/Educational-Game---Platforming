using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using EducationalGame.Component;
using UnityEngine;

namespace EducationalGame
{
    public class PlayerController : ISystem
    {

        Player player;
        // System Layer, Receive Player input, 处理预输入        
        public void Update()
        {
            // Set moving related player input
            InputComponent inputC = EntityManager.Instance.GetComponent<InputComponent>(player);

            Vector2 inputDirection = Vector2.zero;
            inputC.JumpInput = Input.GetKeyDown(KeyCode.Space);
            inputC.MoveInput = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
            inputC.InteractInput = Input.GetKeyDown(KeyCode.E);

            if (inputC.InteractInput){
                // Debug.Log("Interact Input");
            }
            if (Input.GetKey(KeyCode.A))
            {
                inputDirection.x = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                inputDirection.x = 1;
            }
            inputC.SetMoveDir(inputDirection);
            
        }

        public void Process()
        {
            
        }

        public void Init()
        {
            player = EntityManager.Instance.GetEntityWithID(0) as Player;
        }
    }
}

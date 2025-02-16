using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using EducationalGame.Component;
using UnityEngine;

namespace EducationalGame
{
    public class PlayerController : ISystem
    {
        // System Layer, Receive Player input, 处理预输入
        private EntityManager entityManager = EntityManager.Instance;
        private Player player;

        public bool IsMovable { get; private set;} = true;
        
        

        public void Update()
        {
            List<int> res = EntityManager.Instance.GetEntitiesWithTag("Player");

            int playerID = res[0];

            // Set moving related player input
            MovementComponent moveC = EntityManager.Instance.GetComponent<MovementComponent>(playerID);

            Vector2 inputVector = Vector2.zero;
            if (Input.GetButtonDown("Jump"))
            {
                inputVector.y = 1;
            }
            // TODO: 处理按键冲突/预输入
            if (Input.GetKeyDown(KeyCode.A)){
                inputVector.x = -1;
            }
            if (Input.GetKeyDown(KeyCode.D)){
                inputVector.x = 1;
            }
            moveC.facing = (int)inputVector.x;
            Debug.Log("inputVector:" + inputVector);
            moveC.SetDirection(inputVector);
            
        }

        public void Process()
        {
            
        }
    }
}

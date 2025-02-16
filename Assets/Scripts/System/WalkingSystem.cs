using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using EducationalGame.Component;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace EducationalGame
{
    public class WalkingSystem : ISystem
    {
        public void Process()
        {
            
        }

        public void Update()
        {
            var entityManager = EntityManager.Instance;
            
            // Handle Player Walking Logic
            Player entity = (Player)entityManager.GetEntityWithID(0);
            MovementComponent movementC = entityManager.GetComponent<MovementComponent>(entity);
            // movementC.ResetSpeed();     // reset speed and set a new speed per frame
            RenderComponent renderC = entityManager.GetComponent<RenderComponent>(entity);
            Vector2 direction = movementC.MoveDir;
            if (direction.y > 0){
                // Handle Jump
                if (movementC.Jumpable != false) 
                {
                    Jump(movementC, renderC);
                }
            }
            if (direction.x != 0){
                // Handle Walk
                HorizontalMove(movementC, renderC);
            }
            Move(movementC, renderC);
            Debug.Log("speed:" + renderC.rb.velocity);
            // Handle other objects' logic when walking

        }
        
        // Set vertical velocity
        private void Jump(MovementComponent movementC, RenderComponent renderC){
            // Using fixedupdate to assure frame independent
            Vector2 speed = new Vector2();
            speed.x += movementC.JumpHBoost * movementC.facing;      // 往前进方向给一个力
            speed.y = movementC.JumpSpeed;
            movementC.SetSpeed(speed);
            renderC.rb.velocity = speed;
        }

        // Set horizontal velocity
        private void HorizontalMove(MovementComponent movementC, RenderComponent renderC){
            int dir = movementC.facing;
            Vector2 speed = new Vector2(dir * movementC.PlayerMoveSpeed, 0);
            renderC.rb.velocity += speed;
        }

        // Execute move action
        private void Move(MovementComponent movementC, RenderComponent renderC){
            renderC.rb.velocity = movementC.Speed;
        }
        
    }
}



using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using EducationalGame.Component;
using UnityEngine;

namespace EducationalGame
{
    public class PhysicsSystem : ISystem
    {
        public void Process()
        {
            
        }

        public void Update()
        {
            var entityManager = EntityManager.Instance;

            // Handle Player Walking Logic
            foreach (Entity entity in entityManager.GetAllEntities())
            {
                

                if (entity is Player)
                {
                    // Player logic, including movement
                    ColliderComponent colliderC = entityManager.GetComponent<ColliderComponent>(entity);
                    MovementComponent movementC = entityManager.GetComponent<MovementComponent>(entity);

                    bool wasGrounded = colliderC.IsGrounded;
                    colliderC.IsGrounded = CheckGrounded(colliderC);
                    InputComponent inputC = entityManager.GetComponent<InputComponent>(entity);
                    StateComponent<PlayerStates> stateC = entityManager.GetComponent<StateComponent<PlayerStates>>(entity);
                    
                    if (inputC.JumpInput){
                        // Handle Jump
                        if (stateC.Jumpable != false) 
                        {
                            // Jump(movementC, inputC);
                            movementC.AddSpeed(movementC.JumpHBoost * inputC.Facing, movementC.JumpSpeed);
                        }
                    }
                    // Handle Walk
                    if (inputC.MoveInput){
                        // HorizontalMove(movementC, inputC);
                        movementC.SetSpeed(inputC.Facing * movementC.PlayerMoveSpeed, movementC.Speed.y);   
                    }
                    else{
                        movementC.SetSpeed(0, movementC.Speed.y);
                    }
                    // Move(movementC, renderC);
                    if (!colliderC.IsGrounded)
                    {
                        // movementC.Speed.y += colliderC.Gravity * Constants.deltaTime;
                        movementC.AddSpeed(0, movementC.Gravity * Constants.deltaTime);                    
                    }
                    else if (!wasGrounded) // 刚刚着地
                    {
                        // movementC.Speed.y = 0;
                        movementC.SetSpeed(movementC.Speed.x, 0);
                    }
                    ApplyMovement(colliderC, movementC.Speed * Constants.deltaTime);
                }

                // Handle Gravity
                
            }


            // Handle other objects' logic when walking

        }
        

        private bool CheckGrounded(ColliderComponent collider)
        {
            float extraHeight = 0.01f; // 避免浮点误差
            RaycastHit2D hit = Physics2D.BoxCast(
                collider.Collider.bounds.center,
                collider.Collider.bounds.size, 
                0, 
                Vector2.down, 
                extraHeight, 
                collider.GroundLayer);  

            return hit.collider != null;
        }

        private void ApplyMovement(ColliderComponent collider, Vector2 movement)
        {

            collider.Collider.transform.position += (Vector3)new Vector2(movement.x, 0);

            collider.Collider.transform.position += (Vector3)new Vector2(0, movement.y);
            
        }

        private bool CheckCollision(ColliderComponent collider, Vector2 movement)
        {
            float extraPadding = 0.05f; // 额外的检测距离，避免浮点误差
            RaycastHit2D hit = Physics2D.BoxCast(
                collider.Collider.bounds.center, 
                collider.Collider.bounds.size, 
                0, 
                movement.normalized, 
                movement.magnitude + extraPadding, // 额外增加一点检测距离
                collider.GroundLayer // 只检测 Ground 层（墙、地面、障碍物）
            );

            return hit.collider != null; // 如果检测到碰撞，返回 true（表示阻挡）
        }

            
    }
}



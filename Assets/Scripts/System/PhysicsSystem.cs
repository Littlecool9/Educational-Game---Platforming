using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using EducationalGame.Component;
using UnityEngine;
using System;

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

            foreach (Entity entity in entityManager.GetAllEntities())
            {
                
                // Handle Player Walking Logic
                if (entity is Player)
                {
                    // Player logic, including movement
                    ColliderComponent colliderC = entityManager.GetComponent<ColliderComponent>(entity);
                    MovementComponent movementC = entityManager.GetComponent<MovementComponent>(entity);
                    InputComponent inputC = entityManager.GetComponent<InputComponent>(entity);
                    StateComponent stateC = entityManager.GetComponent<StateComponent>(entity);
                    
                    bool wasGrounded = colliderC.IsGrounded;
                    colliderC.IsGrounded = CheckGrounded(entity);
                    Debug.Log("IsGrounded: " + colliderC.IsGrounded);

                    // Gravity
                    
                    // float mult = (Math.Abs(movementC.Speed.y) < Constants.HalfGravThreshold && (inputC.JumpInput)) ? .5f : 1f;
                    // //空中的情况,需要计算Y轴速度
                    // movementC.Speed.y = Mathf.MoveTowards(movementC.Speed.y, Constants.MaxFall, Constants.Gravity * mult * Constants.deltaTime);
                    if (!colliderC.IsGrounded) {
                        // Vector2 gravity = new Vector2(0, movementC.Gravity);
                        // movementC.AddSpeed(gravity);
                        Debug.Log("applying gravity");
                        movementC.SetSpeed(movementC.Speed.x, 
                        Mathf.MoveTowards(movementC.Speed.y, Constants.MaxFall, Constants.Gravity * 1f * Constants.deltaTime
                        ));
                    }
                    else if (!wasGrounded) // 刚刚着地
                    {
                        movementC.SetSpeed(movementC.Speed.x, 0);
                        
                    }

                    // Handle User Input
                    if (inputC.JumpInput){
                        movementC.AddSpeed(movementC.JumpHBoost * inputC.Facing, movementC.JumpSpeed);
                    }
                    // Handle Walk
                    if (inputC.MoveInput){
                        movementC.AddSpeed(inputC.Facing * movementC.PlayerMoveSpeed, movementC.Speed.y);   
                    }
                    else{
                        movementC.SetSpeed(0, movementC.Speed.y);
                    }


                    

                    // Handle Ceiling
                    bool hitCeil = CheckCeil(entity);
                    if (hitCeil) {
                        movementC.SetSpeed(movementC.Speed.x, 0);
                    }

                    ApplyMovement(colliderC, movementC.Speed * Constants.deltaTime);

                    // UpdateCollideX(entity, movementC.Speed.x * Constants.deltaTime);
                    // UpdateCollideY(entity, movementC.Speed.y * Constants.deltaTime);
                    
                }

                // Handle Gravity
                
            }
            // Handle other objects' logic when walking

        }
        

        private bool CheckGrounded(Entity entity)
        {
            return CheckGrounded(entity, Vector2.zero);
        }

        private bool CheckGrounded(Entity entity, Vector2 offset)
        {
            ColliderComponent colliderC = EntityManager.Instance.GetComponent<ColliderComponent>(entity);
            RenderComponent renderC = EntityManager.Instance.GetComponent<RenderComponent>(entity);
            if (colliderC == null || renderC == null) throw new Exception("Missing ColliderComponent or RenderComponent in CheckGrounded()");

            RaycastHit2D hit = Physics2D.BoxCast(
                colliderC.Collider.bounds.center,
                colliderC.Collider.bounds.size, 
                0, 
                Vector2.down, 
                colliderC.DEVIATION, 
                colliderC.GroundLayer);
            if (hit && hit.normal == Vector2.up)
            {
                return true;
            }
            return false;
        }

        private bool CheckWall(Entity entity, int direction) 
        {
            // 1 = right, -1 = left
            Vector2 offset = new Vector2(direction, 0);
            return CheckGrounded(entity, offset);
        }

        private bool CheckCeil(Entity entity) 
        {
            return CheckGrounded(entity, Vector2.up);
        }

        private void ApplyMovement(ColliderComponent collider, Vector2 movement)
        {
            // movement is the speed in this frame
            collider.Collider.transform.position += (Vector3)new Vector2(movement.x, 0);

            collider.Collider.transform.position += (Vector3)new Vector2(0, movement.y);
            
        }

        protected void UpdateCollideX(Entity entity, float distX)
        {
            //使用校正
            float distance = distX;
            int correctTimes = 1;

            ColliderComponent colliderC = EntityManager.Instance.GetComponent<ColliderComponent>(entity);
            RenderComponent renderC = EntityManager.Instance.GetComponent<RenderComponent>(entity);
            MovementComponent movementC = EntityManager.Instance.GetComponent<MovementComponent>(entity);
            if (colliderC == null || renderC == null) throw new Exception("Missing ColliderComponent or RenderComponent in CheckGrounded()");

            while (true)
            {
                float moved = MoveXStepWithCollide(entity, distance);
                //无碰撞退出循环
                renderC.position += Vector2.right * moved;
                if (moved == distance || correctTimes == 0) //无碰撞，且校正次数为0
                {
                    Debug.Log("break");
                    break;
                }
                float tempDist = distance - moved;
                correctTimes--;
                // if (!CorrectX(entity, tempDist))
                // {
                //     movementC.Speed.x = 0;//未完成校正，则速度清零

                //     //Speed retention
                //     // if (wallSpeedRetentionTimer <= 0)
                //     // {
                //     //     wallSpeedRetained = this.Speed.x;
                //     //     wallSpeedRetentionTimer = Constants.WallSpeedRetentionTime;
                //     // }
                //     break;
                // }
                distance = tempDist;
            }
        }

        private float MoveXStepWithCollide(Entity entity, float distX)
        {
            Vector2 moved = Vector2.zero;
            Vector2 direct = Math.Sign(distX) > 0 ? Vector2.right : Vector2.left;

            ColliderComponent colliderC = EntityManager.Instance.GetComponent<ColliderComponent>(entity);
            RenderComponent renderC = EntityManager.Instance.GetComponent<RenderComponent>(entity);
            if (colliderC == null || renderC == null) throw new Exception("Missing ColliderComponent or RenderComponent in CheckGrounded()");

            RaycastHit2D hit = Physics2D.BoxCast(
                colliderC.Collider.bounds.center,
                colliderC.Collider.bounds.size, 
                0, 
                Vector2.down, 
                colliderC.DEVIATION, 
                colliderC.GroundLayer);

            if (hit && hit.normal == -direct)
            {
                //如果发生碰撞,则移动距离
                moved += direct * Mathf.Max((hit.distance - colliderC.DEVIATION), 0);
            }
            else
            {
                moved += Vector2.right * distX;
            }
            return moved.x;
        }

        private bool CorrectX(Entity entity, float distX)
        {

            ColliderComponent colliderC = EntityManager.Instance.GetComponent<ColliderComponent>(entity);
            RenderComponent renderC = EntityManager.Instance.GetComponent<RenderComponent>(entity);
            MovementComponent movementC = EntityManager.Instance.GetComponent<MovementComponent>(entity);
            if (colliderC == null || renderC == null || movementC == null) throw new Exception("Missing ColliderComponent or RenderComponent in CheckGrounded()");

            Vector2 direct = Math.Sign(distX) > 0 ? Vector2.right : Vector2.left;

            // if ((this.stateMachine.State == (int)EActionState.Dash))
            // {
            if (colliderC.IsGrounded && DuckFreeAt(entity, renderC.position + Vector2.right * distX))
            {
                // Ducking = true;
                return true;
            }
            else if (movementC.Speed.y == 0 && movementC.Speed.x!=0)
            {
                for (int i = 1; i <= Constants.DashCornerCorrection; i++)
                {
                    for (int j = 1; j >= -1; j -= 2)
                    {
                        if (!CollideCheck(renderC.position + new Vector2(0, j * i * 0.1f), direct, entity, Mathf.Abs(distX)))
                        {
                            renderC.position += new Vector2(distX, j * i * 0.1f);
                            return true;
                        }
                    }
                }
            }
            // }
            return false;
        }
            
        public bool CollideCheck(Vector2 position, Vector2 dir, Entity entity, float dist = 0)
        {
            ColliderComponent colliderC = EntityManager.Instance.GetComponent<ColliderComponent>(entity);
            RenderComponent renderC = EntityManager.Instance.GetComponent<RenderComponent>(entity);
            if (colliderC == null || renderC == null) throw new Exception("Missing ColliderComponent or RenderComponent in CheckGrounded()");

            Vector2 origion = position + renderC.position;
            return Physics2D.OverlapBox(origion + dir * (dist + colliderC.DEVIATION), colliderC.Collider.bounds.size, 0, colliderC.GroundLayer);
        }

        public bool DuckFreeAt(Entity entity, Vector2 at)
        {
            ColliderComponent colliderC = EntityManager.Instance.GetComponent<ColliderComponent>(entity);
            RenderComponent renderC = EntityManager.Instance.GetComponent<RenderComponent>(entity);
            if (colliderC == null || renderC == null) throw new Exception("Missing ColliderComponent or RenderComponent in CheckGrounded()");

            Vector2 oldP = renderC.position;
            renderC.position = at;
            // colliderC.Collider = duckHitbox;

            bool ret = !CollideCheck(renderC.position, Vector2.zero, entity);

            renderC.position = oldP;

            return ret;
        }

        protected void UpdateCollideY(Entity entity, float distY)
        {

            RenderComponent renderC = EntityManager.Instance.GetComponent<RenderComponent>(entity);
            MovementComponent movementC = EntityManager.Instance.GetComponent<MovementComponent>(entity);
            if (renderC == null || movementC == null) throw new Exception("Missing ColliderComponent or RenderComponent in CheckGrounded()");

            Vector2 targetPosition = renderC.position;
            //使用校正
            float distance = distY;
            int correctTimes = 1; //默认可以迭代位置10次
            bool collided = true;
            float speedY = Mathf.Abs(movementC.Speed.y);
            while (true)
            {
                float moved = MoveYStepWithCollide(entity, distance);
                //无碰撞退出循环
                renderC.position += Vector2.up * moved;
                if (moved == distance || correctTimes == 0) //无碰撞，且校正次数为0
                {
                    collided = false;
                    break;
                }
                float tempDist = distance - moved;
                correctTimes--;
                if (!CorrectY(entity, tempDist))
                {
                    movementC.Speed.y = 0;//未完成校正，则速度清零
                    break;
                }
                distance = tempDist;
            }

            //落地时候，进行缩放
            // if (collided && distY < 0)
            // {
            //     if (this.stateMachine.State != (int)EActionState.Climb)
            //     {
            //         this.PlayLandEffect(this.SpritePosition, speedY);
            //     }
            // }
        }

        private float MoveYStepWithCollide(Entity entity, float distY)
        {

            ColliderComponent colliderC = EntityManager.Instance.GetComponent<ColliderComponent>(entity);
            MovementComponent movementC = EntityManager.Instance.GetComponent<MovementComponent>(entity);
            if (colliderC == null || movementC == null) throw new Exception("Missing ColliderComponent or RenderComponent in CheckGrounded()");

            Vector2 moved = Vector2.zero;
            Vector2 direct = Math.Sign(distY) > 0 ? Vector2.up : Vector2.down;
            // Vector2 origion = this.Position + collider.position;

            // RaycastHit2D hit = Physics2D.BoxCast(origion, collider.size, 0, direct, Mathf.Abs(distY) + colliderC.DEVIATION, GroundLayer);
            RaycastHit2D hit = Physics2D.BoxCast(
                colliderC.Collider.bounds.center,
                colliderC.Collider.bounds.size, 
                0, 
                direct, 
                colliderC.DEVIATION, 
                colliderC.GroundLayer);
                
            if (hit && hit.normal == -direct)
            {
                //如果发生碰撞,则移动距离
                moved += direct * Mathf.Max((hit.distance - colliderC.DEVIATION), 0);
            }
            else
            {
                moved += Vector2.up * distY;
            }
            return moved.y;
        }

        private bool CorrectY(Entity entity, float distY)
        {
            ColliderComponent colliderC = EntityManager.Instance.GetComponent<ColliderComponent>(entity);
            RenderComponent renderC = EntityManager.Instance.GetComponent<RenderComponent>(entity);
            MovementComponent movementC = EntityManager.Instance.GetComponent<MovementComponent>(entity);
            if (colliderC == null || renderC == null || movementC == null) throw new Exception("Missing ColliderComponent or RenderComponent in CheckGrounded()");


            // Vector2 origion = renderC.position + collider.position;
            Vector2 origion = renderC.position;
            Vector2 direct = Math.Sign(distY) > 0 ? Vector2.up : Vector2.down;
            
            // 向下移动
            if (movementC.Speed.y < 0)
            {
                // if ((this.stateMachine.State == (int)EActionState.Dash) && !DashStartedOnGround)
                // {
                //     if (movementC.Speed.x <= 0)
                //     {
                //         for (int i = -1; i >= -Constants.DashCornerCorrection; i--)
                //         {
                //             float step = (Mathf.Abs(i * 0.1f) + colliderC.DEVIATION);
                            
                //             if (!CheckGrounded(entity, new Vector2(-step, 0)))
                //             {
                //                 renderC.position += new Vector2(-step, distY);
                //                 return true;
                //             }
                //         }
                //     }

                //     if (movementC.Speed.x >= 0)
                //     {
                //         for (int i = 1; i <= Constants.DashCornerCorrection; i++)
                //         {
                //             float step = (Mathf.Abs(i * 0.1f) + colliderC.DEVIATION);
                //             if (!CheckGrounded(entity, new Vector2(step, 0)))
                //             {
                //                 renderC.position += new Vector2(step, distY);
                //                 return true;
                //             }
                //         }
                //     }
                // }
            }
            //向上移动
            else if (movementC.Speed.y > 0)
            {
                //Y轴向上方向的Corner Correction
                {
                    if (movementC.Speed.x <= 0)
                    {
                        for (int i = 1; i <= Constants.UpwardCornerCorrection; i++)
                        {
                            RaycastHit2D hit = Physics2D.BoxCast(
                                colliderC.Collider.bounds.center,
                                colliderC.Collider.bounds.size,
                                0,
                                direct,
                                Mathf.Abs(distY) + colliderC.DEVIATION, 
                                colliderC.GroundLayer);
                            if (!hit)
                            {
                                renderC.position += new Vector2(-i * 0.1f, 0);
                                return true;
                            }
                        }
                    }

                    if (movementC.Speed.x >= 0)
                    {
                        for (int i = 1; i <= Constants.UpwardCornerCorrection; i++)
                        {
                            RaycastHit2D hit = Physics2D.BoxCast(
                                colliderC.Collider.bounds.center,
                                colliderC.Collider.bounds.size,
                                0, 
                                direct, 
                                Mathf.Abs(distY) + colliderC.DEVIATION, 
                                colliderC.GroundLayer);
                            if (!hit)
                            {
                                renderC.position += new Vector2(i * 0.1f, 0);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}



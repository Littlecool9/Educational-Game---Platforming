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

                MovementComponent movementC = entityManager.GetComponent<MovementComponent>(entity);
                

                // Handle Player Walking Logic
                if (entity is Player)
                {
                    // Player logic, including movement
                    InputComponent inputC = entityManager.GetComponent<InputComponent>(entity);
                    StateComponent stateC = entityManager.GetComponent<StateComponent>(entity);

                    // Gravity
                    float mult = (Math.Abs(movementC.Speed.y) < Constants.HalfGravThreshold && stateC.CurrentState == PlayerState.Jumping) ? .5f : 1f;
                    if (!stateC.IsGrounded ) {
                        movementC.AddSpeed(0, 
                        Mathf.MoveTowards(movementC.Speed.y, Constants.MaxFall, Constants.Gravity * mult 
                        ));
                    }

                    // Handle User Input

                    // Handle Jump
                    Debug.Log("input movement: " + inputC.MoveDir);
                    if (stateC.CurrentState == PlayerState.Jumping){
                        // Add an additional horizontal boost, apply jump speed
                        
                        movementC.AddSpeed(Constants.JumpHBoost * inputC.MoveDir.x, Constants.JumpSpeed);
                    }
                    // Handle Walk
                    // On Ground
                    else if (stateC.CurrentState == PlayerState.Walking || stateC.CurrentState == PlayerState.OnAir){
                        movementC.AddSpeed(movementC.MoveSpeed * inputC.MoveDir.x, 0); 
                    }
                    else if (stateC.CurrentState == PlayerState.Idle){
                        movementC.SetSpeed(0, movementC.Speed.y);
                    }
                    ApplyMovement(entity, movementC.Speed * Constants.deltaTime);
                }
            }

            // Handle other objects' logic when walking

        }

        private void ApplyMovement(Entity entity, Vector2 movement)
        {
            UpdateCollideX(entity, movement.x);
            UpdateCollideY(entity, movement.y);
        }

        protected void UpdateCollideX(Entity entity, float distX)
        {
            //使用校正
            float distance = distX;
            int correctTimes = 1;

            RenderComponent renderC = EntityManager.Instance.GetComponent<RenderComponent>(entity);
            MovementComponent movementC = EntityManager.Instance.GetComponent<MovementComponent>(entity);
            if (renderC == null || movementC == null) throw new Exception("Missing RenderComponent or MovementComponent in UpdateCollideX()");

            while (true)
            {
                float moved = MoveXStepWithCollide(entity, distance);       // 得出实际移动距离

                //无碰撞退出循环
                renderC.MoveTransform(Vector2.right * moved);

                if (moved == distance || correctTimes == 0) //无碰撞，且校正次数为0
                {
                    break;
                }
                float tempDist = distance - moved;
                correctTimes--;
                if (!CorrectX(entity, tempDist))
                {
                    movementC.Speed.x = 0;//未完成校正，则速度清零

                    //Speed retention
                    // if (wallSpeedRetentionTimer <= 0)
                    // {
                    //     wallSpeedRetained = this.Speed.x;
                    //     wallSpeedRetentionTimer = Constants.WallSpeedRetentionTime;
                    // }
                    break;
                }
                distance = tempDist;
            }
        }

        private float MoveXStepWithCollide(Entity entity, float distX)
        {
            Vector2 moved = Vector2.zero;
            Vector2 direct = Math.Sign(distX) > 0 ? Vector2.right : Vector2.left;

            RenderComponent renderC = EntityManager.Instance.GetComponent<RenderComponent>(entity);
            if (renderC == null) throw new Exception("Missing RenderComponent in MoveXStepWithCollide()");

            RaycastHit2D hit = Physics2D.BoxCast(
                renderC.Collider.bounds.center,
                renderC.Collider.bounds.size, 
                0, 
                Vector2.down, 
                Constants.DEVIATION, 
                Constants.GroundLayer);

            if (hit && hit.normal == -direct)
            {
                //如果发生碰撞,则移动距离
                moved += direct * Mathf.Max(hit.distance - Constants.DEVIATION, 0);
            }
            else
            {
                moved += Vector2.right * distX;
            }
            return moved.x;
        }

        private bool CorrectX(Entity entity, float distX)
        {

            RenderComponent renderC = EntityManager.Instance.GetComponent<RenderComponent>(entity);
            MovementComponent movementC = EntityManager.Instance.GetComponent<MovementComponent>(entity);
            if (renderC == null || movementC == null) throw new Exception("Missing RenderComponent in CorrectX()");

            return false;
        }
            
        public bool CollideCheck(Vector2 position, Vector2 dir, Entity entity, float dist = 0)
        {
            RenderComponent renderC = EntityManager.Instance.GetComponent<RenderComponent>(entity);
            if (renderC == null) throw new Exception("Missing RenderComponent in CollideCheck()");

            Vector2 origion = position + renderC.position;
            return Physics2D.OverlapBox(origion + dir * (dist + Constants.DEVIATION), renderC.Collider.bounds.size, 0, Constants.GroundLayer);
        }

        public bool DuckFreeAt(Entity entity, Vector2 at)
        {
            RenderComponent renderC = EntityManager.Instance.GetComponent<RenderComponent>(entity);
            if (renderC == null) throw new Exception("Missing RenderComponent in DuckFreeAt()");

            Vector2 oldP = renderC.position;
            renderC.position = at;
            // renderC.Collider = duckHitbox;

            bool ret = !CollideCheck(renderC.position, Vector2.zero, entity);

            renderC.position = oldP;

            return ret;
        }

        protected void UpdateCollideY(Entity entity, float distY)
        {
            RenderComponent renderC = EntityManager.Instance.GetComponent<RenderComponent>(entity);
            MovementComponent movementC = EntityManager.Instance.GetComponent<MovementComponent>(entity);
            if (renderC == null || movementC == null) throw new Exception("Missing MovementComponent or RenderComponent in UpdateCollideY()");

            //使用校正
            float distance = distY;
            int correctTimes = 1; //默认可以迭代位置10次
            bool collided = true;
            while (true)
            {
                float moved = MoveYStepWithCollide(entity, distance);       // distance是距离目标位置剩余的距离
                //无碰撞退出循环
                // renderC.position += Vector2.up * moved;
                renderC.MoveTransform(Vector2.up * moved);
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
        }

        private float MoveYStepWithCollide(Entity entity, float distY)
        {

            MovementComponent movementC = EntityManager.Instance.GetComponent<MovementComponent>(entity);
            RenderComponent renderC = EntityManager.Instance.GetComponent<RenderComponent>(entity);
            if (renderC == null || movementC == null) throw new Exception("Missing MovementComponent or RenderComponent in MoveYStepWithCollide()");

            Vector2 moved = Vector2.zero;
            Vector2 direct = Math.Sign(distY) > 0 ? Vector2.up : Vector2.down;      // 判断往上还是往下移动
            
            RaycastHit2D hit = Physics2D.BoxCast(
                renderC.Collider.bounds.center,
                renderC.Collider.bounds.size, 
                0, 
                direct, 
                Constants.DEVIATION, 
                Constants.GroundLayer);
                
            if (hit && hit.normal == -direct)
            {
                //如果发生碰撞,则移动距离
                moved += direct * Mathf.Max(hit.distance - Constants.DEVIATION, 0);
            }
            else
            {
                moved += Vector2.up * distY;        
            }
            return moved.y;
        }

        private bool CorrectY(Entity entity, float distY)
        {
            RenderComponent renderC = EntityManager.Instance.GetComponent<RenderComponent>(entity);
            MovementComponent movementC = EntityManager.Instance.GetComponent<MovementComponent>(entity);
            if (renderC == null || movementC == null) throw new Exception("Missing MovementComponent or RenderComponent in CorrectY()");


            // Vector2 origion = renderC.position + collider.position;
            Vector2 origion = renderC.position;
            Vector2 direct = Math.Sign(distY) > 0 ? Vector2.up : Vector2.down;
            
            // 向下移动
            if (movementC.Speed.y < 0)
            {
                
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
                                renderC.Collider.bounds.center,
                                renderC.Collider.bounds.size,
                                0,
                                direct,
                                Mathf.Abs(distY) + Constants.DEVIATION, 
                                renderC.GroundLayer);
                            if (!hit)
                            {
                                // renderC.position += new Vector2(-i * 0.1f, 0);
                                renderC.MoveTransform(new Vector2(-i * 0.1f, 0));
                                return true;
                            }
                        }
                    }

                    if (movementC.Speed.x >= 0)
                    {
                        for (int i = 1; i <= Constants.UpwardCornerCorrection; i++)
                        {
                            RaycastHit2D hit = Physics2D.BoxCast(
                                renderC.Collider.bounds.center,
                                renderC.Collider.bounds.size,
                                0, 
                                direct, 
                                Mathf.Abs(distY) + Constants.DEVIATION, 
                                Constants.GroundLayer);
                            if (!hit)
                            {
                                // renderC.position += new Vector2(i * 0.1f, 0);
                                renderC.MoveTransform(new Vector2(i * 0.1f, 0));
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }


        public void Init()
        {
            
        }
    }
}



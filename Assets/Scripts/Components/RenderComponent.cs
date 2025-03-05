using System;
using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using UnityEngine;

namespace EducationalGame.Component
{
    public class RenderComponent : IComponent
    {
        // Holds GameObject & Position; Links Unity Components to Customized Components
        public GameObject GameObject { get; private set; }
        public Transform transform;
        public Vector2 position
        {
            get => transform.position;
            set => transform.position = value;
        }
        public SpriteRenderer sr { get; private set;}
        public RenderComponent(){}
        public Collider2D collider;
        public Animator animator;

        public Collider2D Collider { get{ return collider;} set{ collider = value; } }
        public LayerMask GroundLayer; // 用于检测地面
        public float DEVIATION = 0.002f;  //碰撞检测误差


        public void SetGameObject(GameObject gameObject){
            this.GameObject = gameObject;
            sr = gameObject.GetComponent<SpriteRenderer>();
            collider = gameObject.GetComponent<Collider2D>();
            // transform = gameObject.transform;
            transform = gameObject.GetComponent<Transform>();
            animator = gameObject.GetComponent<Animator>();
        }

        public void InitComponent()
        {
            
        }

        public void InitComponent(GameObject gameObject){
            SetGameObject(gameObject);
        }

        // Set Position in Engine
        public void MoveTransform(Vector2 position) => transform.position += new Vector3(position.x, position.y, 0);

        public void SetAnimatorBool(string name, bool value) => animator?.SetBool(name, value);

        public void Flip(){
            if (sr == null) throw new Exception("Missing SpriteRenderer in Flip()");
            sr.flipX = !sr.flipX;
        }
    }

}


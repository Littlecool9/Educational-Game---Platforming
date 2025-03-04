using System;
using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using UnityEngine;

namespace EducationalGame.Component
{
    public class RenderComponent : IComponent
    {
        // Holds GameObject & Position
        public GameObject GameObject { get; private set; }
        public Transform transform;
        public Vector2 position;
        public SpriteRenderer sr { get; private set;}
        public RenderComponent(){}
        public Collider2D collider;
        public Animator animator;


        public void SetGameObject(GameObject gameObject){
            this.GameObject = gameObject;
            sr = gameObject.GetComponent<SpriteRenderer>();
            collider = gameObject.GetComponent<Collider2D>();
            position = gameObject.transform.position;
            transform = gameObject.transform;
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
            sr.flipX = !sr.flipX;
        }
    }

}


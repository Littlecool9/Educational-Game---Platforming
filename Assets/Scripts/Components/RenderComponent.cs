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
        public Vector2 position;
        public SpriteRenderer sr { get; private set;}
        public RenderComponent(){}

        public Collider2D collider;

        public void SetGameObject(GameObject gameObject){
            this.GameObject = gameObject;
            sr = gameObject.GetComponent<SpriteRenderer>();
            collider = gameObject.GetComponent<Collider2D>();
        }

        public void InitComponent()
        {
            
        }

        public void InitComponent(GameObject gameObject){
            SetGameObject(gameObject);
        }
    }

}


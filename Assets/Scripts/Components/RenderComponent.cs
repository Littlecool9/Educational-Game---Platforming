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
        public Rigidbody2D rb { get; private set;}
        public SpriteRenderer sr { get; private set; }

        public RenderComponent(){}

        public void SetGameObject(GameObject gameObject){
            this.GameObject = gameObject;
            rb = gameObject.GetComponent<Rigidbody2D>();
            sr = gameObject.GetComponent<SpriteRenderer>();
        }
    }

}


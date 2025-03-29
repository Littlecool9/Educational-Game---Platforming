using System;
using EducationalGame.Core;
using TMPro;
using UnityEngine;

namespace EducationalGame.Component
{
    public class RenderComponent : IComponent
    {
        // Holds GameObject & Position & Unity Components
        // Links Unity Components to Customized Components
        public RenderComponent(){}


        public GameObject GameObject { get; private set; }
        public Transform transform;
        public Vector2 position
        {
            get => transform.position;
            set => transform.position = value;
        }
        public Collider2D collider;
        public Collider2D Collider { get{ return collider;} set{ collider = value; } }



        // Interactable Specific
        public Trigger trigger;

        public IBridge bridge;

        public TextMeshPro textMeshPro;


        // Player Specific
        public Animator animator;
        public SpriteRenderer sr { get; private set;}



        public void SetGameObject(GameObject gameObject){
            this.GameObject = gameObject;
            sr = gameObject.GetComponent<SpriteRenderer>();
            collider = gameObject.GetComponent<Collider2D>();
            transform = gameObject.GetComponent<Transform>();
            animator = gameObject.GetComponent<Animator>();

            textMeshPro = gameObject.GetComponent<TextMeshPro>();

            trigger = gameObject.GetComponent<Trigger>();

            bridge = Game.GetBridgeComponent(gameObject);
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


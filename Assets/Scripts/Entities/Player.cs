using System.Collections;
using System.Collections.Generic;
using EducationalGame.Component;
using EducationalGame.Core;
using UnityEngine;

namespace EducationalGame
{
    public class Player : Entity
    {
        private static Player _instance;
        public static Player Instance
        {
            get
            {
                _instance ??= new Player();
                return _instance;
            }
        }


        private EntityManager entityManager = EntityManager.Instance;
        
        private Player() : base()
        {
            
        }

        public override void InitComponents()
        {
            entityManager.AddComponent(this, new MovementComponent());
            entityManager.AddComponent(this, new StateComponent());
            entityManager.AddComponent(this, new InteractionComponent());
            entityManager.AddComponent(this, new RenderComponent());
        }
    }
}

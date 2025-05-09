using System.Collections;
using System.Collections.Generic;
using EducationalGame.Component;
using EducationalGame.Core;
using UnityEngine;

namespace EducationalGame
{
    public class Player : Entity
    {
        // Singleton
        private static Player _instance;
        public static Player Instance
        {
            get
            {
                _instance ??= new Player();
                return _instance;
            }
        }


        
        private Player()
        {
            ID = 0;
        }

        public override void InitEntity()
        {
            entityManager.AddComponent(this, new MovementComponent());
            entityManager.AddComponent(this, new StateComponent());
            entityManager.AddComponent(this, new RenderComponent());
            entityManager.AddComponent(this, new InputComponent());
            
            foreach (var component in entityManager.GetComponents(this.ID)) { component.InitComponent(); }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using UnityEngine;

namespace EducationalGame
{
    public class PlayerStateMachine : ISystem
    {
        Player player;
        // Handle Player State Logic
        public void Init()
        {
            player = EntityManager.Instance.GetEntityWithID(0) as Player;
        }

        public void Process()
        {
            
        }

        public void Update()
        {
            
        }
    }
}


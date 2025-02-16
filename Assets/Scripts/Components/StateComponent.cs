using System.Collections;
using System.Collections.Generic;
using EducationalGame.Core;
using UnityEngine;

namespace EducationalGame.Component
{
    public enum PlayerStates { Idle, Walking, Interacting }
    public enum States {}

    public class StateComponent : IComponent
    {
        public PlayerStates currentState;
        public string stateName;
    }

    // public class PlayerState : StateComponent
    // {
    //     public static PlayerState Idle = new PlayerState();
    //     public static PlayerState Walking = new PlayerState();
    //     public static PlayerState Interacting = new PlayerState();
    // }

}

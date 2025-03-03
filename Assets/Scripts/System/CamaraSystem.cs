using System.Collections;
using System.Collections.Generic;
using EducationalGame.Component;
using EducationalGame.Core;
using UnityEngine;

namespace EducationalGame
{
    public class CamaraSystem : ISystem
    {
        public void Process() => throw new System.NotImplementedException();

        public void Update()
        {
            UpdateCamera(Constants.deltaTime);
        }

        private Vector2 cameraPosition;
        private Player player;

        private Bounds bounds;

        public void UpdateCamera(float deltaTime)
        {
            var from = cameraPosition;
            var target = CameraTarget;
            var multiplier = 1f;

            cameraPosition = from + (target - from) * (1f - (float)Mathf.Pow(0.01f / multiplier, deltaTime));
        }

        public Vector2 GetCameraPosition() 
        {
            return cameraPosition;
        }

        public void Init()
        {
            player = EntityManager.Instance.GetEntityWithID(0) as Player;
        }

        protected Vector2 CameraTarget
        {
            get
            {
                player ??= EntityManager.Instance.GetEntityWithID(0) as Player;
                RenderComponent renderC = EntityManager.Instance.GetComponent<RenderComponent>(player.ID);
                Vector2 at = new Vector2();
                Vector2 target = new Vector2(renderC.position.x, renderC.position.y);

                at.x = Mathf.Clamp(target.x, bounds.min.x + 3200 / 100 / 2f, bounds.max.x - 3200 / 100 / 2f);
                at.y = Mathf.Clamp(target.y, bounds.min.y + 1800 / 100 / 2f, bounds.max.y - 1800 / 100 / 2f);
                return at;
            }
        }
    }
}

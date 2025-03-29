using EducationalGame.Component;
using EducationalGame.Core;

namespace EducationalGame
{
    public class TypeConsole : Entity
    {
        public override void InitEntity()
        {
            entityManager.AddComponent(this, new TypeConsoleComponent());
            entityManager.AddComponent(this, new RenderComponent());
            entityManager.AddComponent(this, new InteractableComponent());
        }
    }
}

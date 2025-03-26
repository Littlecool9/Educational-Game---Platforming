using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

namespace EducationalGame.Core
{
    public enum EntityType { Player, 
        SortingBoxSlot, SortingBoxes,
        NumberSlot, NumberSwitch
        }
    public abstract class Entity
    {
        public int ID { get; protected set; }
        
        private static int nextID = 1;

        protected EntityManager entityManager = EntityManager.Instance;


        public static Entity CreateInstance(EntityType name){
            if (name is EntityType.Player) return Player.Instance;
            else if (name is EntityType.SortingBoxes) return new SortingBoxes();
            else if (name is EntityType.SortingBoxSlot) return new SortingBoxSlot();
            else if (name is EntityType.NumberSlot) return new NumberSlot();
            else if (name is EntityType.NumberSwitch) return new NumberSwitch();
            else return null;
        }

        public Entity()
        {
            ID = nextID++;
        }

        public abstract void InitEntity();

    }

    public class EntityManager
    {
        private static EntityManager _instance;
        public static EntityManager Instance
        {
            get
            {
                _instance ??= new EntityManager();
                return _instance;
            }
        }

        private EntityManager(){}
        
        
        private Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
        private Dictionary<int, List<IComponent>> components = new Dictionary<int, List<IComponent>>();
        private Dictionary<int, string> entitiesTag = new Dictionary<int, string>();



        public Entity CreateEntity(EntityType entityType, string tag = "")
        {
            Entity entity = Entity.CreateInstance(entityType);
            if (tag != "") { entitiesTag[entity.ID] = tag; }
            entities[entity.ID] = entity;
            components[entity.ID] = new List<IComponent>();
            entity.InitEntity();
            return entity;
        }

        public void AddComponent<T>(Entity entity, T component) where T : IComponent
        {
            if (components.ContainsKey(entity.ID))
            {
                components[entity.ID].Add(component);
            }
        }

        public T GetComponent<T>(Entity entity) where T : IComponent
        {
            return (T)components[entity.ID].Find(c => c is T);
        }

        public T GetComponent<T>(int entityID) where T : IComponent
        {
            return (T)components[entityID].Find(c => c is T);
        }

        public bool HasComponent<T>(Entity entity) where T : IComponent
        {
            return components[entity.ID].Exists(c => c is T);
        }

        public List<Entity> GetAllEntities()
        {
            return new List<Entity>(entities.Values);
        }

        public List<int> GetEntitiesWithTag(string tag){
            List<int> keys = new List<int>();
            foreach (var kvp in entitiesTag)
            {
                if (kvp.Value == tag)
                {
                    keys.Add(kvp.Key);
                }
            }
            return keys;
        }
        public Entity GetEntityWithID(int ID){
            return entities[ID];
        }
        public Player GetPlayer(){
            return GetEntityWithID(0) as Player;
        }
        
        public void DestroyEntity(int ID){
            if (entities.ContainsKey(ID)){
                entities.Remove(ID);
                components.Remove(ID);
                if (entitiesTag.ContainsKey(ID)){
                    entitiesTag.Remove(ID);
                }
            }
        }

        public List<IComponent> GetComponents(Entity entity){
            return components[entity.ID];
        }
        public List<IComponent> GetComponents(int entityID){
            return components[entityID];
        }
    }
}

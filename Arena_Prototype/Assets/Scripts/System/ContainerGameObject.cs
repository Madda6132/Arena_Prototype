using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Core {
    public class ContainerGameObject : MonoBehaviour {

        static ContainerGameObject instance;

        Dictionary<string, Transform> Containers = new();

        public static void AddToContainer<TypeObject>(GameObject storeObject) where TypeObject : class{

            if (!instance) throw new Exception($"No Container Object. Please make a GameObject with {typeof(ContainerGameObject).ToString()} script");

            string typeName = typeof(TypeObject).Name;

            if(!instance.Containers.TryGetValue(typeName, out Transform selectedContainer)) {
                selectedContainer = new GameObject(typeName).transform;
                instance.Containers.Add(typeName, selectedContainer);
                selectedContainer.SetParent(instance.transform);
            }
            
            //Store Object the selected Container
            storeObject.transform.SetParent(selectedContainer);

        }

        private void Awake() {
            instance = this;
        }

    }

}

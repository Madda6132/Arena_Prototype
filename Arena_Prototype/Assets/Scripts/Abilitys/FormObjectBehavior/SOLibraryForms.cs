using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RPG.Abilitys.Form {
    /// <summary>
    /// Serves as a singleton for all the FormBehavior Prefabs
    /// </summary>
    [CreateAssetMenu(fileName = "Form Library", menuName = "Forms List", order = 0)]
    public class SOLibraryForms : ScriptableObject {

        //While this works, the Scriptable Object needs to be selected once when starting the editor
        //Otherwise the behaviorLibrary will be empty and will return errors
        public static SOLibraryForms instance;

        [SerializeField] List<AbstractFormBehavior> listOfFireBehaviors = new();
        [SerializeField] List<AbstractFormBehavior> listOfFrostBehaviors = new();
        [SerializeField] List<AbstractFormBehavior> listOfLightningBehaviors = new();

        Dictionary<AbilityElement, List<AbstractFormBehavior>> behaviorLibrary = new();

        public static AbstractFormBehavior GetForm<FormType>(AbilityElement element) where FormType : AbstractFormBehavior =>
            instance.behaviorLibrary[element].Where(a => a.GetType() == typeof(FormType)).First();
        
        public SOLibraryForms() {

            //Delete any other of the same Object
            //Only problem is if a new object is created and it's not aware of the old one the old one will be deleted instead :(
            if (instance != null && instance != this) {
                Debug.LogWarning("Already exists");
                DestroyImmediate(this, true);
            } else {
                instance = this;
            }

            //Add to Library for when form creates a behavior
            behaviorLibrary.Add(AbilityElement.Fire, listOfFireBehaviors);
            behaviorLibrary.Add(AbilityElement.Frost, listOfFrostBehaviors);
            behaviorLibrary.Add(AbilityElement.Lightning, listOfLightningBehaviors);
        }

        private void OnEnable() {
            if (instance != null && instance != this) {
                Debug.LogWarning("Already exists");
                DestroyImmediate(this, true);
            } else {
                instance = this; 
            }
        }

        

    }

}

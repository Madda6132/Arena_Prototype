using UnityEngine;

namespace RPG.Creatures.UI {
    public class UILookAtCamera : MonoBehaviour {

        Transform cameraTransform;
        [SerializeField] bool isActive = true;



        public void Activate() => isActive = true;
        public void Deactivate() => isActive = false;

        /*---Private---*/

        private void Start() {
            cameraTransform = Camera.main.transform;
        }

        // Update is called once per frame
        void LateUpdate() {

            //Look at camera
            if (isActive) {
                transform.LookAt(transform.position + cameraTransform.forward);
            }
        }
    }
}


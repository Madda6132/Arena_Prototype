using UnityEngine;

namespace RPG.Creatures {
    /// <summary>
    /// Equipment Manager uses these to find placement for equipment
    /// </summary>
    public class ItemHolder : MonoBehaviour {

        [SerializeField] ItemPlacement placement;

        public enum ItemPlacement {
            RightArm,
            LeftArm
        }

        public ItemPlacement GetPlacement => placement;

    }
}

using UnityEngine;

namespace RPG.Abilitys {
    /// <summary>
    /// Used on Form to filter out GameObjects
    /// </summary>
    public interface IFilter
    {
        public GameObject[] Filter(GameObject[] list);
    }
}
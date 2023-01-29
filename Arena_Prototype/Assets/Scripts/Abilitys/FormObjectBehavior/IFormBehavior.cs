using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Abilitys.Form {


    public interface IFormBehavior
    {
        /// <summary>
        /// When the form starts this call is called once
        /// </summary> 
        public void SubToOnStartForm(Action<AbstractFormBehavior, Vector3> listener);
        /// <summary>
        /// When the form collides with something it's triggered
        /// </summary> 
        public void SubToOnTriggeredForm(Action<AbstractFormBehavior, GameObject[]> listener);
        /// <summary>
        /// During the forms existence a timer will call tick, depending on the tickTimer variable 
        /// </summary> 
        public void SubToOnTickForm(Action<AbstractFormBehavior, Vector3> listener);
        /// <summary>
        /// When the form ends this call is called once
        /// </summary> 
        public void SubToOnEndForm(Action<AbstractFormBehavior, Vector3> listener);

        /// <summary>
        /// When the form starts this call is called once
        /// </summary> 
        public void UnsubToOnStartForm(Action<AbstractFormBehavior, Vector3> listener);
        /// <summary>
        /// When the form collides with something it's triggered
        /// </summary>
        public void UnsubToOnTriggeredForm(Action<AbstractFormBehavior, GameObject[]> listener);
        /// <summary>
        /// During the forms existence a timer will call tick, depending on the tickTimer variable 
        /// </summary>
        public void UnsubToOnTickForm(Action<AbstractFormBehavior, Vector3> listener);
        /// <summary>
        /// When the form ends this call is called once
        /// </summary> 
        public void UnsubToOnEndForm(Action<AbstractFormBehavior, Vector3> listener);
    }
}
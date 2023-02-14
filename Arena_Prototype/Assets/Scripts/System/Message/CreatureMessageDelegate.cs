using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.GameSystem {

    public class CreatureMessageDelegate : MonoBehaviour {

        //ScriptableObject soundList
        //ScriptableObject textStyleList

        //MessageListGameObject errorList
        //MessageListGameObject messageList
        //MessageListGameObject PresentationList

        //No text? Only sound
        //No Sound Quiet or default message sound?
        //-Error
        //-Message
        //-Alert
        //-Presentation

        public void DelegateError(string message) {

            Debug.LogWarning(message);
        }
    }
}


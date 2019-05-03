using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
namespace vrctwitter {
    public class texInput : MonoBehaviour {

        public GameObject SystemBase = null;
        public vrctwitter.variable variableScript;
        public InputField usernameInput = null;
        public InputField passwordInput = null;
        public InputField timerInput = null;

        public void texEdit(){
            variableScript.username = usernameInput.text;
            variableScript.password = passwordInput.text;
            variableScript.timer = float.Parse(timerInput.text);
            StartCoroutine(texUpdate());
        }

        public IEnumerator texUpdate() {
            SystemBase.SetActive (false);
            yield return new WaitForSeconds (1.0f); 
            SystemBase.SetActive (true);
        }
    }
}
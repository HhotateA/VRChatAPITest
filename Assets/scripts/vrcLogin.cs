using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using static vrctwitter.getJson;
namespace vrctwitter {
    public class vrcLogin : MonoBehaviour {

        //private GameObject variableObject;
        public vrctwitter.variable variableScript;
        private Camera cam;

        void Start() {
            cam = Camera.main;
        }

        void OnEnable() {
            //variableObject = GameObject.Find ("variable");
            //variableScript = variableObject.GetComponent<vrctwitter.variable>();
            StartCoroutine(getAPIkey());
            StartCoroutine(loginAPI());
        }

        string authenticate(string username, string password){
            string auth = username + ":" + password;
            auth = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
            auth = "Basic " + auth;
            return auth;
        }

        IEnumerator getAPIkey() {
            UnityWebRequest loginRequest = UnityWebRequest.Get("https://api.vrchat.cloud/api/1/config"); //APIキーをリクエスト
            yield return loginRequest.SendWebRequest();
            if(loginRequest.isNetworkError || loginRequest.isHttpError) {
                Debug.Log(loginRequest.error);
                cam.backgroundColor = new Color(255,0,14,1.0f);
            }
            else {
                vrctwitter.getJson keyJson = JsonUtility.FromJson(loginRequest.downloadHandler.text, typeof(vrctwitter.getJson)) as vrctwitter.getJson; //APIキーを入手
                Debug.Log(keyJson.clientApiKey);
                cam.backgroundColor = new Color(0,244,255,1.0f);
                variableScript.apikey = keyJson.clientApiKey; //APIキーを記録
            }
        }

        IEnumerator loginAPI() {
            UnityWebRequest loginRequest = UnityWebRequest.Get("https://api.vrchat.cloud/api/1/auth/user?apiKey=" + variableScript.apikey); //セッションにログイン
            loginRequest.SetRequestHeader("ContentType", "application/json");
            loginRequest.SetRequestHeader("Authorization", authenticate(variableScript.username,variableScript.password)); //Basic認証
            yield return loginRequest.SendWebRequest();
            if(loginRequest.isNetworkError || loginRequest.isHttpError) {
                Debug.Log(loginRequest.error);
                cam.backgroundColor = new Color(255,0,14,1.0f);
            }
            else {
                vrctwitter.getJson session = JsonUtility.FromJson(loginRequest.downloadHandler.text, typeof(vrctwitter.getJson)) as vrctwitter.getJson;
                variableScript.friendInt = session.friends.Length; //フレンド数を記録
                variableScript.friendList = session.friends; //フレンドリストを記録
                Debug.Log(session.friends.Length);
                cam.backgroundColor = new Color(0,244,255,1.0f);
            }
        }

    }
}
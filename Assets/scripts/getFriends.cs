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
    public class getFriends : MonoBehaviour {

        //private GameObject variableObject;
        public vrctwitter.variable variableScript;
        private Camera cam;

        void Start() {
            cam = Camera.main;
        }

        void OnEnable() {
            StartCoroutine(roop());
        }

        IEnumerator roop(){
            while(true){
                yield return new WaitForSeconds (Math.Min(variableScript.timer,5.0f)); 
                StartCoroutine(getFriend());
                if(variableScript.friendInt>=variableScript.friendList.Length){
                    variableScript.friendInt = variableScript.friendList.Length;
                }else{
                    for(int index=variableScript.friendInt;index<variableScript.friendList.Length;index++){
                        Debug.Log(index);
                        Debug.Log(variableScript.friendList[index]);
                        StartCoroutine(getTwitter(variableScript.friendList[index]));
                    }
                    variableScript.friendInt = variableScript.friendList.Length;
                }
            }
        }

        IEnumerator getFriend() {
            UnityWebRequest friendRequest = UnityWebRequest.Get("https://api.vrchat.cloud/api/1/auth/user?apiKey=" + variableScript.apikey); //セッションにログイン
            yield return friendRequest.SendWebRequest();
            if(friendRequest.isNetworkError || friendRequest.isHttpError) {
                Debug.Log(friendRequest.error);
                cam.backgroundColor = new Color(255,0,14,1.0f);
            }
            else {
                vrctwitter.getJson session = JsonUtility.FromJson(friendRequest.downloadHandler.text, typeof(vrctwitter.getJson)) as vrctwitter.getJson;
                //variableScript.friendInt = session.friends.Length; //フレンド数を記録
                variableScript.friendList = session.friends; //フレンドリストを記録
                Debug.Log(session.friends.Length);
                cam.backgroundColor = new Color(0,244,255,1.0f);
            }
        }

        IEnumerator getTwitter(string id) {
            UnityWebRequest twitterRequest = UnityWebRequest.Get("https://vrcprofile.com/api/user_profiles?ids=" + id); //VRCProfileにアクセス
            yield return twitterRequest.SendWebRequest();
            if(twitterRequest.isNetworkError || twitterRequest.isHttpError) {
                Debug.Log(twitterRequest.error);
                cam.backgroundColor = new Color(255,0,14,1.0f);
            }
            else {
                vrctwitter.getProfile profile = JsonUtility.FromJson(twitterRequest.downloadHandler.text, typeof(vrctwitter.getProfile)) as vrctwitter.getProfile;
                Debug.Log(profile.userProfiles[0].twitter);
                cam.backgroundColor = new Color(0,244,255,1.0f);
                if(profile.userProfiles[0].twitter != "") Application.OpenURL("https://twitter.com/" + profile.userProfiles[0].twitter); //twitterIDがあればブラウザで開く
            }
        }
    }
}
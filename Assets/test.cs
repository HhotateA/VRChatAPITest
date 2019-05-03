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

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(getTwitter("usr_d6caa57b-ccf5-4380-b171-9c75ef51a6cc"));
    }

        IEnumerator getTwitter(string id) {
            UnityWebRequest twitterRequest = UnityWebRequest.Get("https://vrcprofile.com/api/user_profiles?ids=" + id); //セッションにログイン
            yield return twitterRequest.SendWebRequest();
            if(twitterRequest.isNetworkError || twitterRequest.isHttpError) {
                Debug.Log(twitterRequest.error);
            }
            else {
                Debug.Log(twitterRequest.downloadHandler.text);
                vrctwitter.getProfile profile = JsonUtility.FromJson(twitterRequest.downloadHandler.text, typeof(vrctwitter.getProfile)) as vrctwitter.getProfile;
                //vrctwitter.getJson account = JsonUtility.FromJson(string(profile.userProfiles), typeof(vrctwitter.getJson)) as vrctwitter.getJson;
                Debug.Log(profile.userProfiles[0].twitter);
                Application.OpenURL("https://twitter.com/" + profile.userProfiles);
            }
        }
}

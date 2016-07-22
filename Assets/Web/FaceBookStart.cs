using UnityEngine;
using System.Collections;
using Facebook.Unity;
using System.Collections.Generic;

public class FaceBookStart : MonoBehaviour
{
    public NickNameUI nickNameUI;

    // Use this for initialization
    void Awake()
    {
        // 이미 초기화 됬는지
        if (!FB.IsInitialized)
        {
            // 초기화, 활성화 콜백을 등록하면서 FB 초기화
            FB.Init(InitCallback, HideCallBack);
            Debug.Log("FaceBook Init");
        }
        else
        {
            FB.ActivateApp();
            Debug.Log("FaceBook Already Init");
        }
    }

    public void FaceBookLogin()
    {
        FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, LoginCallback);
    }

    public void FaceBookLinkShare()
    {
        FB.ShareLink(new System.Uri("https://developers.facebook.com/"), callback: ShareCallback);
    }

    public void FaceBookLogEvent()
    {
        var tutParams = new Dictionary<string, object>();
        tutParams[AppEventParameterName.ContentID] = "tutorial_step_1";
        tutParams[AppEventParameterName.Description] = "First step in the tutorial, clicking the first button!";
        tutParams[AppEventParameterName.Success] = "1";

        FB.LogAppEvent(
            AppEventName.CompletedTutorial,
            parameters: tutParams
        );
    }

    // 페이스북 API 처음 초기화 콜백
    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    // 앱 활성화/비활성화 콜백
    private void HideCallBack(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    // 페이스북 로그인 콜백 
    private void LoginCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID

            if (PlayerPrefs.HasKey("FaceBookKey"))
            {

            }
            else
            {
                Debug.Log("FaceBookKey : " + aToken.UserId);
                PlayerPrefs.SetString("FaceBookKey", aToken.UserId);
            }

            // 기존 페이스북을 연동한 계정이 존재하는지
            if (nickNameUI.CheckDBFromFaceBook(PlayerPrefs.GetString("FaceBookKey").ToString()))
            {
                nickNameUI.ChangeActive(UIState.success, UIState.facebook);
            }
            else
            {
                if (!nickNameUI.CreateUserDB(PlayerPrefs.GetString("FaceBookKey").ToString()))
                {
                    Debug.Log("Create DB Error");
                }
                else
                {
                    nickNameUI.ChangeActive(UIState.success, UIState.facebook);
                }
            }
            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions)
            {
                Debug.Log(perm);
            }
        }
        else {
            Debug.Log("User cancelled login");
        }
    }

    private void ShareCallback(IShareResult result)
    {
        if (result.Cancelled || !string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("ShareLink Error: " + result.Error);
        }
        else if (!string.IsNullOrEmpty(result.PostId))
        {
            // Print post identifier of the shared content
            Debug.Log(result.PostId);
        }
        else {
            // Share succeeded without postID
            Debug.Log("ShareLink success!");
        }
    }
}

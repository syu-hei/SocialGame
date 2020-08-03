using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
	[SerializeField]
	Text userID;

    private UserProfileModel userProfileModel;

	void Awake()
	{
		string DBPath = Application.persistentDataPath + "/game.db";

		if (!File.Exists(DBPath)) {
			File.Create(DBPath);
		}

		UserProfile.CreateTable();
    }

    void Start()
	{
		userProfileModel = UserProfile.Get();
		if (!string.IsNullOrEmpty(userProfileModel.user_id)) {
			userID.text = "ID : " + userProfileModel.user_id;
		}
	}

	public void LoginButtonEvent()
	{
		UserProfileModel userProfileModel = UserProfile.Get();
		if (string.IsNullOrEmpty(userProfileModel.user_id)) {
			Action action = () => {
				UnityEngine.Debug.Log("登録完了");
			};
			StartCoroutine(CommunicationManager.ConnectServer("registration", "", action));
		} else {
			Action action = () => {
				//ログイン後の処理を記載
		};
		StartCoroutine(CommunicationManager.ConnectServer("login","&user_id=" + userProfileModel.user_id, action));
		}
	}
}
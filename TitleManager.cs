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
		Action action = () => {
			userProfileModel = UserProfile.Get();
		if (!string.IsNullOrEmpty(userProfileModel.user_id)) {
			userID.text = "ID : " + userProfileModel.user_id;
			}
    	};
	StartCoroutine(CommunicationManager.ConnectServer("registration", "", action));
	}
}
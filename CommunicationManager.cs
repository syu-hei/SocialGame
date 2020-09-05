using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Linq;

[Serializable]
public class ResponseObjects
{
    public int master_data_version;
	public UserProfileModel user_profile;
    public UserLoginModel user_login;
    public UserQuestModel[] user_quest;
    public UserCharacterModel[] user_character;
    public UserPresentModel[] user_present;
    public MasterLoginItemModel[] master_login_item;
    public MasterQuestModel[] master_quest;
    public MasterCharacterModel[] master_character;
    public MasterGachaModel[] master_gacha;
    public MasterShopModel[] master_shop;
}

public class CommunicationManager : MonoBehaviour
{
	//接続先のサーバーのアドレスを設定
	const string URL = "https://service-dev/public/";
	private const string ERROR_MASTER_DATA_UPDATE = "1";
    private const string ERROR_DB_UPDATE = "2";
    private const string ERROR_INVALID_DATA = "3";
    private const string ERROR_INVALID_SCHEDULE = "4";
    private const string ERROR_COST_SHORTAGE = "5";

    public static IEnumerator ConnectServer(string endpoint, string paramater, Action action = null)
	{
        int masterDataVersion = PlayerPrefs.GetInt("master_data_version", 0);
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(URL + endpoint + "?client_master_version=" + masterDataVersion + paramater);
        yield return unityWebRequest.SendWebRequest();

        if (!string.IsNullOrEmpty(unityWebRequest.error)) {
			//エラー
			UnityEngine.Debug.LogError(unityWebRequest.error);
            yield break;
        }

        //レスポンスを取得
		string text = unityWebRequest.downloadHandler.text;

        if (text.All(char.IsNumber)) {
			//エラーの場合
			switch (text) {
				case ERROR_MASTER_DATA_UPDATE:
                    UnityWebRequest masterDataRequest = UnityWebRequest.Get("ttps://service-dev/public/master_data");
                    yield return masterDataRequest.SendWebRequest();

                    //レスポンス取得
                    string masterText = unityWebRequest.downloadHandler.text;
                    ResponseObjects masterResponseObjects = JsonUtility.FromJson<ResponseObjects>(masterText);

                    if (masterResponseObjects.master_login_item != null)
                    {
                        MasterLoginItem.Set(masterResponseObjects.master_login_item);
                    }

                    if (masterResponseObjects.master_quest != null)
                    {
                        MasterQuest.Set(masterResponseObjects.master_quest);
                    }

                    if (masterResponseObjects.master_character != null)
                    {
                        MasterCharacter.Set(masterResponseObjects.master_character);
                    }

                    if (masterResponseObjects.master_gacha != null)
                    {
                        MasterGacha.Set(masterResponseObjects.master_gacha);
                    }

                    if (masterResponseObjects.master_shop != null)
                    {
                        MasterShop.Set(masterResponseObjects.master_shop);
                    }

                    //マスターデータのバージョンはローカルに保存
                    PlayerPrefs.SetInt("master_data_version", masterResponseObjects.master_data_version);
                    break;
				case ERROR_DB_UPDATE:
					UnityEngine.Debug.LogError("サーバーでエラーが発生しました。[データベース更新エラー]");
					break;
                case ERROR_INVALID_DATA:
                    UnityEngine.Debug.LogError("サーバーでエラーが発生しました。[不正なデータ]");
                    break;
                case ERROR_INVALID_SCHEDULE:
                    UnityEngine.Debug.LogError("サーバーでエラーが発生しました。[期間外]");
                    break;
                case ERROR_COST_SCHEDULE:
                    UnityEngine.Debug.LogError("サーバーでエラーが発生しました。[アイテム不足]");
                    break;
                default:
					break;
			}
		} else {
			ResponseObjects responseObjects = JsonUtility.FromJson<ResponseObjects>(text);

            //SQLiteへ保存
            if (!string.IsNullOrEmpty(responseObjects.user_profile.user_id))
            {
            user_profile.Set(responseObjects.user_profile);
            }

            if (!string.IsNullOrEmpty(responseObjects.user_login.user_id))
            {
            user_login.Set(responseObjects.user_login);
            }

            if (responseObjects.user_quest != null)
            {
                UserQuest.Set(responseObjects.user_quest);
            }

            if (responseObjects.user_character != null)
            {
                UserCharacter.Set(responseObjects.user_character);
            }

            if (responseObjects.user_present != null)
            {
                UserPresent.Set(responseObjects.user_present);
            }

            if (action != null) {
                action();
            }
        }
    }
}
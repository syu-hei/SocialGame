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
    public MasterLoginItemModel[] master_login_item;
}

public class CommunicationManager : MonoBehaviour
{
	//接続先のサーバーのアドレスを設定
	const string URL = "https://service-dev/public/";
	private const string ERROR_MASTER_DATA_UPDATE = "1";
    private const string ERROR_DB_UPDATE = "2";
    private const string ERROR_INVALID_DATA = "3";

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

                    //マスターデータのバージョンはローカルに保存
                    PlayerPrefs.SetInt("master_data_version", masterResponseObjects.master_data_version);
                    break;
				case ERROR_DB_UPDATE:
					UnityEngine.Debug.LogError("サーバーでエラーが発生しました。[データベース更新エラー]");
					break;
                case ERROR_INVALID_DATA:
                    UnityEngine.Debug.LogError("サーバーでエラーが発生しました。[不正なデータ]");
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

        if (action != null) {
				action();
            }
        }
    }
}
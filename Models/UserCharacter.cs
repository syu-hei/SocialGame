using System;

[Serializable]
public class UserCharacterModel
{
	public int id;
	public int character_id;
}

public static class UserCharacter
{
	public static void CreateTable()
	{
		string query = "create table if not exists user_character (id int, character_id int, primary key(id))";
		SqliteDatabase sqlDB = new SqliteDatabase("game.db");
		sqlDB.ExecuteQuery(query);
	}

    public static void Set(UserCharacterModel[] user_character_model_list)
	{
		//キャラクターが売却されてもデータが残り続けないように一度ドロップする
		string dropQuery = "drop table if exists user_character";
		SqliteDatabase sqlDB = new SqliteDatabase("game.db");
		sqlDB.ExecuteQuery(dropQuery);

		CreateTable();

		foreach (UserCharacterModel userCharacterModel in user_character_model_list) {
			string query = "insert or replace into user_character (id, character_id) values(" +
				userCharacterModel.id + ", " +
				userCharacterModel.character_id + ")";
			sqlDB.ExecuteNonQuery(query);
		}
	}
}
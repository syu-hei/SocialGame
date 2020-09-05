using System;
using System.Collections.Generic;

[Serializable]
public class MasterCharacterModel
{
	public int character_id;
	public string asset_id;
	public string character_name;
	public int rarity;
	public int type;
	public int sell_point;
}

public static class MasterCharacter
{
	public static void CreateTable()
	{
		string query = "create table if not exists master_character (character_id int, asset_id text, character_name text, rarity int, type int, sell_point int, primary key(character_id))";
		SqliteDatabase sqlDB = new SqliteDatabase("game.db");
		sqlDB.ExecuteQuery(query);
	}

    public static void Set(MasterCharacterModel[] master_character_model_list)
	{
		SqliteDatabase sqlDB = new SqliteDatabase("game.db");
		foreach (MasterCharacterModel masterCharacterModel in master_character_model_list) {
			string query = "insert or replace into master_character (character_id, asset_id, character_name, rarity, type, sell_point) values(" +
				masterCharacterModel.character_id + ", \"" +
				masterCharacterModel.asset_id + "\", \"" +
				masterCharacterModel.character_name + "\", " +
				masterCharacterModel.rarity + ", " +
				masterCharacterModel.type + ", " +
				masterCharacterModel.sell_point + ")";
			sqlDB.ExecuteNonQuery(query);
		}
	}
}
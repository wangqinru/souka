using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;

namespace GraduationProject
{
	public class JsonHandler {

		public static SaveData GetCharacterData ()
		{
			/*TextAsset jsonAsset = Resources.Load ("Data/AttackInfo", typeof(TextAsset)) as TextAsset;
			JsonData data = JsonMapper.ToObject(jsonAsset.text);

			SaveData saveData = JsonMapper.ToObject<SaveData> (jsonAsset.text);
			data = data["CharacterData"];
			saveData.characters = JsonMapper.ToObject<CharacterData[]> (data.ToJson ());

			for (int i=0; i<data.Count; i++)
			{
				saveData.characters[i].attackInfo = JsonMapper.ToObject<att[]> ((data[i]["AttackInfo"]).ToJson ());
			}*/

			return new SaveData ();
		}
	};

	public class SaveData {

		public string version {get; set;}
		public int money {get; set;}
		public CharacterData[] characters {get; set;}

		public SaveData ()
		{
		}

	};
}



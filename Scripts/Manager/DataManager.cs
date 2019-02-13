using UnityEngine;
using System.Collections;
using LitJson;

namespace GraduationProject
{
	public class DataManager {

		static public DataManager instance = null;

		public StageData[] stageData {get; set;}
		public CharacterData[] characterData {get; set;}
		public EnemyData[] enemyData {get; set;}
		public FreeModeData freeModeData {get; set;}

		static public DataManager GetInstance ()
		{
			if (instance == null)
				instance = new DataManager ();

			return instance;
		}

		private DataManager ()
		{
			TextAsset jsonAsset = Resources.Load ("Data/StageData", typeof(TextAsset)) as TextAsset;
			JsonData[] array = JsonMapper.ToObject<JsonData[]> (jsonAsset.text);
			
			stageData = JsonMapper.ToObject<StageData[]> (jsonAsset.text);
			for (int i=0; i<stageData.Length; i++)
			{
				stageData[i].labelInfo = JsonMapper.ToObject<LabelInformation> (array[i]["LabelInfo"].ToJson ());
			}
			
			jsonAsset = Resources.Load ("Data/CharacterData", typeof(TextAsset)) as TextAsset;
			array = JsonMapper.ToObject<JsonData[]> (jsonAsset.text);
			
			characterData = new CharacterData[array.Length];
			
			for (int i=0; i<array.Length; i++)
			{
				characterData[i] = new CharacterData (array[i]);
			}

			jsonAsset = Resources.Load ("Data/EnemyData", typeof(TextAsset)) as TextAsset;
			enemyData = JsonMapper.ToObject<EnemyData[]> (jsonAsset.text);
			
			freeModeData = new FreeModeData (0, 0);
		}
		// Use this for initialization
		void Start () {

		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}

	public class FreeModeData
	{
		public int characterSelectNumber;
		public int stageSelectNumber;

		public FreeModeData (int c, int s)
		{
			characterSelectNumber = c;
			stageSelectNumber = s;
		}
	}

	public class StageData
	{
		public string name {get; private set;}
		public string Conditions {get; private set;}
		public float[] startPosition {get; private set;} 
		public float[] startAngle {get; private set;} 
		public int difficulty {get; private set;}
		public bool open {get; set;}
		public LabelInformation labelInfo {get; set;}

		public StageData ()
		{
			
		}

	}

	public class CharacterData
	{
		public bool open {get; set;}
		public string[] audio {get; set;}
		public CharacterStatus status {get; set;}
		public AttackInformation[] attackInfo {get; set;}
		public LabelInformation labelInfo {get; set;}

		public CharacterData ()
		{
		}

		public CharacterData (JsonData data)
		{
			open = (bool)data["open"];
			audio = JsonMapper.ToObject<string[]> (data["audio"].ToJson ());
			status = JsonMapper.ToObject<CharacterStatus> (data["status"].ToJson ());
			attackInfo = JsonMapper.ToObject<AttackInformation[]> (data["AttackInfo"].ToJson ());
			labelInfo = JsonMapper.ToObject<LabelInformation> (data["LabelInfo"].ToJson ());
		}
	}

	public class CharacterStatus
	{
		public string name {get; private set;}
		public int level {get; set;}
		public int experience {get; set;}
		public int hp {get; set;}
		public int attack {get; set;}
		public int skill {get; set;}
		public int defense {get; set;}
		public int agile {get; set;}

		public CharacterStatus ()
		{
		}
	}

	public class LabelInformation
	{
		public string modelPath {get; private set;}
		public string[] buttonImageFile {get; private set;}
		public string stageImageFile {get; private set;}
		public string faceImageFile {get; private set;}
		public string explanatoryText {get; private set;}

		public LabelInformation ()
		{
		}
	}

	public class AttackInformation
	{
		public string[] stateName {get; private set;}
		public float[] moveSpeed {get; private set;}
		public float[] acceleration {get; private set;}
		public float[] jumpPower {get; private set;}
		public float[] gravity {get; private set;}
		public float[] eulerZ {get; private set;}
		public float[] ForceY {get; private set;}
		public float[] ForceZ {get; private set;}
		public int[] se {get; private set;}
		
		public AttackInformation ()
		{
		}
	}

	public class EnemyData 
	{
		public string name {get; private set;}
		public string modlePath {get; private set;}
		public int Hp {get; private set;}
		public int attack {get; private set;}
		public int range {get; private set;}
		public int speed {get; private set;}

		public EnemyData ()
		{
		}
	}
}
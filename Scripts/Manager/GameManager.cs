using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace GraduationProject
{
	public class GameManager : MonoBehaviour, IGameManager {

		public DataManager dataManager {get; private set;}
		public SoundManager soundManager {get; private set;}
		private InputManager inputManager;
		private CreateEnemyPoint[] childPoints;
		private PlayerController player;
		private float ACTIVE_RANGE = 50f;
		public BattleUI battleUI {get; private set;}
		public int killerNumber {get; set;}

		public int signalCounter {get; set;}

		// Use this for initialization
		void Awake () {
			soundManager = GetComponent<SoundManager> ();
			dataManager = DataManager.GetInstance ();
			InvokeRepeating ("CheckCreatePoint", 3f, 2f);
		}

		void Start ()
		{
			inputManager = GetComponent<InputManager> ();
			CharacterData nowChara = dataManager.characterData[dataManager.freeModeData.characterSelectNumber];
			soundManager.SetCurrentSE (nowChara.audio);
			StageData nowStage = dataManager.stageData[dataManager.freeModeData.stageSelectNumber];

			Instantiate (Resources.Load (nowStage.labelInfo.modelPath, typeof (GameObject)) );
			GameObject playerObject = Instantiate (Resources.Load (nowChara.labelInfo.modelPath, typeof (GameObject)), 
				                      new Vector3 (nowStage.startPosition[0], nowStage.startPosition[1]/10f, nowStage.startPosition[2]),
				                      Quaternion.Euler (nowStage.startAngle[0], nowStage.startAngle[1], nowStage.startAngle[2])) as GameObject; 

			PlayerCamera pcamera = GameObject.Find ("Main Camera").AddComponent<PlayerCamera> ();

			player = playerObject.AddComponent <PlayerController> ();
			player.Prepare (this, pcamera, nowChara);

			battleUI = new BattleUI (nowChara.labelInfo, dataManager);


			childPoints = new CreateEnemyPoint[transform.childCount];

			for (int i=0; i<childPoints.Length; i++)
			{
				GameObject obj = transform.GetChild (i).gameObject;
				obj.SetActive (true);
				childPoints[i] = obj.GetComponent<CreateEnemyPoint> ();
				childPoints[i].player = player;
			}

			soundManager.PlayBGM (2);

			foreach (CreateEnemyPoint child in childPoints)
				child.SetEnemyEnable (false);
			
			player.enabled = false;
			player.GetComponent<Animator> ().enabled = false;
		}
		
		// Update is called once per frame
		void Update () {

			if (signalCounter == 30)
			{
				foreach (CreateEnemyPoint child in childPoints)
				{
					if (child.enabled)child.SetCaptainTargetPosition ();
				}
			}
			
			battleUI.UpdateUI (player);
			battleUI.UpdateBattleMenu (inputManager, this);
			Insurance ();
		}

		public void Result (int index)
		{
			string[] files = new string[2]
			{
				"UserInterface/label_win",
				"UserInterface/label_lose"
			};
			battleUI.result.sprite = Resources.Load (files[index], typeof (Sprite)) as Sprite;
			StartCoroutine ("StartResult");
		}

		private void CheckCreatePoint ()
		{
			foreach (CreateEnemyPoint child in childPoints)
			{
				if (Vector3.Distance (child.transform.position, player.transform.position) < ACTIVE_RANGE)
					child.enabled = true;
				else
					child.enabled = false;
			}
		}

		public void Insurance ()
		{
			if (player.transform.position.y <= -20f)
			{
				StageData nowStage = dataManager.stageData[dataManager.freeModeData.stageSelectNumber];
				player.transform.position = new Vector3 (nowStage.startPosition[0], nowStage.startPosition[1]/10f, nowStage.startPosition[2]);
			}
		}

		void IGameManager.SetEnemyUnitsEnable (bool flag)
		{
			foreach (CreateEnemyPoint child in childPoints)
				child.SetEnemyEnable (flag);

			player.enabled = flag;
			player.GetComponent<Animator> ().enabled = flag;

			if (flag) battleUI.SetMapSize (false);
		}

		void IGameManager.SetEnemyUnitsActive ()
		{
			foreach (CreateEnemyPoint child in childPoints)
				child.SetEnemyEnable (true);
			
			player.enabled = true;
			player.GetComponent<Animator> ().enabled = true;
		}

		IEnumerator StartResult ()
		{
			int count = 0;

			while (++count <= 60)
			{
				battleUI.result.color = new Color (1, 1, 1, (float)count/60f);
			}

			yield return new WaitForSeconds (5f);

			Application.LoadLevel ("TitleScene");

			yield return 0;
		}
	}
}

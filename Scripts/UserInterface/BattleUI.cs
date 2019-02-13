using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace GraduationProject
{
	public class BattleUI 
	{
		static public Canvas canvas;
		static public Canvas battleMapCanvas;
		private Image hpGauge;
		private Image musouGauge;
		private Image player_maker;
		private Image[] killNumber;

		private DataManager dataManager;
		private IInputEvent battleMenu;
		public Image result;

		private string[] fileName = new string[10]
		{
			"UserInterface/Number/z0",
			"UserInterface/Number/z1",
			"UserInterface/Number/z2",
			"UserInterface/Number/z3",
			"UserInterface/Number/z4",
			"UserInterface/Number/z5",
			"UserInterface/Number/z6",
			"UserInterface/Number/z7",
			"UserInterface/Number/z8",
			"UserInterface/Number/z9",
		};

		static public float ratio_width = Screen.width / 1920f;
		static public float ratio_height = Screen.height / 1080f;
		
		static public float FIELD_MIN_X = 66;
		static public float FIELD_MAX_X = -100;
		static public float FIELD_MIN_Z = -45;
		static public float FIELD_MAX_Z = 48;
		
		static public float MAP_MIN_X = 1591*(Screen.width / 1920f);
		static public float MAP_MAX_X = 1896*(Screen.width / 1920f);
		static public float MAP_MIN_Z = 751*(Screen.height / 1080f);
		static public float MAP_MAX_Z = 1056*(Screen.height / 1080f);

		static private float BMAP_MIN_X = 926*(Screen.width / 1920f);
		static private float BMAP_MAX_X = 1900*(Screen.width / 1920f);
		static private float BMAP_MIN_Z = 226*(Screen.height / 1080f);
		static private float BMAP_MAX_Z = 1048*(Screen.height / 1080f);

		static public Vector2 fieldSize;
		static public Vector2 mapSize;
		static private Vector2 bigMapSize;
		static private Vector2 mapRatio;

		public BattleUI (LabelInformation info, DataManager dataMan)
		{
			dataManager = dataMan;
			GameObject canvasObject = MonoBehaviour.Instantiate (Resources.Load ("Prefabs/BattleUICanvas", typeof(GameObject))) as GameObject;
			canvas = canvasObject.GetComponent<Canvas> ();

			GameObject canvasM = new GameObject ();
			canvasM.name = "battleMapCanvas";
			battleMapCanvas = canvasM.AddComponent<Canvas> ();
			battleMapCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

			for (int i=0; i<canvasObject.transform.childCount; i++)
			{
				Image image = canvasObject.transform.GetChild (i).GetComponent<Image> ();
				image.rectTransform.position = new Vector3 (image.rectTransform.position.x*ratio_width, image.rectTransform.position.y*ratio_height, 0f);
				image.rectTransform.sizeDelta = new Vector2 (image.rectTransform.sizeDelta.x*ratio_width, image.rectTransform.sizeDelta.y*ratio_height);

				if (image.transform.childCount > 0)
				{
					for (int j=0; j<image.transform.childCount; j++)
					{
						Image child = image.transform.GetChild (j).GetComponent<Image> ();
						child.rectTransform.position = new Vector3 (child.rectTransform.position.x*ratio_width, child.rectTransform.position.y*ratio_height, 0f);
						child.rectTransform.sizeDelta = new Vector2 (child.rectTransform.sizeDelta.x*ratio_width, child.rectTransform.sizeDelta.y*ratio_height);
					}
				}
			}
			
			fieldSize = new Vector2 (FIELD_MAX_X-FIELD_MIN_X, FIELD_MAX_Z-FIELD_MIN_Z);
			mapSize = new Vector2 (MAP_MAX_X-MAP_MIN_X, MAP_MAX_Z-MAP_MIN_Z);
			bigMapSize = new Vector2 (BMAP_MAX_X-BMAP_MIN_X, BMAP_MAX_Z-BMAP_MIN_Z);
			mapRatio = new Vector2 (mapSize.x/bigMapSize.x, mapSize.y/bigMapSize.y);

			Image miniMap = canvasObject.transform.Find ("minimap").GetComponent<Image> ();
			miniMap.transform.SetParent (battleMapCanvas.transform);

			Image face = canvasObject.transform.Find ("chara_image").GetComponent<Image> ();
			face.sprite = Resources.Load ("UserInterface/"+info.faceImageFile, typeof (Sprite)) as Sprite;

			Image name = canvasObject.transform.Find ("chara_name").GetComponent<Image> ();
			name.sprite = Resources.Load ("UserInterface/"+info.buttonImageFile[0], typeof (Sprite)) as Sprite;

			player_maker = canvasObject.transform.Find ("unit_player").GetComponent<Image> ();
			player_maker.transform.SetParent (battleMapCanvas.transform);
			hpGauge = canvasObject.transform.Find ("gauge_hp").GetComponent<Image> ();
			result = canvasObject.transform.Find ("result").GetComponent <Image> ();

			killNumber = new Image[3];
			for (int i=0; i<killNumber.Length; i++)
			{
				killNumber[i] = canvasObject.transform.Find ("killer_number00"+(i+1).ToString ()).GetComponent <Image> ();
				killNumber[i].color = new Color (1, 1, 1, 0);
			}

			killNumber[0].sprite = Resources.Load (fileName[0], typeof (Sprite)) as Sprite;
			killNumber[0].color = new Color (1, 1, 1, 1);

			//SetMapSize (true);
			battleMenu = new ManualMenu (new string[3] {"Other/manual_op", "Other/manual_op2", "Other/manual_op3"});
			battleMenu.CreateCanvas (dataManager);
		}

		public void UpdateBattleMenu (InputManager inputManager, IGameManager igameManager)
		{
			if (inputManager.StartButton == 1)
			{
				if (battleMenu == null)
				{
					igameManager.SetEnemyUnitsEnable (false);
					battleMenu = new BattleMenu ();
					battleMenu.CreateCanvas (dataManager);
					SetMapSize (true);
				}
			}

			if (battleMenu != null)
			{
				battleMenu.UpdateAnimation ();
				if (inputManager.StickUD == 1)
				{
					battleMenu.MoveCursor (1);
				}
				else if (inputManager.StickUD == -1)
				{
					battleMenu.MoveCursor (-1);
				}
				
				if (inputManager.DecisionButton == 1)
				{
					battleMenu.ButtonEvent (0, igameManager);
				}
				
				if (inputManager.CancelButton == 1)
				{
					battleMenu.ButtonEvent (1, igameManager);
				}

				battleMenu = battleMenu.NextMenu ();
			}
		}

		public void UpdateUI (PlayerController player)
		{
			if (player.enabled)
				MoveMaker (player.transform.position, player.transform.eulerAngles.y);
			SetHpGauge (player.GetNowHp ());
		}

		public void SetMapSize (bool size)
		{
			if (size)
			{
				battleMapCanvas.transform.SetAsLastSibling ();
				battleMapCanvas.sortingOrder = 1;

				Image miniMap = battleMapCanvas.transform.GetChild (0).gameObject.GetComponent<Image> ();
				miniMap.rectTransform.position = new Vector3 (BMAP_MIN_X, BMAP_MIN_Z);
				miniMap.rectTransform.sizeDelta = bigMapSize;

				for (int i=1; i<battleMapCanvas.transform.childCount; i++)
				{
					Image image = battleMapCanvas.transform.GetChild (i).GetComponent<Image> ();
					image.rectTransform.position = new Vector3 ((image.rectTransform.position.x - MAP_MIN_X)/mapRatio.x + BMAP_MIN_X,
					                                            (image.rectTransform.position.y - MAP_MIN_Z)/mapRatio.y + BMAP_MIN_Z,
					                                            0);
				}
			}
			else
			{
				Image miniMap = battleMapCanvas.transform.GetChild (0).gameObject.GetComponent<Image> ();
				miniMap.rectTransform.position = new Vector3 (MAP_MIN_X, MAP_MIN_Z);
				miniMap.rectTransform.sizeDelta = mapSize;

				for (int i=1; i<battleMapCanvas.transform.childCount; i++)
				{
					Image image = battleMapCanvas.transform.GetChild (i).GetComponent<Image> ();
					image.rectTransform.position = new Vector3 ((image.rectTransform.position.x - BMAP_MIN_X)*mapRatio.x + MAP_MIN_X,
					                                            (image.rectTransform.position.y - BMAP_MIN_Z)*mapRatio.y + MAP_MIN_Z,
					                                            0);
				}
			}
		}

		private void MoveMaker (Vector3 position, float angle)
		{
			player_maker.rectTransform.position = new Vector3 ((position.x - BattleUI.FIELD_MIN_X)* (BattleUI.mapSize.x/BattleUI.fieldSize.x) + BattleUI.MAP_MIN_X,
				                                              ((position.z - BattleUI.FIELD_MIN_Z)*-1)* (BattleUI.mapSize.y/BattleUI.fieldSize.y) + BattleUI.MAP_MAX_Z,
				                                               0);
			player_maker.rectTransform.eulerAngles = new Vector3 (0, 0, angle-90);
		}

		private void SetHpGauge (float amount)
		{
			hpGauge.fillAmount = amount;
		}

		public void DrawKillNumber (int number)
		{
			int temp = 100;
			int tempNumber = number;

			for (int i=killNumber.Length-1; i>=0; i--)
			{
				int temp2 = tempNumber/temp;
				killNumber[i].sprite = Resources.Load (fileName[temp2], typeof (Sprite)) as Sprite;
				if (temp2 == 0 && number/temp == 0)
					killNumber[i].color = new Color (1, 1, 1, 0);
				else
					killNumber[i].color = new Color (1, 1, 1, 1);

				tempNumber -= temp2 * temp;
				temp /= 10;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

namespace GraduationProject
{
	public class BattleMenu : IInputEvent
	{
		protected IInputEvent nextMenu;
		protected Canvas canvas;
		protected ButtonsInfo[] buttonsInfo;
		protected List<BaseButton> buttonList;

		private FilldCursor cursor;
		private BaseText conditions;
		private DataManager dataManager;

		public BattleMenu ()
		{
		}

		public void SetBattleMenuEnable (bool flag)
		{
			BattleUI.battleMapCanvas.gameObject.SetActive (flag);
			canvas.gameObject.SetActive (flag);

			if (flag) nextMenu = this;
		}

		void IInputEvent.UpdateAnimation ()
		{
			cursor.CursorAnimation ();
		}

		void IInputEvent.ButtonEvent (int input, IGameManager igameManager)
		{
			if (input == 0)
			{
				switch (cursor.select)
				{
				case	0:
					SetBattleMenuEnable (false);
					nextMenu = new StatusMenu (this);
					nextMenu.CreateCanvas (dataManager);
					break;
				case	1:
					SetBattleMenuEnable (false);
					nextMenu = new ImageMenu (this, "Other/manual_attack");
					nextMenu.CreateCanvas (dataManager);
					break;
				case	2:
					SetBattleMenuEnable (false);
					nextMenu = new ImageMenu (this, "Other/manual");
					nextMenu.CreateCanvas (dataManager);
					break;
				case	3:
					nextMenu = null;
					MonoBehaviour.Destroy (canvas.gameObject);
					igameManager.SetEnemyUnitsEnable (true);
					break;
				case	4:
					nextMenu = new WindowMenu (this, "return_title");
					nextMenu.CreateCanvas (dataManager);
					break;
				}
			}
			else if (input == 1)
			{
				nextMenu = null;
				MonoBehaviour.Destroy (canvas.gameObject);
				igameManager.SetEnemyUnitsEnable (true);
			}
		}

		void IInputEvent.MoveCursor (int input)
		{
			cursor.ChangeSelect (input);
			foreach (BaseButton buttons in buttonList)
				buttons.ButtonEvent (cursor.select);

			cursor.StartAnimation (0, 0);
			cursor.SetCursorPosition (new Vector3 (buttonsInfo[cursor.select].position.x*BattleUI.ratio_width,buttonsInfo[cursor.select].position.y*BattleUI.ratio_height,0f));
		}

		void IInputEvent.CreateCanvas (DataManager dataManager)
		{
			nextMenu = this;
			this.dataManager = dataManager;
			buttonList = new List<BaseButton> ();

			GameObject canvasObject = new GameObject ();
			canvas = canvasObject.AddComponent<Canvas> ();
			canvas.name = "MenuCanvas";
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;

			TextAsset jsonAsset = Resources.Load ("Data/BattleMainMenu", typeof(TextAsset)) as TextAsset;
			JsonData jsonData = JsonMapper.ToObject (jsonAsset.text);
						
			JsonData[] jsonArray = JsonMapper.ToObject<JsonData[]> (jsonData["labels"].ToJson ());
			foreach (JsonData array in jsonArray)
			{
				BaseLabel label = new BaseLabel ();
				label.Json = array;
				label.CreateMenuItem (canvas);
			}

			JsonData data = jsonData["nameLabels"];

			GameObject stageName = MonoBehaviour.Instantiate (canvas.transform.GetChild(1).gameObject) as GameObject;
			stageName.name = "stageName";
			stageName.transform.SetParent (canvas.transform);
			Image stageNameImage = stageName.GetComponent<Image> ();

			Vector2 bPos = new Vector2 ((float)((int)data["position_x"]), (float)((int)data["position_y"]));
			Vector2 bSize = new Vector2 ((float)((int)data["width"]), (float)((int)data["height"]));

			stageNameImage.sprite = Resources.Load ("UserInterface/"+dataManager.stageData[dataManager.freeModeData.stageSelectNumber].labelInfo.buttonImageFile[0],
			                                        typeof (Sprite)) as Sprite;
			stageNameImage.rectTransform.position = new Vector3 (bPos.x*BattleUI.ratio_width, bPos.y*BattleUI.ratio_height, 0f);
			stageNameImage.rectTransform.sizeDelta = new Vector2 (bSize.x*BattleUI.ratio_width, bSize.y*BattleUI.ratio_height);

			cursor = new FilldCursor ();
			cursor.Json = jsonData["cursor"];
			cursor.CreateMenuItem (canvas);

			data = jsonData["buttonInfo"];
			jsonArray = JsonMapper.ToObject<JsonData[]> (jsonData["buttons"].ToJson ());
			
			buttonsInfo = new ButtonsInfo[jsonArray.Length];
			cursor.max_number = buttonsInfo.Length;
			
			for (int i=0; i<buttonsInfo.Length; i++)
			{
				buttonsInfo[i].position = new Vector2 ((int)data["position_x"]+(int)data["distance_x"]*i, (int)data["position_y"]+(int)data["distance_y"]*i);
				buttonsInfo[i].size = new Vector2 ((int)data["width"], (int)data["height"]);
				buttonsInfo[i].alpha = JsonMapper.ToObject<float[]> (data["alpha"].ToJson ());
				buttonsInfo[i].pivot = JsonMapper.ToObject<float[]> (data["pivot"].ToJson ());
			}
			
			for (int i=0; i<jsonArray.Length; i++)
			{
				BaseButton button = new BaseButton ();
				button.Json = jsonArray[i];
				button.CreateMenuItem (canvas, buttonsInfo[i]);
				buttonList.Add (button);
			}

			foreach (BaseButton buttons in buttonList)
				buttons.ButtonEvent (cursor.select);

			conditions = new BaseText ();
			conditions.Json = jsonData["text"];
			conditions.CreateText (canvas);
			conditions.ChangeText (dataManager.stageData[dataManager.freeModeData.stageSelectNumber].Conditions);
		}

		IInputEvent IInputEvent.NextMenu ()
		{
			return nextMenu;
		}
	}
}


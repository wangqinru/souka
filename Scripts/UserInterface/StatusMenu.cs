//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.34209
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

namespace GraduationProject
{
	public class StatusMenu : IInputEvent
	{
		protected IInputEvent nextMenu;
		protected Canvas canvas;
		protected ButtonsInfo[] buttonsInfo;
		protected BattleMenu rootMenu;

		public StatusMenu (BattleMenu root)
		{
			rootMenu = root;
		}

		void IInputEvent.UpdateAnimation ()
		{
		}
		
		void IInputEvent.ButtonEvent (int input, IGameManager igameManager)
		{
			if (input == 0)
			{
			}
			else if (input == 1)
			{
				nextMenu = rootMenu;
				rootMenu.SetBattleMenuEnable (true);
				MonoBehaviour.Destroy (canvas.gameObject);
			}
		}
		
		void IInputEvent.MoveCursor (int input)
		{
		}
		
		void IInputEvent.CreateCanvas (DataManager dataManager)
		{
			nextMenu = this;
			
			GameObject canvasObject = new GameObject ();
			canvas = canvasObject.AddComponent<Canvas> ();
			canvas.name = "StatusCanvas";
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			
			TextAsset jsonAsset = Resources.Load ("Data/StatusMenu", typeof(TextAsset)) as TextAsset;
			JsonData jsonData = JsonMapper.ToObject (jsonAsset.text);
			
			JsonData[] jsonArray = JsonMapper.ToObject<JsonData[]> (jsonData["labels"].ToJson ());
			foreach (JsonData array in jsonArray)
			{
				BaseLabel label = new BaseLabel ();
				label.Json = array;
				label.CreateMenuItem (canvas);
			}
			
			JsonData data = jsonData["nameImage"];
			
			GameObject imageObject = MonoBehaviour.Instantiate (canvas.transform.GetChild(0).gameObject) as GameObject;
			imageObject.name = "CharaName";
			imageObject.transform.SetParent (canvas.transform);
			Image nameImage = imageObject.GetComponent<Image> ();

			Vector2 bPos = new Vector2 ((float)((int)data["position_x"]), (float)((int)data["position_y"]));
			Vector2 bSize = new Vector2 ((float)((int)data["width"]), (float)((int)data["height"]));
			
			nameImage.sprite = Resources.Load ("UserInterface/"+dataManager.characterData[dataManager.freeModeData.characterSelectNumber].labelInfo.buttonImageFile[0],
			                                        typeof (Sprite)) as Sprite;
			nameImage.rectTransform.position = new Vector3 (bPos.x*BattleUI.ratio_width, bPos.y*BattleUI.ratio_height, 0f);
			nameImage.rectTransform.sizeDelta = new Vector2 (bSize.x*BattleUI.ratio_width, bSize.y*BattleUI.ratio_height);
			
			data = jsonData["playerImage"];
			imageObject = MonoBehaviour.Instantiate (canvas.transform.GetChild(0).gameObject) as GameObject;
			imageObject.name = "CharaImage";
			imageObject.transform.SetParent (canvas.transform);
			nameImage = imageObject.GetComponent<Image> ();
			
			bPos = new Vector2 ((float)((int)data["position_x"]), (float)((int)data["position_y"]));
			bSize = new Vector2 ((float)((int)data["width"]), (float)((int)data["height"]));
			
			nameImage.sprite = Resources.Load ("UserInterface/"+dataManager.characterData[dataManager.freeModeData.characterSelectNumber].labelInfo.stageImageFile,
			                                   typeof (Sprite)) as Sprite;
			nameImage.rectTransform.position = new Vector3 (bPos.x*BattleUI.ratio_width, bPos.y*BattleUI.ratio_height, 0f);
			nameImage.rectTransform.sizeDelta = new Vector2 (bSize.x*BattleUI.ratio_width, bSize.y*BattleUI.ratio_height);

			data = jsonData["gaugeInfo"];
			float[] startPos = JsonMapper.ToObject<float[]> (data["position"].ToJson ());
			float[] size = JsonMapper.ToObject<float[]> (data["size"].ToJson ());
			float[] distance = JsonMapper.ToObject<float[]> (data["distance"].ToJson ());

			CharacterStatus statusData = dataManager.characterData[dataManager.freeModeData.characterSelectNumber].status;

			float[] status = new float[6]
			{
				statusData.hp,
				statusData.skill,
				statusData.attack,
				statusData.defense,
				statusData.agile,
				statusData.experience
			};

			for (int i=0; i<6; i++)
			{
				imageObject = MonoBehaviour.Instantiate (canvas.transform.GetChild(0).gameObject) as GameObject;
				imageObject.name = "Gauge00"+i.ToString ();
				imageObject.transform.SetParent (canvas.transform);
				nameImage = imageObject.GetComponent<Image> ();

				nameImage.sprite = Resources.Load ("UserInterface/status_gauge",
				                                   typeof (Sprite)) as Sprite;
				nameImage.rectTransform.position = new Vector3 ((startPos[0]+distance[0]*i)*BattleUI.ratio_width, (startPos[1]+distance[1]*i)*BattleUI.ratio_height, 0f);
				nameImage.rectTransform.sizeDelta = new Vector2 (size[0]*BattleUI.ratio_width, size[1]*BattleUI.ratio_height);

				nameImage.type = Image.Type.Filled;
				nameImage.fillMethod = Image.FillMethod.Horizontal;
				nameImage.fillAmount = status[i]/999f;
			}

			BaseText level = new BaseText ();
			level.Json = jsonData["text"];
			level.CreateText (canvas);
			level.ChangeText (dataManager.characterData[dataManager.freeModeData.characterSelectNumber].status.level.ToString ());
		}
		
		IInputEvent IInputEvent.NextMenu ()
		{
			return nextMenu;
		}
	}
}


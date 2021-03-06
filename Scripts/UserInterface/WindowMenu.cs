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
	public class WindowMenu : ImageMenu, IInputEvent
	{
		public WindowMenu (BattleMenu root, string file) : base (root, file)
		{
		}

		void IInputEvent.UpdateAnimation ()
		{
		}
		
		void IInputEvent.ButtonEvent (int input, IGameManager igameManager)
		{
			if (input == 0)
			{
				Resources.UnloadUnusedAssets ();
				Application.LoadLevel ("TitleScene");
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
			canvas.name = "ImageCanvas";
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			
			TextAsset jsonAsset = Resources.Load ("Data/ImageMenu", typeof(TextAsset)) as TextAsset;
			JsonData jsonData = JsonMapper.ToObject (jsonAsset.text);
			
			JsonData[] jsonArray = JsonMapper.ToObject<JsonData[]> (jsonData["labels"].ToJson ());

			BaseLabel label = new BaseLabel ();
			label.Json = jsonArray[0];
			label.CreateMenuItem (canvas);

			canvas.transform.GetChild(0).GetComponent<Image> ().sprite = Resources.Load ("UserInterface/fade", typeof (Sprite)) as Sprite;

			GameObject imageObject = MonoBehaviour.Instantiate (canvas.transform.GetChild(0).gameObject) as GameObject;
			imageObject.name = "Image";
			imageObject.transform.SetParent (canvas.transform);
			Image nameImage = imageObject.GetComponent<Image> ();
			
			nameImage.sprite = Resources.Load ("UserInterface/"+imageFile,
			                                   typeof (Sprite)) as Sprite;

			nameImage.rectTransform.position = new Vector3 (658f*BattleUI.ratio_width, 390f*BattleUI.ratio_height);
			nameImage.rectTransform.sizeDelta = new Vector3 (600f*BattleUI.ratio_width, 300f*BattleUI.ratio_height);

			canvas.transform.GetChild(0).GetComponent<Image> ().color = new Color (1, 1, 1, 0.5f);
		}
		
		IInputEvent IInputEvent.NextMenu ()
		{
			return nextMenu;
		}
	}
}


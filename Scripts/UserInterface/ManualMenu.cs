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
	public class ManualMenu : IInputEvent
	{
		protected IInputEvent nextMenu;
		protected Canvas canvas;
		private Image mauanlImage;
		private string[] filePath;
		private int nowNumber;

		public ManualMenu (string[] file)
		{
			filePath = file;
			nowNumber = 0;
		}

		void IInputEvent.UpdateAnimation ()
		{
		}
		
		void IInputEvent.ButtonEvent (int input, IGameManager igameManager)
		{
			if (input == 0)
			{
				nowNumber++;
			}
			else if (input == 1)
			{
				nowNumber --;
				if (nowNumber < 0) nowNumber = 0;
			}

			if (nowNumber < filePath.Length)
			{
				mauanlImage.sprite = Resources.Load ("UserInterface/"+filePath[nowNumber], typeof (Sprite)) as Sprite;
			}
			else
			{
				igameManager.SetEnemyUnitsActive ();
				nextMenu = null;
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
			foreach (JsonData array in jsonArray)
			{
				BaseLabel label = new BaseLabel ();
				label.Json = array;
				label.CreateMenuItem (canvas);
			}

			mauanlImage = canvas.transform.GetChild(0).GetComponent<Image> ();
			mauanlImage.sprite = Resources.Load ("UserInterface/"+filePath[nowNumber], typeof (Sprite)) as Sprite;
		}
		
		IInputEvent IInputEvent.NextMenu ()
		{
			return nextMenu;
		}
	}
}


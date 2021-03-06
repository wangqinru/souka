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
	public class GalleryMenu : BaseMenu
	{
		private BaseText remarkText;
		private ImageList imageList;
		private AnimationCursor cursor_a;

		private Image[] previewImage;
		private bool preview = false;

		public GalleryMenu (Canvas canv, SoundManager soundMan) : base (canv, soundMan)
		{
			productionFile = "Data/GalleryProduction";
		}

		public override List<BaseProduction> GetProductionList ()
		{
			TextAsset jsonAsset = Resources.Load (productionFile, typeof(TextAsset)) as TextAsset;
			JsonData[] jsonArray = JsonMapper.ToObject<JsonData[]> (jsonAsset.text);
			
			List<BaseProduction> list = new List<BaseProduction> ();
			
			foreach (JsonData data in jsonArray)
			{
				Type type = Type.GetType ("GraduationProject." + (string)data["ProductionType"]);
				if (type != null)
				{
					BaseProduction production = (BaseProduction)Activator.CreateInstance (type);
					production.Json = data;
					list.Add (production);
				}
			}
			
			return list;
		}

		public override void CreateBackGround ()
		{
			counter = 30;
			
			TextAsset jsonAsset = Resources.Load ("Data/GalleryUIData", typeof(TextAsset)) as TextAsset;
			JsonData jsonData = JsonMapper.ToObject (jsonAsset.text);

			imageList = new ImageList ();
			imageList.Json = jsonData["imagelistInfo"];
			imageList.CreateImageList (canvas);
			
			cursor_a = new AnimationCursor ();
			cursor_a.Json = jsonData["cursor"];
			cursor_a.CreateMenuItem (canvas);
			labelList.Add (cursor_a);
			cursor_a.StartAnimation (new float[1] {1}, (float)counter);

			previewImage = new Image[2];
			for (int i=0; i<previewImage.Length; i++)
			{
				GameObject obj = new GameObject ();
				obj.name = "previewImage";
				obj.transform.SetParent (canvas.transform);

				previewImage[i] = obj.AddComponent<Image> ();
				previewImage[i].rectTransform.anchorMax = Vector2.zero;
				previewImage[i].rectTransform.anchorMin = Vector2.zero;
				previewImage[i].rectTransform.position = new Vector3 (Screen.width/2f, Screen.height/2f, 0f);
				previewImage[i].rectTransform.sizeDelta = new Vector2 (Screen.width, Screen.height);
				previewImage[i].color = new Color (1, 1, 1, 0);
			}

			previewImage[0].sprite = Resources.Load ("UserInterface/fade", typeof(Sprite)) as Sprite;

			JsonData[] jsonArray = JsonMapper.ToObject<JsonData[]> (jsonData["labels"].ToJson ());
			foreach (JsonData array in jsonArray)
			{
				AnimationLabel label = new AnimationLabel ();
				label.Json = array;
				labelList.Add (label);
				label.CreateMenuItem (canvas);
				label.StartAnimation (new float[1] {1}, (float)counter);
			}

			remarkText = new BaseText ();
			remarkText.Json = jsonData["text"];
			remarkText.CreateText (canvas);

			cursor_a.max_number = imageList.textData.Length;
			cursor_a.SetCursorPosition (imageList.ChangeSelectImage (0));
			remarkText.ChangeText (imageList.textData [0]);
		}

		public override void Execute (InputManager inputManager, IProductionObserver observer)
		{
			if (counter > 0)
			{
				counter --;
				
				foreach (AnimationLabel lab in labelList)
				{
					lab.ButtonAnimation ();
					if (counter == 0) lab.ClearAmount ();
				}
			}
			else
			{
				cursor_a.CursorAnimation ();
				if (isDelete)
				{
					Delete ();
					return;
				}
				
				if (inputManager.StickLR == 1)
				{
					cursor_a.ChangeSelect (1);
					ChangeEvent (-1);
				}
				else if (inputManager.StickLR == -1)
				{
					cursor_a.ChangeSelect (-1);
					ChangeEvent (1);
				}


				if (inputManager.DecisionButton == 1)
				{
					ButtonEvent ();
				}

				if (inputManager.CancelButton == 1)
				{
					bool rewind = preview;
					CancelEvent ();
					if (!rewind) observer.Rewind (6);
				}
				
			}
		}
		
		public override void ChangeEvent (int var)
		{
			cursor_a.SetCursorPosition (imageList.ChangeSelectImage (cursor_a.select));
			remarkText.ChangeText (imageList.textData [cursor_a.select]);
			if (preview)
				previewImage[1].sprite = imageList.GetCurrentImage (cursor_a.select);

			soundManager.PlaySE (1);
		}
		
		public override void ButtonEvent ()
		{
			if (!preview)
			{
				previewImage[0].color = new Color (1, 1, 1, 0.5f);
				previewImage[1].color = new Color (1, 1, 1, 1);
				previewImage[1].sprite = imageList.GetCurrentImage (cursor_a.select);
				preview = true;
			}
		}
		
		public override void CancelEvent ()
		{
			if (!preview)
			{
				nextMenu = new TitleMenu (canvas, soundManager);
				soundManager.PlaySE (2);
				soundManager.PlayBGM (0);

			}
			else
			{
				foreach (Image img in previewImage)
					img.color = new Color (1, 1, 1, 0f);

				preview = false;
			}

		}
	}
}


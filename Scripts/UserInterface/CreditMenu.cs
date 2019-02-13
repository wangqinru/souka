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
	public class CreditMenu : BaseMenu
	{
		public CreditMenu (Canvas canv, SoundManager soundMan) : base (canv, soundMan)
		{
			productionFile = "Data/CreditProduction";
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
			
			TextAsset jsonAsset = Resources.Load ("Data/CreditUIData", typeof(TextAsset)) as TextAsset;
			JsonData jsonData = JsonMapper.ToObject (jsonAsset.text);
	
			JsonData[] jsonArray = JsonMapper.ToObject<JsonData[]> (jsonData["labels"].ToJson ());
			foreach (JsonData array in jsonArray)
			{
				AnimationLabel label = new AnimationLabel ();
				label.Json = array;
				labelList.Add (label);
				label.CreateMenuItem (canvas);
				label.StartAnimation (new float[1] {1}, (float)counter);
			}
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
				if (inputManager.DecisionButton == 1)
				{
					ButtonEvent ();
				}
				
				if (inputManager.CancelButton == 1)
				{
					CancelEvent ();
				}
				
			}
		}
		
		public override void ChangeEvent (int var)
		{
		}
		
		public override void ButtonEvent ()
		{
		}
		
		public override void CancelEvent ()
		{
			Application.LoadLevel ("TitleScene");
		}
	}
}


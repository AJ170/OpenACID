using System.Collections.Generic;
using AssetBundleData;
using SharedBaseLib.Serialization;
using UnityEngine;

namespace Database
{
	[Database("ColorTableDefinition")]
	public class ColorTableDef : DBEntry
	{
		public List<GameID> Colors;

		public bool Linked;

		public ColorTableDef(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
			Colors = new List<GameID>();
		}

		public void GetRandomColors(GameDB _gameDb, out Color _color0, out Color _color1)
		{
			int num = Random.Range(0, Colors.Count);
			int index = ((!Linked) ? Random.Range(0, Colors.Count) : num);
			ColorDef item = _gameDb.GetItem<ColorDef>(Colors[num]);
			ColorDef item2 = _gameDb.GetItem<ColorDef>(Colors[index]);
			_color0 = item.Color0;
			_color1 = item2.Color1;
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			Linked = _s.SerializeBool("Linked", Linked);
			_s.SerializeBeginNode("Colors");
			int num = _s.SerializeItemCount(Colors.Count);
			if (_s.Mode == EMode.EMode_Read)
			{
				Colors = new List<GameID>(num);
			}
			for (int i = 0; i < num; i++)
			{
				_s.SerializeBeginNode("Color");
				if (_s.Mode == EMode.EMode_Read)
				{
					Colors.Add(SerializeGameID("GameID", _s, this));
				}
				else
				{
					SerializeGameID("GameID", _s, this, Colors[i]);
				}
				_s.SerializeEndNode();
			}
			_s.SerializeEndNode();
		}
	}
}

using UnityEngine;

namespace UbiCore.Utils.Extensions
{
	public static class LayerMaskExtensions
	{
		private static int BITLENGTH = 32;

		public static LayerMask Inverse(this LayerMask _mask)
		{
			return ~(int)_mask;
		}

		public static LayerMask Create(params string[] _names)
		{
			LayerMask layerMask = default(LayerMask);
			foreach (string layerName in _names)
			{
				layerMask = (int)layerMask | (1 << LayerMask.NameToLayer(layerName));
			}
			return layerMask;
		}

		public static LayerMask Create(string _name)
		{
			return (int)default(LayerMask) | (1 << LayerMask.NameToLayer(_name));
		}

		public static LayerMask AddLayers(this LayerMask _layer, params string[] _layers)
		{
			return (int)_layer | (int)Create(_layers);
		}

		public static LayerMask RemoveLayers(this LayerMask _layer, params string[] _layers)
		{
			return Inverse((int)_layer.Inverse() | (int)Create(_layers));
		}

		public static string[] ToNames(this LayerMask _mask)
		{
			string[] array = new string[BITLENGTH];
			int num = 0;
			for (int i = 0; i < BITLENGTH; i++)
			{
				int num2 = 1 << i;
				if (((int)_mask & num2) == num2)
				{
					string text = LayerMask.LayerToName(i);
					if (!string.IsNullOrEmpty(text))
					{
						array[num++] = text;
					}
				}
			}
			return array;
		}

		public static string ToString(this LayerMask _mask, string _delimiter = ", ")
		{
			return string.Join(_delimiter, _mask.ToNames());
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AcierProtocol;
using Database;

namespace Assets.Scripts.Mission.Utilities
{
	public static class Era
	{
		public static readonly EEra[] Eras = Enum.GetValues(typeof(EEra)).Cast<EEra>().ToArray();

		public static readonly List<EEra> SkippedEras = new List<EEra>(new EEra[2]
		{
			EEra.EEra_None,
			EEra.America
		});

		public static readonly EEra[] HumanoidEras = new EEra[2]
		{
			EEra.Italy,
			EEra.ACM
		};

		private static EEra[] s_ExportedEras;

		private static EEra s_ExportedErasFlagSet = EEra.EEra_None;

		private static EEra[] s_HumanoidBundleFlagSets;

		public static EEra[] ExportedEras
		{
			get
			{
				if (s_ExportedEras == null)
				{
					List<EEra> list = new List<EEra>();
					foreach (int value in Enum.GetValues(typeof(EEra)))
					{
						if (((EEra)value).IsExported())
						{
							list.Add((EEra)value);
						}
					}
					s_ExportedEras = list.ToArray();
				}
				return s_ExportedEras;
			}
		}

		public static EEra ExportedErasFlagSet
		{
			get
			{
				if (s_ExportedErasFlagSet == EEra.EEra_None)
				{
					foreach (int value in Enum.GetValues(typeof(EEra)))
					{
						if (!SkippedEras.Contains((EEra)value))
						{
							s_ExportedErasFlagSet |= (EEra)value;
						}
					}
				}
				return s_ExportedErasFlagSet;
			}
		}

		public static EEra[] HumanoidBundleFlagSets
		{
			get
			{
				if (s_HumanoidBundleFlagSets == null)
				{
					List<EEra> list = new List<EEra>();
					EEra[] exportedEras = ExportedEras;
					foreach (EEra item in exportedEras)
					{
						if (!HumanoidEras.Contains(item))
						{
							list.Add(item);
						}
					}
					int num = (int)Math.Pow(2.0, HumanoidEras.Length);
					for (int j = 1; j < num; j++)
					{
						EEra eEra = EEra.EEra_None;
						for (int k = 0; k < HumanoidEras.Length; k++)
						{
							if ((j & (1 << k)) != 0)
							{
								eEra |= HumanoidEras[k];
							}
						}
						if (eEra.IsExported())
						{
							list.Add(eEra);
						}
					}
					s_HumanoidBundleFlagSets = list.ToArray();
				}
				return s_HumanoidBundleFlagSets;
			}
		}

		public static EEra ShrinkToContext(this EEra _era)
		{
			EEra preloadedEra = HumanoidData.PreloadedEra;
			EEra eEra = _era.Fix() & (preloadedEra | EEra.Common);
			if (eEra != EEra.Common)
			{
				eEra &= (EEra)(-17);
			}
			return eEra;
		}

		public static IEnumerable<EEra> IterateEras(this EEra _eraFlags)
		{
			EEra era = EEra.Italy;
			for (int i = 0; i < Eras.Length - 1; i++)
			{
				if ((_eraFlags & era) == era)
				{
					yield return era;
				}
				era = (EEra)((int)era << 1);
			}
		}

		public static string ToListString(this EEra _eraFlags, string _separator = "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (EEra item in _eraFlags.IterateEras())
			{
				if (!flag)
				{
					stringBuilder.Append(_separator);
				}
				stringBuilder.Append(item);
				flag = false;
			}
			return stringBuilder.ToString();
		}

		public static string ToString(this EEra _eraFlags)
		{
			return _eraFlags.ToListString(string.Empty);
		}

		public static bool Intersects(this EEra _era, EEra _flagSet)
		{
			return (_era & _flagSet) != 0;
		}

		public static bool Contains(this EEra _era, EEra _flagSet)
		{
			return (_era & _flagSet) == _flagSet && (_era != 0 || _flagSet == EEra.EEra_None);
		}

		public static bool IsExported(this EEra _era)
		{
			return _era.Intersects(ExportedErasFlagSet);
		}
	}
}

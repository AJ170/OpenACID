using System;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Core;
using SharedBaseLib.Serialization;
using SharedBaseLib.System.Extensions;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class StatEnumMapValue_Float<TKey> : StatCollectionValue<StatValueWrapper_Float>, IDataSerializable, IReadonlyStatCollectionValue, IStatCollectionValue, IStatEnumMapValue, IReadonlyStatValueBase, IStatValueBase, IManagedStatValueBase, IAcierDataSerializable, IAcierDataTemplateSerializable, IAcierDataDeSerializable where TKey : struct
	{
		public Type KeyType
		{
			get
			{
				return typeof(TKey);
			}
		}

		public StatEnumMapValue_Float(StatTypes.EStatType _statType, StatValueCollection _statValueCollection)
			: base(_statType, _statValueCollection)
		{
		}

		public override void Serialize(string _name, DataSerializerBase _s, object _customData = null)
		{
			if (_s.Mode == EMode.EMode_Read)
			{
				m_Collection.Clear();
				BBSimpleList<float> list = XmlHelpers.SerializeEnumMap<TKey, float>("Value", _s);
				m_Collection.AddRange(list.Select((float floatValue) => new StatValueWrapper_Float
				{
					Float = floatValue
				}));
			}
			else
			{
				XmlHelpers.SerializeEnumMap<TKey, float>("Value", _s, m_Collection.Select((StatValueWrapper_Float wrapper) => wrapper.Float));
			}
		}

		public override IReadonlyStatValueBase Clone(StatValueCollection _newStatValueCollection)
		{
			StatEnumMapValue_Float<TKey> statEnumMapValue_Float = new StatEnumMapValue_Float<TKey>(StatType, _newStatValueCollection);
			statEnumMapValue_Float.CopyFrom(this);
			return statEnumMapValue_Float;
		}
	}
}

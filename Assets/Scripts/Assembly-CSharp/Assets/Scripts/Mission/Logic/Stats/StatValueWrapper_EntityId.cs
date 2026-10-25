using System;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class StatValueWrapper_EntityId : StatValueWrapperBase<StatValueWrapper_EntityId>
	{
		private EntityId m_EntityId = EntityId.InvalidId;

		public override EntityId EntityId
		{
			get
			{
				return m_EntityId;
			}
			set
			{
				if (!Assets.Scripts.Engine.Helpers.Verify(value != null))
				{
					value = EntityId.InvalidId;
				}
				m_EntityId = value;
			}
		}

		public override Type ValueType
		{
			get
			{
				return typeof(EntityId);
			}
		}

		public override bool Equals(object obj)
		{
			if (!base.Equals(obj))
			{
				return false;
			}
			return EntityId.Equals(Casted(obj).EntityId);
		}

		public override int GetHashCode()
		{
			return EntityId.GetHashCode();
		}

		public override IStatValueWrapper Clone()
		{
			StatValueWrapper_EntityId statValueWrapper_EntityId = new StatValueWrapper_EntityId();
			statValueWrapper_EntityId.EntityId = EntityId;
			return statValueWrapper_EntityId;
		}

		public override void CopyFrom(IStatValueWrapper _wrapper)
		{
			EntityId = _wrapper.EntityId;
		}

		public override void Serialize(string _name, DataSerializerBase _s, object _customData = null)
		{
			if (_s.Mode == EMode.EMode_Read)
			{
				EntityId = XmlHelpers.Serialize<EntityId>("value", _s);
			}
			else
			{
				XmlHelpers.Serialize("value", _s, EntityId);
			}
		}

		public override string ToString()
		{
			return EntityId.ToString();
		}
	}
}

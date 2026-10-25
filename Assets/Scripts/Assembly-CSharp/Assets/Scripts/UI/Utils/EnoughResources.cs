using System.Collections.Generic;
using AcierProtocol;
using SharedBaseLib.Core;

namespace Assets.Scripts.UI.Utils
{
	public class EnoughResources
	{
		private readonly BBDictionary<int, long> m_amount = new BBDictionary<int, long>();

		private readonly BBDictionary<int, long> m_price = new BBDictionary<int, long>();

		public void AddResourceAndSetCurrent(Resource _resource, long _currentAmount)
		{
			m_amount[(int)_resource.type] = _currentAmount;
			if (m_price.ContainsKey((int)_resource.type))
			{
				BBDictionary<int, long> price;
				BBDictionary<int, long> bBDictionary = (price = m_price);
				int type;
				int key = (type = (int)_resource.type);
				long num = price[type];
				bBDictionary[key] = num + _resource.amount;
			}
			else
			{
				m_price[(int)_resource.type] = _resource.amount;
			}
		}

		public bool Enough(EResourceType _type)
		{
			return Amount(_type) - Price(_type) >= 0;
		}

		public long Amount(EResourceType _type)
		{
			long value;
			m_amount.TryGetValue((int)_type, out value);
			return value;
		}

		public long Price(EResourceType _type)
		{
			long value;
			m_price.TryGetValue((int)_type, out value);
			return value;
		}

		public static implicit operator bool(EnoughResources _obj)
		{
			foreach (KeyValuePair<int, long> item in _obj.m_amount)
			{
				if (!_obj.Enough((EResourceType)item.Key))
				{
					return false;
				}
			}
			return true;
		}
	}
}

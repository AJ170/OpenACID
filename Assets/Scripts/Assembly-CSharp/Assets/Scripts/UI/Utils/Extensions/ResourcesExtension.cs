using System;
using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.State;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class ResourcesExtension
	{
		public static Resource GetResourceByType(this LootTemplate[] _lootTemplates, EResourceType _type)
		{
			Resource result = null;
			_lootTemplates.DoForResourceTemplates(delegate(LootTemplate.ResourceTemplate _template)
			{
				if (_template.minResource.type == _type)
				{
					result = _template.minResource;
				}
			});
			return result;
		}

		public static long GetResourceAmountByType(this LootTemplate[] _lootTemplates, EResourceType _type)
		{
			Resource resourceByType = _lootTemplates.GetResourceByType(_type);
			if (resourceByType != null)
			{
				return resourceByType.amount;
			}
			return 0L;
		}

		public static long GetTotalResourceAmount(this LootTemplate[] _lootTemplates)
		{
			long totalCount = 0L;
			_lootTemplates.DoForResourceTemplates(delegate(LootTemplate.ResourceTemplate _template)
			{
				totalCount += _template.minResource.amount;
			});
			return totalCount;
		}

		public static void FillResourceStringBuilder(this LootTemplate[] _lootTemplates, ref StringMapper _stringMapper, string _sufix)
		{
			for (EResourceType eResourceType = EResourceType.HardCurrency; eResourceType <= EResourceType.SocialCurrency; eResourceType++)
			{
				long resourceAmountByType = _lootTemplates.GetResourceAmountByType(eResourceType);
				_stringMapper.Add(string.Format("{0}_{1}", eResourceType, _sufix), resourceAmountByType);
			}
		}

		public static string GetResourceLocaName(this Resource _resource)
		{
			ResourceDescription value;
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.ResourceDescriptions.TryGetValue((int)_resource.type, out value))
			{
				return LocalizationManager.Instance.Get(value.InfoHeadingId);
			}
			return string.Empty;
		}

		public static Resource GetResourceByType(this Resources _resources, EResourceType _type)
		{
			if (_resources != null)
			{
				for (int i = 0; i < _resources.resources.Length; i++)
				{
					Resource resource = _resources.resources[i];
					if (resource.type == _type)
					{
						return resource;
					}
				}
			}
			return null;
		}

		public static long GetResourceAmountByType(this Resources _resources, EResourceType _type)
		{
			Resource resourceByType = _resources.GetResourceByType(_type);
			if (resourceByType != null)
			{
				return resourceByType.amount;
			}
			return 0L;
		}

		public static long GetResourceAmountByType(this ResourceFromTemplate[] _resources, EResourceType _type)
		{
			long num = 0L;
			if (_resources != null)
			{
				for (int i = 0; i < _resources.Length; i++)
				{
					Resource resource = _resources[i].resource;
					if (resource.type == _type)
					{
						num += resource.amount;
					}
				}
			}
			return num;
		}

		public static long GetTotalResourceAmount(this Resources _resources)
		{
			long num = 0L;
			for (int i = 0; i < _resources.resources.Length; i++)
			{
				num += _resources.resources[i].amount;
			}
			return num;
		}

		public static void FillResourceStringBuilder(this Resources _resources, ref StringMapper _stringMapper, string _sufix)
		{
			for (EResourceType eResourceType = EResourceType.HardCurrency; eResourceType <= EResourceType.SocialCurrency; eResourceType++)
			{
				long resourceAmountByType = _resources.GetResourceAmountByType(eResourceType);
				_stringMapper.Add(string.Format("{0}_{1}", eResourceType, _sufix), resourceAmountByType);
			}
		}

		public static Resource Find(this Resources _resources, EResourceType _type)
		{
			for (int i = 0; i < _resources.resources.Length; i++)
			{
				if (_resources.resources[i].type == _type)
				{
					return _resources.resources[i];
				}
			}
			return null;
		}

		public static Resources Join(this Resources _left, Resources _right)
		{
			Resources resources = new Resources();
			for (int i = 0; i < _left.resources.Length; i++)
			{
				Resource resource = _left.resources[i];
				resources.resources = resources.resources.Add(new Resource
				{
					type = resource.type,
					amount = resource.amount
				});
			}
			for (int j = 0; j < _right.resources.Length; j++)
			{
				Resource resource2 = _right.resources[j];
				Resource resource3 = resources.Find(resource2.type);
				if (resource3 == null)
				{
					Resource resource4 = new Resource();
					resource4.type = resource2.type;
					resource3 = resource4;
					resources.resources = resources.resources.Add(resource3);
				}
				resource3.amount += resource2.amount;
			}
			return resources;
		}

		public static Resources Intersect(this Resources _left, Resources _right)
		{
			Resources resources = new Resources();
			for (int i = 0; i < _left.resources.Length; i++)
			{
				Resource resource = _left.resources[i];
				Resource resource2 = _right.Find(resource.type);
				if (resource2 != null && resource2.amount <= resource.amount)
				{
					resources.resources = resources.resources.Add(new Resource
					{
						type = resource2.type,
						amount = resource2.amount
					});
				}
			}
			return resources;
		}

		public static void Subtract(this Resources _resources, Resources _costs, EResourceType _type)
		{
			Resource resource = _resources.Find(_type);
			Resource resource2 = _costs.Find(_type);
			resource.amount -= resource2.amount;
			if (resource.amount <= 0)
			{
				Resource[] resources = _resources.resources;
				resources = Array.FindAll(resources, (Resource x) => x != resource);
			}
		}

		public static Resources ComputeReviveOptions(this Resources _resources)
		{
			Resources reviveCosts = Singleton<StateManager>.Instance.TemplateManager.Globals.reviveCosts;
			return _resources.Intersect(reviveCosts);
		}
	}
}

using System;
using AssetBundleData;
using Database;
using SharedBaseLib.Serialization;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Definitions
{
	public static class Data
	{
		public interface IDefinition
		{
			int Id { get; }

			GameID GameID { get; }

			Type InstanceType { get; }

			string Name { get; }
		}

		public abstract class Definition : DBEntry, IDefinition
		{
			private readonly int m_Id;

			public int Id
			{
				get
				{
					return m_Id;
				}
			}

			public abstract Type InstanceType { get; }

			protected virtual Definition DefaultTemplateDefinition
			{
				get
				{
					return null;
				}
			}

			protected Definition TemplateDefinition
			{
				get
				{
					Definition defaultTemplateDefinition = DefaultTemplateDefinition;
					if (defaultTemplateDefinition == null)
					{
						return null;
					}
					return (!(base.GameID == defaultTemplateDefinition.GameID)) ? defaultTemplateDefinition : null;
				}
			}

			protected Definition(AssetBundleInfo _assetBundleInfo)
				: base(_assetBundleInfo)
			{
			}

			protected Definition(AssetBundleInfo _assetBundleInfo, bool _addToDefinitionManager)
				: base(_assetBundleInfo)
			{
				m_Id = CreateId(_addToDefinitionManager);
			}

			protected Definition(AssetBundleInfo _assetBundleInfo, bool _addToDefinitionManager, Definition _definition)
				: base(_assetBundleInfo, _definition)
			{
				m_Id = CreateId(_addToDefinitionManager);
			}

			public override void Serialize(DataSerializerBase _s)
			{
				base.Serialize(_s);
			}

			public override int CompareTo(DBEntry _other)
			{
				if (object.ReferenceEquals(this, _other))
				{
					return 0;
				}
				if (base.Name == "__DefaultTemplate")
				{
					return -1;
				}
				if (_other.Name == "__DefaultTemplate")
				{
					return 1;
				}
				return base.CompareTo(_other);
			}

			public virtual void InitializePass1()
			{
			}

			public virtual void InitializePass2()
			{
			}

			protected int CreateId(bool _add)
			{
				if (_add && UbiCore.Utils.Singleton<DefinitionManager>.IsAvailable)
				{
					return UbiCore.Utils.Singleton<DefinitionManager>.Instance.RegisterDefinition(this);
				}
				return -1;
			}

		}

		public const string DefaultTemplateTypeName = "__DefaultTemplate";
	}
}

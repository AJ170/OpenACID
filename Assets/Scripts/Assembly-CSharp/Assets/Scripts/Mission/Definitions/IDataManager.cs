using System.Collections.ObjectModel;

namespace Assets.Scripts.Mission.Definitions
{
	public interface IDataManager<TType, TDefinition> where TType : DataType<TType, TDefinition> where TDefinition : class, Data.IDefinition
	{
		ReadOnlyCollection<TType> Types { get; }

		TDefinition GetDefinition(TType _type);

		TDefinition GetDefinitionByName(string _name);

		bool IsValid(TType _type);
	}
}

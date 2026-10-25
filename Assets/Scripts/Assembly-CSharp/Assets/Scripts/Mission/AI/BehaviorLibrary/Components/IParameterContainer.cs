using System;
using System.Xml;

namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components
{
	public interface IParameterContainer : ICloneable
	{
		void Read(XmlReader _childrenReader);
	}
}

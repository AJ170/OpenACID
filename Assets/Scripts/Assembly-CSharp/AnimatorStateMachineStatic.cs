using System;
using System.Collections.Generic;
using System.IO;
using AcierNetwork.AssetBundles;
using AssetBundleData;
using SharedBaseLib.Core;
using SharedBaseLib.Serialization;
using UbiCore.Debug;
using UbiCore.Utils;
using UnityEngine;
using UnityEngine.Profiling;

public class AnimatorStateMachineStatic
{
	public class HashValue
	{
		private string m_Name;

		public string Name
		{
			get
			{
				return m_Name;
			}
			private set
			{
				m_Name = value;
			}
		}

		public int NameHash { get; private set; }

		public bool IsInvalid
		{
			get
			{
				return string.IsNullOrEmpty(m_Name);
			}
		}

		public HashValue()
		{
		}

		public HashValue(string _name)
		{
			m_Name = _name;
			NameHash = Animator.StringToHash(m_Name);
		}
	}

	public class StateHashValue : HashValue
	{
		public StateHashValue()
		{
		}

		public StateHashValue(string _name)
			: base(_name)
		{
		}
	}

	public class StateMachineHashValue : HashValue
	{
		public StateMachineHashValue()
		{
		}

		public StateMachineHashValue(string _name)
			: base(_name)
		{
		}
	}

	public class Parameter
	{
		public enum EType
		{
			Bool = 0,
			Float = 1,
			Integer = 2,
			Trigger = 3
		}

		public string Name { get; set; }

		public int NameHash { get; set; }

		public EType Type { get; set; }

		public bool ValueBool { get; set; }

		public float ValueFloat { get; set; }

		public int ValueInteger { get; set; }

		internal Parameter(DataSerializerBase _s, bool _includeName)
		{
			Serialize(_s, _includeName);
		}

		internal void Serialize(DataSerializerBase _s, bool _includeName)
		{
			_s.SerializeBeginNode("Parameter");
			Name = ((!_includeName) ? string.Empty : _s.SerializeString("n", Name));
			NameHash = _s.SerializeInt("nh", NameHash);
			Type = (EType)_s.SerializeEnum("t", typeof(EType), (int)Type);
			switch (Type)
			{
			case EType.Bool:
				ValueBool = _s.SerializeBool("default", ValueBool);
				break;
			case EType.Float:
				ValueFloat = _s.SerializeFloat("default", ValueFloat);
				break;
			case EType.Integer:
				ValueInteger = _s.SerializeInt("default", ValueInteger);
				break;
			case EType.Trigger:
				ValueBool = _s.SerializeBool("default", ValueBool);
				break;
			}
			_s.SerializeEndNode();
		}
	}

	public class Transition
	{
		public bool Atomic { get; private set; }

		public string Name { get; private set; }

		public int NameHash { get; private set; }

		public State SourceState { get; set; }

		public State TargetState { get; set; }

		public int TargetStateNameHash { get; private set; }

		public Transition(State _sourceState, string _name, int _nameHash, bool _atomic, int _targetHash)
		{
			SourceState = _sourceState;
			Atomic = _atomic;
			Name = _name;
			NameHash = _nameHash;
			TargetStateNameHash = _targetHash;
		}

		public Transition(State _sourceState, DataSerializerBase _s, bool _includeName)
		{
			SourceState = _sourceState;
			Serialize(_s, _includeName);
		}

		internal void Serialize(DataSerializerBase _s, bool _includeName)
		{
			_s.SerializeBeginNode("Transition");
			Name = ((!_includeName) ? string.Empty : _s.SerializeString("n", Name));
			NameHash = _s.SerializeInt("nh", NameHash);
			Atomic = _s.SerializeBool("atomic", Atomic);
			TargetStateNameHash = _s.SerializeInt("target", TargetStateNameHash);
			_s.SerializeEndNode();
		}
	}

	public class State
	{
		private AnimatorStateMachineStatic m_StateMachine;

		private BBList<Transition> m_Transitions;

		public string Name { get; private set; }

		public int NameHash { get; private set; }

		public StateMachine ParentStateMachine { get; private set; }

		public BBList<Transition> Transitions
		{
			get
			{
				return m_Transitions;
			}
		}

		public State(AnimatorStateMachineStatic _stateMachine, string _name, int _nameHash, StateMachine _parent)
		{
			m_StateMachine = _stateMachine;
			Name = _name;
			NameHash = _nameHash;
			ParentStateMachine = _parent;
			m_Transitions = new BBList<Transition>();
		}

		internal State(AnimatorStateMachineStatic _stateMachine, StateMachine _parent, DataSerializerBase _s, bool _includeName)
		{
			m_StateMachine = _stateMachine;
			ParentStateMachine = _parent;
			Serialize(_s, _includeName);
		}

		public bool CompareNameHash(HashValue _hash)
		{
			return NameHash == _hash.NameHash;
		}

		internal void Serialize(DataSerializerBase _s, bool _includeName)
		{
			_s.SerializeBeginNode("State");
			Name = ((!_includeName) ? string.Empty : _s.SerializeString("n", Name));
			NameHash = _s.SerializeInt("nh", NameHash);
			int num = 0;
			if (_s.Mode == EMode.EMode_Read)
			{
				num = _s.SerializeItemCount();
				m_Transitions = new BBList<Transition>(num);
			}
			else
			{
				num = m_Transitions.Count;
				_s.SerializeItemCount(num);
			}
			for (int i = 0; i < num; i++)
			{
				if (_s.Mode == EMode.EMode_Read)
				{
					m_Transitions.Add(new Transition(this, _s, _includeName));
				}
				else
				{
					m_Transitions[i].Serialize(_s, _includeName);
				}
			}
			_s.SerializeEndNode();
		}

		public bool HasTransition(int _nameHash, int _destHash)
		{
			for (int i = 0; i < m_Transitions.Count; i++)
			{
				Transition transition = m_Transitions[i];
				if (transition.NameHash == _nameHash && transition.TargetStateNameHash == _destHash)
				{
					return true;
				}
			}
			return false;
		}
	}

	public class StateMachine
	{
		private readonly AnimatorStateMachineStatic m_StateMachine;

		private BBList<StateMachine> m_StateMachines;

		private BBList<State> m_States;

		public int DefaultStateHash { get; set; }

		public string Name { get; private set; }

		public int NameHash { get; private set; }

		public StateMachine ParentStateMachine { get; private set; }

		public BBList<StateMachine> StateMachines
		{
			get
			{
				return m_StateMachines;
			}
		}

		public BBList<State> States
		{
			get
			{
				return m_States;
			}
		}

		public StateMachine(AnimatorStateMachineStatic _stateMachine, string _name, int _nameHash, StateMachine _parent, int _defaultStateHash)
		{
			m_StateMachine = _stateMachine;
			Name = _name;
			NameHash = _nameHash;
			ParentStateMachine = _parent;
			DefaultStateHash = _defaultStateHash;
			m_StateMachines = new BBList<StateMachine>();
			m_States = new BBList<State>();
		}

		internal StateMachine(AnimatorStateMachineStatic _stateMachine, StateMachine _parent, DataSerializerBase _s, bool _includeName)
		{
			m_StateMachine = _stateMachine;
			ParentStateMachine = _parent;
			Serialize(_s, _includeName);
		}

		internal void Serialize(DataSerializerBase _s, bool _includeName)
		{
			_s.SerializeBeginNode("StateMachine");
			Name = ((!_includeName) ? string.Empty : _s.SerializeString("n", Name));
			NameHash = _s.SerializeInt("nh", NameHash);
			DefaultStateHash = _s.SerializeInt("defaultHash", DefaultStateHash);
			_s.SerializeBeginNode("StateMachines");
			int num = 0;
			if (_s.Mode == EMode.EMode_Read)
			{
				num = _s.SerializeItemCount();
				m_StateMachines = new BBList<StateMachine>(num);
			}
			else
			{
				num = m_StateMachines.Count;
				_s.SerializeItemCount(num);
			}
			for (int i = 0; i < num; i++)
			{
				if (_s.Mode == EMode.EMode_Read)
				{
					m_StateMachines.Add(new StateMachine(m_StateMachine, this, _s, _includeName));
				}
				else
				{
					m_StateMachines[i].Serialize(_s, _includeName);
				}
			}
			_s.SerializeEndNode();
			_s.SerializeBeginNode("State");
			int num2 = 0;
			if (_s.Mode == EMode.EMode_Read)
			{
				num2 = _s.SerializeItemCount();
				m_States = new BBList<State>(num2);
			}
			else
			{
				num2 = m_States.Count;
				_s.SerializeItemCount(num2);
			}
			for (int j = 0; j < num2; j++)
			{
				if (_s.Mode == EMode.EMode_Read)
				{
					m_States.Add(new State(m_StateMachine, this, _s, _includeName));
				}
				else
				{
					m_States[j].Serialize(_s, _includeName);
				}
			}
			_s.SerializeEndNode();
			_s.SerializeEndNode();
		}

		public bool HasChild(StateMachine _other)
		{
			bool flag = false;
			for (int i = 0; i < StateMachines.Count; i++)
			{
				if (StateMachines[i] == _other)
				{
					flag = true;
					break;
				}
				flag = StateMachines[i].HasChild(_other);
				if (flag)
				{
					break;
				}
			}
			return flag;
		}

		public bool Exists(HashValue _hash)
		{
			return m_StateMachine.StateMachines.ContainsKey(_hash.NameHash);
		}

		public bool CompareNameHash(HashValue _hash)
		{
			bool result = false;
			if (NameHash == _hash.NameHash)
			{
				result = true;
			}
			return result;
		}

		public bool CompareNameHashNoSanityCheck(HashValue _hash)
		{
			return NameHash == _hash.NameHash;
		}
	}

	public class Layer
	{
		private readonly AnimatorStateMachineStatic m_owner;

		public StateMachine StateMachine { get; set; }

		internal Layer(AnimatorStateMachineStatic _owner, DataSerializerBase _s, bool _includeName)
		{
			m_owner = _owner;
			Serialize(_s, _includeName);
		}

		internal void Serialize(DataSerializerBase _s, bool _includeName)
		{
			_s.SerializeBeginNode("Layer");
			if (_s.Mode == EMode.EMode_Read)
			{
				StateMachine = new StateMachine(m_owner, null, _s, _includeName);
			}
			else
			{
				StateMachine.Serialize(_s, _includeName);
			}
			_s.SerializeEndNode();
		}
	}

	public class StateMachineRoute : IEquatable<StateMachineRoute>
	{
		public State Start { get; set; }

		public State End { get; set; }

		public StateMachineRoute()
		{
			Start = null;
			End = null;
		}

		public StateMachineRoute(State _start, State _end)
		{
			Start = _start;
			End = _end;
		}

		public bool Equals(StateMachineRoute obj)
		{
			return obj != null && obj.Start == Start && obj.End == End;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as StateMachineRoute);
		}

		public override int GetHashCode()
		{
			if (Start == null)
			{
				return End.NameHash;
			}
			return Start.NameHash ^ End.NameHash;
		}
	}

	private const string kParameter = "Parameter";

	private const string kName = "n";

	private const string kNameHash = "nh";

	private const string kParamType = "t";

	private const string kParamDefault = "default";

	private const string kTransition = "Transition";

	private const string kTransitionAtomic = "atomic";

	private const string kTransitionTarget = "target";

	private const string kState = "State";

	private const string kStates = "State";

	private const string kStateMachine = "StateMachine";

	private const string kStateMachines = "StateMachines";

	private const string kStateDefaultHash = "defaultHash";

	private const string kRoot = "Root";

	private const string kParameters = "Parameters";

	private const string kLayer = "Layer";

	private const string kLayers = "Layers";

	private readonly BBList<Layer> m_Layer = new BBList<Layer>(3);

	private BBDictionary<int, StateMachine> m_StateMachines = new BBDictionary<int, StateMachine>();

	private BBDictionary<int, State> m_States = new BBDictionary<int, State>();

	private BBDictionary<int, Transition> m_Transitions = new BBDictionary<int, Transition>();

	private BBDictionary<StateMachineRoute, BBList<StateMachine>> m_StateMachineRoutes = new BBDictionary<StateMachineRoute, BBList<StateMachine>>();

	private BBDictionary<int, Parameter> m_DefaultParameters = new BBDictionary<int, Parameter>();

	public string ControllerName { get; private set; }

	public BBDictionary<int, StateMachine> StateMachines
	{
		get
		{
			return m_StateMachines;
		}
		private set
		{
			m_StateMachines = value;
		}
	}

	public BBDictionary<int, State> States
	{
		get
		{
			return m_States;
		}
		private set
		{
			m_States = value;
		}
	}

	public BBDictionary<int, Transition> Transitions
	{
		get
		{
			return m_Transitions;
		}
		private set
		{
			m_Transitions = value;
		}
	}

	public BBDictionary<StateMachineRoute, BBList<StateMachine>> StateMachineRoutes
	{
		get
		{
			return m_StateMachineRoutes;
		}
		private set
		{
			m_StateMachineRoutes = value;
		}
	}

	public BBDictionary<int, Parameter> DefaultParameters
	{
		get
		{
			return m_DefaultParameters;
		}
		private set
		{
			m_DefaultParameters = value;
		}
	}

	public AnimatorStateMachineStatic(string _name)
	{
		Profiling.Start("AnimatorStateMachineStatic::ctor", float.MaxValue);
		ControllerName = _name;
		string name = CreateFileName(_name, true, false);
		AssetBundleInfo assetBundleInfo = Main.Instance.GameDB.GetAssetBundleInfo(UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_animatorCourtesan);
		TextAsset textAsset = AssetBundleManager.Instance.LoadAsset<TextAsset>(assetBundleInfo, name);
		uint monoUsedSize = Profiler.GetMonoUsedSize();
		using (MemoryStream memoryStream = new MemoryStream(textAsset.bytes))
		{
			Resources.UnloadAsset(textAsset);
			memoryStream.Position = 0L;
			Profiling.Start(_name, float.MaxValue);
			using (DataBinarySerializer s = new DataBinarySerializer(memoryStream, EMode.EMode_Read))
			{
				Serialize(s, false);
			}
			Profiling.Stop();
		}
		Profiling.Stop();
		monoUsedSize = Profiler.GetMonoUsedSize() - monoUsedSize;
	}

	public string GetStateNameByNameHash(int _nameHash)
	{
		string result = string.Empty;
		State value;
		if (m_States.TryGetValue(_nameHash, out value))
		{
			result = value.Name;
		}
		return result;
	}

	public string GetTransitionNameByNameHash(int _nameHash)
	{
		string result = string.Empty;
		Transition value;
		if (m_Transitions.TryGetValue(_nameHash, out value))
		{
			result = value.Name;
		}
		return result;
	}

	public Transition GetTransition(int _nameHash)
	{
		Transition value;
		m_Transitions.TryGetValue(_nameHash, out value);
		return value;
	}

	public static string CreateFileName(string _name, bool _binary, bool _includeNames)
	{
		return string.Format("Assets/RuntimeAssets/Character/controller/{0}_statemachine{1}.{2}", _name, (!_includeNames) ? "_strip" : string.Empty, (!_binary) ? "xml" : "bytes");
	}

	public void Serialize(DataSerializerBase _s, bool _includeName)
	{
		bool flag = _s.Mode == EMode.EMode_Read;
		_s.SerializeBeginNode("Root");
		if (flag)
		{
			m_StateMachines = new BBDictionary<int, StateMachine>(_s.SerializeUInt16("smcount", 0));
			m_States = new BBDictionary<int, State>(_s.SerializeUInt16("stcount", 0));
			m_Transitions = new BBDictionary<int, Transition>(_s.SerializeUInt16("tcount", 0));
		}
		else
		{
			m_States.Clear();
			m_Transitions.Clear();
			m_StateMachines.Clear();
			for (int i = 0; i < m_Layer.Count; i++)
			{
				FillLookup(m_Layer[i].StateMachine);
			}
			_s.SerializeUInt16("smcount", (ushort)m_StateMachines.Count);
			_s.SerializeUInt16("stcount", (ushort)m_States.Count);
			_s.SerializeUInt16("tcount", (ushort)m_Transitions.Count);
		}
		Profiling.Start("DefaultParameters", float.MaxValue);
		_s.SerializeBeginNode("Parameters");
		int num = _s.SerializeItemCount(m_DefaultParameters.Values.Count);
		if (flag)
		{
			m_DefaultParameters = new BBDictionary<int, Parameter>(num);
		}
		if (flag)
		{
			for (int j = 0; j < num; j++)
			{
				Parameter parameter = new Parameter(_s, _includeName);
				m_DefaultParameters.Add(parameter.NameHash, parameter);
			}
		}
		else
		{
			foreach (Parameter value in m_DefaultParameters.Values)
			{
				value.Serialize(_s, _includeName);
			}
		}
		_s.SerializeEndNode();
		Profiling.Stop();
		_s.SerializeBeginNode("Layers");
		num = _s.SerializeItemCount(m_Layer.Count);
		for (int k = 0; k < num; k++)
		{
			if (flag)
			{
				Layer layer = new Layer(this, _s, _includeName);
				m_Layer.Add(layer);
				Profiling.Start("FillLookup", float.MaxValue);
				FillLookup(layer.StateMachine);
				Profiling.Stop();
			}
			else if (k < m_Layer.Count)
			{
				m_Layer[k].Serialize(_s, _includeName);
			}
		}
		_s.SerializeEndNode();
		Profiling.Start("Finalize", float.MaxValue);
		if (flag)
		{
			foreach (KeyValuePair<int, Transition> transition in m_Transitions)
			{
				transition.Value.TargetState = m_States[transition.Value.TargetStateNameHash];
			}
		}
		Profiling.Stop();
	}

	private void FillLookup(StateMachine _sm)
	{
		if (!m_StateMachines.ContainsKey(_sm.NameHash))
		{
			m_StateMachines.Add(_sm.NameHash, _sm);
		}
		for (int i = 0; i < _sm.StateMachines.Count; i++)
		{
			FillLookup(_sm.StateMachines[i]);
		}
		for (int j = 0; j < _sm.States.Count; j++)
		{
			State state = _sm.States[j];
			if (!m_States.ContainsKey(state.NameHash))
			{
				m_States.Add(state.NameHash, state);
			}
			for (int k = 0; k < state.Transitions.Count; k++)
			{
				Transition transition = state.Transitions[k];
				if (!m_Transitions.ContainsKey(transition.NameHash))
				{
					m_Transitions.Add(transition.NameHash, transition);
				}
			}
		}
	}
}

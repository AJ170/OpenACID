using System.Collections.Generic;

namespace Assets.Scripts.UI.Notifications
{
	public class UIAggregatedNewsEvaluator : UINewsEvaluatorBase
	{
		private readonly List<UINewsEvaluatorBase> m_evaluatorList = new List<UINewsEvaluatorBase>(8);

		public override ENewsType Type
		{
			get
			{
				return ENewsType.Invalid;
			}
		}

		public override string DisplayIcon
		{
			get
			{
				for (int i = 0; i < m_evaluatorList.Count; i++)
				{
					UINewsEvaluatorBase uINewsEvaluatorBase = m_evaluatorList[i];
					if (uINewsEvaluatorBase.Value)
					{
						return uINewsEvaluatorBase.DisplayIcon;
					}
				}
				return base.DisplayIcon;
			}
		}

		public override void Destroy()
		{
			base.Destroy();
			for (int i = 0; i < m_evaluatorList.Count; i++)
			{
				UINewsEvaluatorBase uINewsEvaluatorBase = m_evaluatorList[i];
				uINewsEvaluatorBase.OnValueChanged -= HandleValueChanged;
				uINewsEvaluatorBase.Destroy();
			}
			m_evaluatorList.Clear();
		}

		public override void Update()
		{
			for (int i = 0; i < m_evaluatorList.Count; i++)
			{
				m_evaluatorList[i].Update();
			}
		}

		public void Add(UINewsEvaluatorBase _evaluator)
		{
			_evaluator.Init();
			_evaluator.OnValueChanged += HandleValueChanged;
			m_evaluatorList.Add(_evaluator);
		}

		private void HandleValueChanged(bool _value)
		{
			bool flag = false;
			for (int i = 0; i < m_evaluatorList.Count; i++)
			{
				flag |= m_evaluatorList[i].Value;
			}
			base.Value = flag;
		}
	}
}

using UnityEngine;
using System;

namespace _Scripts.Frameworks.Modules.StateMachine.Core.Modular
{
	[CreateAssetMenu(fileName = "AnimationParameterAction", menuName = "State Machine/Actions/Animation Parameter Action")]
	public class AnimationParameterActionSO : StateActionSO
	{
		#region Fields
		[SerializeField] private ParameterType _parameterType;
		[SerializeField] private string _parameterName;
		
		public bool BoolValue;
		public int IntValue;
		public float FloatValue;
		public StateAction.SpecificMoment WhenToRun;
		public enum ParameterType
		{
			Bool, Int, Float, Trigger
		}
		#endregion
		public ParameterType GetParameterType => _parameterType;
		protected override StateAction CreateStateAction() => new AnimationParameterAction(Animator.StringToHash(_parameterName));
	}
	
	public class AnimationParameterAction : StateAction
	{
	    #region Fields
	    private Animator _playerAnimator;
	    private int _parameterHash;
	    /* Property (Origin SO Getter) */
		protected new AnimationParameterActionSO ActionOriginSO => (AnimationParameterActionSO)base.ActionOriginSO;
		
		#endregion
		
		
		
		#region Constructor
		public AnimationParameterAction(int parameterHash)
		{
			_parameterHash = parameterHash;
		}
		#endregion
	    #region Override
	    public override void Initialize(global::StateMachine stateMachine)
		{
			_playerAnimator = stateMachine.GetComponent<Animator>();
		}
		
		public override void OnStateEnter()
		{
			if (ActionOriginSO.WhenToRun == SpecificMoment.OnEnter)
			{
				SetAnimationParameter();
			}
		}
		
		public override void OnStateExit()
		{
			if (ActionOriginSO.WhenToRun == SpecificMoment.OnExit)
			{
				SetAnimationParameter();
			}
		}
		private void SetAnimationParameter()
		{
			switch (ActionOriginSO.GetParameterType)
			{
				case AnimationParameterActionSO.ParameterType.Bool:
					_playerAnimator.SetBool(_parameterHash, ActionOriginSO.BoolValue);
					break;
				case AnimationParameterActionSO.ParameterType.Int:
					_playerAnimator.SetInteger(_parameterHash, ActionOriginSO.IntValue);
					break;
				case AnimationParameterActionSO.ParameterType.Float:
					_playerAnimator.SetFloat(_parameterHash, ActionOriginSO.FloatValue);
					break;
				case AnimationParameterActionSO.ParameterType.Trigger:
					_playerAnimator.SetTrigger(_parameterHash);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		
		public override void OnStateUpdate() { }
		#endregion
	}
}
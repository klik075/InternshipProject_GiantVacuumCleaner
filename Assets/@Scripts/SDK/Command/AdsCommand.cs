
using System;
using Cysharp.Threading.Tasks;

public class AdsCommand : AbstractCommand
{
    #region Fields

    private readonly Action _invokeAction;
    private readonly CommandOrderer _orderer;

    #endregion


    
    #region Constructor

    public AdsCommand(Action onShowAction, CommandOrderer orderer)
    {
        _invokeAction = onShowAction;
        _orderer = orderer;
    }

    #endregion



    #region Override Execute Command

    public override void Execute()
    {
        base.Execute();
        _invokeAction?.Invoke();
        SDKIntegrationSystem.Instance.OnCommandInvoke(_orderer);
    }

    #endregion
}

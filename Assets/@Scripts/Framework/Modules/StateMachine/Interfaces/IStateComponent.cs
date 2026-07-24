
public interface IStateComponent
{
    /// <summary>
    /// 스테이트에 진입 했을 때
    /// </summary>
    void OnStateEnter();
    
    /// <summary>
    /// 스테이트에서 빠져 나왔을 때
    /// </summary>
    void OnStateExit();
}
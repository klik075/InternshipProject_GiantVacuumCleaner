
public interface IStateUpdater
{
    /// <summary>
    /// 스테이트 머신 업데이트
    /// </summary>
    void OnStateUpdate();
    
    /// <summary>
    /// 스테이트 머신 픽스드 업데이트
    /// </summary>
    void OnStateFixedUpdate();
}
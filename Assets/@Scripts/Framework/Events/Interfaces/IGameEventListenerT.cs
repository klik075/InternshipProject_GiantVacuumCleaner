namespace Scripts.Framework.Events.Interfaces
{
    /// <summary>
    /// 이벤트가 발생했을 때 호출되는 메서드 정의
    /// 즉, 모든 이벤트 리스너 (Listener)들은 해당 인터페이스를 구현함.
    /// </summary>
    public interface IGameEventListener<T>
    {
        void OnEventRaised(T item);
    }
}

public interface IObservable
{
    void SetChanged();
    void AddObserver(IObserver observer);
    void RemoveObserver(IObserver observer);
}
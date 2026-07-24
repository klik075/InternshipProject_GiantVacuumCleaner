namespace Scripts.Helper.TimeModules
{
    public interface ITimeHandler
    {
        bool IsSince(string checkKey, int value, bool isUpdate);
    }
}
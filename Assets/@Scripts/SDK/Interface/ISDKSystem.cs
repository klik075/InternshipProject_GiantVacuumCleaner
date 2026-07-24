
using Cysharp.Threading.Tasks;

public interface ISDKSystem
{
    public bool IsInitialize { get; set; }
    
    UniTask Initialize();
}
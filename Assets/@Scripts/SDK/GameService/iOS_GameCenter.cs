using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Scripts.SDK.GameService
{
    public class iOS_GameCenter
    {
        public async UniTaskVoid LoginGameCenter()
        {
            if (Social.localUser.authenticated)
            {
#if DEV
                Debugger.Log("Already GameCenter Login!");
#endif
                return;
            }
            Social.localUser.Authenticate((bool result) =>
            {
               
            });
        }
    }
}
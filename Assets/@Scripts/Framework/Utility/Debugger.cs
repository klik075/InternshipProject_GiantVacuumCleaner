
using System;
using UnityEngine;

/// <summary>
/// Scripting Define Symbol을 활용한 `Debugger`
///
/// ` Usable `
///     Edit -> Project Setting -> Player -> Other Settings -> Scripting Define Symbol setting
///     'DEV' 해당 심볼을 위 경로에 정의하면 됌.
/// </summary>
public static class Debugger
{
    public static void Log(string message)
    {
#if DEV
        Debug.Log(message);
#endif
    }

    public static void LogWarning(string message)
    {
#if DEV
        Debug.LogWarning(message);
#endif
    }

    public static void LogError(string message)
    {
#if DEV
        Debug.LogError(message);
#endif
    }
    
    public static void LogException(Exception exception)
    {
#if DEV
        Debug.LogException(exception);
#endif
    }
}
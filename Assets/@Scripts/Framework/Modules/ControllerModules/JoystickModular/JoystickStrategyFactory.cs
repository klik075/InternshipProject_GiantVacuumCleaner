

using System;

public static class JoystickStrategyFactory
{
    public static InputScreenJoystickStrategy CreateStrategyJoystick(
        InputScreenJoystick.JoystickHandlerType joystickHandlerType,
        InputScreenJoystick joystick)
    {
        return joystickHandlerType switch
        {
            InputScreenJoystick.JoystickHandlerType.OnlyHandle => new JoystickOnlyHandle(joystick),
            InputScreenJoystick.JoystickHandlerType.HandleWithBg => new JoystickHandleWithBg(joystick),
            InputScreenJoystick.JoystickHandlerType.DirectionWithBg => new JoystickDirectionAndBg(joystick),
            InputScreenJoystick.JoystickHandlerType.HandleBgDirection => new JoystickAll(joystick),
            _ => throw new ArgumentOutOfRangeException(nameof(joystickHandlerType), joystickHandlerType, null)
        };
    }
}

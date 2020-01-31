using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerControlActions : PlayerActionSet
{
    public PlayerAction MoveRight;
    public PlayerAction MoveLeft;
    public PlayerAction MoveForward;
    public PlayerAction MoveBackward;
    public PlayerAction LookRight;
    public PlayerAction LookLeft;
    public PlayerAction LookUp;
    public PlayerAction LookDown;
    public PlayerAction Shoot;
    public PlayerAction Reload;
    public PlayerAction Use;
    public PlayerAction ShotgunSwitch;
    public PlayerAction FlashlightSwitch;
    public PlayerAction CompassSwitch;
    public PlayerOneAxisAction MoveX;
    public PlayerOneAxisAction MoveY;
    public PlayerOneAxisAction LookX;
    public PlayerOneAxisAction LookY;

    public PlayerControlActions()
    {
        MoveRight = CreatePlayerAction("MoveRight");
        MoveLeft = CreatePlayerAction("MoveLeft");
        MoveForward = CreatePlayerAction("MoveForward");
        MoveBackward = CreatePlayerAction("MoveBackward");
        LookRight = CreatePlayerAction("LookRight");
        LookLeft = CreatePlayerAction("LookLeft");
        LookUp = CreatePlayerAction("LookUp");
        LookDown = CreatePlayerAction("LookDown");
        Shoot = CreatePlayerAction("Shoot");
        Reload = CreatePlayerAction("Reload");
        Use = CreatePlayerAction("Use");
        ShotgunSwitch = CreatePlayerAction("ShotgunSwitch");
        FlashlightSwitch = CreatePlayerAction("FlashlightSwitch");
        CompassSwitch = CreatePlayerAction("CompassSwitch");
        MoveX = CreateOneAxisPlayerAction(MoveLeft, MoveRight);
        MoveY = CreateOneAxisPlayerAction(MoveBackward, MoveForward);
        LookX = CreateOneAxisPlayerAction(LookLeft, LookRight);
        LookY = CreateOneAxisPlayerAction(LookDown, LookUp);
    }

    public static PlayerControlActions CreateWithDefaultBindings()
    {
        var playerControlActions = new PlayerControlActions();

        // Keyboard and mouse inputs
        playerControlActions.MoveRight.AddDefaultBinding(Key.D);
        playerControlActions.MoveLeft.AddDefaultBinding(Key.A);
        playerControlActions.MoveForward.AddDefaultBinding(Key.W);
        playerControlActions.MoveBackward.AddDefaultBinding(Key.S);
        playerControlActions.LookRight.AddDefaultBinding(Mouse.PositiveX);
        playerControlActions.LookLeft.AddDefaultBinding(Mouse.NegativeX);
        playerControlActions.LookUp.AddDefaultBinding(Mouse.PositiveY);
        playerControlActions.LookDown.AddDefaultBinding(Mouse.NegativeY);
        playerControlActions.Shoot.AddDefaultBinding(Mouse.LeftButton);
        playerControlActions.Reload.AddDefaultBinding(Key.R);
        playerControlActions.Use.AddDefaultBinding(Key.E);
        playerControlActions.ShotgunSwitch.AddDefaultBinding(Key.Key1);
        playerControlActions.FlashlightSwitch.AddDefaultBinding(Key.Key2);
        playerControlActions.CompassSwitch.AddDefaultBinding(Key.Key3);
        // Controller inputs
        playerControlActions.MoveRight.AddDefaultBinding(InputControlType.LeftStickRight);
        playerControlActions.MoveLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
        playerControlActions.MoveForward.AddDefaultBinding(InputControlType.LeftStickUp);
        playerControlActions.MoveBackward.AddDefaultBinding(InputControlType.LeftStickDown);
        playerControlActions.LookRight.AddDefaultBinding(InputControlType.RightStickRight);
        playerControlActions.LookLeft.AddDefaultBinding(InputControlType.RightStickLeft);
        playerControlActions.LookUp.AddDefaultBinding(InputControlType.RightStickUp);
        playerControlActions.LookDown.AddDefaultBinding(InputControlType.RightStickDown);
        playerControlActions.Shoot.AddDefaultBinding(InputControlType.RightTrigger);
        playerControlActions.Reload.AddDefaultBinding(InputControlType.Button3);
        playerControlActions.Use.AddDefaultBinding(InputControlType.Button1);
        playerControlActions.ShotgunSwitch.AddDefaultBinding(InputControlType.DPadUp);
        playerControlActions.FlashlightSwitch.AddDefaultBinding(InputControlType.DPadRight);
        playerControlActions.CompassSwitch.AddDefaultBinding(InputControlType.DPadDown);

        return playerControlActions;
    }
}

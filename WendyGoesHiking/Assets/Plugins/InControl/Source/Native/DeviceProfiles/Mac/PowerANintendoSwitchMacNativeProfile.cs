namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class PowerANintendoSwitchMacNativeProfile : NativeInputDeviceProfile
	{
		public PowerANintendoSwitchMacNativeProfile()
		{
			Name = "PowerA Nintendo Switch Controller";
			Meta = "PowerA Nintendo Switch Controller on Mac";
			// Link = "https://www.amazon.com/dp/B075DNGDWM";

			DeviceClass = InputDeviceClass.Controller;
			DeviceStyle = InputDeviceStyle.NintendoSwitch;

			// LowerDeadZone = 0.5f;

			IncludePlatforms = new[]
			{
				"OS X"
			};

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x20d6,
					ProductID = 0xa711,
					// VersionNumber = 0x200,
				},
			};

			ButtonMappings = new[]
			{
				new InputControlMapping
				{
					Handle = "Action3", Target = InputControlType.Action3, Source = Button( 0 ),
				},
				new InputControlMapping
				{
					Handle = "Action1", Target = InputControlType.Action1, Source = Button( 1 ),
				},
				new InputControlMapping
				{
					Handle = "Action2", Target = InputControlType.Action2, Source = Button( 2 ),
				},
				new InputControlMapping
				{
					Handle = "Action4", Target = InputControlType.Action4, Source = Button( 3 ),
				},
				new InputControlMapping
				{
					Handle = "Left Bumper", Target = InputControlType.LeftBumper, Source = Button( 4 ),
				},
				new InputControlMapping
				{
					Handle = "Right Bumper", Target = InputControlType.RightBumper, Source = Button( 5 ),
				},
				new InputControlMapping
				{
					Handle = "Left Trigger", Target = InputControlType.LeftTrigger, Source = Button( 6 ),
				},
				new InputControlMapping
				{
					Handle = "Right Trigger", Target = InputControlType.RightTrigger, Source = Button( 7 ),
				},
				new InputControlMapping
				{
					Handle = "Minus", Target = InputControlType.Minus, Source = Button( 8 ),
				},
				new InputControlMapping
				{
					Handle = "Plus", Target = InputControlType.Plus, Source = Button( 9 ),
				},
				new InputControlMapping
				{
					Handle = "Left Stick Button", Target = InputControlType.LeftStickButton, Source = Button( 10 ),
				},
				new InputControlMapping
				{
					Handle = "Right Stick Button", Target = InputControlType.RightStickButton, Source = Button( 11 ),
				},
				new InputControlMapping
				{
					Handle = "Home", Target = InputControlType.Home, Source = Button( 12 ),
				},
				new InputControlMapping
				{
					Handle = "Capture", Target = InputControlType.Capture, Source = Button( 13 ),
				},
				new InputControlMapping
				{
					Handle = "DPad Up", Target = InputControlType.DPadUp, Source = Button( 14 ),
				},
				new InputControlMapping
				{
					Handle = "DPad Down", Target = InputControlType.DPadDown, Source = Button( 15 ),
				},
				new InputControlMapping
				{
					Handle = "DPad Left", Target = InputControlType.DPadLeft, Source = Button( 16 ),
				},
				new InputControlMapping
				{
					Handle = "DPad Right", Target = InputControlType.DPadRight, Source = Button( 17 ),
				},
			};

			AnalogMappings = new[]
			{
				new InputControlMapping
				{
					Handle = "Left Stick Left",
					Target = InputControlType.LeftStickLeft,
					Source = Analog( 0 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping
				{
					Handle = "Left Stick Right",
					Target = InputControlType.LeftStickRight,
					Source = Analog( 0 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping
				{
					Handle = "Left Stick Up",
					Target = InputControlType.LeftStickUp,
					Source = Analog( 1 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping
				{
					Handle = "Left Stick Down",
					Target = InputControlType.LeftStickDown,
					Source = Analog( 1 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping
				{
					Handle = "Right Stick Left",
					Target = InputControlType.RightStickLeft,
					Source = Analog( 2 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping
				{
					Handle = "Right Stick Right",
					Target = InputControlType.RightStickRight,
					Source = Analog( 2 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping
				{
					Handle = "Right Stick Up",
					Target = InputControlType.RightStickUp,
					Source = Analog( 3 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping
				{
					Handle = "Right Stick Down",
					Target = InputControlType.RightStickDown,
					Source = Analog( 3 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne,
				},
			};
		}
	}

	// @endcond
}

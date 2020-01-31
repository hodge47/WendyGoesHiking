namespace InControl
{
	/* @cond nodoc */
	[AutoDiscover, Preserve]
	public class Xbox360MacFirefoxUnityProfile : UnityInputDeviceProfile
	{
		public Xbox360MacFirefoxUnityProfile()
		{
			Name = "Xbox 360 Controller";
			Meta = "Xbox 360 Controller on Mac Firefox";

			DeviceClass = InputDeviceClass.Controller;
			DeviceStyle = InputDeviceStyle.Xbox360;

			IncludePlatforms = new[]
			{
				"Mac Firefox"
			};

			JoystickNames = new[]
			{
				"45e-28e-Xbox 360 Wired Controller"
			};

			LastResortRegex = "Xbox 360";

			ButtonMappings = new[]
			{
				new InputControlMapping
				{
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = Button( 0 ),
				},
				new InputControlMapping
				{
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = Button( 1 ),
				},
				new InputControlMapping
				{
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = Button( 2 ),
				},
				new InputControlMapping
				{
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = Button( 3 ),
				},
				new InputControlMapping
				{
					Handle = "Start",
					Target = InputControlType.Start,
					Source = Button( 4 ),
				},
				new InputControlMapping
				{
					Handle = "Back",
					Target = InputControlType.Back,
					Source = Button( 5 ),
				},
				new InputControlMapping
				{
					Handle = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = Button( 6 ),
				},
				new InputControlMapping
				{
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = Button( 7 ),
				},
				new InputControlMapping
				{
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = Button( 8 ),
				},
				new InputControlMapping
				{
					Handle = "Right Bumper",
					Target = InputControlType.RightBumper,
					Source = Button( 9 ),
				},
				new InputControlMapping
				{
					Handle = "System",
					Target = InputControlType.System,
					Source = Button( 10 ),
				},
				new InputControlMapping
				{
					Handle = "A",
					Target = InputControlType.Action1,
					Source = Button( 11 ),
				},
				new InputControlMapping
				{
					Handle = "B",
					Target = InputControlType.Action2,
					Source = Button( 12 ),
				},
				new InputControlMapping
				{
					Handle = "X",
					Target = InputControlType.Action3,
					Source = Button( 13 ),
				},
				new InputControlMapping
				{
					Handle = "Y",
					Target = InputControlType.Action4,
					Source = Button( 14 ),
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
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = Analog( 2 ),
					SourceRange = InputRange.MinusOneToOne,
					TargetRange = InputRange.ZeroToOne,
					IgnoreInitialZeroValue = true
				},
				new InputControlMapping
				{
					Handle = "Right Stick Left",
					Target = InputControlType.RightStickLeft,
					Source = Analog( 3 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping
				{
					Handle = "Right Stick Right",
					Target = InputControlType.RightStickRight,
					Source = Analog( 3 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping
				{
					Handle = "Right Stick Up",
					Target = InputControlType.RightStickUp,
					Source = Analog( 4 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping
				{
					Handle = "Right Stick Down",
					Target = InputControlType.RightStickDown,
					Source = Analog( 4 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping
				{
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = Analog( 5 ),
					SourceRange = InputRange.MinusOneToOne,
					TargetRange = InputRange.ZeroToOne,
					IgnoreInitialZeroValue = true
				},
			};
		}
	}

	/* @endcond */
}

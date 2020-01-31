namespace InControl
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class HavitHVG95WLinuxProfile : UnityInputDeviceProfile
	{
		// From http://steamcommunity.com/app/340520/discussions/1/1470841715978033762/
		public HavitHVG95WLinuxProfile()
		{
			Name = "Havit HV-G95W 2.4G Wireless Gamepad";
			Meta = "Havit HV-G95W 2.4G Wireless Gamepad on Linux";

			DeviceClass = InputDeviceClass.Controller;
			DeviceStyle = InputDeviceStyle.PlayStation2;

			IncludePlatforms = new[]
			{
				"Linux"
			};

			JoystickNames = new[]
			{
				"ShanWan Twin USB Joystick"
			};

			ButtonMappings = new[]
			{
				new InputControlMapping
				{
					Handle = "Cross",
					Target = InputControlType.Action1,
					Source = Button2
				},
				new InputControlMapping
				{
					Handle = "Circle",
					Target = InputControlType.Action2,
					Source = Button1
				},
				new InputControlMapping
				{
					Handle = "Square",
					Target = InputControlType.Action3,
					Source = Button3
				},
				new InputControlMapping
				{
					Handle = "Triangle",
					Target = InputControlType.Action4,
					Source = Button0
				},
				new InputControlMapping
				{
					Handle = "L1",
					Target = InputControlType.LeftBumper,
					Source = Button4
				},
				new InputControlMapping
				{
					Handle = "R1",
					Target = InputControlType.RightBumper,
					Source = Button5
				},
				new InputControlMapping
				{
					Handle = "L2",
					Target = InputControlType.LeftTrigger,
					Source = Button6
				},
				new InputControlMapping
				{
					Handle = "R2",
					Target = InputControlType.RightTrigger,
					Source = Button7
				},
				new InputControlMapping
				{
					Handle = "Select",
					Target = InputControlType.Select,
					Source = Button8
				},
				new InputControlMapping
				{
					Handle = "Start",
					Target = InputControlType.Start,
					Source = Button9
				},
				new InputControlMapping
				{
					Handle = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = Button10
				},
				new InputControlMapping
				{
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = Button11
				}
			};

			AnalogMappings = new[]
			{
				LeftStickLeftMapping( Analog0 ),
				LeftStickRightMapping( Analog0 ),
				LeftStickUpMapping( Analog1 ),
				LeftStickDownMapping( Analog1 ),

				RightStickLeftMapping( Analog2 ),
				RightStickRightMapping( Analog2 ),
				RightStickUpMapping( Analog3 ),
				RightStickDownMapping( Analog3 ),

				DPadLeftMapping( Analog4 ),
				DPadRightMapping( Analog4 ),
				DPadUpMapping( Analog5 ),
				DPadDownMapping( Analog5 ),
			};
		}
	}

	// @endcond
}

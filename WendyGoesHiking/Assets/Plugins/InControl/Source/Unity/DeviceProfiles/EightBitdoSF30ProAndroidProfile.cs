namespace InControl
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class EightBitdoSF30ProAndroidProfile : UnityInputDeviceProfile
	{
		public EightBitdoSF30ProAndroidProfile()
		{
			Name = "8Bitdo SF30 Pro Controller";
			Meta = "8Bitdo SF30 Pro Controller on Android";
			// Link = "https://www.amazon.com/8Bitdo-Bluetooth-Controller-Raspberry-Compatible/dp/B0781NZ2BB/";

			DeviceClass = InputDeviceClass.Controller;
			DeviceStyle = InputDeviceStyle.NintendoSNES;

			IncludePlatforms = new[]
			{
				"Android"
			};

			JoystickNames = new[]
			{
				"8Bitdo SF30 Pro",
			};

			ButtonMappings = new[]
			{
				new InputControlMapping
				{
					Handle = "A",
					Target = InputControlType.Action1,
					Source = Button0
				},
				new InputControlMapping
				{
					Handle = "B",
					Target = InputControlType.Action2,
					Source = Button1
				},
				new InputControlMapping
				{
					Handle = "X",
					Target = InputControlType.Action3,
					Source = Button2
				},
				new InputControlMapping
				{
					Handle = "Y",
					Target = InputControlType.Action4,
					Source = Button3
				},
				new InputControlMapping
				{
					Handle = "Select",
					Target = InputControlType.Select,
					Source = Button11
				},
				new InputControlMapping
				{
					Handle = "Start",
					Target = InputControlType.Start,
					Source = Button10
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
				DPadDownMapping( Analog5 )
			};
		}
	}

	// @endcond
}

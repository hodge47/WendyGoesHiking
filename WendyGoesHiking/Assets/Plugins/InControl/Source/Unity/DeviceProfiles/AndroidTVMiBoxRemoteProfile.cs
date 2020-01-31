namespace InControl
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class AndroidTVMiBoxRemoteProfile : UnityInputDeviceProfile
	{
		public AndroidTVMiBoxRemoteProfile()
		{
			Name = "Xiaomi Remote";
			Meta = "Xiaomi Remote on Android TV";

			DeviceClass = InputDeviceClass.Remote;

			IncludePlatforms = new[]
			{
				"Android"
			};

			JoystickNames = new[]
			{
				"Xiaomi Remote"
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
					Handle = "Back",
					Target = InputControlType.Back,
					Source = EscapeKey
				},
			};

			AnalogMappings = new[]
			{
				DPadLeftMapping( Analog4 ),
				DPadRightMapping( Analog4 ),
				DPadUpMapping( Analog5 ),
				DPadDownMapping( Analog5 ),
			};
		}
	}

	// @endcond
}

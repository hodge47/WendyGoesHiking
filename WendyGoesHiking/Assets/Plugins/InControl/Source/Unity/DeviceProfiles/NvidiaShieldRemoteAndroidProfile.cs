namespace InControl
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class NvidiaShieldRemoteAndroidProfile : UnityInputDeviceProfile
	{
		public NvidiaShieldRemoteAndroidProfile()
		{
			Name = "NVIDIA Shield Remote";
			Meta = "NVIDIA Shield Remote on Android";

			DeviceClass = InputDeviceClass.Remote;
			DeviceStyle = InputDeviceStyle.NVIDIAShield;

			IncludePlatforms = new[] {
				"Android"
			};

			JoystickNames = new[] {
				"SHIELD Remote"
			};

			JoystickRegex = new[] {
				"SHIELD Remote"
			};

			ButtonMappings = new[] {
				new InputControlMapping {
					Handle = "A",
					Target = InputControlType.Action1,
					Source = Button0
				},
			};

			AnalogMappings = new[] {
				DPadLeftMapping( Analog4 ),
				DPadRightMapping( Analog4 ),
				DPadUpMapping( Analog5 ),
				DPadDownMapping( Analog5 ),
			};
		}
	}
	// @endcond
}


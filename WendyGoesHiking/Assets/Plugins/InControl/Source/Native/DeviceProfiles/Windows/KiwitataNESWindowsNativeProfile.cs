namespace InControl.NativeProfile
{
	[AutoDiscover, Preserve]
	public class KiwitataNESWindowsNativeProfile : NativeInputDeviceProfile
	{
		public KiwitataNESWindowsNativeProfile()
		{
			Name = "Kiwitata NES Controller";
			Meta = "Kiwitata NES on Windows";

			DeviceClass = InputDeviceClass.Controller;
			DeviceStyle = InputDeviceStyle.NintendoNES;

			IncludePlatforms = new[]
			{
				"Windows"
			};

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x79,
					ProductID = 0x11,
					// VersionNumber = 0x0,
				},
			};

			ButtonMappings = new[]
			{
				new InputControlMapping
				{
					Handle = "A",
					Target = InputControlType.Action1,
					Source = Button( 2 ),
				},
				new InputControlMapping
				{
					Handle = "B",
					Target = InputControlType.Action2,
					Source = Button( 1 ),
				},
				new InputControlMapping
				{
					Handle = "X",
					Target = InputControlType.Action3,
					Source = Button( 3 ),
				},
				new InputControlMapping
				{
					Handle = "Y",
					Target = InputControlType.Action4,
					Source = Button( 0 ),
				},
				new InputControlMapping
				{
					Handle = "L1",
					Target = InputControlType.LeftBumper,
					Source = Button( 4 ),
				},
				new InputControlMapping
				{
					Handle = "R1",
					Target = InputControlType.RightBumper,
					Source = Button( 5 ),
				},
				new InputControlMapping
				{
					Handle = "L2",
					Target = InputControlType.LeftTrigger,
					Source = Button( 4 ),
				},
				new InputControlMapping
				{
					Handle = "R2",
					Target = InputControlType.RightTrigger,
					Source = Button( 5 ),
				},
				new InputControlMapping
				{
					Handle = "Select",
					Target = InputControlType.Select,
					Source = Button( 8 ),
				},
				new InputControlMapping
				{
					Handle = "Start",
					Target = InputControlType.Start,
					Source = Button( 9 ),
				},
			};

			AnalogMappings = new[]
			{
				DPadLeftMapping( 1 ),
				DPadRightMapping( 1 ),
				DPadUpMapping( 0 ),
				DPadDownMapping( 0 )
			};
		}
	}
}

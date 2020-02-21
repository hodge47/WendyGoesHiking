namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class SteelseriesNimbusMacNativeProfile : NativeInputDeviceProfile
	{
		// DPad doesn't work on Mac for whatever reason.
		//
		public SteelseriesNimbusMacNativeProfile()
		{
			Name = "Steelseries Nimbus Controller";
			Meta = "Steelseries Nimbus Controller on Mac";
			// Link = "https://www.amazon.com/SteelSeries-Nimbus-Wireless-Gaming-Controller/dp/B01AZC3III";

			IncludePlatforms = new[]
			{
				"OS X"
			};

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x111,
					ProductID = 0x1420,
					// VersionNumber = 0x1,
				},
			};

			ButtonMappings = new[]
			{
				new InputControlMapping
				{
					Handle = "A",
					Target = InputControlType.Action1,
					Source = Button( 0 ),
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
					Source = Button( 2 ),
				},
				new InputControlMapping
				{
					Handle = "Y",
					Target = InputControlType.Action4,
					Source = Button( 3 ),
				},
				new InputControlMapping
				{
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = Button( 4 ),
				},
				new InputControlMapping
				{
					Handle = "Right Bumper",
					Target = InputControlType.RightBumper,
					Source = Button( 5 ),
				},
				new InputControlMapping
				{
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = Button( 6 ),
				},
				new InputControlMapping
				{
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = Button( 7 ),
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
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping
				{
					Handle = "Left Stick Down",
					Target = InputControlType.LeftStickDown,
					Source = Analog( 1 ),
					SourceRange = InputRange.ZeroToMinusOne,
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
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping
				{
					Handle = "Right Stick Down",
					Target = InputControlType.RightStickDown,
					Source = Analog( 3 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne,
				},
			};
		}
	}

	// @endcond
}

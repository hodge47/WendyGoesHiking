namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class HarmonixKeyboardMacProfile : Xbox360DriverMacProfile
	{
		public HarmonixKeyboardMacProfile()
		{
			Name = "Harmonix Keyboard";
			Meta = "Harmonix Keyboard on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x1bad,
					ProductID = 0x1338,
				},
			};
		}
	}

	// @endcond
}



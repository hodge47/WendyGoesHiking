namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class MadCatzBeatPadMacProfile : Xbox360DriverMacProfile
	{
		public MadCatzBeatPadMacProfile()
		{
			Name = "Mad Catz Beat Pad";
			Meta = "Mad Catz Beat Pad on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0738,
					ProductID = 0x4740,
				},
			};
		}
	}

	// @endcond
}



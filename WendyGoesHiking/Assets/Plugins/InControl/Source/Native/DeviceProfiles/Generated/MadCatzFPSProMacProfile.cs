namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class MadCatzFPSProMacProfile : Xbox360DriverMacProfile
	{
		public MadCatzFPSProMacProfile()
		{
			Name = "Mad Catz FPS Pro";
			Meta = "Mad Catz FPS Pro on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x1bad,
					ProductID = 0xf027,
				},
			};
		}
	}

	// @endcond
}



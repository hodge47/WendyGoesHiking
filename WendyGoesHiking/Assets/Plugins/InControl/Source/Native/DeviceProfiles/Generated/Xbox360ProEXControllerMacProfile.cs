namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class Xbox360ProEXControllerMacProfile : Xbox360DriverMacProfile
	{
		public Xbox360ProEXControllerMacProfile()
		{
			Name = "Xbox 360 Pro EX Controller";
			Meta = "Xbox 360 Pro EX Controller on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x20d6,
					ProductID = 0x281f,
				},
			};
		}
	}

	// @endcond
}



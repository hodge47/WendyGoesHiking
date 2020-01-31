namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class LogitechControllerMacProfile : Xbox360DriverMacProfile
	{
		public LogitechControllerMacProfile()
		{
			Name = "Logitech Controller";
			Meta = "Logitech Controller on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x046d,
					ProductID = 0xf301,
				},
			};
		}
	}

	// @endcond
}



namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class LogitechThunderpadMacProfile : Xbox360DriverMacProfile
	{
		public LogitechThunderpadMacProfile()
		{
			Name = "Logitech Thunderpad";
			Meta = "Logitech Thunderpad on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x046d,
					ProductID = 0xca88,
				},
			};
		}
	}

	// @endcond
}



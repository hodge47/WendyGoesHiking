namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class HoriFightingCommanderMacProfile : Xbox360DriverMacProfile
	{
		public HoriFightingCommanderMacProfile()
		{
			Name = "Hori Fighting Commander";
			Meta = "Hori Fighting Commander on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0f0d,
					ProductID = 0x00c5,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x24c6,
					ProductID = 0x5510,
				},
			};
		}
	}

	// @endcond
}



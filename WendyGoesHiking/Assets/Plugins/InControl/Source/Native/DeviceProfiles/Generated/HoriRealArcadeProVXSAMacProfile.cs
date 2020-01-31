namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class HoriRealArcadeProVXSAMacProfile : Xbox360DriverMacProfile
	{
		public HoriRealArcadeProVXSAMacProfile()
		{
			Name = "Hori Real Arcade Pro VX SA";
			Meta = "Hori Real Arcade Pro VX SA on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x1bad,
					ProductID = 0xf502,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x24c6,
					ProductID = 0x5501,
				},
			};
		}
	}

	// @endcond
}



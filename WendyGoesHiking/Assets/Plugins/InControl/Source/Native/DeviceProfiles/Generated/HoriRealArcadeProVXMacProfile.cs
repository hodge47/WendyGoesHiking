namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class HoriRealArcadeProVXMacProfile : Xbox360DriverMacProfile
	{
		public HoriRealArcadeProVXMacProfile()
		{
			Name = "Hori Real Arcade Pro VX";
			Meta = "Hori Real Arcade Pro VX on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0f0d,
					ProductID = 0x001b,
				},
			};
		}
	}

	// @endcond
}



namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class HoriRealArcadeProVKaiFightingStickMacProfile : Xbox360DriverMacProfile
	{
		public HoriRealArcadeProVKaiFightingStickMacProfile()
		{
			Name = "Hori Real Arcade Pro V Kai Fighting Stick";
			Meta = "Hori Real Arcade Pro V Kai Fighting Stick on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x24c6,
					ProductID = 0x550e,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0f0d,
					ProductID = 0x0078,
				},
			};
		}
	}

	// @endcond
}



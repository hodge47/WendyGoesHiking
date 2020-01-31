namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class HoriRAPNFightingStickMacProfile : Xbox360DriverMacProfile
	{
		public HoriRAPNFightingStickMacProfile()
		{
			Name = "Hori RAP N Fighting Stick";
			Meta = "Hori RAP N Fighting Stick on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0f0d,
					ProductID = 0x00ae,
				},
			};
		}
	}

	// @endcond
}



namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class RedOctaneGuitarMacProfile : Xbox360DriverMacProfile
	{
		public RedOctaneGuitarMacProfile()
		{
			Name = "RedOctane Guitar";
			Meta = "RedOctane Guitar on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x1430,
					ProductID = 0x070b,
				},
			};
		}
	}

	// @endcond
}



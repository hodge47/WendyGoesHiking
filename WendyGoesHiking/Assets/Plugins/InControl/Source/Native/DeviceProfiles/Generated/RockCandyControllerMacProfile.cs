namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class RockCandyControllerMacProfile : Xbox360DriverMacProfile
	{
		public RockCandyControllerMacProfile()
		{
			Name = "Rock Candy Controller";
			Meta = "Rock Candy Controller on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0e6f,
					ProductID = 0x011f,
				},
			};
		}
	}

	// @endcond
}



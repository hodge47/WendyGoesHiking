namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class HoneyBeeControllerMacProfile : Xbox360DriverMacProfile
	{
		public HoneyBeeControllerMacProfile()
		{
			Name = "Honey Bee Controller";
			Meta = "Honey Bee Controller on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x12ab,
					ProductID = 0x5500,
				},
			};
		}
	}

	// @endcond
}



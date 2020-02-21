namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class RazerStrikeControllerMacProfile : Xbox360DriverMacProfile
	{
		public RazerStrikeControllerMacProfile()
		{
			Name = "Razer Strike Controller";
			Meta = "Razer Strike Controller on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x1689,
					ProductID = 0x0001,
				},
			};
		}
	}

	// @endcond
}



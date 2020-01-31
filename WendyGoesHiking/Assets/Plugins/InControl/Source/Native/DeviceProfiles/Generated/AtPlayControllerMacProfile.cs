namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class AtPlayControllerMacProfile : Xbox360DriverMacProfile
	{
		public AtPlayControllerMacProfile()
		{
			Name = "At Play Controller";
			Meta = "At Play Controller on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x24c6,
					ProductID = 0xfafa,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x24c6,
					ProductID = 0xfafb,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0e6f,
					ProductID = 0x02b2,
				},
			};
		}
	}

	// @endcond
}



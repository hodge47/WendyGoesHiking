namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class PDPAfterglowPrismaticControllerMacProfile : Xbox360DriverMacProfile
	{
		public PDPAfterglowPrismaticControllerMacProfile()
		{
			Name = "PDP Afterglow Prismatic Controller";
			Meta = "PDP Afterglow Prismatic Controller on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0e6f,
					ProductID = 0x0139,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0e6f,
					ProductID = 0x02b3,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0e6f,
					ProductID = 0x02b8,
				},
			};
		}
	}

	// @endcond
}



namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class HoriControllerMacProfile : Xbox360DriverMacProfile
	{
		public HoriControllerMacProfile()
		{
			Name = "Hori Controller";
			Meta = "Hori Controller on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0f0d,
					ProductID = 0x00dc,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0f0d,
					ProductID = 0x0067,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0f0d,
					ProductID = 0x0100,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x1bad,
					ProductID = 0x5500,
				},
				new NativeInputDeviceMatcher
				{
					VendorID = 0x1bad,
					ProductID = 0x028e,
				},
			};
		}
	}

	// @endcond
}



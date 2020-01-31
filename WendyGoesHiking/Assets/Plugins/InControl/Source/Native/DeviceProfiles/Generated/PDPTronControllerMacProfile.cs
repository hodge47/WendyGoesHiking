namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class PDPTronControllerMacProfile : Xbox360DriverMacProfile
	{
		public PDPTronControllerMacProfile()
		{
			Name = "PDP Tron Controller";
			Meta = "PDP Tron Controller on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x1bad,
					ProductID = 0xf903,
				},
			};
		}
	}

	// @endcond
}



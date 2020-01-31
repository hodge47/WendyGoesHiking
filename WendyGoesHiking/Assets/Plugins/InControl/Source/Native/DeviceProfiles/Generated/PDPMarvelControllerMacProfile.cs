namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class PDPMarvelControllerMacProfile : Xbox360DriverMacProfile
	{
		public PDPMarvelControllerMacProfile()
		{
			Name = "PDP Marvel Controller";
			Meta = "PDP Marvel Controller on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0e6f,
					ProductID = 0x0147,
				},
			};
		}
	}

	// @endcond
}



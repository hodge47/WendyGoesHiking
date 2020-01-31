namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class PDPMetallicsLEControllerMacProfile : Xbox360DriverMacProfile
	{
		public PDPMetallicsLEControllerMacProfile()
		{
			Name = "PDP Metallics LE Controller";
			Meta = "PDP Metallics LE Controller on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0e6f,
					ProductID = 0x0159,
				},
			};
		}
	}

	// @endcond
}



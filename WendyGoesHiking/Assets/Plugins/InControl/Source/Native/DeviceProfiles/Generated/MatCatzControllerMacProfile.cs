namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class MatCatzControllerMacProfile : Xbox360DriverMacProfile
	{
		public MatCatzControllerMacProfile()
		{
			Name = "Mat Catz Controller";
			Meta = "Mat Catz Controller on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x1bad,
					ProductID = 0xf016,
				},
			};
		}
	}

	// @endcond
}



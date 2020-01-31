namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class MadCatzInnoControllerMacProfile : Xbox360DriverMacProfile
	{
		public MadCatzInnoControllerMacProfile()
		{
			Name = "Mad Catz Inno Controller";
			Meta = "Mad Catz Inno Controller on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0738,
					ProductID = 0xf401,
				},
			};
		}
	}

	// @endcond
}



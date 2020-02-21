namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class ArdwiinoControllerMacProfile : Xbox360DriverMacProfile
	{
		public ArdwiinoControllerMacProfile()
		{
			Name = "Ardwiino Controller";
			Meta = "Ardwiino Controller on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x1209,
					ProductID = 0x2882,
				},
			};
		}
	}

	// @endcond
}



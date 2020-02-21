namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class AirFloControllerMacProfile : Xbox360DriverMacProfile
	{
		public AirFloControllerMacProfile()
		{
			Name = "Air Flo Controller";
			Meta = "Air Flo Controller on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x24c6,
					ProductID = 0x5303,
				},
			};
		}
	}

	// @endcond
}



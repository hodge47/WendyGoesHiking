namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class McbazelAdapterMacProfile : Xbox360DriverMacProfile
	{
		public McbazelAdapterMacProfile()
		{
			Name = "Mcbazel Adapter";
			Meta = "Mcbazel Adapter on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0e6f,
					ProductID = 0x028e,
				},
			};
		}
	}

	// @endcond
}



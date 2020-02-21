namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class PowerAMiniControllerMacProfile : Xbox360DriverMacProfile
	{
		public PowerAMiniControllerMacProfile()
		{
			Name = "PowerA Mini Controller";
			Meta = "PowerA Mini Controller on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x24c6,
					ProductID = 0x541a,
				},
			};
		}
	}

	// @endcond
}



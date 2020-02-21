namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class RockCandyPS3ControllerMacProfile : Xbox360DriverMacProfile
	{
		public RockCandyPS3ControllerMacProfile()
		{
			Name = "Rock Candy PS3 Controller";
			Meta = "Rock Candy PS3 Controller on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0e6f,
					ProductID = 0x011e,
				},
			};
		}
	}

	// @endcond
}



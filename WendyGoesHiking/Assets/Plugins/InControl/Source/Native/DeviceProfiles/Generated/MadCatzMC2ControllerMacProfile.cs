namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class MadCatzMC2ControllerMacProfile : Xbox360DriverMacProfile
	{
		public MadCatzMC2ControllerMacProfile()
		{
			Name = "MadCatz MC2 Controller";
			Meta = "MadCatz MC2 Controller on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0738,
					ProductID = 0x4720,
				},
			};
		}
	}

	// @endcond
}



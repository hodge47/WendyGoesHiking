namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class MadCatzMC2RacingWheelMacProfile : Xbox360DriverMacProfile
	{
		public MadCatzMC2RacingWheelMacProfile()
		{
			Name = "MadCatz MC2 Racing Wheel";
			Meta = "MadCatz MC2 Racing Wheel on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x1bad,
					ProductID = 0xf020,
				},
			};
		}
	}

	// @endcond
}



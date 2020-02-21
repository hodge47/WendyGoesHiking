namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class ThrustMasterFerrari430RacingWheelMacProfile : Xbox360DriverMacProfile
	{
		public ThrustMasterFerrari430RacingWheelMacProfile()
		{
			Name = "ThrustMaster Ferrari 430 Racing Wheel";
			Meta = "ThrustMaster Ferrari 430 Racing Wheel on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x044f,
					ProductID = 0xb65b,
				},
			};
		}
	}

	// @endcond
}



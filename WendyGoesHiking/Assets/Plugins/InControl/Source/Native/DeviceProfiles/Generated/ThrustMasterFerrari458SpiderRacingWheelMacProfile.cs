namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class ThrustMasterFerrari458SpiderRacingWheelMacProfile : Xbox360DriverMacProfile
	{
		public ThrustMasterFerrari458SpiderRacingWheelMacProfile()
		{
			Name = "ThrustMaster Ferrari 458 Spider Racing Wheel";
			Meta = "ThrustMaster Ferrari 458 Spider Racing Wheel on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x044f,
					ProductID = 0xb671,
				},
			};
		}
	}

	// @endcond
}



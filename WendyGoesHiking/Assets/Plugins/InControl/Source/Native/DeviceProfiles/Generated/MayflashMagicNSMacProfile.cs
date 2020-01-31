namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class MayflashMagicNSMacProfile : Xbox360DriverMacProfile
	{
		public MayflashMagicNSMacProfile()
		{
			Name = "Mayflash Magic-NS";
			Meta = "Mayflash Magic-NS on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0079,
					ProductID = 0x18d3,
				},
			};
		}
	}

	// @endcond
}



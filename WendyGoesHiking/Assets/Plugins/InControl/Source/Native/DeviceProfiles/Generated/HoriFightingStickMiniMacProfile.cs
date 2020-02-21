namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class HoriFightingStickMiniMacProfile : Xbox360DriverMacProfile
	{
		public HoriFightingStickMiniMacProfile()
		{
			Name = "Hori Fighting Stick Mini";
			Meta = "Hori Fighting Stick Mini on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0f0d,
					ProductID = 0x00ed,
				},
			};
		}
	}

	// @endcond
}



namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class HoriEdgeFightingStickMacProfile : Xbox360DriverMacProfile
	{
		public HoriEdgeFightingStickMacProfile()
		{
			Name = "Hori Edge Fighting Stick";
			Meta = "Hori Edge Fighting Stick on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0f0d,
					ProductID = 0x006d,
				},
			};
		}
	}

	// @endcond
}



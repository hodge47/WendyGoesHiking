namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class HoriFightingEdgeArcadeStickMacProfile : Xbox360DriverMacProfile
	{
		public HoriFightingEdgeArcadeStickMacProfile()
		{
			Name = "Hori Fighting Edge Arcade Stick";
			Meta = "Hori Fighting Edge Arcade Stick on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x24c6,
					ProductID = 0x5503,
				},
			};
		}
	}

	// @endcond
}



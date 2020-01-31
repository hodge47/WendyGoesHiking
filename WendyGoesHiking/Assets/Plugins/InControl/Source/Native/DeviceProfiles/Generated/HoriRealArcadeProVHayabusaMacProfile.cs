namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class HoriRealArcadeProVHayabusaMacProfile : Xbox360DriverMacProfile
	{
		public HoriRealArcadeProVHayabusaMacProfile()
		{
			Name = "Hori Real Arcade Pro V Hayabusa";
			Meta = "Hori Real Arcade Pro V Hayabusa on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0f0d,
					ProductID = 0x00d8,
				},
			};
		}
	}

	// @endcond
}



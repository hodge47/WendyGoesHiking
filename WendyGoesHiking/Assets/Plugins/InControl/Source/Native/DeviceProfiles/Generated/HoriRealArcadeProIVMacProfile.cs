namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class HoriRealArcadeProIVMacProfile : Xbox360DriverMacProfile
	{
		public HoriRealArcadeProIVMacProfile()
		{
			Name = "Hori Real Arcade Pro IV";
			Meta = "Hori Real Arcade Pro IV on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0f0d,
					ProductID = 0x008c,
				},
			};
		}
	}

	// @endcond
}



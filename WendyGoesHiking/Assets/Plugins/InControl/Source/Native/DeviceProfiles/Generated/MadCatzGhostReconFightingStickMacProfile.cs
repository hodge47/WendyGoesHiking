namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class MadCatzGhostReconFightingStickMacProfile : Xbox360DriverMacProfile
	{
		public MadCatzGhostReconFightingStickMacProfile()
		{
			Name = "Mad Catz Ghost Recon Fighting Stick";
			Meta = "Mad Catz Ghost Recon Fighting Stick on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x1bad,
					ProductID = 0xf021,
				},
			};
		}
	}

	// @endcond
}



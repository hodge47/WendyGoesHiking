namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class HyperkinX91MacProfile : Xbox360DriverMacProfile
	{
		public HyperkinX91MacProfile()
		{
			Name = "Hyperkin X91";
			Meta = "Hyperkin X91 on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x2e24,
					ProductID = 0x1688,
				},
			};
		}
	}

	// @endcond
}



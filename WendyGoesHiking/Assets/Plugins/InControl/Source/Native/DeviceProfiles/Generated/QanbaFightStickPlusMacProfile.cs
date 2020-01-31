namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class QanbaFightStickPlusMacProfile : Xbox360DriverMacProfile
	{
		public QanbaFightStickPlusMacProfile()
		{
			Name = "Qanba Fight Stick Plus";
			Meta = "Qanba Fight Stick Plus on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0738,
					ProductID = 0xbeef,
				},
			};
		}
	}

	// @endcond
}



namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class TrustPredatorJoystickMacProfile : Xbox360DriverMacProfile
	{
		public TrustPredatorJoystickMacProfile()
		{
			Name = "Trust Predator Joystick";
			Meta = "Trust Predator Joystick on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x0810,
					ProductID = 0x0003,
				},
			};
		}
	}

	// @endcond
}



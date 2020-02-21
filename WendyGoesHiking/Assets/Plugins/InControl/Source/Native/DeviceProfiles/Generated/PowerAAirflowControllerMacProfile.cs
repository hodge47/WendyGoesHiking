namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover, Preserve]
	public class PowerAAirflowControllerMacProfile : Xbox360DriverMacProfile
	{
		public PowerAAirflowControllerMacProfile()
		{
			Name = "PowerA Airflow Controller";
			Meta = "PowerA Airflow Controller on Mac";

			Matchers = new[]
			{
				new NativeInputDeviceMatcher
				{
					VendorID = 0x15e4,
					ProductID = 0x3f0a,
				},
			};
		}
	}

	// @endcond
}



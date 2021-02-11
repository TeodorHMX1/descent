namespace Items
{
	
	/// <summary>
	///     <para> FlashPattern </para>
	///     <author> @TeodorHMX1 </author>
	/// </summary>
	public class FlashPattern
	{
		private int _time;
		private bool _isDark;

		/// <summary>
		///     <para> IsDark </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public bool IsDark
		{
			get => _isDark;
			set => _isDark = value;
		}

		/// <summary>
		///     <para> Time </para>
		///     <author> @TeodorHMX1 </author>
		/// </summary>
		public int Time
		{
			get => _time;
			set => _time = value;
		}
	}
}
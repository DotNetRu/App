namespace XamarinEvolve.DataObjects
{
	public class ConferenceFeedback : BaseDataObject
	{
		public string UserId { get; set; }
		public int Question1 { get; set; }
		public int Question2 { get; set; }
		public int Question3 { get; set; }
		public int Question4 { get; set; }
		public int Question5 { get; set; }
		public int Question6 { get; set; }
		public int Question7 { get; set; }
		public int Question8 { get; set; }
		public int Question9 { get; set; }
		public int Question10 { get; set; }
		public string DeviceOS { get; set; }
		public string AppVersion { get; set; }
	}
}

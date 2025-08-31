namespace ConfigurationManagement.Models
{
	public class ConfigurationItemViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public string Type { get; set; } = null!;
		public string Value { get; set; } = null!;
		public bool IsActive { get; set; }
		public string ApplicationName { get; set; } = null!;
	}
}

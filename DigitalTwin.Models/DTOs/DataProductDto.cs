namespace DigitalTwin.Models.DTOs
{
	public class DataProductDto
	{
		public Guid? Id { get; set; }
		public Guid? EntityMapId { get; set; }
		public Guid? EntityId { get; set; }
		public string? Name { get; set; }
		public string? Unit { get; set; }
		public string? NumValues { get; set; }
		public string? Color { get; set; }
		public string? KpiPath { get; set; }
		public string? EntityGroupName { get; set; }
	}
}


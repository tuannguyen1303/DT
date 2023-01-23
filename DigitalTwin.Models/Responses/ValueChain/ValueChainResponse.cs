namespace DigitalTwin.Models.Responses.ValueChain
{
    public class ValueChainResponse
    {
        public string? Title { get; set; }
        public List<EntityDto>? Entities { get; set; }
        public List<ProductDto>? Products { get; set; }
        public int TotalEntity
        {
            get
            {
                return Entities!.Count;
            }
        }
        public int TotalProduct
        {
            get
            {
                return Products!.Count;
            }
        }
    }

    public class Entity
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid EntityId { get; set; }
        public string? Alias { get; set; }
        public string? Type { get; set; }
        public string? Section { get; set; }
        public string? Category { get; set; }
    }

    public class EntityDto
    {
        public Guid Id { get; set; }
        public Guid EntityId { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
        public string? Type { get; set; }
        public string? Path { get; set; }
        public string? Status { get; set; }
        public string? Category { get; set; }
        public Guid? EntityParentId { get; set; }
        public Guid? EntityTypeId { get; set; }
        public int EntityTypeMasterId { get; set; }
        public string? Section { get; set; }
        public int Depth { get; set; }
        public List<Entity>? ParentList { get; set; }
        public List<Entity>? ChildrenList { get; set; }
    }

    public class ProductDto
    {
        public Guid Id { get; set; }
        public Guid EntityId { get; set; }
        public Guid ProductionVolumeId { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
        public string? Type { get; set; }
        public string? Path { get; set; }
        public string? Status { get; set; }
        public string? Category { get; set; }
        public Guid? EntityParentId { get; set; }
        public Guid? EntityTypeId { get; set; }
        public int EntityTypeMasterId { get; set; }
        public string? Section { get; set; }
        public decimal? Value { get; set; }
        public decimal? Percentage { get; set; }
        public decimal? Variance { get; set; }
        public string? Unit { get; set; }
        public string? Color { get; set; }
        public decimal? Planned { get; set; }
        public decimal? Target { get; set; }
        public decimal? Kbpi { get; set; }
        public int Depth { get; set; }
        public Guid? Children { get; set; }
        public Guid? Parent { get; set; }
    }

    public class ProductDetailDto
    {
        public Guid Id { get; set; }
        public Guid? EntityMapId { get; set; }
        public string? Alias { get; set; }
        public string? Name { get; set; }
        public string? Unit { get; set; }
        public string? NumberValues { get; set; }
        public string? Color { get; set; }
        public string? Planned { get; set; }
        public string? Target { get; set; }
        public string? Kbpi { get; set; }
        public string? Value { get; set; }
        public string? Percentage { get; set; }
        public string? Variance { get; set; }
        public string? Forecast { get; set; }
        public DateTime DateData { get; set; }
    }
}

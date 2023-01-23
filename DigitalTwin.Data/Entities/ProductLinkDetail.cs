using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalTwin.Data.Entities
{
    public class ProductLinkDetail : BaseEntity
    {
        public Guid? ProductLinkId { get; set; }

        public string? Frequency { get; set; }

        /// <summary>
        /// Data date of entity, using for select 
        /// </summary>
        public DateTime? DataDate { get; set; }

        /// <summary>
        /// Defining frequency types for display
        /// </summary>
        #region Frequency type

        public bool IsRealTime { get; set; }
        public bool IsDaily { get; set; }
        public bool IsWeekly { get; set; }
        public bool IsMonthly { get; set; }
        public bool IsMonthToDate { get; set; }
        public bool IsQuarterly { get; set; }
        public bool IsQuarterToDate { get; set; }
        public bool IsYearToDaily { get; set; }
        public bool IsYearToMonthly { get; set; }
        public bool IsYearEndProjection { get; set; }
        #endregion

        /// <summary>
        /// Display unit of measure's name
        /// </summary>
        public string? UomName { get; set; }
        public string? NorCode { get; set; }
        public string? Color { get; set; }

        /// <summary>
        /// Display actual values for display on canvas view
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// Display percentage
        /// </summary>
        public decimal? Percentage { get; set; }

        /// <summary>
        /// Display variance
        /// </summary>
        public decimal? Variance { get; set; }

        [Column(TypeName = "jsonb")]
        public string? NumValues { get; set; }
    }
}

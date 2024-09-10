namespace SampleBackend.Model.Model.Model
{
    public class CommonModel : CommonPaginationResponse
    {
        public long LoggedInUserId { get; set; }
        public string? LoggedInEmailId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? StrCreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? StrUpdatedOn { get; set; }
        public string? LoginToken { get; set; }
        public long? BranchId { get; set; }
    }

    public class CommonPaginationModel : CommonIdModel
    {
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public string? StrSearch { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public List<ColumnFilterModel>? ColumnFilters { get; set; }
    }

    public class ColumnFilterModel
    {
        public string ColumnName { get; set; }
        public string FilterValue { get; set; }
    }

    public class CommonIdModel : CommonModel
    {
        public long? Id { get; set; }
    }

    public class CommonPaginationResponse
    {
        public long? TotalRecord { get; set; }
        public long? TotalFilteredRecord { get; set; }
    }
}

namespace SampleBackend.Model.Model.Model
{
    public class DashboardModel : CommonModel
    {
        public long? UserId { get; set; }
        public long? CompanyId { get; set; }
        public long? BranchId { get; set; }
        public long? DashboardId { get; set; }
        public decimal? CarbonEmission { get; set; }
        public decimal? GarmentPurchased { get; set; }
        public int? LifeRecycledKG { get; set; }
        public long? ParcelDelivered { get; set; }
        public decimal? EVPercentage { get; set; }
        public int? SavedCO2 { get; set; }
        public string? Result { get; set; }
        public List<CarbonReductionModel>? carbonReduction { get; set; }
        public List<GRSMaterialPurchaseDetailModel>? grsMaterials { get; set; }
    }

    public class GRSMaterialPurchaseDetailModel : CommonModel
    {
        public long? MaterialPurchaseId { get; set; }
        public int? MaterialNumber { get; set; }
        public decimal? Value { get; set; }
    }

    public class CarbonReductionModel : CommonModel
    {
        public long? CarbonReductionId { get; set; }
        public string? Month { get; set; }
        public decimal? Value { get; set; }
        public long? SortOrder { get; set; }
    }
}

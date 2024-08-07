using SampleBackend.Model.Model.Model;

namespace SampleBackend.Data.DBRepository.Dashboard
{
    public interface IDashboardRepository
    {
        #region Post
        Task<DashboardModel> GetDashboardMasterDetail(UserModel model);
        Task<List<GRSMaterialPurchaseDetailModel>> GetGRSMaterialPurchasesDetail(UserModel model);
        Task<List<CarbonReductionModel>> GetCarbonReductionChartMasterDetail(UserModel model);
        Task<long> SaveDashboardMaster(DashboardModel model);
        Task<DashboardModel> SaveGRSMaterialsMaster(DashboardModel model);
        Task<DashboardModel> SaveCarbonReductionMaster(DashboardModel model);
        #endregion
    }
}

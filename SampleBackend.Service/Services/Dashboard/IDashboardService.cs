using SampleBackend.Model.Model.Model;

namespace SampleBackend.Service.Services.Dashboard
{
    public interface IDashboardService
    {
        #region Get
        Task<DashboardModel> GetDashboardMasterDetail(UserModel model);
        Task<List<GRSMaterialPurchaseDetailModel>> GetGRSMaterialPurchasesDetail(UserModel model);
        Task<List<CarbonReductionModel>> GetCarbonReductionChartMasterDetail(UserModel model);
        #endregion
        #region Post
        Task<long> SaveDashboardMaster(DashboardModel model);
        Task<DashboardModel> SaveGRSMaterialsMaster(DashboardModel model);
        Task<DashboardModel> SaveCarbonReductionMaster(DashboardModel model);
        #endregion
    }
}

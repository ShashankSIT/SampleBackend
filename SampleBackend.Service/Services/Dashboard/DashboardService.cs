using SampleBackend.Model;
using SampleBackend.Model.Model.Model;
using SampleBackend.Data.DBRepository.Dashboard;

namespace SampleBackend.Service.Services.Dashboard
{
    public class DashboardService(IDashboardRepository repository) : IDashboardService
    {
        #region Fields
        private readonly IDashboardRepository _repository = repository;
        #endregion

        #region Get
        public async Task<DashboardModel> GetDashboardMasterDetail(UserModel model)
        {
            return await _repository.GetDashboardMasterDetail(model);
        }

        public async Task<List<GRSMaterialPurchaseDetailModel>> GetGRSMaterialPurchasesDetail(UserModel model)
        {
            return await _repository.GetGRSMaterialPurchasesDetail(model);
        }

        public async Task<List<CarbonReductionModel>> GetCarbonReductionChartMasterDetail(UserModel model)
        {
            return await _repository.GetCarbonReductionChartMasterDetail(model);
        }
        #endregion
        #region Post
        public async Task<long> SaveDashboardMaster(DashboardModel model)
        {
            return await _repository.SaveDashboardMaster(model);
        }
        public async Task<DashboardModel> SaveGRSMaterialsMaster(DashboardModel model)
        {
            return await _repository.SaveGRSMaterialsMaster(model);
        }
        public async Task<DashboardModel> SaveCarbonReductionMaster(DashboardModel model)
        {
            return await _repository.SaveCarbonReductionMaster(model);
        }
        #endregion
    }
}

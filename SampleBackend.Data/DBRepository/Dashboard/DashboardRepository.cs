using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SampleBackend.Common.Helper;
using System.Data;
using SampleBackend.Model.Model.Model;
using SampleBackend.Model.Model;

namespace SampleBackend.Data.DBRepository.Dashboard
{
    public class DashboardRepository(IConfiguration config, IOptions<ConnectionStrings> connectionString) : BaseRepository(connectionString), IDashboardRepository
    {
        #region Fields
        private readonly IConfiguration _config = config;
        #endregion

        #region Post
        public async Task<DashboardModel> GetDashboardMasterDetail(UserModel model)
        {
            try
            {
                var param = new DynamicParameters();
                if (model.UserId > 0)
                {
                    param.Add("@UserId", model.UserId);
                }
                else
                {
                    param.Add("@UserId", model.LoggedInUserId);
                }
                param.Add("@BranchId", model.BranchId);
                return await QueryFirstOrDefaultAsync<DashboardModel>(StoreProcedure.GetDashboardMasterDetail, param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<GRSMaterialPurchaseDetailModel>> GetGRSMaterialPurchasesDetail(UserModel model)
        {
            try
            {
                var param = new DynamicParameters();
                if (model.UserId > 0)
                {
                    param.Add("@UserId", model.UserId);
                }
                else
                {
                    param.Add("@UserId", model.LoggedInUserId);
                }
                param.Add("@BranchId", model.BranchId);
                var data = await QueryAsync<GRSMaterialPurchaseDetailModel>(StoreProcedure.GetGRSMaterialPurchasesDetail, param, commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CarbonReductionModel>> GetCarbonReductionChartMasterDetail(UserModel model)
        {
            try
            {
                var param = new DynamicParameters();
                if (model.UserId > 0)
                {
                    param.Add("@UserId", model.UserId);
                }
                else
                {
                    param.Add("@UserId", model.LoggedInUserId);
                }
                param.Add("@CreatedBy", model.LoggedInUserId);
                param.Add("@BranchId", model.BranchId);
                var data = await QueryAsync<CarbonReductionModel>(StoreProcedure.GetCarbonReductionChartMasterDetail, param, commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<long> SaveDashboardMaster(DashboardModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@BranchId", model.BranchId);
                param.Add("@DashboardId", model.DashboardId);
                param.Add("@CarbonEmission", model.CarbonEmission);
                param.Add("@GarmentPurchased", model.GarmentPurchased);
                param.Add("@LifeRecycledKG", model.LifeRecycledKG);
                param.Add("@ParcelDelivered", model.ParcelDelivered);
                param.Add("@EVPercentage", model.EVPercentage);
                param.Add("@SavedCO2", model.SavedCO2);
                param.Add("@IsActive", model.IsActive);
                param.Add("@CreatedBy", model.LoggedInUserId);
                return await QueryFirstOrDefaultAsync<long>(StoreProcedure.SaveCustomerDashboardDetail, param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DashboardModel> SaveGRSMaterialsMaster(DashboardModel model)
        {
            try
            {
                var dataTable = new DataTable();
                dataTable.Columns.Add("MaterialNumber", typeof(int));
                dataTable.Columns.Add("Value", typeof(decimal));
                dataTable.Columns.Add("IsActive", typeof(bool));

                foreach (var item in model.grsMaterials)
                {
                    dataTable.Rows.Add(item.MaterialNumber, item.Value, true); // Set IsActive to true or adjust as per your model
                }

                var param = new DynamicParameters();
                param.Add("@CreatedBy", model.LoggedInUserId);
                param.Add("@BranchId", model.BranchId);
                param.Add("@GRSMaterialPurchasesDetailTbl", dataTable.AsTableValuedParameter("tbl_GRSMaterialPurchases"));

                var result = await QueryFirstOrDefaultAsync<DashboardModel>(StoreProcedure.SaveGRSMaterialPurchasesDetail, param, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DashboardModel> SaveCarbonReductionMaster(DashboardModel model)
        {
            try
            {
                var dataTable = new DataTable();
                dataTable.Columns.Add("Month", typeof(string));
                dataTable.Columns.Add("Value", typeof(decimal));
                dataTable.Columns.Add("SortOrder", typeof(long));

                foreach (var item in model.carbonReduction)
                {
                    dataTable.Rows.Add(item.Month, item.Value, item.SortOrder);
                }

                var param = new DynamicParameters();
                param.Add("@CreatedBy", model.LoggedInUserId);
                param.Add("@BranchId", model.BranchId);
                param.Add("@CarbonReductionDetailTbl", dataTable.AsTableValuedParameter("tbl_CarbonReduction"));

                var result = await QueryFirstOrDefaultAsync<DashboardModel>(StoreProcedure.SaveCarbonReductionDetail, param, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}

using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SampleBackend.Common.Helper;
using SampleBackend.Model.Model;
using SampleBackend.Model.Model.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBackend.Data.DBRepository.UserProfile
{
    public class UserProfileRepository(IConfiguration config, IOptions<ConnectionStrings> connectionString) : BaseRepository(connectionString), IUserProfileRepository
    {
        #region Fields
        private readonly IConfiguration _config = config;
        #endregion

        #region GetCityList
        public async Task<List<CityModel>> GetCityListById(int StateId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@StateId", StateId);

                var data = await QueryAsync<CityModel>(StoreProcedure.GetCityList, param, commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region GetCountryList
        public async Task<List<CountryModel>> GetCountryList()
        {
           
            try
            {
                var data = await QueryAsync<CountryModel>(StoreProcedure.GetCountryList, commandType: CommandType.StoredProcedure);
                return data.ToList();

            }
            catch (Exception ex)
            { 

                throw ex;
            }
        }
        #endregion

        #region GetLanguageList
        public async Task<List<LanguageModel>> GetLanguageList()
        {
            try
            {
                var data = await QueryAsync<LanguageModel>(StoreProcedure.GetLanguageList, commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        #endregion

        #region GetStateList
        public async Task<List<StateModel>> GetStateListById(int CountryId)
        {
            

            try
            {
                var param = new DynamicParameters();
                param.Add("@CountryId", CountryId);

                var data = await QueryAsync<StateModel>(StoreProcedure.GetStateList, param, commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region GetDataByUserId
        public async Task<UserDetailModel> GetUserDetailById(long UserId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", UserId);

                UserDetailModel data = await QueryFirstOrDefaultAsync<UserDetailModel>(StoreProcedure.GetUserDetailById, param, commandType: CommandType.StoredProcedure);
                
                var param1 = new DynamicParameters();
                param1.Add("@UserDetailId", data.UserDetailId);
                data.Address = (await QueryAsync<AddressModel>(StoreProcedure.GetAddressListById, param1, commandType: CommandType.StoredProcedure)).ToList();
                return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region AddUpdate - Address
        public async Task<AddressResponse> SaveUserProfileAddress(List<AddressModel>? addresses)
        {
            var dt = new DataTable();
            dt.Columns.Add("AddressId", typeof(int));
            dt.Columns.Add("Address1", typeof(string));
            dt.Columns.Add("Address2", typeof(string));
            dt.Columns.Add("CountryId", typeof(int));
            dt.Columns.Add("StateId", typeof(int));
            dt.Columns.Add("CityId", typeof(int));
            dt.Columns.Add("UserDetailId", typeof(long));
            dt.Columns.Add("IsDelete", typeof(bool));

            foreach (var address in addresses)
            {
                dt.Rows.Add(address.AddressId.HasValue ? (object)address.AddressId : DBNull.Value, address.Address1, address.Address2, address.CountryId, address.StateId, address.CityId, address.UserDetailId,address.IsDelete);
            }

            var param = new DynamicParameters();
            param.Add("@AddressTable", dt.AsTableValuedParameter("dbo.AddressType"));

            AddressResponse addressModel = await QueryFirstOrDefaultAsync<AddressResponse>(StoreProcedure.UserProfileAddress_AddUpdate, param, commandType: CommandType.StoredProcedure);
            return addressModel;
        }
        #endregion

        #region AddUpdate - UserProfile
        public async Task<UserDetailModel> SaveUserProfileDetail(UserDetailModel model)
            {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserDetailId", model.UserDetailId);
                param.Add("@FirstName", model.FirstName);
                param.Add("@LastName", model.LastName);
                param.Add("@Email", model.Email);
                param.Add("@PhoneNo", model.PhoneNo);
                param.Add("@UserPhoto", model.UserPhoto);
                param.Add("@Gender", model.Gender);
                param.Add("@DOB", model.DOB);
                param.Add("@Languages", model.Languages);
                param.Add("@UserId", model.LoggedInUserId);
                param.Add("@CreatedBy", model.LoggedInUserId);

                UserDetailModel userDetailModel =  await QueryFirstOrDefaultAsync<UserDetailModel>(StoreProcedure.UserProfileDetail_AddUpdate, param, commandType: CommandType.StoredProcedure);

                if(userDetailModel != null && userDetailModel.UserDetailId > 0)
                {
                    model?.Address?.ForEach(address => address.UserDetailId = (long)userDetailModel.UserDetailId);
                    AddressResponse addressModel = await SaveUserProfileAddress(model?.Address);
                    if (addressModel != null)
                    {
                        Console.WriteLine(addressModel.Success);
                    }
                    else
                    {
                        Console.WriteLine(addressModel.ErrorMessage);
                    }

                }
                return userDetailModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

    }
}

namespace SampleBackend.Common.Helper
{
    public static class StoreProcedure
    {
        #region Login
        public const string LoginUser = "SP_UserMaster_Login";
        public const string LoginWithoutPassword = "SP_GetPasswordByTemporaryPassword";
        public const string UserLogInTrack_AddUpdate = "SP_UserLogInTrack_AddUpdate";
        public const string UserMaster_ValidateEmail = "SP_UserMaster_ValidateEmail";
        #endregion

        #region User
        public const string UserGetList = "SP_UserMaster_GetList";
        public const string UserDetailsGetList = "SP_UserDetailsMaster_GetList";
        public const string UserDropDownList = "SP_UserMaster_DropDown";
        public const string BranchDropDownList = "SP_BranchMaster_DropDown";
        public const string CompanyDropDownList = "SP_CompanyMaster_DropDown";
        public const string SaveUser = "SP_UserMaster_AddUpdate";
        public const string DeleteUser = "SP_UserMaster_Delete";
        public const string DeleteAllUser = "SP_UserMaster_DeleteAll";
        public const string DeleteUserMultiple = "SP_UserMaster_DeleteMultiple";
        public const string UserMaster_UpdatePassword = "SP_UserMaster_UpdatePassword";
        public const string UserMaster_ValidateResetPassword = "SP_UserMaster_ValidateResetPassword";
        public const string UserMaster_ValidateTFACode = "SP_UserMaster_ValidateTFACode";
        #endregion

        #region Dashboard
        public const string SaveCustomerDashboardDetail = "SP_CustomerDashboardMaster_AddUpdate";
        public const string SaveGRSMaterialPurchasesDetail = "SP_GRSMaterialPurchasesDetail_AddUpdate";
        public const string SaveCarbonReductionDetail = "SP_CarbonReductionChartMaster_AddUpdate";
        public const string GetDashboardMasterDetail = "SP_CustomerDashboardMaster_GetDetails";
        public const string GetGRSMaterialPurchasesDetail = "SP_GRSMaterialPurchasesDetail_GetDetails";
        public const string GetCarbonReductionChartMasterDetail = "SP_CarbonReductionChartMaster_GetDetails";
        #endregion

        #region Roles
        public const string GetRoleList = "SP_RoleMaster_GetList";
        #endregion

        #region RoleRights
        public const string RoleRightsGetById = "SP_RoleRights_GetByRoleId";
        public const string RoleRightsAddUpdate = "SP_RoleRights_Add";
        #endregion

        public const string CountryList= "getCountryList";
        public const string StateList = "getStateList";
        public const string CityList = "getCityList";

    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using VyacheslavsMegaServer.Data.Repositories;

namespace VyacheslavsMegaServer.Areas.Admin.Models
{
    public class UserManagerViewModel
    {
        #region RolesLists
        [ValidateNever]
        public List<SelectListItem> RolesSelectListToFilter { get; set; }

        [ValidateNever]
        public List<SelectListItem> RolesSelectListToSet { get; set; }
        #endregion

        private List<UserViewModel> _users;

        public List<UserViewModel> Users
        {
            get
            {
                if (_users is null)
                    return new List<UserViewModel>();

                if (!UsersFilters.Any())
                    return _users;
                
                return _users.Where(ResultFilter).ToList();
            }

            set 
            {
                _users = value;
            }
        }

        private string _nameSearchQuery;

        [Display(Name = "Поиск по имени")]
        [ValidateNever]
        public string NameSearchQuery 
        { 
            get
            {
                return _nameSearchQuery?.Trim();
            }
            set 
            { 
                _nameSearchQuery = value; 
                if (!string.IsNullOrEmpty(value))
                {
                    UsersFilters.Add(FilterByName);
                }
                else
                {
                    UsersFilters.Remove(FilterByName);
                }
            }
        }

        private string _emailSearchQuery;

        [Display(Name = "Поиск по email")]
        [ValidateNever]
        public string EmailSearchQuery 
        { 
            get
            {
                return _emailSearchQuery?.Trim();
            }
            set
            {
                _emailSearchQuery = value;
                if (!string.IsNullOrEmpty(value))
                {
                    UsersFilters.Add(FilterByEmail);
                }
                else
                {
                    UsersFilters.Remove(FilterByEmail);
                }
            }
        }

        private string _roleSearchQuery;

        [Display(Name = "Поиск по роли")]
        public string RoleSearchQuery
        {
            get
            {
                return _roleSearchQuery?.Trim();
            }
            set
            {
                _roleSearchQuery = value;
                if (!string.IsNullOrEmpty(value) && _roleSearchQuery != IdentityRepository.NO_ROLE)
                {
                    UsersFilters.Add(FilterByRole);
                }
                else
                {
                    UsersFilters.Remove(FilterByRole);
                }
            }
        }

        #region Filters
        private List<Func<UserViewModel, bool>> UsersFilters = new List<Func<UserViewModel, bool>>();

        private Func<UserViewModel, bool> ResultFilter => UsersFilters.Aggregate((c, n) => u => c(u) && n(u));

        private bool FilterByName(UserViewModel model)
        {
            return model?.IdentityUserInfo.UserName?.Contains(NameSearchQuery) ?? false;
        }

        private bool FilterByEmail(UserViewModel model)
        {
            return model?.IdentityUserInfo.Email?.Contains(EmailSearchQuery) ?? false;
        }

        private bool FilterByRole(UserViewModel model)
        {
            return model?.Role.Equals(RoleSearchQuery, StringComparison.OrdinalIgnoreCase) ?? false;
        }
        #endregion
    }
}

using Site.DataAccess.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.DataAccess.Interface
{
	public interface IUserAuth
	{
        string CheckLogin(Login_VM obj);
        Portal_User GetUserData(Login_VM obj);

        string CheckEmailExist(string email);
        string SaveUserData(Save_PortalUser obj);
        IEnumerable<Portal_User> GetUserList();
        Portal_User GetUserDetail(int id);
        Portal_User GetStaffDetailsById(int id);
        string UpdateStaffDetail(Portal_User obj);
        string VerifyUserDetail(int id);
        string DisableUserDetail(int id);
        string SaveStaffData(AddStaff obj);


        IEnumerable<Portal_User> GetMemberList();
        Portal_User GetMemberDetail(int id);

        string VerifyMemberDetail(int id);
        string DisableMemberDetail(int id);
        string SaveMemberData(AddMember_VM obj);

        IEnumerable<Portal_User> GetStaffList();
    }
}

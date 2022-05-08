using FastEndpoints.Security;

namespace Articles.Api.Infrastructure.Auth;

public class ClaimPermissions : Permissions
{

    //ADMIN PERMISSIONS
    public const string Article_Moderate = "100";
    public const string Article_Delete = "101";
    public const string Article_Get_Pending_List = "102";
    public const string Article_Update = "103";
    public const string Author_Update_Profile = "104";

    //AUTHOR PERMISSIONS
    public const string Author_Get_Own_List = "200";
    public const string Author_Save_Own = "201";
    public const string Author_Update_Own_Profile = "202";

    //USER PERMISSIONS
    public const string User_Reads = "301";
}

public class RolePermissions
{
    public const string Admin = "Admin";
    public const string Author = "Author";
    public const string User = "User";
}
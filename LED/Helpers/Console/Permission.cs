#region References

using System.Security.Principal;

#endregion

namespace LED.Helpers.Console
{
    class Permission
    {
        #region Check Administrator

        public static bool Administrator()
        {
            using (WindowsIdentity Identity = WindowsIdentity.GetCurrent())
            {
                if (!new WindowsPrincipal(Identity).IsInRole(WindowsBuiltInRole.Administrator))
                {
                    return false;
                }
                else return true;
            }
        }

        #endregion
    }
}

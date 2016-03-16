using Improbable.Auth;

namespace Improbable.Unity
{
    public static class User
    {
        public static string GetDisplayNameFromLoginToken(string loginToken)
        {
            return new ImprobableToken(loginToken).DisplayName;
        }
    }
}

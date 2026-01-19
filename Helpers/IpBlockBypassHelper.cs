namespace Authentication_Servie.Helpers
{
    public static class IpBlockBypassHelper
    {
        public static bool IsInternalEndpoint(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower() ?? "";

            return path.StartsWith("/api/security") ||
                   path.StartsWith("/swagger") ||
                   path.StartsWith("/api/admin");
        }

        public static bool IsInternalServiceIp(string ip)
        {
            return ip == "::1" || ip == "127.0.0.1";
        }
    }

}

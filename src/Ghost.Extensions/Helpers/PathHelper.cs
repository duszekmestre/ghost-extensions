namespace Ghost.Extensions.Helpers
{
    using System;
    using System.IO;

    public static class PathHelper
    {
        public static string RootPath
        {
            get
            {                
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        public static string ResolveRelativePath(string relativePath)//, bool useBinFolder = false)
        {
            return Path.GetFullPath(Path.Combine(RootPath, relativePath));
        }
    }
}

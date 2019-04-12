using System;
using System.IO;

namespace AssessmentDomain.Common
{
    public static class StringUtils
    {
        public static string GetRandomBlobName(this string filename) => $"{DateTime.Now.Ticks:10}_{Guid.NewGuid():N}{Path.GetExtension(filename)}";
    }
}
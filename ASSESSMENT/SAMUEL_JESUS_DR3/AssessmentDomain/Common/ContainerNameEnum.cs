using System.ComponentModel;

namespace AssessmentDomain.Common
{
    public enum ContainerName : byte
    {
        [Description("general-files")]
        General = 0,
        [Description("profile-pictures")]
        Profile = 1,
        [Description("country-pictures")]
        CountryFlag = 2,
        [Description("stateflag-pictures")]
        StateFlag = 3
    }
}
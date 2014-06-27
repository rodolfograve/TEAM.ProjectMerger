// Guids.cs
// MUST match guids.h
using System;

namespace TEAM.TEAM_ProjectMerger
{
    static class GuidList
    {
        public const string guidTEAM_ProjectMergerPkgString = "124be291-dc59-4cfa-b045-421d346fdc9c";
        public const string guidTEAM_ProjectMergerCmdSetString = "9a7a0693-5447-4f7e-8b1e-df1799a309ac";

        public static readonly Guid guidTEAM_ProjectMergerCmdSet = new Guid(guidTEAM_ProjectMergerCmdSetString);
    };
}
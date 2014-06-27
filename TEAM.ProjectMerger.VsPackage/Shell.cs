using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TEAM.TEAM_ProjectMerger
{
   public class Shell
   {

      public Shell(IVsUIShell uiShell)
      {
         UiShell = uiShell;
      }

      private readonly IVsUIShell UiShell;

      public void MessageBox(string message)
      {
         Guid clsid = Guid.Empty;
         int result;
         ErrorHandler.ThrowOnFailure(UiShell.ShowMessageBox(
                    0,
                    ref clsid,
                    "TEAM.ProjectMerger",
                    "Inside " + ToString() + ".MenuItemCallback()",
                    string.Empty,
                    0,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
                    OLEMSGICON.OLEMSGICON_INFO,
                    0,        // false
                    out result));
      }

   }
}

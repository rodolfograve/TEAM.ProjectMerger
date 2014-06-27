using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TEAM.TEAM_ProjectMerger
{
   public class OutputWindow
   {

      public static OutputWindow Create(IVsOutputWindow outputWindowService)
      {
         IVsOutputWindowPane outputWindowPane;
         var generalPaneId = VSConstants.OutputWindowPaneGuid.GeneralPane_guid;
         outputWindowService.CreatePane(generalPaneId, "General", 1, 0);
         outputWindowService.GetPane(ref generalPaneId, out outputWindowPane);
         return new OutputWindow(outputWindowPane);
      }

      public OutputWindow(IVsOutputWindowPane outputWindowPane)
      {
         OutputWindowPane = outputWindowPane;
      }

      private readonly IVsOutputWindowPane OutputWindowPane;

      public void WriteLine(string message)
      {
         OutputWindowPane.OutputString(message + Environment.NewLine);
      }

   }
}

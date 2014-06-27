using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;

namespace TEAM.TEAM_ProjectMerger
{
   /// <summary>
   /// This is the class that implements the package exposed by this assembly.
   ///
   /// The minimum requirement for a class to be considered a valid package for Visual Studio is to implement the IVsPackage interface and register itself with the shell.
   /// This package uses the helper classes defined inside the Managed Package Framework (MPF) to do it: it derives from the Package class that provides the implementation of the 
   /// IVsPackage interface and uses the registration attributes defined in the framework to register itself and its components with the shell.
   /// </summary>
   // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is a package.
   [PackageRegistration(UseManagedResourcesOnly = true)]
   // This attribute is used to register the information needed to show this package in the Help/About dialog of Visual Studio.
   [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
   // This attribute is needed to let the shell know that this package exposes some menus.
   [ProvideMenuResource("Menus.ctmenu", 1)]
   [Guid(GuidList.guidTEAM_ProjectMergerPkgString)]
   public sealed class TEAM_ProjectMergerPackage : Package
   {
      /// <summary>
      /// Default constructor of the package.
      /// Inside this method you can place any initialization code that does not require any Visual Studio service because at this point the package object is created but 
      /// not sited yet inside Visual Studio environment. The place to do all the other initialization is the Initialize method.
      /// </summary>
      public TEAM_ProjectMergerPackage()
      {
         Debug.WriteLine("Entering constructor for: " + ToString());
      }

      private OutputWindow OutputWindow;
      private Shell Shell;
      private MonitorSelection MonitorSelection;

      /// <summary>
      /// Initialization of the package; this method is called right after the package is sited, so this is the place where you can put all the initialization code that rely on services provided by VisualStudio.
      /// </summary>
      protected override void Initialize()
      {
         Debug.WriteLine("Entering Initialize() of: " + ToString());
         base.Initialize();

         // Add our command handlers for menu (commands must exist in the .vsct file)
         var menuCommandService = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
         if (null != menuCommandService)
         {
            // Create the command for the menu item.
            CommandID menuCommandID = new CommandID(GuidList.guidTEAM_ProjectMergerCmdSet, (int)PkgCmdIDList.teamMergeProjects);
            MenuCommand menuItem = new MenuCommand(MenuItemCallback, menuCommandID);
            menuCommandService.AddCommand(menuItem);
         }

         OutputWindow = OutputWindow.Create(GetService(typeof(SVsOutputWindow)) as IVsOutputWindow);
         Shell = new Shell((IVsUIShell)GetService(typeof(SVsUIShell)));
         MonitorSelection = new MonitorSelection(((IVsMonitorSelection)Package.GetGlobalService(typeof(SVsShellMonitorSelection))));
      }

      /// <summary>
      /// This function is the callback used to execute a command when the a menu item is clicked. See the Initialize method to see how the menu item is associated to this function using
      /// the OleMenuCommandService service and the MenuCommand class.
      /// </summary>
      private void MenuItemCallback(object sender, EventArgs e)
      {
         OutputWindow.WriteLine("TEAM.MergeProjects invoked...");

         var selectedProjects = MonitorSelection.GetSelectedProjects();

         OutputWindow.WriteLine("TEAM.MergeProjects finished.");
      }

   }
}

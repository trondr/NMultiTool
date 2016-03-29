using System.Collections;
using System.ComponentModel;

namespace NMultiTool.Module
{
    [RunInstaller(true)]
    public partial class CustomInstaller : System.Configuration.Install.Installer
    {
        public CustomInstaller()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            //Example: Adding a command to windows explorer contect menu
            //this.Context.LogMessage("Adding NMultiTool to File Explorer context menu...");
            //new WindowsExplorerContextMenuInstaller().Install("NMultiTool", "Create Something...", Assembly.GetExecutingAssembly().Location, "CreateSomething /exampleParameter=\"%1\"");
            //this.Context.LogMessage("Finnished adding NMultiTool to File Explorer context menu.");
            
            base.Install(stateSaver);
        }

        public override void Uninstall(IDictionary savedState)
        {
            //Example: Removing previously installed command from windows explorer contect menu
            //this.Context.LogMessage("Removing NMultiTool from File Explorer context menu...");
            //new WindowsExplorerContextMenuInstaller().UnInstall("NMultiTool");
            //this.Context.LogMessage("Finished removing NMultiTool from File Explorer context menu.");
            
            base.Uninstall(savedState);
        }        
    }
}

using Oqtane.Models;
using Oqtane.Modules;

namespace Emp.form
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "form",
            Description = "form",
            Version = "1.0.0",
            ServerManagerType = "Emp.form.Manager.formManager, Emp.form.Server.Oqtane",
            ReleaseVersions = "1.0.0",
            Dependencies = "Emp.form.Shared.Oqtane"
        };
    }
}

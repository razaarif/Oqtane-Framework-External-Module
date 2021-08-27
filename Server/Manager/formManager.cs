using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Oqtane.Modules;
using Oqtane.Models;
using Oqtane.Infrastructure;
using Oqtane.Repository;
using Emp.form.Models;
using Emp.form.Repository;

namespace Emp.form.Manager
{
    public class formManager : IInstallable, IPortable
    {
        private IformRepository _formRepository;
        private ISqlRepository _sql;

        public formManager(IformRepository formRepository, ISqlRepository sql)
        {
            _formRepository = formRepository;
            _sql = sql;
        }

        public bool Install(Tenant tenant, string version)
        {
            return _sql.ExecuteScript(tenant, GetType().Assembly, "Emp.form." + version + ".sql");
        }

        public bool Uninstall(Tenant tenant)
        {
            return _sql.ExecuteScript(tenant, GetType().Assembly, "Emp.form.Uninstall.sql");
        }

        public string ExportModule(Module module)
        {
            string content = "";
            List<Models.form> forms = _formRepository.Getforms(module.ModuleId).ToList();
            if (forms != null)
            {
                content = JsonSerializer.Serialize(forms);
            }
            return content;
        }

        public void ImportModule(Module module, string content, string version)
        {
            List<Models.form> forms = null;
            if (!string.IsNullOrEmpty(content))
            {
                forms = JsonSerializer.Deserialize<List<Models.form>>(content);
            }
            if (forms != null)
            {
                foreach(var form in forms)
                {
                    _formRepository.Addform(new Models.form { ModuleId = module.ModuleId, Name = form.Name });
                }
            }
        }
    }
}
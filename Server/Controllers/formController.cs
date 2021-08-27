using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Emp.form.Models;
using Emp.form.Repository;

namespace Emp.form.Controllers
{
    [Route(ControllerRoutes.Default)]
    public class formController : Controller
    {
        private readonly IformRepository _formRepository;
        private readonly ILogManager _logger;
        protected int _entityId = -1;

        public formController(IformRepository formRepository, ILogManager logger, IHttpContextAccessor accessor)
        {
            _formRepository = formRepository;
            _logger = logger;

            if (accessor.HttpContext.Request.Query.ContainsKey("entityid"))
            {
                _entityId = int.Parse(accessor.HttpContext.Request.Query["entityid"]);
            }
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public IEnumerable<Models.form> Get(string moduleid)
        {
            return _formRepository.Getforms(int.Parse(moduleid));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public Models.form Get(int id)
        {
            Models.form form = _formRepository.Getform(id);
            if (form != null && form.ModuleId != _entityId)
            {
                form = null;
            }
            return form;
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public Models.form Post([FromBody] Models.form form)
        {
            if (ModelState.IsValid && form.ModuleId == _entityId)
            {
                form = _formRepository.Addform(form);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "form Added {form}", form);
            }
            return form;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public Models.form Put(int id, [FromBody] Models.form form)
        {
            if (ModelState.IsValid && form.ModuleId == _entityId)
            {
                form = _formRepository.Updateform(form);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "form Updated {form}", form);
            }
            return form;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public void Delete(int id)
        {
            Models.form form = _formRepository.Getform(id);
            if (form != null && form.ModuleId == _entityId)
            {
                _formRepository.Deleteform(id);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "form Deleted {formId}", id);
            }
        }
    }
}

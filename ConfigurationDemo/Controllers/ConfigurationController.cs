using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ConfigurationDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigurationController(IConfiguration configuration) : Controller
    {
        [HttpGet]
        [Route("my-key")]
        public ActionResult GetMyKey()
        {
            var myKey = configuration["MyKey"];
            return Ok(myKey);
        }
        [HttpGet]
        [Route("database-config")]
        public ActionResult getDatabaseConfiguration()
        {
            var type = configuration["Database:Type"];
            var connectionstring = configuration["Database:ConnectionString"];
            return Ok(new {Type = type, Connectionstring = connectionstring});
        }

        [HttpGet]
        [Route("database-config-with-bind")]
        public ActionResult GetDatabaseConfigurationWithBind()
        {
            var databaseOption = new DatabaseOption();
            configuration.GetSection(DatabaseOption.SectionName).Bind(databaseOption);
            return Ok(new {databaseOption.Type, databaseOption.ConnectionString});
        }
        [HttpGet]
        [Route("database-config-with-generic-type")]
        public ActionResult GetDatabaseConfigurationWithGenericType()
        {
            var databaseOption = configuration.GetSection(DatabaseOption.SectionName).Get<DatabaseOption>();
            return Ok(new { databaseOption.Type, databaseOption.ConnectionString });
        }
        [HttpGet]
        [Route("database-config-with-ioptions")]
        public ActionResult GetDatabaseConfigWithIOptions([FromServices] IOptions<DatabaseOption> options)
        {
            var databaseOption = options.Value;
            return Ok(new { databaseOption.Type, databaseOption.ConnectionString });
        }
        [HttpGet]
        [Route("database-config-with-ioptions-snapshot")]
        public ActionResult GetDatabaseConfigWithIOptionsSnapshot([FromServices] IOptionsSnapshot<DatabaseOption> options)
        {
            var databaseOption = options.Value;
            return Ok(new { databaseOption.Type, databaseOption.ConnectionString });
        }
        [HttpGet]
        [Route("database-config-with-ioptions-monitor")]
        public ActionResult GetDatabaseConfigWithIOptionsMonitor([FromServices] IOptionsMonitor<DatabaseOption> options)
        {
            var databaseOption = options.CurrentValue;
            return Ok(new { databaseOption.Type, databaseOption.ConnectionString });
        }
        [HttpGet]
        [Route("database-config-with-named-options")]
        public ActionResult GetDatabaseConfigWithNamedOptions([FromServices] IOptionsSnapshot<DatabaseOption> options)
        {
            var systemDatabaseOption = options.Get(DatabaseOption.SystemDatabaseSectionName);
            var businessDatabaseOption = options.Get(DatabaseOption.BusinessDatabaseSectionName);
            return Ok(new {SystemDatabaseOption = systemDatabaseOption, BusinessDatabaseOption = businessDatabaseOption});
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}

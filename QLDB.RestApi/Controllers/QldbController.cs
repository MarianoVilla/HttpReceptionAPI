using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AmazonQLDB.ServiceCore;
using QLDB.Interface;
using Microsoft.Extensions.Configuration;

namespace QLDB.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QldbController : ControllerBase
    {
        private string ApiKey;
        private readonly ILogger<QldbController> _logger;
        private readonly IConfiguration _configuration;
        ConnectionService Connection = new ConnectionService();

        public QldbController(ILogger<QldbController> logger, IConfiguration configuration)
        {
            this._logger = logger;
            this._configuration = configuration;
            this.ApiKey = _configuration.GetValue<string>("ApiSettings:ApiKey");
        }

        [HttpPost]
        [Route("write")]
        public IActionResult Write([FromBody] QldbRequestModel QldbRequest)
        {
            if (QldbRequest.ApiKey != ApiKey)
                return Unauthorized();

            SetupGlobal(QldbRequest);
            var Result = Connection.WriteData(QldbRequest.TableName, QldbRequest.Statement);
            return new JsonResult(Result);
        }

        [HttpPost]
        [Route("read")]
        public IActionResult Read([FromBody] QldbRequestModel QldbRequest)
        {
            if (QldbRequest.ApiKey != ApiKey)
                return Unauthorized();


            SetupGlobal(QldbRequest);
            var Result = Connection.ReadData(QldbRequest.Statement);
            return new JsonResult(Result);
        }

        void SetupGlobal(QldbRequestModel Request)
        {
            Connection = new ConnectionService();
            Global.LedgerName = Request.LedgerName;
            Global.AccessKey = Request.AccessKey;
            Global.SecretKey = Request.SecretKey;
        }


    }
}
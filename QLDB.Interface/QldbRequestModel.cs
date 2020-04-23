using System;
using System.Collections.Generic;
using System.Text;

namespace QLDB.Interface
{
    public class QldbRequestModel
    {
        public string LedgerName { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string TableName { get; set; }
        public string Statement { get; set; }
        public string ApiKey { get; set; }
    }
}

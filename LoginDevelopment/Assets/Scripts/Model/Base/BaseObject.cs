using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Model.Base
{
    public class BaseObject
    {

        public int DataVersion { get; set; }
        public string TimestampModified { get; set; }
        public string TimestampCreation { get; set; }
        public string TimestampExpiration { get; set; }


        public string AsJson()
        {
            return ThirdParty.Json.LitJson.JsonMapper.ToJson(this);
        }
        
    }
}

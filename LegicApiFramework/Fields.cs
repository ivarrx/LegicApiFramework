using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegicApiFramework
{
    public class Fields
    {
        private string _apikey;  // the name field
        public string ApiKey    // the Name property
        {
            get => _apikey;
            set => _apikey = value;
        }

        private int _projectKey;  // the name field

        public int ProjectKey    // the Name property
        {
            get => _projectKey;
            set => _projectKey = value;
        }
        private int _mobileAppId;  // the name field

        public int MobileAppId    // the Name property
        {
            get => _mobileAppId;
            set => _mobileAppId = value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LegicApiFramework
{
    interface IRequests
    {
        HttpResponseMessage listProsivisionedFiles(string apiKey, int projectId, long phoneNumber);
        HttpResponseMessage removeFileFroMobileDevice(string apiKey,int mobileAppId,string fileId,long phoneNumber);
        HttpResponseMessage writeVcpToMobileDevice(string apiKey,int projectKey,int mobileAppId,string fileId,string vcpName,string[] rfInterfaces,long phoneNumber);
        HttpResponseMessage writeDataToMobile(string apiKey, int projectId, long phoneNumber, int mobileAppId, string fileId, string fileName, string[] customParameterName = null,string[] customParamteterValue = null,string[] subFileId = null,string[] subFileData = null);
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LegicApiFramework
{
   public class LegicApiRequests : IRequests
    {
        private async Task<HttpResponseMessage> PostRequest(string apiKey,string JsonBody,string requestUri)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://integration.legicconnect.com/public/v7/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("APIKEY", apiKey);
                HttpContent content = new StringContent(JsonBody, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(requestUri, content);
                var result = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Succesfull request");
                }
                return response;
            }

        }

        private string HexToBase64(string strInput)
        {
            try
            {
                var bytes = new byte[strInput.Length / 2];
                for (var i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = Convert.ToByte(strInput.Substring(i * 2, 2), 16);
                }
                return Convert.ToBase64String(bytes);
            }
            catch (Exception)
            {
                return "-1";
            }
        }
        public HttpResponseMessage listProsivisionedFiles(string apiKey, int projectId, long phoneNumber)
        {

            string jsonBody = string.Format(@"{{""listProvisionedFileRequest"":{{""projectId"":""{0}"",""publicRegistrationId"":[""phone#+{1}""]}}}}",projectId,phoneNumber);
            string requestUri = "listProvisionedFile";
            var result = PostRequest(apiKey, jsonBody, requestUri).Result;
            return result;
        }

        public HttpResponseMessage removeFileFroMobileDevice(string apiKey, int mobileAppId, string fileId, long phoneNumber)
        {
            var fileIdBase64 = HexToBase64(fileId);
            string jsonBody = string.Format(@"{{""removeFileFromMobileDeviceRequest"":{{""publicRegistrationId"":""{0}"",""mobileAppId"":""{1}"",""fileId"":""{2}""}}}}",phoneNumber,mobileAppId,fileIdBase64);
            string requestUri = "RemoveFileFromMobileDevice";
            var result = PostRequest(apiKey, jsonBody, requestUri).Result;
            return result;
        }
    

        public HttpResponseMessage writeVcpToMobileDevice(string apiKey, int projectKey, int mobileAppId, string fileId, string vcpName, string[] rfInterfaces,long phoneNumber)
        {
            var fileIdBase64 = HexToBase64(fileId);
            string jsonBody = string.Format(@"{{""writeVcpToMobileDeviceRequest"":{{""publicRegistrationId"":""{0}"",""mobileAppId"":""{1}"",""fileId"":""{2}"",""vcpName"":""{3}"",""rfInterfaces"":[""ble""]}}}}",phoneNumber,mobileAppId,fileIdBase64,vcpName,rfInterfaces);
            string requestUri = "writeVcpToMobileDevice";
            var result = PostRequest(apiKey, jsonBody, requestUri).Result;
            return result;
        }


        public HttpResponseMessage writeDataToMobile(string apiKey, int projectId, long phoneNumber, int mobileAppId, string fileId, string fileName, string[] customParameterName = null, string[] customParamteterValue = null, string[] subFileId = null, string[] subFileData = null)
        {
            var fileIdBase64 = HexToBase64(fileId);
            string jsonBody = string.Format(@"{{""writeDataToMobileDeviceRequest"":{{""publicRegistrationId"":""phone#+{0}"",""mobileAppId"":""{1}"",""fileId"":""{2}"",""fileDefinitionName"":""{3}""}}}}", phoneNumber,mobileAppId,fileIdBase64,fileName);
            StringBuilder sb = new StringBuilder(jsonBody);
            if (customParameterName  != null)
            {
                for (int i = 0; i < customParameterName.Length; i++)
                {
                    if (i == 0)
                    {
                        var metaDataString = string.Format(@",""metadata"":{{""meta"":[{{""customParamName"":""{0}"",""value"":{{""stringValue"":""{1}""}}}}]}}",customParameterName[i],customParamteterValue[i]);
                        jsonBody = sb.Insert((jsonBody.Length - 2), metaDataString).ToString();
                        Console.WriteLine(sb);
                        
                    }
                    else
                    {
                        var metaDataString = string.Format(@"}},{{""customParamName"":""{0}"",""value"":{{""stringValue"":""{1}""}}",customParameterName[i],customParamteterValue[i]);
                        jsonBody = sb.Insert((jsonBody.Length - 5), metaDataString).ToString();
                        Console.WriteLine(jsonBody);
                    }
                }
            }
            if(subFileId != null)
            {
                for (int i = 0; i < subFileId.Length ; i++)
                {
                    if (i == 0)
                    {
                        var subFileIdHex = HexToBase64(subFileId[i]);
                        var subFileDataHex = HexToBase64(subFileData[i]);

                        var metaDataString = string.Format(@",""subfiles"":[{{""fileId"":""{0}"",""data"":""{1}""}}]", subFileIdHex, subFileDataHex);
                        jsonBody = sb.Insert((jsonBody.Length - 2), metaDataString).ToString();
                        Console.WriteLine(sb);
                    }
                    else
                    {
                        var subFileIdHex = HexToBase64(subFileId[i]);
                        var subFileDataHex = HexToBase64(subFileData[i]);

                        var metaDataString = string.Format(@"}},{{""fileId"":""{0}"",""data"":""stringValue"":""{1}""}}", subFileIdHex, subFileDataHex);
                        jsonBody = sb.Insert((jsonBody.Length - 5), metaDataString).ToString();
                        Console.WriteLine(jsonBody);
                    }
                }
            }
            string requestUri = "WriteDataToMobileDevice";
            var result = PostRequest(apiKey, jsonBody, requestUri).Result;
            var response = result.Content.ReadAsStringAsync();
            return result;
        }
        public Fields ReadConfigJsonFile()
        {

            string root = AppDomain.CurrentDomain.BaseDirectory;
            string p1 = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string parent = System.IO.Directory.GetParent(p1).FullName;
            string parent1 = Directory.GetParent(Directory.GetParent(Directory.GetParent(parent).FullName).FullName).FullName;

            string[] allfiles = Directory.GetFiles(parent1, "legic_configuration.json*", SearchOption.AllDirectories);
            string filePath = allfiles.GetValue(0).ToString();

            StreamReader r = new StreamReader(filePath);
            string jsonFile = r.ReadToEnd();
            Fields configField = JsonConvert.DeserializeObject<Fields>(jsonFile);
            return configField;
            
        }
    }
}

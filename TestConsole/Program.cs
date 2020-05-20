using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegicApiFramework;

namespace TestConsole
{
    class Program
    {

        static void Main(string[] args)
        {
            string[] dizi = { "010000000000000000000000" };
            string[] dizi1 = { string.Format(@"{{\""displayName\"":\""berkaykey \"",\""version\"":\""1.0\"",\""company\"":{{\""name\"":\""dormakaba\""}},\""issueDate\"":\""2018-09-18 10:07:48 +02:00\"",\""description\"":\""Infini-ID\""}}") };
            LegicApiRequests legic = new LegicApiRequests();
            legic.writeDataToMobile("api1508764901729zn3NWDUBBB5trw==-mLRVzBLPNuFBPV6pUNf3NVOIyknxr0wNRoEivA0t1YQ=", 37624775, 905532723714, 37584157, "010000000000000000000000", "staticCred",dizi,dizi1);
        }
    }
}

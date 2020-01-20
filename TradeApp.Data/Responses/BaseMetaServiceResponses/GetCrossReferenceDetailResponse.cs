using System.Collections.Generic;

namespace TradeApp.Data.Responses.BaseMetaServiceResponses
{
    public class GetCrossReferenceDetailResponse
    {
        public int XId { get; set; }
        public string ServerName { get; set; }
        public string IpAddress { get; set; }
        public string Dns { get; set; }
        public string RegulationName { get; set; }
        public string BranchName { get; set; }
        public string CompanyName { get; set; }
        public List<string> Groups { get; set; }
    }
}

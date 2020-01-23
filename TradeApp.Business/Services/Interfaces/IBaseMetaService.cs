using System.Collections.Generic;
using TradeApp.Business.BaseMetaModels;

namespace TradeApp.Business.Services.Interfaces
{
    public interface IBaseMetaService
    {
        List<GroupCrossReferenceResponse> GetGroupsByServerId(int serverId);
    }
}
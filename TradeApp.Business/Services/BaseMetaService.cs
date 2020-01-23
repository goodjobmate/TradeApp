using System.Collections.Generic;
using System.Linq;
using TradeApp.Business.BaseMetaModels;
using TradeApp.Business.Services.Interfaces;
using TradeApp.Data.Contexts;

namespace TradeApp.Business.Services
{
    public class BaseMetaService : IBaseMetaService
    {
        private readonly BaseMetaDbContext _context;

        public BaseMetaService(BaseMetaDbContext context)
        {
            _context = context;
        }

        public List<GroupCrossReferenceResponse> GetGroupsByServerId(int serverId)
        {
            var response = from cr in _context.CrossReferences
                where cr.ServerId == serverId
                join groupCrossReference in _context.GroupCrossReferences on cr.Id equals groupCrossReference
                    .CrossReferenceId
                join server in _context.Servers on cr.ServerId equals server.Id
                join regulation in _context.Regulations on cr.RegulationId equals regulation.Id
                join branch in _context.Branches on cr.BranchId equals branch.Id
                join company in _context.Companies on cr.CompanyId equals company.Id
                select new GroupCrossReferenceResponse
                {
                    ServerName = server.Name,
                    RegulationName = regulation.Name,
                    BranchName = branch.Name,
                    CompanyName = company.Name,
                    GroupName = groupCrossReference.GroupName,
                    ServerId = server.Id,
                    RegulationId = regulation.Id,
                    BranchId = branch.Id,
                    CompanyId = company.Id,
                    CrossReferenceId = cr.Id,
                    GroupCrossReferenceId = groupCrossReference.Id
                };

            return response.ToList();
        }
    }
}
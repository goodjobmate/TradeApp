using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TradeApp.Business.Services.Interfaces;
using TradeApp.Business.WidgetModels;
using TradeApp.Data.Contexts;

namespace TradeApp.Business.Services
{
    public class WidgetService : IWidgetService
    {
        private readonly TradeDbContext _context;

        public WidgetService(TradeDbContext context)
        {
            _context = context;
        }

        public UserWidgetDto GetWidget(int userDashboardId)
        {
            var userDashboard = _context.UserDashboard
                .Include(x => x.UserDashboardWidgets)
                .ThenInclude(x => x.Widget)
                .FirstOrDefault(x => x.Id == userDashboardId);

            if (userDashboard == null)
            {
                return null;
            }

            var result = new UserWidgetDto
            {
                PageId = userDashboard.Id,
                PageName = userDashboard.Name,
                Item = new List<WidgetItemDto>()
            };

            foreach (var widget in userDashboard.UserDashboardWidgets.Where(x => x.WidgetId != null).ToList())
            {
                var item = new WidgetItemDto
                {
                    WidgetId = widget.WidgetId,
                    WidgetName = widget.Widget.Name,
                    W = widget.Width,
                    I = widget.Index,
                    X = widget.XAxis,
                    H = widget.Height,
                    Y = widget.YAxis,
                    IsActive = true
                };

                result.Item.Add(item);
            }

            return result;
        }
    }
}
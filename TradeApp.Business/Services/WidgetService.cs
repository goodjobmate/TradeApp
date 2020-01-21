using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TradeApp.Business.Services.Interfaces;
using TradeApp.Business.WidgetModels;
using TradeApp.Data.Contexts;
using TradeApp.Data.Models.TradeDbModels;

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

        public List<ResultDto> GetLookUp()
        {
            var result = _context.Widget.Select(x => new ResultDto
            {
                Key = x.Id,
                Value = x.Name
            }).ToList();

            return result;
        }

        public List<ResultDto> GetMenu(int userId)
        {
            var result = _context.UserDashboard.Where(x => x.UserId == userId).Select(x => new ResultDto
            {
                Key = x.Id,
                Value = x.Name
            }).ToList();

            return result;
        }

        public int CreateUserDashboardWidget(int userId, UserDashboardWidgetDto userDashboardWidget)
        {
            int userDashboardId;

            if (userDashboardWidget.PageId == null)
            {
                var newUserDashboard = new UserDashboard
                {
                    UserId = userId,
                    Name = userDashboardWidget.PageName
                };

                _context.UserDashboard.Add(newUserDashboard);

                _context.SaveChanges();

                userDashboardId = newUserDashboard.Id;
            }
            else
            {
                var existingUserDashboard = _context.UserDashboard.Include(x => x.UserDashboardWidgets)
                    .FirstOrDefault(x => x.Id == userDashboardWidget.PageId);

                if (existingUserDashboard == null)
                {
                    return 0;
                }

                userDashboardId = existingUserDashboard.Id;

                _context.UserDashboardWidget.RemoveRange(existingUserDashboard.UserDashboardWidgets);
                _context.SaveChanges();
            }

            foreach (var item in from lgDto in userDashboardWidget.Item.Lg.ToList()
                let widgetName = userDashboardWidget.Widget.FirstOrDefault(w => w.IndexId == Convert.ToInt32(lgDto.I))
                    ?.Name
                let widgetId = _context.Widget.SingleOrDefault(x => x.Name == widgetName)?.Id
                select new UserDashboardWidget
                {
                    CreatedAt = DateTime.Now,
                    CreatedById = userId,
                    Height = lgDto.H,
                    Width = lgDto.W,
                    XAxis = lgDto.X,
                    YAxis = lgDto.Y,
                    UserDashboardId = userDashboardId,
                    WidgetId = widgetId,
                    Index = lgDto.I
                })
            {
                _context.UserDashboardWidget.Add(item);
                _context.SaveChanges();
            }

            return userDashboardId;
        }

        public void DeleteWidget(int id)
        {
            var existingUserDashboard = _context.UserDashboard
                .Include(x => x.UserDashboardWidgets)
                .FirstOrDefault(x => x.Id == id);

            if (existingUserDashboard == null)
            {
                return;
            }

            _context.UserDashboardWidget.RemoveRange(existingUserDashboard.UserDashboardWidgets);
            _context.SaveChanges();

            existingUserDashboard.UserDashboardWidgets = null;

            _context.UserDashboard.Remove(existingUserDashboard);
            _context.SaveChanges();
        }
    }
}
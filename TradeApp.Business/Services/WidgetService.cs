using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

        public int CreateTag(TagDto dto)
        {
            var (tagFilterJson, tagLoginsJson) = GetTagAsJson(dto);

            var tag = new Tag
            {
                Key = tagFilterJson,
                Name = dto.Name,
                Logins = tagLoginsJson
            };

            _context.Tags.Add(tag);
            _context.SaveChanges();

            return tag.Id;
        }

        public (bool exist, int id) CheckIfTagExists(TagDto dto)
        {
            var (jFilter, jLogins) = GetTagAsJson(dto);

            var existingTag =
                _context.Tags.FirstOrDefault(f =>
                    string.Equals(f.Key, jFilter) && string.Equals(f.Logins, jLogins));

            return (existingTag == null, existingTag?.Id ?? 0);
        }

        public List<ResultDto> GetTagsWithServerAndRegulation(int? serverId, int? regulationId)
        {
            var allTags = _context.Tags.ToList();

            var results = allTags.Select(x => new
            {
                Filter = JsonConvert.DeserializeObject<TagFilterDto>(x.Key),
                x.Name,
                x.Id
            });

            //Get tags that contains server and regulation or have none of them.
            if (serverId.HasValue)
            {
                results = results.Where(x => x.Filter.ServerIds.Contains(serverId.Value));
            }

            if (regulationId.HasValue)
            {
                results = results.Where(x => x.Filter.RegulationId == regulationId.Value);
            }

            if (serverId == null && regulationId == null)
            {
                results = results.Where(x => x.Filter.RegulationId == null && !x.Filter.ServerIds.Any());
            }

            var response = new List<ResultDto>();

            results.ToList().ForEach(x =>
            {
                var item = new ResultDto
                {
                    Key = x.Id,
                    Value = x.Name
                };

                response.Add(item);
            });

            return response;
        }

        public void UpdateTag(TagDto dto)
        {
            var existingTag = _context.Tags.Find(dto.Id);

            var (jFilter, jLogin) = GetTagAsJson(dto);

            existingTag.Name = dto.Name;
            existingTag.Key = jFilter;
            existingTag.Logins = jLogin;

            _context.Tags.Update(existingTag);
            _context.SaveChanges();
        }

        public TagDto GetTagById(int id)
        {
            var tag = _context.Tags.Find(id);

            if (tag == null)
            {
                return null;
            }

            var dto = new TagDto
            {
                Name = tag.Name,
                Id = tag.Id,
                TagFilter = JsonConvert.DeserializeObject<TagFilterDto>(tag.Key)
            };

            dto.TagFilter.Logins = JsonConvert.DeserializeObject<Dictionary<int, List<int>>>(tag.Logins);

            return dto;
        }

        private (string tagFilterJson, string tagLoginsJson) GetTagAsJson(TagDto dto)
        {
            var filter = dto.TagFilter;
            var logins = dto.TagFilter.Logins;

            filter.Logins = new Dictionary<int, List<int>>();

            var jFilter = JsonConvert.SerializeObject(filter);

            var jLogins = string.Empty;

            if (logins.Any())
            {
                jLogins = JsonConvert.SerializeObject(logins);
            }

            return (jFilter, jLogins);
        }
    }
}
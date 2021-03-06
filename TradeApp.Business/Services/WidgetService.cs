﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TradeApp.Business.Services.Interfaces;
using TradeApp.Business.WidgetModels;
using TradeApp.Data.Contexts;
using TradeApp.Data.Models.TradeDbModels;
using TradeApp.Redis;

namespace TradeApp.Business.Services
{
    public class WidgetService : IWidgetService
    {
        private readonly TradeDbContext _context;
        private readonly RedisCache _redisDb;

        public WidgetService(TradeDbContext context)
        {
            _context = context;
            _redisDb = new RedisCache(10);
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
            var (tagFilterJson, tagIncludedLoginsJson, tagExcludedLoginsJson) = GetTagAsJson(dto);

            var tag = new Tag
            {
                Key = tagFilterJson,
                Name = dto.Name,
                IncludedLogins = tagIncludedLoginsJson,
                ExcludedLogins = tagExcludedLoginsJson
            };

            _context.Tags.Add(tag);
            _context.SaveChanges();

            return tag.Id;
        }

        public (bool exist, int id) CheckIfTagExists(TagDto dto)
        {
            var (jFilter, jIncludedLogins, jExcludedLogins) = GetTagAsJson(dto);

            var existingTag =
                _context.Tags.FirstOrDefault(f => string.Equals(f.Key, jFilter) && string.Equals(f.IncludedLogins, jIncludedLogins) && string.Equals(f.ExcludedLogins, jExcludedLogins));

            return (existingTag != null, existingTag?.Id ?? 0);
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

            var (jFilter, jIncludedLogin, jExcludedLogin) = GetTagAsJson(dto);

            existingTag.Name = dto.Name;
            existingTag.Key = jFilter;
            existingTag.IncludedLogins = jIncludedLogin;
            existingTag.ExcludedLogins = jExcludedLogin;

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
            dto.TagFilter.IncludedLogins = JsonConvert.DeserializeObject<Dictionary<int, List<int>>>(tag.IncludedLogins);
            dto.TagFilter.ExcludedLogins = JsonConvert.DeserializeObject<Dictionary<int, List<int>>>(tag.ExcludedLogins);
            
            return dto;
        }

        private (string tagFilterJson, string tagIncludedLoginsJson, string tagExcludedLoginsJson) GetTagAsJson(TagDto dto)
        {
            var filter = new TagFilterDto();//dto.TagFilter;
            var includedLogins = dto.TagFilter.IncludedLogins;
            var excludedLogins = dto.TagFilter.ExcludedLogins;

            filter.IncludedLogins = new Dictionary<int, List<int>>();
            filter.ExcludedLogins = new Dictionary<int, List<int>>();
            filter.TagIds = dto.TagFilter.TagIds;
            filter.XIds = dto.TagFilter.XIds;
            filter.Groups = dto.TagFilter.Groups;
            filter.OperatorByCalculation = dto.TagFilter.OperatorByCalculation;
            filter.RegulationId = dto.TagFilter.RegulationId;
            filter.ServerIds = dto.TagFilter.ServerIds;

            var jFilter = JsonConvert.SerializeObject(filter);

            var jIncludedLogins = string.Empty;
            var jExcludedLogins = string.Empty;

            if (includedLogins.Any())
            {
                jIncludedLogins = JsonConvert.SerializeObject(includedLogins);
            }

            if (excludedLogins.Any())
            {
                jExcludedLogins = JsonConvert.SerializeObject(excludedLogins);
            }

            return (jFilter, jIncludedLogins, jExcludedLogins);
        }

        public int CreateUserDashboardWidgetFilter(int userId, WidgetFilterDto widgetFilterDto)
        {

            var existingUserDashboardWidget = _context.UserDashboardWidget.Include(w => w.Widget).FirstOrDefault(w =>
               w.Id == widgetFilterDto.UserDashboardWidgetId && w.WidgetId != null);


            if (existingUserDashboardWidget == null)
                return 0;

            foreach (var filterSet in widgetFilterDto.Filters)
            {
                var str = JsonConvert.SerializeObject(filterSet.FilterSet, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                var existingFilter = _context.Filter.FirstOrDefault(f => f.Key == str);

                var filterId = existingFilter?.Id ?? 0;
                if (existingFilter == null)
                {
                    var filter = new Filter
                    {
                        CreatedAt = DateTime.Now,
                        CreatedById = userId,
                        Key = str
                    };
                    _context.Filter.Add(filter);
                    _context.SaveChanges();
                    filterId = filter.Id;
                }
                var redisKey = existingUserDashboardWidget.Widget.Type.ToString();
                if (!_redisDb.HashGetAll(redisKey).FirstOrDefault(r => r.Name == str).Value.HasValue)
                    _redisDb.WriteAndUpdate(redisKey, str, 0);//todo

                var existingUserDashboardWidgetFilter = _context.UserDashboardWidgetFilter.FirstOrDefault(d =>
                    d.FilterId == filterId && d.UserDashboardWidgetId == existingUserDashboardWidget.Id);
                if (existingUserDashboardWidgetFilter != null)
                    continue;

                var userDashboardWidgetFilter = new UserDashboardWidgetFilter
                {
                    FilterId = filterId,
                    CreatedAt = DateTime.Now,
                    CreatedById = userId,
                    UserDashboardWidgetId = existingUserDashboardWidget.Id,
                    Name = filterSet.Name
                };
                _context.UserDashboardWidgetFilter.Add(userDashboardWidgetFilter);
                _context.SaveChanges();
            }
            existingUserDashboardWidget.ModifiedById = userId;
            existingUserDashboardWidget.ModifiedAt = DateTime.Now;
            _context.UserDashboardWidget.Update(existingUserDashboardWidget);
            _context.SaveChanges();
            return existingUserDashboardWidget.Id;

        }


        public List<FilterResultDto> GetUserDashboardWidgetFilter(int userDashboardWidgetId)
        {
            var existingUserDashboardWidget = _context.UserDashboardWidget.Include(d =>
                d.Widget).FirstOrDefault(d => d.Id == userDashboardWidgetId && d.WidgetId != null);
            if (existingUserDashboardWidget == null)
                return null;

            var filters = _context.UserDashboardWidgetFilter.Where(f => f.UserDashboardWidgetId == existingUserDashboardWidget.Id).Include(f => f.Filter);

            var redisKey = existingUserDashboardWidget.Widget.Type.ToString();
            var result = _redisDb.HashGetAll(redisKey);
            var resultList = new List<FilterResultDto>();
            foreach (var filter in filters)
            {
                resultList.Add(new FilterResultDto()
                {
                    Value = result?.FirstOrDefault(r => r.Name == filter.Filter.Key).Value,
                    Name = filter.Name
                });
            }
            return resultList;
        }

        public List<ExposureFilterResultDto> GetExposureUserDashboardWidgetFilter(int userDashboardWidgetId)
        {
            var existingUserDashboardWidget = _context.UserDashboardWidget.Include(d =>
                d.Widget).FirstOrDefault(d => d.Id == userDashboardWidgetId && d.WidgetId != null);
            if (existingUserDashboardWidget == null)
                return null;

            var filters = _context.UserDashboardWidgetFilter.Where(f => f.UserDashboardWidgetId == existingUserDashboardWidget.Id).Include(f => f.Filter);

            var redisKey = existingUserDashboardWidget.Widget.Type.ToString();
            var result = _redisDb.HashGetAll(redisKey);
            var resultList = new List<ExposureFilterResultDto>();
            foreach (var filter in filters)
            {
                resultList.Add(new ExposureFilterResultDto()
                {
                    Value = JsonConvert.DeserializeObject<List<ExposureResultDto>>(result?.FirstOrDefault(r => r.Name == filter.Filter.Key).Value),
                    Name = filter.Name
                });
            }
            return resultList;
        }
    }
}
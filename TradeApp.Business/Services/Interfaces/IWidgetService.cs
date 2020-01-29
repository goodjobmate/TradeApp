using System.Collections.Generic;
using TradeApp.Business.WidgetModels;

namespace TradeApp.Business.Services.Interfaces
{
    public interface IWidgetService
    {
        UserWidgetDto GetWidget(int userDashboardId);
        List<ResultDto> GetLookUp();
        List<ResultDto> GetMenu(int userId);
        int CreateUserDashboardWidget(int userId, UserDashboardWidgetDto userDashboardWidget);
        void DeleteWidget(int id);
        int CreateTag(TagDto dto);
        (bool exist, int id) CheckIfTagExists(TagDto dto);
        List<ResultDto> GetTagsWithServerAndRegulation(int? serverId, int? regulationId);
        void UpdateTag(TagDto dto);
        TagDto GetTagById(int id);
        int CreateUserDashboardWidgetFilter(int userId, WidgetFilterDto widgetFilterDto);
        List<FilterResultDto> GetUserDashboardWidgetFilter(int userDashboardWidgetId);
        List<ExposureFilterResultDto> GetExposureUserDashboardWidgetFilter(int userDashboardWidgetId);
    }
}
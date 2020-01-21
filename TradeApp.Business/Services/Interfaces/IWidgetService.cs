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
    }
}
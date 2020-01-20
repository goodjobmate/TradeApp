using TradeApp.Business.WidgetModels;

namespace TradeApp.Business.Services.Interfaces
{
    public interface IWidgetService
    {
        UserWidgetDto GetWidget(int userDashboardId);
    }
}
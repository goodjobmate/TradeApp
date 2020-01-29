using System.Collections.Generic;

namespace TradeApp.Business.WidgetModels
{
    public class ExposureFilterResultDto
    {
        public string Name { get; set; }
        public List<ExposureResultDto> Value { get; set; }
    }
    public class ExposureResultDto
    {
        public string Key { get; set; }
        public double Value { get; set; }
    }
}

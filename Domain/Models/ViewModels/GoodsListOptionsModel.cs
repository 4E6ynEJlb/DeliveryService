using Domain.Models.ApplicationModels;

namespace Domain.Models.VievModels
{
    public class GoodsListOptionsModel
    {
        public string? TextInTitle { get; set; }
        public SortCriteria Criterium { get; set; }
        public bool IsAsc { get; set; }
    }
}

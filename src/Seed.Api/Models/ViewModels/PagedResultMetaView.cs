using AutoMapper;
using Seed.Domain.ApiWrappers.Tpd.Entities;

namespace Seed.Api.Models.ViewModels
{
    public class PagedResultMetaView
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }


        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<TpdPagedResult, PagedResultMetaView>();
            }
        }
    }
}
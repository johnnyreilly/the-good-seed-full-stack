using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Seed.Domain.ApiWrappers.Tpd.Entities;

namespace Seed.Api.Models.Inputs
{
    public class QueryOptionsInput
    {
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(1, int.MaxValue)]
        public int PageSize { get; set; } = 15;

        [MaxLength(128)]
        public string SortColumn { get; set; }

        public SortDirection SortDirection { get; set; }


        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<QueryOptionsInput, TpdQueryOptions>();
            }
        }
    }
}
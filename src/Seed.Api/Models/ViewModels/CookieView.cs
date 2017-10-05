using System;
using AutoMapper;
using Seed.Domain.Entities;

namespace Seed.Api.Models.ViewModels
{
    public class CookieView
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public DateTime Expires { get; set; }
        public bool IsSecure { get; set; }


        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Cookie, CookieView>()
                    .ForMember(d => d.Path, o => o.Ignore())
                    .ForMember(d => d.Expires, o => o.Ignore())
                    .ForMember(d => d.IsSecure, o => o.Ignore());
            }
        }
    }
}
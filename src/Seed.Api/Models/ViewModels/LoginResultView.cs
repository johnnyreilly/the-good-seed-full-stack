using AutoMapper;
using Seed.Domain.Entities;

namespace Seed.Api.Models.ViewModels
{
    public class LoginResultView
    {
        public string UserId { get; set; }
        public CookieView Cookie { get; set; }


        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<LoginResult, LoginResultView>();
            }
        }
    }
}
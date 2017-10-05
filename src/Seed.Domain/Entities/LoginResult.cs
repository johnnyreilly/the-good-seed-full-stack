using AutoMapper;
using Seed.Domain.ApiWrappers.Tpd.Entities;

namespace Seed.Domain.Entities
{
    public class LoginResult
    {
        public string UserId { get; set; }

        public Cookie Cookie { get; set; }


        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<TpdLoginResult, LoginResult>()
                    .ForMember(d => d.UserId, o => o.MapFrom(s => s.Gcn))
                    .ForMember(d => d.Cookie,
                        o => o.ResolveUsing(s =>
                            new Cookie {Name = s.CookieName, Value = s.CookieValue, Domain = s.CookieDomain}));
            }
        }
    }
}
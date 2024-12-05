

using AutoMapper;
using EmailManagement.Domain.Dtos.v1.Request;
using EmailManagement.Domain.Enum;
using EmailManagement.Domain.Models.Email;

namespace EmailManagement.Application.Mapeamento
{
    public class EmailMappingProfile : Profile
    {
        public EmailMappingProfile()
        {
            CreateMap<EmailPostParametersRequest, Email>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => EmailStatus.Pending))
                .ForMember(dest => dest.Attempts, opt => opt.MapFrom(_ => 0))
                .ForMember(dest => dest.SentAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastAttemptAt, opt => opt.Ignore());
        }
    }
}

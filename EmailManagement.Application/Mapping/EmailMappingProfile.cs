

using AutoMapper;
using EmailManagement.Domain.Dtos.v1.Request;
using EmailManagement.Domain.Dtos.v1.Response;
using EmailManagement.Domain.Enum;
using EmailManagement.Domain.Models.Email;

namespace EmailManagement.Application.Mapping
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

            CreateMap<Email, EmailPostParametersResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<Email, EmailGetParametersResponse>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
               .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.Sender))
               .ForMember(dest => dest.Recipients, opt => opt.MapFrom(src => src.Recipients))
               .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject))
               .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Body))
               .ForMember(dest => dest.Atachments, opt => opt.MapFrom(src => src.Atachments))
               .ForMember(dest => dest.SentAt, opt => opt.MapFrom(src => src.SentAt))
               .ForMember(dest => dest.Attempts, opt => opt.MapFrom(src => src.Attempts))
               .ForMember(dest => dest.LastAttemptAt, opt => opt.MapFrom(src => src.LastAttemptAt))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }

    }
}

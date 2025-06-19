using AutoMapper;
using Quizzy.Data.Entities.Identity;
using Quizzy.Models;

namespace Quizzy.Mapper;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<UserEntity, PreferencesViewModel>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.About, opt => opt.MapFrom(src => src.About))
            .ForMember(dest => dest.School, opt => opt.MapFrom(src => src.School != null ? src.School.Name : null))
            .ForMember(dest => dest.PhotoName,
                opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Image) ? "default.webp" : src.Image))
            .ForMember(dest => dest.Photo, opt => opt.Ignore()) // cannot map IFormFile from entity
            .ForMember(dest => dest.Schools, opt => opt.Ignore()) // populated separately from DB
            .ForMember(dest => dest.OldPassword, opt => opt.Ignore())
            .ForMember(dest => dest.NewPassword, opt => opt.Ignore())
            .ForMember(dest => dest.ConfirmPassword, opt => opt.MapFrom(src => src.Email));

        // PreferencesViewModel -> UserEntity
        CreateMap<PreferencesViewModel, UserEntity>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.About, opt => opt.MapFrom(src => src.About))
            .ForMember(dest => dest.Image, opt => opt.Ignore()) // updated manually on upload
            .ForMember(dest => dest.SchoolId, opt => opt.Ignore()) // resolved separately from DB
            .ForMember(dest => dest.School, opt => opt.Ignore())
            .ForMember(dest => dest.Tests, opt => opt.Ignore())
            .ForMember(dest => dest.TestHomeworks, opt => opt.Ignore())
            .ForMember(dest => dest.TestSessions, opt => opt.Ignore())
            .ForMember(dest => dest.UserRoles, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedUtc, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.Ignore())
            .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
            .ForMember(dest => dest.UserName, opt => opt.Ignore())
            .ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
            .ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
            .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
            .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
            .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
            .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
            .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
            .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
            .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

        CreateMap<RegisterViewModel, UserEntity>()
            .ForMember(x => x.Image, opt => opt.Ignore())
            .ForMember(x => x.UserName, opt => opt.MapFrom(x=>x.Email));
    }
}
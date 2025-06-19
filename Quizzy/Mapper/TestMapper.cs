using AutoMapper;
using Quizzy.Data.Entities;
using Quizzy.Models.Tests;

namespace Quizzy.Mapper;

public class TestMapper : Profile
{
    public TestMapper()
    {
        CreateMap<CreateViewModel, Test>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.Ignore()) // You can adjust this
            .ForMember(dest => dest.SubjectId, opt => opt.Ignore())   // Mapped manually later
            .ForMember(dest => dest.GradeId, opt => opt.Ignore())     // Mapped manually later
            .ForMember(dest => dest.CreatedById, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedUtc, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsPrivate, opt => opt.MapFrom(_ => false))
            .ForMember(dest => dest.IsCopyable, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.QuestionsQuantity, opt => opt.MapFrom(_ => 0))
            .ForMember(dest => dest.IsPublished, opt => opt.MapFrom(_ => false))
            .ForMember(dest => dest.Questions, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Subject, opt => opt.Ignore())
            .ForMember(dest => dest.Grade, opt => opt.Ignore());
        
        CreateMap<Test, EditViewModel>()
            .ForMember(dest => dest.TestId, opt => opt.MapFrom(src => src.TestId))
            .ForMember(dest => dest.Subject, opt => opt.Ignore())
            .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.GradeId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.IsPrivate, opt => opt.MapFrom(src => src.IsPrivate))
            .ForMember(dest => dest.IsCopyable, opt => opt.MapFrom(src => src.IsCopyable));

        // Mapping FROM ViewModel TO Entity (when saving changes)
        CreateMap<EditViewModel, Test>()
            .ForMember(dest => dest.TestId, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.IsPrivate, opt => opt.MapFrom(src => src.IsPrivate))
            .ForMember(dest => dest.IsCopyable, opt => opt.MapFrom(src => src.IsCopyable))
            .ForMember(dest => dest.GradeId, opt => opt.Ignore())
            .ForMember(dest => dest.SubjectId, opt => opt.Ignore()) // Needs manual mapping
            .ForMember(dest => dest.CreatedUtc, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedById, opt => opt.Ignore())
            .ForMember(dest => dest.QuestionsQuantity, opt => opt.Ignore())
            .ForMember(dest => dest.IsPublished, opt => opt.Ignore())
            .ForMember(dest => dest.Questions, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Subject, opt => opt.Ignore())
            .ForMember(dest => dest.Grade, opt => opt.Ignore());
    }
}
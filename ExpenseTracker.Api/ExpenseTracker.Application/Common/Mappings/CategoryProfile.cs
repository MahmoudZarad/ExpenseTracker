using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Features.Categories.Commands.Models;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Common.Mappings;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>();

        CreateMap<CreateCategoryCommand, Category>();

        CreateMap<UpdateCategoryCommand, Category>();
    }
}

using AutoMapper;
using ExpenseTracker.Application.DTOs.Budget;
using ExpenseTracker.Application.Features.Budgets.Commands.Models;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Common.Mappings
{
    public class BudgetProfile : Profile
    {
        public BudgetProfile()
        {
            CreateMap<Budget, BudgetDto>();

            CreateMap<CreateBudgetCommand, Budget>();

            CreateMap<UpdateBudgetCommand, Budget>();
        }
    }
}

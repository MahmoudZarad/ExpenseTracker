using AutoMapper;
using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Features.Categories.Commands.Models;
using ExpenseTracker.Application.Features.Transactions.Commands.Models;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Common.Mappings;

public class TransactionProfile : Profile
{
    public TransactionProfile()
    {
        CreateMap<Transaction, TransactionDto>()
            .ForMember(
                dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.Name));

        CreateMap<CreateTransactionCommand, Transaction>();

        CreateMap<UpdateTransactionCommand, Transaction>();
    }
}
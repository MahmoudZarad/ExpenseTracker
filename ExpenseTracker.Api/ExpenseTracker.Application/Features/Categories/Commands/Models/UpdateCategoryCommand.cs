using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Application.Features.Categories.Commands.Models;

public record UpdateCategoryCommand(
    int Id,
    string Name,
    TransactionType Type) : IRequest<Result<bool>>;

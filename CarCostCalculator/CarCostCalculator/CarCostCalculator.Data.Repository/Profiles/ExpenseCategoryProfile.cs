using AutoMapper;
using DTO = CarCostCalculator.Domain.Model;
using Entities = CarCostCalculator.Data.EF.Entities;

namespace CarCostCalculator.Data.Repository.Profiles;

public class ExpenseCategoryProfile : Profile
{
    #region Public Instantiation

    public ExpenseCategoryProfile()
    {
        CreateMap<Entities.ExpenseCategory, DTO.ExpenseCategory.ExpenseCategoryAddable>()
            .ReverseMap();

        CreateMap<Entities.ExpenseCategory, DTO.ExpenseCategory.ExpenseCategoryChangeable>()
            .IncludeBase<Entities.ExpenseCategory, DTO.ExpenseCategory.ExpenseCategoryAddable>()
            .ReverseMap();
    }

    #endregion
}
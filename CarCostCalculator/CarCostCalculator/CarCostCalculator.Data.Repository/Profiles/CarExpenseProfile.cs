using AutoMapper;
using DTO = CarCostCalculator.Domain.Model;
using Entities = CarCostCalculator.Data.EF.Entities;

namespace CarCostCalculator.Data.Repository.Profiles;

public class CarExpenseProfile : Profile
{
    #region Public Instantiation

    public CarExpenseProfile()
    {
        CreateMap<Entities.CarExpense, DTO.CarExpense.CarExpenseAddable>()
            .ReverseMap();

        CreateMap<Entities.CarExpense, DTO.CarExpense.CarExpenseChangeable>()
            .IncludeBase<Entities.CarExpense, DTO.CarExpense.CarExpenseAddable>()
            .ReverseMap();
    }

    #endregion
}

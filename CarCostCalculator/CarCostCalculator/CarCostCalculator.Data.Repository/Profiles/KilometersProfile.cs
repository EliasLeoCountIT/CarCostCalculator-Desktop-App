using AutoMapper;
using DTO = CarCostCalculator.Domain.Model;
using Entities = CarCostCalculator.Data.EF.Entities;

namespace CarCostCalculator.Data.Repository.Profiles;

public class KilometersProfile : Profile
{
    #region Public Instantiation

    public KilometersProfile()
    {
        CreateMap<Entities.Kilometers, DTO.Kilometers.KilometersAddable>()
           .ReverseMap();

        CreateMap<Entities.Kilometers, DTO.Kilometers.KilometersChangeable>()
            .IncludeBase<Entities.Kilometers, DTO.Kilometers.KilometersAddable>()
            .ReverseMap();
    }

    #endregion
}
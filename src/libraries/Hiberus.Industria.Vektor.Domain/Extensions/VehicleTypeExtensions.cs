using Vektor.API.Domain.Entities;
using Vektor.API.Domain.Enums;

namespace Vektor.API.Domain.Extensions;

public static class VehicleTypeExtensions
{
    public static RoutingProfile GetRoutingProfile(this VehicleType type) =>
        type switch
        {
            VehicleType.Van => RoutingProfile.Car,
            _ => RoutingProfile.HeavyGoodsVehicle,
        };
}

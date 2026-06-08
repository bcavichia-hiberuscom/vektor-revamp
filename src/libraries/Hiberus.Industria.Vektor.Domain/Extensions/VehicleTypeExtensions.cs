using Hiberus.Industria.Vektor.Domain.Vehicle;

namespace Hiberus.Industria.Vektor.Domain.Extensions;

public static class VehicleTypeExtensions
{
    public static RoutingProfile GetRoutingProfile(this VehicleType type) =>
        type switch
        {
            VehicleType.Van => RoutingProfile.Car,
            _ => RoutingProfile.HeavyGoodsVehicle,
        };
}

// Domain/Common/Interfaces/ISoftDeletable.cs
namespace Hiberus.Industria.Vektor.Domain.Common.Interfaces;

public interface ISoftDeletable
{
    bool IsActive { get; }
    DateTime? DeletedAt { get; }
    string? DeletedBy { get; }
    bool IsDeleted { get; }
}

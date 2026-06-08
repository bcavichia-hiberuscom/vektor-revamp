namespace Hiberus.Industria.Vektor.Domain.Common.Interfaces;

/// <summary>
/// Interface for entities that need auditing information.
/// </summary>
public interface IAuditable
{
    /// <summary>
    /// Gets the date and time when the entity was created.
    /// </summary>
    DateTime CreatedAt { get; }

    /// <summary>
    /// Gets the date and time when the entity was last updated.
    /// </summary>
    DateTime? UpdatedAt { get; }

    /// <summary>
    /// Gets the identifier of the user who created the entity.
    /// </summary>
    string CreatedBy { get; }

    /// <summary>
    /// Gets the identifier of the user who last updated the entity.
    /// </summary>
    string? UpdatedBy { get; }
}

using MediatR;

namespace EmailManagement.Domain.Primitives;

public record DomainEvent(Guid Id) : INotification;

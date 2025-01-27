using MediatR;

namespace QuickFly.Server.Shared.Shared.Shared.CQRS
{
    public interface ICommand : ICommand<Unit>
    {

    }
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}

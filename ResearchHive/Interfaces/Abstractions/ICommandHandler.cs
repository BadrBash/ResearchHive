
using MediatR;
using ResearchHive.Wrapper;

namespace ResearchHive.Abstractions.Messaging
{
    public interface ICommandHandler<TCommand>  : IRequestHandler<TCommand, Result> where TCommand : ICommand
    {
    }

    public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICommand<TResponse>
    {
    }
}

using System.Threading;
using System.Threading.Tasks;
using Audacia.Commands;

namespace Audacia.DataAccess.Commands.Decorators
{
    /// <summary>
    /// <see cref="ICommandHandler{T}"/> implementation to add the functionality to save changes made by wrapped handlers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SavingDecorator<T> : ICommandHandler<T> where T : ICommand
    {
        private readonly ISaveableDataRepository _context;
        private readonly ICommandHandler<T> _wrappedHandler;

        public SavingDecorator(ISaveableDataRepository context, ICommandHandler<T> wrappedHandler)
        {
            _context = context;
            _wrappedHandler = wrappedHandler;
        }

        public async Task<CommandResult> HandleAsync(T command, CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await _wrappedHandler.HandleAsync(command, cancellationToken);
            if (result.IsSuccess)
            {
                await _context.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
    }

    /// <summary>
    /// <see cref="ICommandHandler{T,TOutput}"/> implementation to add the functionality to save changes made by wrapped handlers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TOutput"></typeparam>
    public class SavingDecorator<T, TOutput> : OutputCommandHandlerBase<T, TOutput> where T : ICommand
    {
        private readonly ISaveableDataRepository _context;
        private readonly ICommandHandler<T, TOutput> _wrappedHandler;

        public SavingDecorator(ISaveableDataRepository context, ICommandHandler<T, TOutput> wrappedHandler)
        {
            _context = context;
            _wrappedHandler = wrappedHandler;
        }

        public override async Task<CommandResult<TOutput>> HandleAsync(T command, CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await _wrappedHandler.HandleAsync(command, cancellationToken);
            if (result.IsSuccess)
            {
                await _context.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
    }
}

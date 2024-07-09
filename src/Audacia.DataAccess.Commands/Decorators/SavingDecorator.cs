using System.Threading;
using System.Threading.Tasks;
using Audacia.Commands;

namespace Audacia.DataAccess.Commands.Decorators;

/// <summary>
/// <see cref="ICommandHandler{T}"/> implementation to add the functionality to save changes made by wrapped handlers.
/// </summary>
/// <typeparam name="T">Type of <see cref="ICommandHandler{T}"/>.</typeparam>
public class SavingDecorator<T> : ICommandHandler<T> where T : ICommand
{
    private readonly ISaveableDataRepository _context;
    private readonly ICommandHandler<T> _wrappedHandler;

    /// <summary>
    /// Constructor takes in an instance of <see cref="ISaveableDataRepository"/> and an instance of <se cref="ICommandHandler"/>.
    /// </summary>
    /// <param name="context">Instance of <see cref="ISaveableDataRepository"/>.</param>
    /// <param name="wrappedHandler">Instance of <se cref="ICommandHandler"/>.</param>
    public SavingDecorator(ISaveableDataRepository context, ICommandHandler<T> wrappedHandler)
    {
        _context = context;
        _wrappedHandler = wrappedHandler;
    }

    /// <summary>
    /// Execute the passed in command Asynchronously. 
    /// </summary>
    /// <param name="command">Type T command instance.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns><see cref="CommandResult"/>.</returns>
    public async Task<CommandResult> HandleAsync(T command, CancellationToken cancellationToken = default)
    {
        var result = await _wrappedHandler.HandleAsync(command, cancellationToken).ConfigureAwait(false);
        if (result.IsSuccess)
        {
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        return result;
    }
}

/// <summary>
/// <see cref="ICommandHandler{T,TOutput}"/> implementation to add the functionality to save changes made by wrapped handlers.
/// </summary>
/// <typeparam name="T">Type of <see cref="OutputCommandHandlerBase{T, TOutput}"/>.</typeparam>
/// <typeparam name="TOutput">Return type of <see cref="OutputCommandHandlerBase{T, TOutput}"/>.</typeparam>
public class SavingDecorator<T, TOutput> : OutputCommandHandlerBase<T, TOutput> where T : ICommand
{
    private readonly ISaveableDataRepository _context;
    private readonly ICommandHandler<T, TOutput> _wrappedHandler;

    /// <summary>
    /// Constructor takes in an instance of <see cref="ISaveableDataRepository"/> context and an isnatnce of <see cref="ICommandHandler{T, TOutput}" /> wrappedHandler.
    /// </summary>
    /// <param name="context">Instance of <see cref="ISaveableDataRepository"/>.</param>
    /// <param name="wrappedHandler">Instance of <see cref="ICommandHandler{T, TOutput}" />.</param>
    public SavingDecorator(ISaveableDataRepository context, ICommandHandler<T, TOutput> wrappedHandler)
    {
        _context = context;
        _wrappedHandler = wrappedHandler;
    }

    /// <summary>
    /// Execute the passed in command Asynchronously. 
    /// </summary>
    /// <param name="command">Type T command instance.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns><see cref="CommandResult{TOutput}"/>.</returns>
    public override async Task<CommandResult<TOutput>> HandleAsync(T command, CancellationToken cancellationToken = default)
    {
        var result = await _wrappedHandler.HandleAsync(command, cancellationToken).ConfigureAwait(false);
        if (result.IsSuccess)
        {
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        return result;
    }
}

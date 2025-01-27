using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickFly.Server.Shared.CQRS
{
    public interface IQueryHandler<in TQuery, TResposne>
        : IRequestHandler<TQuery, TResposne>
        where TQuery : IQuery<TResposne>
        where TResposne : notnull
    {
    }
}

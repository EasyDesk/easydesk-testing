using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace EasyDesk.Testing.MatrixExpansion
{
    public delegate IEnumerable Expansion(IImmutableList<object> currentParams);

    public abstract class MatrixBuilderBase<T, TBuilder>
        where TBuilder : MatrixBuilderBase<T, TBuilder>
    {
        protected IImmutableStack<Expansion> _expansions;

        public MatrixBuilderBase(IImmutableStack<Expansion> expansions)
        {
            _expansions = expansions;
        }

        private T ConvertToTuple(IImmutableList<object> paramList)
        {
            var enumerator = paramList.GetEnumerator();
            object Next()
            {
                enumerator.MoveNext();
                return enumerator.Current;
            }
            return AsTuple(Next);
        }

        protected abstract T AsTuple(Func<object> next);

        protected IImmutableStack<Expansion> AddExpansion<T2>(Func<T, IEnumerable<T2>> axis) =>
            _expansions.Push(ps => axis(ConvertToTuple(ps)));

        public TBuilder Filter(Func<T, bool> predicate)
        {
            _expansions = _expansions.Pop(out var expansion);
            _expansions = _expansions.Push(ps => FilteredExpansion(ps, expansion, predicate));
            return this as TBuilder;
        }

        private IEnumerable FilteredExpansion(IImmutableList<object> currentParams, Expansion expansion, Func<T, bool> predicate)
        {
            foreach (var p in expansion(currentParams))
            {
                var tuple = ConvertToTuple(currentParams.Add(p));
                if (predicate(tuple))
                {
                    yield return p;
                }
            }
        }

        public IEnumerable<object[]> Build()
        {
            var result = new List<object[]>();
            var stack = new Stack<object>();

            BuildResult(_expansions.ToArray(), stack, result);

            return result;
        }

        private void BuildResult(Expansion[] expansions, Stack<object> currentParams, List<object[]> result)
        {
            if (currentParams.Count == expansions.Length)
            {
                result.Add(currentParams.Reverse().ToArray());
                return;
            }
            var expansionIndex = expansions.Length - currentParams.Count - 1;
            var expansion = expansions[expansionIndex];
            var nextParams = expansion(currentParams.ToImmutableList());
            foreach (var param in nextParams)
            {
                currentParams.Push(param);
                BuildResult(expansions, currentParams, result);
                currentParams.Pop();
            }
        }
    }
}

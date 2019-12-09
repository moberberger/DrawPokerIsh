using Morpheus;
using Protobuf.DrawPoker;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DrawPoker
{
    /// <summary>
    /// A caching class which uses reflection to find all static methods that are decorated with
    /// the <see cref="APaylineEvaluatorAttribute"/> so that each PaylineEvaluator can be found
    /// in O(1) time during Poker Hand evaluation.
    /// </summary>
    public static class PaylineEvaluatorCache
    {
        private static Dictionary<string, Func<Evaluation, bool>> sm_cachedPaylines;

        public static string NormalizedPaylineId( string paylineId ) => paylineId.Replace( " ", "" ).ToLower();

        /// <summary>
        /// Get the evaluator for a given PaylineId. PaylineIds should be globally unique.
        /// </summary>
        /// <param name="paylineId">The PaylineId to look up</param>
        /// <returns>
        /// The lambda used to test a poker hand Evaluation. If that lambda returns TRUE, then
        /// the payline assocated with the PaylineId is the correct payline.
        /// </returns>
        public static Func<Evaluation, bool> FindPayline( string paylineId )
        {
            if (sm_cachedPaylines == null)
                BuildCachedPaylines();

            string pid = NormalizedPaylineId( paylineId );
            sm_cachedPaylines.TryGetValue( pid, out var retval );
            return retval;
        }

        /// <summary>
        /// Build the cache.
        /// </summary>
        private static void BuildCachedPaylines()
        {
            sm_cachedPaylines = new Dictionary<string, Func<Evaluation, bool>>();
            var functions =Morpheus.ReflectionExtenstions.GetStaticFunctionsWithAttribute<APaylineEvaluatorAttribute>( true );
            var inParam = Expression.Parameter( typeof( Evaluation ), "theEvaluation" );

            foreach (var kv in functions)
            {
                var pid = NormalizedPaylineId( kv.Value.PaylineId );

                // Important- turn the MethodInfo into a Lambda for massive performance
                // improvements over reflection.
                var func = Expression.Lambda<Func<Evaluation, bool>>
                    (
                        Expression.Call( kv.Key, inParam ),
                        inParam
                    ).Compile();

                sm_cachedPaylines[pid] = func;
            }
        }
    }
}

using System;

namespace DrawPoker
{
    [AttributeUsage( AttributeTargets.Method )]
    public class APaylineEvaluatorAttribute : Attribute
    {
        /// <summary>
        /// The ID of the payline- must match the .PAYTABLE file
        /// </summary>
        public string PaylineId { get; }

        /// <summary>
        /// Decorate an evaluation routine with only an identification
        /// </summary>
        /// <param name="paylineId">
        /// The PaylineId for the evaluation, which must match that found in the Paytable.
        /// Spaces are removed, and this is a case insensitive comparison
        /// </param>
        public APaylineEvaluatorAttribute( string paylineId )
        {
            PaylineId = paylineId;
        }
    }
}

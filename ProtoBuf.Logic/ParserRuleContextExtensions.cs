using Antlr4.Runtime.Misc;
using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace ProtoBuf.Logic
{
    public static class ParserRuleContextExtensions
    {
        [return: NotNullIfNotNull(nameof(context))]
        public static string? GetFullText(this ParserRuleContext? context)
        {
            if (context==null)
            {
                return null;
            }
            if (context.Start == null || context.Stop == null || context.Start.StartIndex < 0 || context.Stop.StopIndex < 0)
                return context.GetText(); // Fallback

            return context.Start.InputStream.GetText(Interval.Of(context.Start.StartIndex, context.Stop.StopIndex));
        }
    }
}

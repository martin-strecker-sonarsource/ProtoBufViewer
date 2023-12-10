using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProtoBuf.Logic
{
    public static class RuleContextExtensions
    {
        public static IEnumerable<RuleContext> AncestorsAndSelf(this RuleContext context)
        {
            while(context != null)
            {
                yield return context;
                context = context.Parent;
            }
        }
    }
}

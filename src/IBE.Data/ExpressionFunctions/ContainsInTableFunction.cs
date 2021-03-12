using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Text;
using IBE.Common.Extensions;
using System.Reflection;

namespace IBE.Data.ExpressionFunctions {
    public class ContainsInTableFunction : ICustomFunctionOperatorQueryable, ICustomFunctionOperator {
        const string FunctionName = nameof(ContainsInTable);
        static readonly ContainsInTableFunction instance = new ContainsInTableFunction();

        public string Name => FunctionName;

        public static void Register() {
            CriteriaOperator.RegisterCustomFunction(instance);
        }
        public static bool Unregister() {
            return CriteriaOperator.UnregisterCustomFunction(instance);
        }

        public static bool ContainsInTable(string input, IEnumerable<string> table) {
            return (bool)instance.Evaluate(input, table);
        }

        public MethodInfo GetMethodInfo() {
            return typeof(ContainsInTableFunction).GetMethod(FunctionName);
        }

        public Type ResultType(params Type[] operands) {
            foreach (Type operand in operands) {
                if (operand != typeof(string)) return typeof(object);
            }
            return typeof(string);
        }

        public object Evaluate(params object[] operands) {
            var input = operands[0].ToString();
            var table = operands[1] as IEnumerable<string>;

            return input.RemovePolishChars().ContainsInTable(true, false, table);
        }
    }
}

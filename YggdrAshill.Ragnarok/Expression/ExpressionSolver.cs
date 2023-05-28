using YggdrAshill.Ragnarok.Memorization;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    /// <summary>
    /// Implementation of <see cref="ISolver"/> with expression.
    /// </summary>
    public sealed class ExpressionSolver :
        ISolver
    {
        public static ExpressionSolver Instance { get; } = new ExpressionSolver();

        private ExpressionSolver()
        {

        }

        public IActivation CreateActivation(ConstructorInjection injection)
        {
            var constructor = injection.Constructor;
            var argumentList = injection.ParameterList;

            var parameterList = Expression.Parameter(typeof(object[]), "parameterList");
            var convertedParameterList = argumentList.Select((argument, index) =>
            {
                var parameter = Expression.ArrayIndex(parameterList, Expression.Constant(index));

                return Expression.Convert(parameter, argument.ParameterType);
            });

            var body = Expression.New(constructor, convertedParameterList);

            var lambda = Expression.Lambda<Func<object[], object>>(body, parameterList).Compile();

            return new ActivateWithFunction(lambda, argumentList.Select(info => new Argument(info.Name, info.ParameterType)).ToArray());
        }

        public IInfusion CreateFieldInfusion(FieldInjection injection)
        {
            var implementedType = injection.ImplementedType;
            var fieldList = injection.FieldList;

            var instance = Expression.Parameter(typeof(object), "instance");
            var parameterList = Expression.Parameter(typeof(object[]), "parameterList");

            var convertedInstance = Expression.Convert(instance, implementedType);
            var assignedFieldList = fieldList.Select((fieldInfo, index) =>
            {
                var field = Expression.Field(convertedInstance, fieldInfo);
                var parameter = Expression.ArrayIndex(parameterList, Expression.Constant(index));
                var convertedParameter = Expression.Convert(parameter, fieldInfo.FieldType);

                return Expression.Assign(field, convertedParameter);
            });

            var body = Expression.Block(assignedFieldList);

            var lambda = Expression.Lambda<Action<object, object[]>>(body, instance, parameterList).Compile();

            return new InfuseWithAction(lambda, fieldList.Select(info => new Argument(info.Name, info.FieldType)).ToArray());
        }

        public IInfusion CreatePropertyInfusion(PropertyInjection injection)
        {
            var implementedType = injection.ImplementedType;
            var propertyList = injection.PropertyList;

            var instance = Expression.Parameter(typeof(object), "instance");
            var parameterList = Expression.Parameter(typeof(object[]), "parameterList");

            var convertedInstance = Expression.Convert(instance, implementedType);

            var assignedPropertyList = propertyList.Select((propertyInfo, index) =>
            {
                var property = Expression.Property(convertedInstance, propertyInfo);
                var parameter = Expression.ArrayIndex(parameterList, Expression.Constant(index));
                var convertedParameter = Expression.Convert(parameter, propertyInfo.PropertyType);

                return Expression.Assign(property, convertedParameter);
            });

            var body = Expression.Block(assignedPropertyList);

            var lambda = Expression.Lambda<Action<object, object[]>>(body, instance, parameterList).Compile();

            return new InfuseWithAction(lambda, propertyList.Select(info => new Argument(info.Name, info.PropertyType)).ToArray());
        }

        public IInfusion CreateMethodInfusion(MethodInjection injection)
        {
            var implementedType = injection.ImplementedType;
            var method = injection.Method;
            var argumentList = injection.ParameterList;

            var instance = Expression.Parameter(typeof(object), "instance");
            var parameterList = Expression.Parameter(typeof(object[]), "parameterList");

            var convertedInstance = Expression.Convert(instance, implementedType);
            var convertedParameterList = argumentList.Select((argument, index) =>
            {
                var parameter = Expression.ArrayIndex(parameterList, Expression.Constant(index));

                return Expression.Convert(parameter, argument.ParameterType);
            });

            var body = Expression.Call(convertedInstance, method, convertedParameterList);

            var lambda = Expression.Lambda<Action<object, object[]>>(body, instance, parameterList).Compile();

            return new InfuseWithAction(lambda, argumentList.Select(info => new Argument(info.Name, info.ParameterType)).ToArray());
        }

        public IActivation CreateCollectionActivation(Type elementType)
        {
            var parameterList = Expression.Parameter(typeof(object[]), "parameterList");
            var length = Expression.Parameter(typeof(int), "length");
            var assignParameterListLength = Expression.Assign(length, Expression.ArrayLength(parameterList));

            var array = Expression.Parameter(elementType.MakeArrayType(), "array");
            var assignArray = Expression.Assign(array, Expression.NewArrayBounds(elementType, length));

            var index = Expression.Parameter(typeof(int), "index");
            var parameterElement = Expression.ArrayAccess(parameterList, index);
            var parameter = Expression.Convert(parameterElement, elementType);
            var arrayElement = Expression.ArrayAccess(array, index);
            var parameterToArrayElement = Expression.Assign(arrayElement, parameter);

            var loopCondition = Expression.LessThan(index, length);
            var increment = Expression.PostIncrementAssign(index);
            var terminal = Expression.Label("Terminal");
            var loopInitializer = Expression.Assign(index, Expression.Constant(0));
            var loopBody
                = Expression.IfThenElse(loopCondition, Expression.Block(parameterToArrayElement, increment), Expression.Break(terminal));
            var loop
                = Expression.Block(new[] { index }, loopInitializer, Expression.Loop(loopBody, terminal));

            var body
                = Expression.Block(typeof(object), new[]{ length , array}, assignParameterListLength, assignArray, loop, array);
            var lambda = Expression.Lambda<Func<object[], object>>(body, parameterList);

            var method = lambda.Compile();

            return new ActivateWithFunction(method, Array.Empty<Argument>());
        }
    }
}

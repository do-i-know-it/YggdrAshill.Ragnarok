using System;
using System.Linq;
using System.Linq.Expressions;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IOperation"/> with <see cref="Expression"/>.
    /// </summary>
    public sealed class ExpressionToOperate : IOperation
    {
        /// <summary>
        /// Singleton instance of <see cref="ExpressionToOperate"/>.
        /// </summary>
        public static ExpressionToOperate Instance { get; } = new();

        private ExpressionToOperate()
        {

        }

        /// <inheritdoc/>
        public IActivation CreateActivation(ConstructorInjectionRequest request)
        {
            var constructor = request.Constructor;
            var argumentList = request.ParameterList;

            var parameterList = Expression.Parameter(typeof(object[]), "parameterList");
            var convertedParameterList = argumentList.Select((argument, index) =>
            {
                var parameter = Expression.ArrayIndex(parameterList, Expression.Constant(index));

                return Expression.Convert(parameter, argument.ParameterType);
            });

            var body = Expression.New(constructor, convertedParameterList);

            var lambda = Expression.Lambda<Func<object[], object>>(body, parameterList).Compile();

            return new ActivateWithFunction(lambda, request.Dependency);
        }

        /// <inheritdoc/>
        public IInfusion CreateFieldInfusion(FieldInjectionRequest request)
        {
            var implementedType = request.ImplementedType;
            var fieldList = request.FieldList;

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

            return new InfuseWithAction(lambda, request.Dependency);
        }

        /// <inheritdoc/>
        public IInfusion CreatePropertyInfusion(PropertyInjectionRequest request)
        {
            var implementedType = request.ImplementedType;
            var propertyList = request.PropertyList;

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

            return new InfuseWithAction(lambda, request.Dependency);
        }

        /// <inheritdoc/>
        public IInfusion CreateMethodInfusion(MethodInjectionRequest request)
        {
            var implementedType = request.ImplementedType;
            var method = request.Method;
            var argumentList = request.ParameterList;

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

            return new InfuseWithAction(lambda, request.Dependency);
        }

        /// <inheritdoc/>
        public IActivation CreateCollectionActivation(CollectionInjectionRequest request)
        {
            var elementType = request.ElementType;
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

            return new ActivateWithFunction(method, request.Dependency);
        }
    }
}

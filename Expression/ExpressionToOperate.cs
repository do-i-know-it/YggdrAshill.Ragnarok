using System;
using System.Linq;
using System.Linq.Expressions;

namespace YggdrAshill.Ragnarok
{
    /// <summary>
    /// Implementation of <see cref="IDependencyOperation"/> with <see cref="Expression"/>.
    /// </summary>
    public sealed class ExpressionToOperate : IDependencyOperation
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
            var implementedType = request.ImplementedType;

            var parameterList = Expression.Parameter(typeof(object[]), "parameterList");
            var convertedParameterList = argumentList.Select((argument, index) =>
            {
                var parameter = Expression.ArrayIndex(parameterList, Expression.Constant(index));
                return argument.ParameterType.IsValueType ? Expression.Convert(parameter, argument.ParameterType) : Expression.TypeAs(parameter, argument.ParameterType);
            });

            var body = (Expression)Expression.New(constructor, convertedParameterList);
            if (implementedType.IsValueType)
            {
                body = Expression.Convert(body, typeof(object));
            }
            var lambda = Expression.Lambda<Func<object[], object>>(body, parameterList).Compile();
            return new ActivateWithFunction(lambda);
        }

        /// <inheritdoc/>
        public IInfusion CreateFieldInfusion(FieldInjectionRequest request)
        {
            var implementedType = request.ImplementedType;
            var fieldList = request.FieldList;

            var instance = Expression.Parameter(typeof(object).MakeByRefType(), "instance");
            var parameterList = Expression.Parameter(typeof(object[]), "parameterList");
            var convertedVariable = Expression.Variable(implementedType);

            var convertedInstance = Expression.Convert(instance, implementedType);
            var assignedInstance = Expression.Assign(convertedVariable, convertedInstance);

            var assignedFieldList = fieldList.Select((fieldInfo, index) =>
            {
                var field = Expression.Field(convertedVariable, fieldInfo);
                var parameter = Expression.ArrayIndex(parameterList, Expression.Constant(index));
                var convertedParameter = Expression.Convert(parameter, fieldInfo.FieldType);
                return Expression.Assign(field, convertedParameter);
            });

            var reconvertedInstance = Expression.Convert(convertedVariable, typeof(object));
            var reassignedInstance = Expression.Assign(instance, reconvertedInstance);
            var blockList = Array.Empty<Expression>().Append(assignedInstance).Concat(assignedFieldList).Append(reassignedInstance);

            var body = Expression.Block(new[] { convertedVariable }, blockList);
            var lambda = Expression.Lambda<ActionToInfuse>(body, instance, parameterList).Compile();
            return new InfuseWithAction(lambda);
        }

        /// <inheritdoc/>
        public IInfusion CreatePropertyInfusion(PropertyInjectionRequest request)
        {
            var implementedType = request.ImplementedType;
            var propertyList = request.PropertyList;

            var instance = Expression.Parameter(typeof(object).MakeByRefType(), "instance");
            var parameterList = Expression.Parameter(typeof(object[]), "parameterList");
            var convertedVariable = Expression.Variable(implementedType);

            var convertedInstance = Expression.Convert(instance, implementedType);
            var assignedInstance = Expression.Assign(convertedVariable, convertedInstance);

            var assignedPropertyList = propertyList.Select((propertyInfo, index) =>
            {
                var property = Expression.Property(convertedVariable, propertyInfo);
                var parameter = Expression.ArrayIndex(parameterList, Expression.Constant(index));
                var convertedParameter = Expression.Convert(parameter, propertyInfo.PropertyType);
                return Expression.Assign(property, convertedParameter);
            });

            var reconvertedInstance = Expression.Convert(convertedVariable, typeof(object));
            var reassignedInstance = Expression.Assign(instance, reconvertedInstance);
            var blockList = Array.Empty<Expression>().Append(assignedInstance).Concat(assignedPropertyList).Append(reassignedInstance);

            var body = Expression.Block(new[] { convertedVariable }, blockList);
            var lambda = Expression.Lambda<ActionToInfuse>(body, instance, parameterList).Compile();
            return new InfuseWithAction(lambda);
        }

        /// <inheritdoc/>
        public IInfusion CreateMethodInfusion(MethodInjectionRequest request)
        {
            var implementedType = request.ImplementedType;
            var method = request.Method;
            var argumentList = request.ParameterList;

            var instance = Expression.Parameter(typeof(object).MakeByRefType(), "instance");
            var parameterList = Expression.Parameter(typeof(object[]), "parameterList");
            var convertedVariable = Expression.Variable(implementedType);

            var convertedInstance = Expression.Convert(instance, implementedType);
            var assignedInstance = Expression.Assign(convertedVariable, convertedInstance);

            var convertedParameterList = argumentList.Select((argument, index) =>
            {
                var parameter = Expression.ArrayIndex(parameterList, Expression.Constant(index));
                return Expression.Convert(parameter, argument.ParameterType);
            });

            var reconvertedInstance = Expression.Convert(convertedVariable, typeof(object));
            var reassignedInstance = Expression.Assign(instance, reconvertedInstance);

            var blockList = new Expression[]
            {
                assignedInstance,
                Expression.Call(convertedVariable, method, convertedParameterList),
                reassignedInstance
            };

            var body = Expression.Block(new[] { convertedVariable }, blockList);
            var lambda = Expression.Lambda<ActionToInfuse>(body, instance, parameterList).Compile();
            return new InfuseWithAction(lambda);
        }

        public IActivation CreateActivation(Type type)
        {
            var parameterList = Expression.Parameter(typeof(object[]), "parameterList");
            var body = (Expression)Expression.New(type);
            if (type.IsValueType)
            {
                body = Expression.Convert(body, typeof(object));
            }
            var lambda = Expression.Lambda<Func<object[], object>>(body, parameterList).Compile();
            return new ActivateWithFunction(lambda);
        }

        /// <inheritdoc/>
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
            return new ActivateWithFunction(method);
        }
    }
}

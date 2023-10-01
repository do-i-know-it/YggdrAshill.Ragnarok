using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal readonly struct DictionaryFactory : IDisposable
    {
        private readonly ICompilation compilation;
        private readonly IEnumerable<IStatement> statementList;
        private readonly Dictionary<Type, IDescription?> tableOfTypeToDescription;
        private readonly Dictionary<Type, List<IDescription>> tableOfTypeToDescriptionList;

        public DictionaryFactory(ICompilation compilation, IEnumerable<IStatement> statementList)
        {
            this.compilation = compilation;
            this.statementList = statementList;

            // TODO: object pooling.
            const int BufferSize = 128;
            tableOfTypeToDescription = new Dictionary<Type, IDescription?>(BufferSize);
            tableOfTypeToDescriptionList = new Dictionary<Type, List<IDescription>>(BufferSize);
        }

        public IDictionary<Type, IDescription?> Create()
        {
            foreach (var statement in statementList)
            {
                var assignedTypeList = statement.AssignedTypeList;
                var description = new Description(statement);

                if (assignedTypeList.Count != 0)
                {
                    foreach (var assignedType in statement.AssignedTypeList)
                    {
                        AddDescription(assignedType, description);
                    }

                    if (!tableOfTypeToDescription.ContainsKey(description.ImplementedType))
                    {
                        tableOfTypeToDescription.Add(description.ImplementedType, null);
                    }
                }
                else
                {
                    var implementedType = statement.ImplementedType;
                    AddDescription(implementedType, description);
                }
            }

            AddCollection();

            return tableOfTypeToDescription;
        }
        private void AddDescription(Type assignedType, IDescription description)
        {
            if (tableOfTypeToDescription.TryGetValue(assignedType, out var found))
            {
                if (tableOfTypeToDescriptionList.TryGetValue(assignedType, out var collection))
                {
                    foreach (var registered in collection)
                    {
                        var lifetime = registered.Lifetime;
                        var implementedType = registered.ImplementedType;

                        if (lifetime == Lifetime.Global && implementedType == description.ImplementedType)
                        {
                            throw new RagnarokAlreadyRegisteredException(implementedType);
                        }
                    }

                    collection.Add(description);
                }
                else
                {
                    collection = new List<IDescription>()
                    {
                        found!,
                        description,
                    };

                    tableOfTypeToDescriptionList.Add(assignedType, collection);
                }

                tableOfTypeToDescription[assignedType] = description;
            }
            else
            {
                tableOfTypeToDescription.Add(assignedType, description);
            }
        }
        private void AddCollection()
        {
            foreach (var pair in tableOfTypeToDescriptionList)
            {
                var elementType = pair.Key;
                var registrationList = pair.Value;

                var implementedType = TypeCache.ArrayTypeOf(elementType);

                var activation = compilation.CreateActivation(implementedType);

                var collection = new CollectionDescription(elementType, activation, registrationList.ToArray());

                foreach (var assignedType in CollectionDescription.AssignedTypeListOf(elementType))
                {
                    if (tableOfTypeToDescription.TryGetValue(assignedType, out _))
                    {
                        throw new RagnarokAlreadyRegisteredException(assignedType);
                    }

                    tableOfTypeToDescription.Add(assignedType, collection);
                }
            }
        }

        public void Validate(IScopedResolver resolver)
        {
            TypeAnalysis.Validate(statementList, resolver);
        }

        public void Dispose()
        {
            tableOfTypeToDescription.Clear();
            tableOfTypeToDescriptionList.Clear();
        }
    }
}

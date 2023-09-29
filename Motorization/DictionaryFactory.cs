using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal readonly struct DictionaryFactory : IDisposable
    {
        private readonly ICompilationV2 compilation;
        private readonly Dictionary<Type, IDepiction?> typeToDepiction;
        private readonly Dictionary<Type, List<IDepiction>> typeToRegistrationList;

        public DictionaryFactory(ICompilationV2 compilation)
        {
            this.compilation = compilation;

            // TODO: object pooling.
            const int BufferSize = 128;
            typeToDepiction = new Dictionary<Type, IDepiction?>(BufferSize);
            typeToRegistrationList = new Dictionary<Type, List<IDepiction>>(BufferSize);
        }

        public IDictionary<Type, IDepiction?> CreateContent(IEnumerable<IDescriptionV2> descriptionList)
        {
            foreach (var description in descriptionList)
            {
                var assignedTypeList = description.AssignedTypeList;
                var depiction = new Depiction(description);

                if (assignedTypeList.Count != 0)
                {
                    foreach (var assignedType in description.AssignedTypeList)
                    {
                        AddRegistration(assignedType, depiction);
                    }

                    if (!typeToDepiction.ContainsKey(depiction.ImplementedType))
                    {
                        typeToDepiction.Add(depiction.ImplementedType, null);
                    }
                }
                else
                {
                    var implementedType = description.ImplementedType;
                    AddRegistration(implementedType, depiction);
                }
            }

            AddCollection();

            return typeToDepiction;
        }
        private void AddRegistration(Type assignedType, IDepiction depiction)
        {
            if (typeToDepiction.TryGetValue(assignedType, out var found))
            {
                if (typeToRegistrationList.TryGetValue(assignedType, out var collection))
                {
                    foreach (var registered in collection)
                    {
                        var lifetime = registered.Lifetime;
                        var implementedType = registered.ImplementedType;

                        if (lifetime == Lifetime.Global && implementedType == depiction.ImplementedType)
                        {
                            throw new RagnarokAlreadyRegisteredException(implementedType);
                        }
                    }

                    collection.Add(depiction);
                }
                else
                {
                    collection = new List<IDepiction>()
                    {
                        found!,
                        depiction,
                    };

                    typeToRegistrationList.Add(assignedType, collection);
                }

                typeToDepiction[assignedType] = depiction;
            }
            else
            {
                typeToDepiction.Add(assignedType, depiction);
            }
        }
        private void AddCollection()
        {
            foreach (var pair in typeToRegistrationList)
            {
                var elementType = pair.Key;
                var registrationList = pair.Value;

                var implementedType = CollectionDepiction.GetImplementedType(elementType);

                var activation = compilation.GetActivation(implementedType);

                var collection = new CollectionDepiction(elementType, activation, registrationList.ToArray());

                foreach (var assignedType in collection.AssignedTypeList)
                {
                    if (typeToDepiction.TryGetValue(assignedType, out _))
                    {
                        throw new RagnarokAlreadyRegisteredException(assignedType);
                    }

                    typeToDepiction.Add(assignedType, collection);
                }
            }
        }

        public void Dispose()
        {
            typeToDepiction.Clear();
            typeToRegistrationList.Clear();
        }
    }
}

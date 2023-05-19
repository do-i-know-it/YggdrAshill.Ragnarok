using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Hierarchization;
using YggdrAshill.Ragnarok.Materialization;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok
{
    internal readonly struct RegistryFactory :
        IDisposable
    {
        private readonly ICodeBuilder codeBuilder;
        private readonly IEnumerable<IDescription> descriptionList;

        public RegistryFactory(ICodeBuilder codeBuilder, IEnumerable<IDescription> descriptionList)
        {
            this.codeBuilder = codeBuilder;
            this.descriptionList = descriptionList;

            // TODO: object pooling.
            const int BufferSize = 128;
            registrationBuffer = new List<IRegistration>(BufferSize);
            typeToRegistration = new Dictionary<Type, IRegistration?>(BufferSize);
            typeToRegistrationList = new Dictionary<Type, List<IRegistration>>(BufferSize);
        }

        private readonly List<IRegistration> registrationBuffer;
        private readonly Dictionary<Type, IRegistration?> typeToRegistration;
        private readonly Dictionary<Type, List<IRegistration>> typeToRegistrationList;

        public IRegistry Create(out IEnumerable<IRegistration> registrationList)
        {
            foreach (var description in descriptionList)
            {
                var assignedTypeList = description.AssignedTypeList;
                var registration = new Registration(description);

                registrationBuffer.Add(registration);

                if (assignedTypeList.Count != 0)
                {
                    foreach (var assignedType in description.AssignedTypeList)
                    {
                        AddRegistration(assignedType, registration);
                    }

                    if (!typeToRegistration.ContainsKey(registration.ImplementedType))
                    {
                        typeToRegistration.Add(registration.ImplementedType, null);
                    }
                }
                else
                {
                    var implementedType = description.ImplementedType;
                    AddRegistration(implementedType, registration);
                }
            }

            AddCollection();

            registrationList = registrationBuffer.ToArray();

            return new Registry(codeBuilder, typeToRegistration);
        }
        private void AddRegistration(Type assignedType, IRegistration registration)
        {
            if (typeToRegistration.TryGetValue(assignedType, out var found))
            {
                if (typeToRegistrationList.TryGetValue(assignedType, out var collection))
                {
                    foreach (var registered in collection)
                    {
                        var lifetime = registered.Lifetime;
                        var implementedType = registered.ImplementedType;

                        if (lifetime == Lifetime.Global && implementedType == registration.ImplementedType)
                        {
                            throw new Exception($"Conflict implementation type : {implementedType}");
                        }
                    }

                    collection.Add(registration);
                }
                else
                {
                    collection = new List<IRegistration>()
                    {
                        found!,
                        registration,
                    };

                    typeToRegistrationList.Add(assignedType, collection);
                }

                typeToRegistration[assignedType] = registration;
            }
            else
            {
                typeToRegistration.Add(assignedType, registration);
            }
        }
        private void AddCollection()
        {
            foreach (var pair in typeToRegistrationList)
            {
                var elementType = pair.Key;
                var registrationList = pair.Value;

                var implementedType = CollectionRegistration.GetImplementedType(elementType);

                var activation = codeBuilder.GetActivation(implementedType);

                var collection = new CollectionRegistration(elementType, activation, registrationList.ToArray());

                foreach (var assignedType in collection.AssignedTypeList)
                {
                    if (typeToRegistration.TryGetValue(assignedType, out _))
                    {
                        throw new Exception($"Collection of {assignedType} already exists.");
                    }

                    typeToRegistration.Add(assignedType, collection);
                }
            }
        }

        public void Dispose()
        {
            registrationBuffer.Clear();
            typeToRegistration.Clear();
            typeToRegistrationList.Clear();
        }
    }
}

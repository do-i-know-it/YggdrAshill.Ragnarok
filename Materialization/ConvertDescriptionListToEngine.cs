using YggdrAshill.Ragnarok.Construction;
using YggdrAshill.Ragnarok.Hierarchization;
using YggdrAshill.Ragnarok.Motorization;
using System;
using System.Collections.Generic;

namespace YggdrAshill.Ragnarok.Materialization
{
    internal readonly struct ConvertDescriptionListToEngine :
        IDisposable
    {
        private readonly ISolver solver;
        private readonly IEnumerable<IDescription> descriptionList;

        public ConvertDescriptionListToEngine(ISolver solver, IEnumerable<IDescription> descriptionList)
        {
            this.solver = solver;
            this.descriptionList = descriptionList;

            // TODO: object pooling.
            const int BufferSize = 128;
            registrationBuffer = new List<IRegistration>(BufferSize);
            typeToRegistration = new Dictionary<Type, IRegistration>(BufferSize);
            typeToRegistrationList = new Dictionary<Type, List<IRegistration>>(BufferSize);
        }

        private readonly List<IRegistration> registrationBuffer;
        private readonly Dictionary<Type, IRegistration> typeToRegistration;
        private readonly Dictionary<Type, List<IRegistration>> typeToRegistrationList;

        public IEngine Convert(out IEnumerable<IRegistration> registrationList)
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
                }
                else
                {
                    var implementedType = description.ImplementedType;
                    AddRegistration(implementedType, registration);
                }
            }

            AddCollection();

            registrationList = registrationBuffer.ToArray();

            return new Engine(typeToRegistration);
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
                        found,
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

                var generation = solver.CreateCollectionGeneration(elementType);

                var collection = new CollectionRegistration(generation, registrationList.ToArray());

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

using System.Collections.Generic;
using System;
using IziHardGames.Apps.Abstractions.Lib;
using System.Linq;

namespace IziHardGames.Apps.Abstractions.NetStd21
{
    [StaticAbstraction]
    public static class IziTypes
    {
        public readonly static SelectorForTypesRegistry select = new SelectorForTypesRegistry();
        public static TypesRegistry Default { get; set; }
    }

    public sealed class SelectorForTypesRegistry
    {
        private readonly Dictionary<Type, TypesRegistry> pairs = new Dictionary<Type, TypesRegistry>();
        public TypesRegistry this[Type type] { get => pairs[type]; set => AddOrUpdate(type, value); }
        public IEnumerable<TypesRegistry> All => pairs.Values;
        private void AddOrUpdate(Type type, TypesRegistry value)
        {
            if (pairs.TryGetValue(type, out var existed))
            {
                pairs[type] = value;
            }
            else
            {
                pairs.Add(type, value);
            }
        }
    }


    public sealed class TypesRegistry
    {
        private readonly Dictionary<Type, MetadataForType> pairs = new Dictionary<Type, MetadataForType>();
        private readonly Dictionary<string, MetadataForType> pairsByString = new Dictionary<string, MetadataForType>();
        private readonly Dictionary<int, MetadataForType> pairsByInt = new Dictionary<int, MetadataForType>();
        public MetadataForType this[Type type] { get => pairs[type]; set => AddOrUpdate(type, value); }
        public MetadataForType this[string key] { get => pairsByString[key]; set => AddOrUpdate(key, value); }
        public MetadataForType this[int key] { get => pairsByInt[key]; set => AddOrUpdate(key, value); }
		public IEnumerable<MetadataForType> All => pairs.Values;

		private void AddOrUpdate(int key, MetadataForType value)
		{
			throw new NotImplementedException();
		}

        private void AddOrUpdate(string key, MetadataForType value)
        {
            throw new NotImplementedException();
        }

        private void AddOrUpdate(Type type, MetadataForType value)
        {
            if (pairs.TryGetValue(type, out var existed))
            {
                pairs[type] = value;
            }
            else
            {
                pairs.Add(type, value);
            }
        }

        public Type GetByIdAsString(string idAsString)
        {
            throw new NotImplementedException();
        }

        public void FromString(Type type, string key)
        {
            var meta = new MetadataForType()
            {
                idAsInt = default,
                idAsString = key,
                type = type,
            };
            pairs.Add(type, meta);
            pairsByString.Add(key, meta);
        }
    }

    public sealed class MetadataForType
    {
        public Type type;
        public int idAsInt;
        public string idAsString;
    }
}

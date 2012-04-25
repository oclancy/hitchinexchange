﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition.Primitives;

namespace HitchinExchange.Core
{
    public class IsolatingCatalog : ComposablePartCatalog
    {
        private readonly object _synchRoot = new object();
        private readonly ComposablePartCatalog _interceptedCatalog;
        private volatile IQueryable<ComposablePartDefinition> _innerPartsQueryable;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsolatingCatalog"/> class.
        /// </summary>
        /// <param name="interceptedCatalog">Source catalog.</param>
        public IsolatingCatalog(ComposablePartCatalog interceptedCatalog)
        {
            if (interceptedCatalog == null)
            {
                throw new ArgumentNullException("interceptedCatalog");
            }

            _interceptedCatalog = interceptedCatalog;
        }

        public override IQueryable<ComposablePartDefinition> Parts
        {
            get { return GetParts(); }
        }

        private IQueryable<ComposablePartDefinition> GetParts()
        {
            if (_innerPartsQueryable == null)
            {
                lock (_synchRoot)
                {
                    if (_innerPartsQueryable == null)
                    {
                        IEnumerable<ComposablePartDefinition> parts =
                            new List<ComposablePartDefinition>(_interceptedCatalog.Parts);
                        
                        _innerPartsQueryable = parts.Select(GetPart).AsQueryable();
                    }
                }
            }

            return _innerPartsQueryable;
        }

        private static ComposablePartDefinition GetPart(ComposablePartDefinition original)
        {
            foreach (var exportDefinition in original.ExportDefinitions)
            {
                if (RequiresIsolation(exportDefinition.Metadata))
                {
                    return new IsolatingComposablePartDefinition(original);
                }
            }

            return original;
        }

        private static bool RequiresIsolation(IDictionary<string,object> metadata)
        {
            if (metadata.ContainsKey("Isolate"))
            {
                return true;
            }

            return false;
        }
    }
}

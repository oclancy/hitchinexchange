using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition.Primitives;

namespace HitchinExchange.Core
{
    class IsolatingComposablePartDefinition : ComposablePartDefinition
    {
        private System.ComponentModel.Composition.Primitives.ComposablePartDefinition original;

        public IsolatingComposablePartDefinition(System.ComponentModel.Composition.Primitives.ComposablePartDefinition original)
        {
            // TODO: Complete member initialization
            this.original = original;
        }

        public override ComposablePart CreatePart()
        {
            return new IsolatingComposablePart(this);
        }

        public override IEnumerable<ExportDefinition> ExportDefinitions
        {
            get { return original.ExportDefinitions; }
        }

        public override IEnumerable<ImportDefinition> ImportDefinitions
        {
            get { return original.ImportDefinitions; }
        }
    }
}

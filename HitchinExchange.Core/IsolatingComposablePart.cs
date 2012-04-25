using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.ReflectionModel;
using System.ComponentModel.Composition;

namespace HitchinExchange.Core
{
    class IsolatingComposablePart : ComposablePart
    {
        private IsolatingComposablePartDefinition m_def;

        public IsolatingComposablePart(IsolatingComposablePartDefinition def)
        {
            m_def = def;
        }

        public override IEnumerable<ExportDefinition> ExportDefinitions
        {
            get { return m_def.ExportDefinitions; }
        }

        public override object GetExportedValue(ExportDefinition definition)
        {
            var memberInfo = ReflectionModelServices.GetExportingMember(definition);
            var type = (Type)memberInfo.GetAccessors()[0];

            var newDomain = AppDomain.CreateDomain(type.FullName);

            return newDomain.CreateInstanceAndUnwrap(type.Assembly.GetName().ToString(), type.FullName);
        }

        public override IEnumerable<ImportDefinition> ImportDefinitions
        {
            get { return m_def.ImportDefinitions; }
        }

        public override void SetImport(ImportDefinition definition, IEnumerable<Export> exports)
        {
            throw new NotImplementedException();
        }
    }
}

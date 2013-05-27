using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TextTemplating;

namespace BenjaminSchroeter.Dsl.DirectiveProcessors
{
    public abstract class FileDirectiveProcessorBase : DirectiveProcessor
    {
        protected abstract string DIRECTIVE_NAME { get; }

        private string classCode;
        protected string templateFile;

        public override bool IsDirectiveSupported(string directiveName)
        {
            return (string.Compare(directiveName, DIRECTIVE_NAME, StringComparison.OrdinalIgnoreCase) == 0);
        }

        public override void ProcessDirective(string directiveName, IDictionary<string, string> arguments)
        {
            if (string.Compare(directiveName, DIRECTIVE_NAME, StringComparison.OrdinalIgnoreCase) == 0)
                classCode = CreateInjectingCode(arguments);
            else
                throw new ArgumentOutOfRangeException("directiveName");
        }

        protected abstract string CreateInjectingCode(IDictionary<string, string> arguments);

        public override void Initialize(ITextTemplatingEngineHost host)
        {
            base.Initialize(host);

            templateFile = host.TemplateFile;
        }

        public override void FinishProcessingRun()
        {
        }

        public override string GetClassCodeForProcessingRun()
        {
            return classCode;
        }

        public override string GetPreInitializationCodeForProcessingRun()
        {
            return "";
        }

        public override string GetPostInitializationCodeForProcessingRun()
        {
            return "";
        }

        public override string[] GetReferencesForProcessingRun()
        {
            return new[] { "System.Xml", "System.Xml.Linq", this.GetType().Assembly.Location };
        }

        public override string[] GetImportsForProcessingRun()
        {
            return new[] { "System.Xml", "System.Xml.Linq", "BenjaminSchroeter.Dsl.DirectiveProcessors" };
        }
    }
}
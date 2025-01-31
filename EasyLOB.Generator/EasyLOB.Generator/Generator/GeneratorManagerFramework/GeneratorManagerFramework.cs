namespace Generator
{
    public partial class GeneratorManagerFramework : GeneratorManager, IGeneratorManager
    {
        public Frameworks Framework { get; }

        public GeneratorManagerFramework()
        {
            Framework = Frameworks.DotNetFramework;
        }
    }
}

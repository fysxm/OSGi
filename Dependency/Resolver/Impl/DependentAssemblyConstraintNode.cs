﻿namespace UIShell.OSGi.Dependency.Resolver.Impl
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using UIShell.OSGi;
    using UIShell.OSGi.Configuration.BundleManifest;
    using UIShell.OSGi.Dependency;
    using UIShell.OSGi.Dependency.Metadata;
    using UIShell.OSGi.Dependency.Resolver;

    [DebuggerDisplay("DependentAssemblyConstraintNode-BundleSymbolicName:{BundleSymbolicName},AssemblyName:{AssemblyName},IsResolved:{IsResolved},IsResolvable:{IsResolvable}")]
    internal class DependentAssemblyConstraintNode : ConstraintNode, IResolvable, IConstraintNode, IDependentable, IDependentAssemblyConstraintNode
    {
        public DependentAssemblyConstraintNode(IResolver resolver, Interface2 owner, IDependentAssemblyConstraint constraint) : base(resolver, owner, constraint)
        {
            this.BundleSymbolicName = constraint.BundleSymbolicName;
            this.BundleVersion = constraint.BundleVersion;
            this.AssemblyName = constraint.AssemblyName;
            this.AssemblyVersion = constraint.AssemblyVersion;
            this.Resolution = constraint.Resolution;
        }

        public override string ToString() => 
            $"Owner bundle:{base.Owner},Dependect bundle name:{this.BundleSymbolicName},version:{this.BundleVersion},dependent assembly:{this.AssemblyName}, assembly version:{this.AssemblyVersion}";

        public string AssemblyName { get; private set; }

        public VersionRange AssemblyVersion { get; private set; }

        public string BundleSymbolicName { get; private set; }

        public VersionRange BundleVersion { get; private set; }

        public ResolutionType Resolution { get; private set; }

        protected override List<IMetadataNode> ResolveNodeSource
        {
            get
            {
                List<IMetadataNode> rt = new List<IMetadataNode>();
                base.ConstraintResolver.ResolvedNodes.ForEach(delegate (Interface2 resolvedNode) {
                    rt.AddRange(resolvedNode.SharedAssemblyNodes.ConvertAll<IMetadataNode>(assemblyNode => assemblyNode));
                });
                base.ConstraintResolver.UnResolverNodes.ForEach(delegate (Interface2 resolvedNode) {
                    rt.AddRange(resolvedNode.SharedAssemblyNodes.ConvertAll<IMetadataNode>(assemblyNode => assemblyNode));
                });
                return rt;
            }
        }
    }
}


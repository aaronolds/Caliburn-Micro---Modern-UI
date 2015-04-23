﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using Caliburn.Micro;
using CaliburnMicroModernUI.Extensions;
using CaliburnMicroModernUI.ViewModels;

namespace CaliburnMicroModernUI
{
    public class AppBootstrapper : BootstrapperBase 
    {
        private static CompositionContainer _container;

        public AppBootstrapper()
        {
            Initialize();
        }

        public static T GetInstance<T>()
        {
            var contract = AttributedModelServices.GetContractName(typeof (T));

            var sexports = _container.GetExportedValues<object>(contract);
            var enumerable = sexports as object[] ?? sexports.ToArray();
            if (enumerable.Any())
                return enumerable.OfType<T>().First();

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        protected override void Configure()
        {
            // Add New ViewLocator Rule
            ViewLocator.NameTransformer.AddRule(
                @"(?<nsbefore>([A-Za-z_]\w*\.)*)?(?<nsvm>ViewModels\.)(?<nsafter>([A-Za-z_]\w*\.)*)(?<basename>[A-Za-z_]\w*)(?<suffix>ViewModel$)",
                @"${nsbefore}Views.${nsafter}${basename}View",
                @"(([A-Za-z_]\w*\.)*)?ViewModels\.([A-Za-z_]\w*\.)*[A-Za-z_]\w*ViewModel$"
                );

            _container = new CompositionContainer(
                new AggregateCatalog(
                    new AssemblyCatalog(typeof (IShellViewModel).Assembly),
                    AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>().FirstOrDefault()
                    )
                );

            var batch = new CompositionBatch();
            batch.AddExport<IWindowManager>(() => new WindowManager());
            batch.AddExport<IEventAggregator>(() => new EventAggregator());
            _container.Compose(batch);
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            var contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;

            var exports = _container.GetExportedValues<object>(contract);
            return exports.FirstOrDefault();
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            var ret = Enumerable.Empty<object>();

            var contract = AttributedModelServices.GetContractName(serviceType);
            return _container.GetExportedValues<object>(contract);
        }

        protected override void BuildUp(object instance)
        {
            _container.SatisfyImportsOnce(instance);
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            DisplayRootViewFor<IShellViewModel>();
        }
    }
}
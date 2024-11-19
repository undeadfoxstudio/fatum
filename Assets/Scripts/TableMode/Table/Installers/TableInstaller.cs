using UnityEngine;
using Zenject;

namespace TableMode
{
    public class TableInstaller : MonoInstaller
    {
        public TableSlotsConfig MainTableSlotsConfig;
        public BoxCollider TableCollider;

        public override void InstallBindings()
        {
            Container.Bind<ITableProvider>()
                .To<TableProvider>()
                .AsSingle()
                .WithArguments(MainTableSlotsConfig, TableCollider);

            Container.Bind<ITableController>()
                .To<TableController>()
                .AsSingle()
                .NonLazy();
        }
    }
}
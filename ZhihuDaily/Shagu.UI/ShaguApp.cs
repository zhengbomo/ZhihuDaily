using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Caliburn.Micro;
using UmengSDK;

namespace Shagu.UI
{
    public class ShaguApp : CaliburnApplication
    {
        protected string UmengAppKey { private get; set; }

        protected WinRTContainer Container { get; private set; }

        protected override void Configure()
        {
            base.Configure();
            Container = new WinRTContainer();
            Container.RegisterWinRTServices();
        }

        protected override void PrepareViewFirst()
        {
            InitStaticValue();
            base.PrepareViewFirst();
        }

        protected virtual void InitStaticValue()
        {
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            await UmengAnalytics.StartTrackAsync(UmengAppKey, "channel");
        }

        protected override object GetInstance(Type service, string key)
        {
            return Container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return Container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            Container.BuildUp(instance);
        }

        protected override async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            base.OnSuspending(sender, e);
            var deferral = e.SuspendingOperation.GetDeferral();

            await UmengAnalytics.EndTrackAsync();

            deferral.Complete();
        }

    }
}

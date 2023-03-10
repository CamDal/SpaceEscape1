using System;
using System.Threading.Tasks;
using Unity.Services.Analytics;
using Unity.Services.Authentication.Internal;
using Unity.Services.Core.Configuration.Internal;
using Unity.Services.Core.Device.Internal;
using Unity.Services.Core.Environments.Internal;
using Unity.Services.Core.Internal;
using UnityEngine;

class Ua2CoreInitializeCallback : IInitializablePackage
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Register()
    {
        CoreRegistry.Instance.RegisterPackage(new Ua2CoreInitializeCallback())
            .DependsOn<IInstallationId>()
            .DependsOn<ICloudProjectId>()
            .DependsOn<IEnvironments>()
            .DependsOn<IProjectConfiguration>()
            .OptionallyDependsOn<IPlayerId>();
    }

    public async Task Initialize(CoreRegistry registry)
    {
        var cloudProjectId = registry.GetServiceComponent<ICloudProjectId>();
        var installationId = registry.GetServiceComponent<IInstallationId>();
        var playerId = registry.GetServiceComponent<IPlayerId>();
        var environments = registry.GetServiceComponent<IEnvironments>();
        var projectConfiguration = registry.GetServiceComponent<IProjectConfiguration>();

        var analyticsUserId = projectConfiguration.GetString("com.unity.services.core.analytics-user-id");

        AnalyticsService.internalInstance = new AnalyticsServiceInstance();
        await AnalyticsService.internalInstance.Initialize(cloudProjectId, installationId, playerId, environments.Current, analyticsUserId);

#if UNITY_ANALYTICS_DEVELOPMENT
        Debug.LogFormat("Core Initialize Callback\nInstall ID: {0}\nPlayer ID: {1}\nCustom Analytics ID: {2}",
            installationId.GetOrCreateIdentifier(),
            playerId?.PlayerId,
            analyticsUserId
        );
#endif

        if (AnalyticsService.internalInstance.ConsentTracker.IsGeoIpChecked())
        {
            AnalyticsService.internalInstance.Flush();
        }
    }
}

﻿using System;
using Unity;

namespace ABitNowMobileApp.Data
{
    public class ContainerManager
    {
        public static IUnityContainer Container { get; private set; }

        public static void Initialize()
        {
            Container = new UnityContainer();
        }
    }
}

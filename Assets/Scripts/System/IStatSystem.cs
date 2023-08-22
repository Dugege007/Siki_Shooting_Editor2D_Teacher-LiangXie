using FrameworkDesign;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：
 * 创建时间：
 */

namespace ShootingEditor2D
{
    public interface IStatSystem : ISystem
    {
        // 击杀数量
        BindableProperty<int> KillCount { get; }
    }

    // 实现类
    public class StatSystem : AbstractSystem, IStatSystem
    {
        public BindableProperty<int> KillCount { get; } = new BindableProperty<int>() { Value = 0 };

        protected override void OnInit()
        {
            
        }
    }
}

using FrameworkDesign;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * 创建人：杜
 * 功能说明：
 * 创建时间：
 */

namespace ShootingEditor2D
{
    public class HurtPlayerCommand : AbstractCommand
    {
        private readonly int mHurt;

        public HurtPlayerCommand(int hurt = 1)
        {
            mHurt = hurt;
        }

        protected override void OnExecute()
        {
            this.GetModel<IPlayerModel>().HP.Value -= mHurt;
        }
    }
}

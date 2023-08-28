using QFramework;

namespace ShootingEditor2D
{
    /// <summary>
    /// ��ѯָ��ǹ֧������ӵ�����
    /// </summary>
    public class MaxBulletCountQuery : AbstractQuery<int>
    {
        // ǹ֧����
        private readonly string mGunName;

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="gunName">Ҫ��ѯ��ǹ֧����</param>
        public MaxBulletCountQuery(string gunName)
        {
            mGunName = gunName;
        }

        /// <summary>
        /// ִ�в�ѯ����
        /// </summary>
        /// <returns>����ָ��ǹ֧������ӵ�����</returns>
        /// <remarks>
        /// ͨ��ǹ֧���ƴ� IGunConfigModel �л�ȡǹ֧�����
        /// </remarks>
        protected override int OnDo()   // ��ѯ����ͨ������Ϊ Do ����ѯ
        {
            IGunConfigModel gunConfigModel = this.GetModel<IGunConfigModel>();
            GunConfigItem gunConfigItem = gunConfigModel.GetItemByName(mGunName);
            return gunConfigItem.MaxBulletCount;
        }
    }
}

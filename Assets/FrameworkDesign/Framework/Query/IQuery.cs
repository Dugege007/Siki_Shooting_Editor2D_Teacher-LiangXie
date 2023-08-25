
namespace FrameworkDesign
{
    /// <summary>
    /// ��ѯ �ӿ�
    /// </summary>
    /// <typeparam name="TResult">��ѯ���������</typeparam>
    /// <remarks>
    /// �����˲�ѯ�Ļ������ܺ���Ϊ����������ܹ��������üܹ����ɻ�ȡϵͳ���ɻ�ȡģ�͡��ɷ��Ͳ�ѯ��
    /// ���ڻ�ȡ�ض��Ľ����
    /// </remarks>
    public interface IQuery<TResult> : IBelongToArchitecture, ICanSetArchitecture, ICanGetSystem, ICanGetModel, ICanSendQuery
    {
        /// <summary>
        /// ִ�в�ѯ
        /// </summary>
        /// <returns>���ز�ѯ�Ľ��</returns>
        TResult Do();
    }

    /// <summary>
    /// ���� ��ѯ��
    /// </summary>
    /// <typeparam name="T">��ѯ���������</typeparam>
    /// <remarks>
    /// ʵ�ֲ�ѯ�ӿڵĻ������ܡ�
    /// </remarks>
    public abstract class AbstractQuery<T> : IQuery<T>
    {
        // ����ܹ��������ڷ�������ܹ��Ĺ���
        private IArchitecture mArchitecture;

        /// <summary>
        /// ִ�в�ѯ
        /// </summary>
        /// <returns>���ز�ѯ�Ľ��</returns>
        public T Do()
        {
            return OnDo();
        }

        /// <summary>
        /// ִ�в�ѯ
        /// </summary>
        /// <returns>���ز�ѯ�Ľ��</returns>
        /// <remarks>
        /// ��������ʵ�־���Ĳ�ѯ�߼���
        /// </remarks>
        protected abstract T OnDo();    // �������д�����

        /// <summary>
        /// ��ȡ�ܹ�
        /// </summary>
        /// <returns>���ص�ǰ��ѯ�����ļܹ��ӿ�</returns>
        public IArchitecture GetArchitecture()
        {
            return mArchitecture;
        }

        /// <summary>
        /// ���üܹ�
        /// </summary>
        /// <param name="architecture">Ҫ���õļܹ�����</param>
        public void SetArchitecture(IArchitecture architecture)
        {
            mArchitecture = architecture;
        }
    }
}

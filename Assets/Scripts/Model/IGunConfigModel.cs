using FrameworkDesign;
using System.Collections.Generic;
using UnityEngine;

namespace ShootingEditor2D
{
    /// <summary>
    /// ǹе�������� �ӿ�
    /// </summary>
    public interface IGunConfigModel : IModel
    {
        GunConfigItem GetItemByName(string gunName);
    }

    /// <summary>
    /// ǹе���ñ�
    /// </summary>
    public class GunConfigItem
    {
        public GunConfigItem(string name, int bulletMaxCount, float attack, float frequency, float shootDistance, float recoil, bool needBullet, float reloadSeconds, string description)
        {
            Name = name;
            BulletMaxCount = bulletMaxCount;
            Attack = attack;
            Frequency = frequency;
            ShootDistance = shootDistance;
            Recoil = recoil;
            NeedBullet = needBullet;
            ReloadSeconds = reloadSeconds;
            Description = description;
        }

        public string Name { get; set; }
        public int BulletMaxCount { get; set; }
        public float Attack { get; set; }
        public float Frequency { get; set; }
        public float ShootDistance { get; set; }
        public float Recoil { get; set; }
        public bool NeedBullet { get; set; }
        public float ReloadSeconds { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// ǹе�������� ʵ����
    /// </summary>
    public class GunConfigModel : AbstractModel, IGunConfigModel
    {
        protected override void OnInit()
        {

        }

        private Dictionary<string, GunConfigItem> mItems = new Dictionary<string, GunConfigItem>()
        {
            // ���ƣ������ӵ�����������������Ƶ�ʣ���̣����������Ƿ���Ҫ�ӵ�������ʱ�䣬����
            { "��ǹ", new GunConfigItem("��ǹ", 7, 3, 3f, 0.5f, 0.1f, false, 1f, "����ƫС ����� ������ ������С ���޵�ҩ") },
            { "���ǹ", new GunConfigItem("���ǹ", 30, 2, 10f, 0.4f, 0.01f, true, 1f, "����С ��̽� ���ٿ� ������С") },
            { "����ǹ", new GunConfigItem("����ǹ", 2, 10, 1f, 0.333f, 0.5f, true, 2f, "������ ��̽� ������ �������� һ�η���6-12��") },
            { "��ǹ", new GunConfigItem("��ǹ", 30, 5, 5f, 0.8f, 0.2f, true, 1.5f, "������ ����� ������ ��������") },
            { "�ѻ�ǹ", new GunConfigItem("�ѻ�ǹ", 10, 10, 0.5f, 1f, 1f, true, 1.5f, "������ ���Զ ������ �������� ������׼") },
            { "���Ͳ", new GunConfigItem("���Ͳ", 1, 15, 0.2f, 1f, 2f, true, 2f, "�����ܴ� ���Զ ���ٺ��� �������ܴ� ����+��ը") },
        };

        public GunConfigItem GetItemByName(string gunName)
        {
            return mItems[gunName];
        }
    }
}

using System;

namespace FrameworkDesign
{
    // BindableProperty 可绑定的属性的简单实现
    // 用于可比较的值的更新
    // 数据 + 数据变更事件 的合体，既存储数据，又充当C#中的属性角色，也可以让别的地方监听它数据的变更事件，这样会大量减少样板代码；
    // 如再加个金币功能就要写一套一模一样的代码，比如增加一个 mGold、OnGoldChanged 委托、Gold 的数值比较逻辑，这种样板代码；
    // 所以很多框架都会用代码生成或者泛型方式去减少样板代码的编写时间；
    // 自底向上（如子节点通知父节点）的逻辑关系可以使用委托或事件，自顶向下（如父节点调用子节点）的逻辑关系可以使用方法调用

    /// <summary>
    /// 可绑定属性的简单实现，用于可比较的值的更新
    /// </summary>
    /// <typeparam name="T">比较的类型</typeparam>
    /// <remarks>
    /// 数据 + 数据变更事件的合体，既存储数据，又充当C#中的属性角色，也可以让别的地方监听它数据的变更事件。
    /// </remarks>
    public class BindableProperty<T>
    {
        private T mValue = default(T);

        /// <summary>
        /// 当属性值发生变化时，会触发变更事件
        /// </summary>
        public T Value
        {
            get => mValue;

            set
            {
                if (!value.Equals(mValue))
                {
                    mValue = value;

                    // 数值发生变化时，通知观察者更新界面（数值驱动）
                    mOnValueChanged?.Invoke(mValue);
                }
            }
        }

        //public Action<T> OnValueChanged; // += -= 事件不是很方便
        // 所以使用下面方法

        // 用于存储值变更时的回调方法
        private Action<T> mOnValueChanged = v => { };    // + -

        /// <summary>
        /// 注册值变更事件
        /// </summary>
        /// <param name="onValueChanged">值变更时的回调方法</param>
        /// <returns>返回一个可以用于注销的接口</returns>
        public IUnRegister RegisterOnValueChanged(Action<T> onValueChanged) // +
        {
            mOnValueChanged += onValueChanged;
            return new BindablePropertyUnregister<T>()
            {
                BindableProperty = this,
                OnValueChanged = onValueChanged
            };
        }

        /// <summary>
        /// 注销值变更事件
        /// </summary>
        /// <param name="onValueChanged">要注销的回调方法</param>
        public void UnRegisterOnValueChanged(Action<T> onValueChanged)  // -
        {
            mOnValueChanged -= onValueChanged;
        }
    }

    /// <summary>
    /// 可绑定属性的注销类，用于注销值变更事件
    /// </summary>
    /// <typeparam name="T">必须是可比较的类型</typeparam>
    public class BindablePropertyUnregister<T> : IUnRegister
    {
        public BindableProperty<T> BindableProperty { get; set; }

        public Action<T> OnValueChanged { get; set; }

        /// <summary>
        /// 注销事件
        /// </summary>
        public void UnRegister()
        {
            BindableProperty.UnRegisterOnValueChanged(OnValueChanged);

            // 更好的方式
            BindableProperty = null;
            OnValueChanged = null;
        }
    }
}

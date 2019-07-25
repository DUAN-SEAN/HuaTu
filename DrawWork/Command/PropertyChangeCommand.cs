using System.Collections;
using System.Windows.Forms;

namespace DrawWork.Command
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    class PropertyChangeCommand : ICommand
    {
        #region 字段

        private readonly ArrayList _itemsWhosePropertyChanged;
        private readonly Object _oldProperty;
        private readonly GridItem _propertyWhichChanged;

        private Object _newProperty;

        #endregion 字段

        #region 构造器

        public PropertyChangeCommand(ArrayList itemsWhosePropertyChanged,
            GridItem propertyWhichChanged,
            Object oldProperty)
        {
            _itemsWhosePropertyChanged = new ArrayList();
            _itemsWhosePropertyChanged.AddRange(itemsWhosePropertyChanged);

            _itemsWhosePropertyChanged = itemsWhosePropertyChanged;
            _propertyWhichChanged = propertyWhichChanged;
            _oldProperty = oldProperty;
        }

        //Disable default constructor
        private PropertyChangeCommand()
        {
        }

        #endregion 构造器

        #region 函数

        public void Execute()
        {
            try
            {
                for (int i = 0; i < _itemsWhosePropertyChanged.Count; i++)
                {
                    Type t = _itemsWhosePropertyChanged[i].GetType();
                    if (_propertyWhichChanged != null)
                    {
                        PropertyInfo pi = t.GetProperty(_propertyWhichChanged.Label);
                        pi.SetValue(_itemsWhosePropertyChanged[i], _newProperty, null);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void UnExecute()
        {
            try
            {
                for (int i = 0; i < _itemsWhosePropertyChanged.Count; i++)
                {
                    Type t = _itemsWhosePropertyChanged[i].GetType();
                    if (_propertyWhichChanged != null)
                    {
                        PropertyInfo pi = t.GetProperty(_propertyWhichChanged.Label);
                        _newProperty = pi.GetValue(_itemsWhosePropertyChanged[i], null);
                        pi.SetValue(_itemsWhosePropertyChanged[i], _oldProperty, null);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion 函数
    }
}
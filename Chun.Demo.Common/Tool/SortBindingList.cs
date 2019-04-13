/**************Code Info**************************
* Copyright(c) 2012-2013
* CLR 版本：4.0
* 文 件 名：
* 创 建 人：Rongqh
* 创建日期：2012/8/13 20:04:16
* 修 改 人：
* 修改日期：
* 备注描述：
************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Chun.Demo.Common
{
    public class SortBindingList<T> : BindingList<T>
    {
        private string defaultSortItem;
        private bool isSortedCore = true;
        private ListSortDirection sortDirectionCore = ListSortDirection.Ascending;
        private PropertyDescriptor sortPropertyCore;

        public SortBindingList() {
        }

        public SortBindingList(IList<T> list) : base(list) {
        }

        protected override bool SupportsSortingCore => true;

        protected override bool SupportsSearchingCore => true;

        protected override bool IsSortedCore => isSortedCore;

        protected override ListSortDirection SortDirectionCore => sortDirectionCore;

        protected override PropertyDescriptor SortPropertyCore => sortPropertyCore;

        public string DefaultSortItem {
            get => defaultSortItem;
            set {
                if (defaultSortItem != value) {
                    defaultSortItem = value;
                    Sort();
                }
            }
        }

        protected override int FindCore(PropertyDescriptor prop, object key) {
            for (var i = 0; i < Count; i++)
                if (Equals(prop.GetValue(this[i]), key))
                    return i;
            return -1;
        }

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction) {
            isSortedCore = true;
            sortPropertyCore = prop;
            sortDirectionCore = direction;
            Sort();
        }

        protected override void RemoveSortCore() {
            if (isSortedCore) {
                isSortedCore = false;
                sortPropertyCore = null;
                sortDirectionCore = ListSortDirection.Ascending;
                Sort();
            }
        }

        private void Sort() {
            var list = Items as List<T>;
            ResetBindings();
        }

        private int CompareCore(T o1, T o2) {
            var ret = 0;
            if (SortPropertyCore != null)
                ret = CompareValue(SortPropertyCore.GetValue(o1), SortPropertyCore.GetValue(o2),
                    SortPropertyCore.PropertyType);
            if (ret == 0 && DefaultSortItem != null) {
                var property = typeof(T).GetProperty(DefaultSortItem,
                    BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.IgnoreCase,
                    null, null, new Type[0], null);
                if (property != null)
                    ret = CompareValue(property.GetValue(o1, null), property.GetValue(o2, null), property.PropertyType);
            }
            if (SortDirectionCore == ListSortDirection.Descending)
                ret = -ret;
            return ret;
        }

        private static int CompareValue(object o1, object o2, Type type) {
            //这里改成自己定义的比较
            if (o1 == null)
                return o2 == null ? 0 : -1;
            if (o2 == null)
                return 1;
            if (type.IsPrimitive || type.IsEnum)
                return Convert.ToDouble(o1).CompareTo(Convert.ToDouble(o2));
            if (type == typeof(DateTime))
                return Convert.ToDateTime(o1).CompareTo(o2);
            if (type == typeof(T))
                return Convert.ToInt64(o1).CompareTo(o2);
            return string.Compare(o1.ToString().Trim(), o2.ToString().Trim());
        }
    }
}
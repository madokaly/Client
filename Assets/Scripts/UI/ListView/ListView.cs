using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[System.Serializable]

public class ItemSelectEvent : UnityEvent<ListView.ListViewItem>
{

}
/// <summary>
/// 列表布局
/// </summary>
public class ListView : MonoBehaviour
{
    /// <summary>
    /// 选择事件
    /// </summary>
    public UnityAction<ListViewItem> onItemSelected;


    /// <summary>
    /// 列表元素
    /// </summary>
    public class ListViewItem : MonoBehaviour, IPointerClickHandler
    {
        private bool selected;
        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                onSelected(selected);
            }
        }

        public virtual void onSelected(bool selected)
        {
            
        }
        /// <summary>
        /// 当前列表
        /// </summary>
        public ListView owner;
        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!this.selected)
            {
                this.Selected = true;
            }
            if (owner != null && owner.SelectedItem != this)
            {
                owner.SelectedItem = this;
            }
        }
    }
    /// <summary>
    /// 列表元素集合
    /// </summary>
    List<ListViewItem> items = new List<ListViewItem>();
    /// <summary>
    /// 选中项
    /// </summary>
    private ListViewItem selectedItem = null;
    public ListViewItem SelectedItem
    {
        get { return selectedItem; }
        set
        {
            if (selectedItem != null && selectedItem != value)
            {
                selectedItem.Selected = false;
            }
            selectedItem = value;
            if (selectedItem != null && onItemSelected != null)
            {
                onItemSelected.Invoke((ListViewItem)value);
            }
        }
    }

    public void AddItem(ListViewItem item)
    {
        item.owner = this;
        this.items.Add(item);
    }

    public void RemoveAll()
    {
        foreach (var it in items)
        {
            Destroy(it.gameObject);
        }
        items.Clear();
    }
}

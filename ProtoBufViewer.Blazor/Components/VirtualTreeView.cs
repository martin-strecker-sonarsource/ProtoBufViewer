using Microsoft.AspNetCore.Components;

namespace ProtoBufViewer.Blazor.Components
{
    public partial class VirtualTreeView<T> : ComponentBase
    {
        readonly record struct ViewItem(T Item, string Key, bool IsExpanded, int Level);

        private List<ViewItem> viewList = new();

        [Parameter]
        public RenderFragment<T>? ItemTemplate { get; set; }

        [Parameter]
        public IReadOnlyList<T>? Items { get; set; }

        [Parameter]
        public Func<T, IReadOnlyList<T>>? ChildrenSelector { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            SynchronizeViewList();
        }

        private void SynchronizeViewList()
        {
            viewList.Clear();
            if (Items is { } items)
            {
                viewList.AddRange(items.Select((x, i) => new ViewItem(x, $"{i}", false, 0)));
            }
        }

        private void OnClick(ViewItem item)
        {
            var viewListIndex = viewList.FindIndex(x => EqualityComparer<ViewItem>.Default.Equals(x, item));
            if (viewListIndex >= 0)
            {
                var newIsExpanded = !item.IsExpanded;
                viewList[viewListIndex] = item with { IsExpanded = newIsExpanded };
                var children = ChildrenSelector?.Invoke(item.Item) ?? [];
                if (newIsExpanded)
                {
                    viewList.InsertRange(viewListIndex + 1, children.Select((x, i) => new ViewItem(x, $"{item.Key}-{i}", false, item.Level + 1)));
                }
                else
                {
                    var cnt = children.Count;
                    cnt += viewList.Skip(viewListIndex + 1 + cnt).TakeWhile(x => x.Key.StartsWith(item.Key)).Count();
                    viewList.RemoveRange(viewListIndex + 1, cnt);
                }
            }
        }
    }
}
